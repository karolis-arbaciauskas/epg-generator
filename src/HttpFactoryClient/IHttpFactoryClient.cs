using System.Threading.Tasks;

namespace awscsharp.HttpFactoryClient
{
    public interface IHttpFactoryClient
    {
        Task<T> GenerateStreamFromSource<T>(string requestUri);
    }
}