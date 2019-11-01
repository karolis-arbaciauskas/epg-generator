using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EpgGenerator.Models;

namespace EpgGenerator.Tv24EpgGenerator
{
    public static class GenerateXml
    {
        public static XDocument Create(List<ChannelOutput> channels, List<TvProgramOutput> tvPrograms)
        {
            channels = channels.GroupBy(c => c.Id).Select(y => y.First()).ToList();
            tvPrograms = tvPrograms.GroupBy(c => new {c.Channel, c.StartTime}).Select(y => y.First()).ToList();

            return new XDocument(
                new XDeclaration("1.0", "utf-8", ""),
                new XElement("tv", channels.Select(c =>
                        new XElement("channel",
                            new XAttribute("id", c.Id),
                            new XElement("icon", new XAttribute("src", c.Logo)),
                            new XElement("display-name", c.DisplayName)
                        )
                    ),
                    tvPrograms.Select(x => new XElement("programme",
                        new XAttribute("start", x.StartTime),
                        new XAttribute("stop", x.EndTime),
                        new XAttribute("channel", x.Channel),
                        new XElement("title", new XAttribute("lang", "lt"), x.Title),
                        new XElement("desc", new XAttribute("lang", "lt"), x.Description),
                        new XElement("category", new XAttribute("lang", "lt"), x.Category),
                        new XElement("episode-num", new XAttribute("system", "onscreen"), x.Episode),
                        new XElement("credits",
                            new XElement("director", x.Credits?.Director),
                            new XElement("actor", x.Credits?.Actor)
                        ),
                        new XElement("country", x.Country),
                        x.Year > 0 ? new XElement("date", x.Year) : null,
                        !string.IsNullOrEmpty(x.ProgrammeImage)
                            ? new XElement("icon", new XAttribute("src", x.ProgrammeImage))
                            : null
                    ))));
        }
    }
}