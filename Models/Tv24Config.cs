using Newtonsoft.Json;

namespace awscsharp.Models
{
    [JsonObject("tv24Config")]
    public class Tv24Config
    {
        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("mediaGalery")]
        public string MediaGalery { get; set; }

        [JsonProperty("groups")]
        public string[] Groups { get; set; }

        [JsonProperty("numOfDaysToGenerate")]
        public int NumOfDaysToGenerate { get; set; }
    }
}
