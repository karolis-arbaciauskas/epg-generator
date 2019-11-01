using Newtonsoft.Json;

namespace EpgGenerator.Models
{
    [JsonObject("tv24Config")]
    public class Tv24Config
    {
        [JsonProperty("baseUrl")] public string BaseUrl { get; set; }

        [JsonProperty("mediaGallery")] public string MediaGallery { get; set; }

        [JsonProperty("groups")] public string[] Groups { get; set; }

        [JsonProperty("numOfDaysToGenerate")] public int NumOfDaysToGenerate { get; set; }
    }
}