using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EpgGenerator.HttpFactoryClient
{
    public class HttpFactoryClient : IHttpFactoryClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpFactoryClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GenerateStreamFromSource<T>(string requestUri, Func<Stream, T> callback)
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
                        return callback(stream);
                    }

                    content = await StreamConverter.ToStringAsync(stream);
                }

                throw new ApiException
                {
                    StatusCode = (int) response.StatusCode,
                    Content = content
                };
            }
        }
    }
}