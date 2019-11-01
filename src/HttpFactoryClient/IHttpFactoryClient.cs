using System.Threading.Tasks;

namespace EpgGenerator.HttpFactoryClient
{
    public interface IHttpFactoryClient
    {
        Task<T> GenerateStreamFromSource<T>(string requestUri);
    }
}