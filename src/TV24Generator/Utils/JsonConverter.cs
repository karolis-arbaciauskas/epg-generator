using System.IO;
using Newtonsoft.Json;

namespace EpgGenerator.Utils
{
    public static class JsonConverter
    {
        public static T DeserializeFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false) return default;

            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jsonTextReader);

                return searchResult;
            }
        }
    }
}