using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EpgGenerator.HttpFactoryClient;
using EpgGenerator.Models;
using EpgGenerator.S3Uploader;
using EpgGenerator.Utils;
using Microsoft.Extensions.Options;

namespace EpgGenerator.Tv24EpgGenerator
{
  public class Tv24EpgGenerator : ITv24EpgGenerator
  {
    private readonly string _baseUrl;
    private readonly string[] _channelsGroups;
    private readonly IHttpFactoryClient _httpClientFactory;
    private readonly string _mediaGallery;
    private readonly int _numOfDays;
    private readonly IS3Uploader _s3Uploader;

    public Tv24EpgGenerator(IHttpFactoryClient httpClientFactory,
        IOptions<Tv24Config> configuration,
        IS3Uploader s3Uploader)
    {
      _httpClientFactory = httpClientFactory;

      _baseUrl = configuration.Value.BaseUrl;
      _mediaGallery = configuration.Value.MediaGallery;
      _channelsGroups = configuration.Value.Groups;
      _numOfDays = configuration.Value.NumOfDaysToGenerate;

      _s3Uploader = s3Uploader;
    }

    public async Task GenerateTVProgram()
    {
      var watch = Stopwatch.StartNew();

      var channelsList = new List<ChannelOutput>();
      var recordsList = new List<TvProgramOutput>();

      var tvGuidesList = await GetTvGuideInParallel<RootObject>(_channelsGroups);

      foreach (var tvGuide in tvGuidesList)
        foreach (var record in tvGuide.Schedule.Programme)
        {
          channelsList.Add(new ChannelOutput
          {
            Id = record.Channel.Slug,
            DisplayName = record.Channel.Name,
            Logo = $"{_mediaGallery}/{record.Channel.Logo_64}"
          });

          recordsList.Add(new TvProgramOutput
          {
            Channel = record.Channel.Slug,
            Title = record.Title,
            Description = record.Description_long,
            ProgrammeImage = record.Image,
            StartTime = DateTimeFormatter.TimestampToString(record.Start_unix),
            EndTime = DateTimeFormatter.TimestampToString(record.Stop_unix),
            Episode = record.Ep_nr,
            Country = record.Country,
            Year = record.Year,
            Credits = new Credit
            {
              Actor = record.Cast,
              Director = record.Director
            }
          });
        }

      var generateXML = GenerateXml.Create(channelsList, recordsList);
      var convertToXml = XmlSerializer.SerializeXml.Convert(generateXML);
      await _s3Uploader.WritingAnObjectAsync(convertToXml);

      watch.Stop();
      var timeSpan = watch.Elapsed;

      var elapsedTime =
          $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}.{timeSpan.Milliseconds / 10:00}";

      Console.WriteLine("RunTime " + elapsedTime);
    }

    private async Task<IEnumerable<T>> GetTvGuideInParallel<T>(IEnumerable<string> channelsGroups)
    {
      var tvGuides = new List<T>();
      var date = DateTime.Today;

      for (var i = 0; i < _numOfDays; i++)
      {
        var tasks = channelsGroups.Select(channelsGroup => _httpClientFactory.GenerateStreamFromSource<T>(
          $"{_baseUrl}/{channelsGroup}/{date:dd-MM-yyyy}", 
          JsonConverter.DeserializeFromStream<T>
        ));
        tvGuides.AddRange(await Task.WhenAll(tasks));
        date = date.AddDays(1);
      }

      return tvGuides;
    }
  }
}