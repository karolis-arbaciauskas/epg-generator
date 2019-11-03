using System;
using System.IO;
using System.Threading.Tasks;

namespace EpgGenerator.HttpFactoryClient
{
    public interface IHttpFactoryClient
    {
        Task<T> GenerateStreamFromSource<T>(string requestUri, Func<Stream, T> callback);
    }
}