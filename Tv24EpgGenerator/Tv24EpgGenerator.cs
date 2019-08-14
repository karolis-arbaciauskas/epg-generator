using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using awscsharp.Configuration;
using awscsharp.Models;
using awscsharp.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace awscsharp.Tv24EpgGenerator
{
    public class Tv24EpgGenerator : ITv24EpgGenerator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        readonly string _baseUrl;
        readonly string[] _channelsGroups;
        readonly int _numOfDays;

        public Tv24EpgGenerator(IHttpClientFactory httpClientFactory, ILambdaConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            _baseUrl = configuration.Configuration["tv24BaseUrl"];
            _channelsGroups = configuration.Configuration.GetSection("tv24Groups").Get<string[]>();
            _numOfDays = int.Parse(configuration.Configuration["numOfDaysToGenerate"]);
        }

        public async Task GenerateTVProgram()
        {
            var watch = Stopwatch.StartNew();

            var channelsList = new List<ChannelOutput>();
            var recordsList = new List<TvProgramOutput>();

            foreach (string program in _channelsGroups)
            {
                DateTime date = DateTime.Today;
                string stringifiedDate = date.ToString("dd-MM-yyyy");

                for (int i = 0; i < _numOfDays; i++)
                {
                    using (HttpClient client = _httpClientFactory.CreateClient())
                    {
                        string response = await client.GetStringAsync($"{_baseUrl}/{program}/{stringifiedDate}");
                        JObject programmes = JObject.Parse(response);
                        IList<JToken> records = programmes["schedule"]["programme"].Children().ToList();

                        foreach (JToken record in records)
                        {
                            TvProgramInput searchResult = record.ToObject<TvProgramInput>();

                            channelsList.Add(new ChannelOutput
                            {
                                Id = searchResult.Channel.Slug,
                                DisplayName = searchResult.Channel.Name
                            });

                            recordsList.Add(new TvProgramOutput
                            {
                                Channel = searchResult.Channel.Slug,
                                Title = searchResult.Title,
                                Description = searchResult.Description,
                                ProgrammeImage = searchResult.Image,
                                StartTime = DateTimeFormater.TimestampToString(searchResult.Start_unix),
                                EndTime = DateTimeFormater.TimestampToString(searchResult.Stop_unix),
                                Episode = searchResult.Ep_nr,
                                Country = searchResult.Country,
                                Year = searchResult.Year
                            });
                        }
                    }
                    Console.WriteLine(date);
                    date = date.AddDays(1);
                }
            }

            XElement generateXML = GenerateXml(channelsList, recordsList);

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;

            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
            timeSpan.Milliseconds / 10);

            Console.WriteLine(generateXML);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        private XElement GenerateXml(List<ChannelOutput> channels, List<TvProgramOutput> tvPrograms)
        {
            channels = channels.GroupBy(c => c.Id).Select(y => y.First()).ToList();
            tvPrograms = tvPrograms.GroupBy(c => new { c.Channel, c.StartTime }).Select(y => y.First()).ToList();

            return new XElement("tv",
            channels.Select(c =>
                new XElement("channel",
                    new XAttribute("id", c.Id),
                    new XElement("display-name", c.DisplayName)
                )
            ),
            tvPrograms.Select(x => new XElement("programme",
                new XAttribute("start", x.StartTime),
                new XAttribute("end", x.EndTime),
                new XAttribute("channel", x.Channel),
                new XElement("title", new XAttribute("lang", "lt"), x.Title),
                new XElement("desc", new XAttribute("lang", "lt"), x.Description),
                new XElement("category", new XAttribute("lang", "lt"), x.Category),
                new XElement("episode-num", new XAttribute("system", "onscreen"), x.Episode),
                new XElement("country", x.Country),
                x.Year > 0 ? new XElement("date", x.Year) : null,
                !string.IsNullOrEmpty(x.ProgrammeImage) ? new XElement("icon", new XAttribute("src", x.ProgrammeImage)) : null
            )));
        }
    }
}
