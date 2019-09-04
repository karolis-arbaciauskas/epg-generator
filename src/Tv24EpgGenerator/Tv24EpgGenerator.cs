using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using awscsharp.HttpFactoryClient;
using awscsharp.Models;
using awscsharp.S3Uploader;
using awscsharp.Utils;
using Microsoft.Extensions.Options;
using static awscsharp.Utils.XmlSerializer;

namespace awscsharp.Tv24EpgGenerator
{
    public class Tv24EpgGenerator : ITv24EpgGenerator
    {
        private readonly IHttpFactoryClient _httpClientFactory;
        private readonly IS3Uploader _s3Uploader;

        readonly string _baseUrl;
        readonly string _mediaGalery;
        readonly string[] _channelsGroups;
        readonly int _numOfDays;

        public Tv24EpgGenerator(IHttpFactoryClient httpClientFactory, IOptions<Tv24Config> configuration, IS3Uploader s3Uploader)
        {
            _httpClientFactory = httpClientFactory;

            _baseUrl = configuration.Value.BaseUrl;
            _mediaGalery = configuration.Value.MediaGalery;
            _channelsGroups = configuration.Value.Groups;
            _numOfDays = configuration.Value.NumOfDaysToGenerate;

            _s3Uploader = s3Uploader;
        }

        public async Task GenerateTVProgram()
        {
            var watch = Stopwatch.StartNew();

            var channelsList = new List<ChannelOutput>();
            var recordsList = new List<TvProgramOutput>();

            IEnumerable<RootObject> tvGuidesList = await GetTvGuideInParallel<RootObject>(_channelsGroups);

            foreach(RootObject tvGuide in tvGuidesList)
            {
                foreach (TvProgramInput record in tvGuide.Schedule.Programme)
                {
                    channelsList.Add(new ChannelOutput
                    {
                        Id = record.Channel.Slug,
                        DisplayName = record.Channel.Name,
                        Logo = $"{_mediaGalery}/{record.Channel.Logo_64}"
                    });

                    recordsList.Add(new TvProgramOutput
                    {
                        Channel = record.Channel.Slug,
                        Title = record.Title,
                        Description = record.Description_long,
                        ProgrammeImage = record.Image,
                        StartTime = DateTimeFormater.TimestampToString(record.Start_unix),
                        EndTime = DateTimeFormater.TimestampToString(record.Stop_unix),
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
            }

            XDocument generateXML = GenerateXml.Create(channelsList, recordsList);
            string convertToXml = SerializeXml.Convert(generateXML);
            await _s3Uploader.WritingAnObjectAsync(convertToXml);

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;

            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
                timeSpan.Milliseconds / 10
            );

            Console.WriteLine("RunTime " + elapsedTime);
        }

        private async Task<IEnumerable<T>> GetTvGuideInParallel<T>(IEnumerable<string> channelsGroups)
        {
            var tvGuides = new List<T>();
            DateTime date = DateTime.Today;

            for (int i = 0; i < _numOfDays; i++)
            {
                var tasks = channelsGroups.Select(channelsGroup => _httpClientFactory.GenerateStreamFromSource<T>($"{_baseUrl}/{channelsGroup}/{date.ToString("dd-MM-yyyy")}"));
                tvGuides.AddRange(await Task.WhenAll(tasks));
                date = date.AddDays(1);
            }

            return tvGuides;
        }
    }
}
