using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace awscsharp.HttpFactoryClient
{
    public class HttpFactoryClient : IHttpFactoryClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpFactoryClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GenerateStreamFromSource<T>(string requestUri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (var client = _httpClientFactory.CreateClient())
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                string content;
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var deserializeInputFromStream = DeserializeJsonFromStream<T>(stream);
                        return deserializeInputFromStream;
                    }

                    content = await StreamToStringAsync(stream);
                }

                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            {
                return default;
            }

            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jsonTextReader);

                return searchResult;
            }
        }

        private async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream == null) return content;
            using (var streamReader = new StreamReader(stream))
            {
                content = await streamReader.ReadToEndAsync();
            }

            return content;
        }
    }
}
