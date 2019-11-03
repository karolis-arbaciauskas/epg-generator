using System.IO;
using System.Threading.Tasks;

namespace EpgGenerator.HttpFactoryClient
{
    public static class StreamConverter
    {
        public static async Task<string> ToStringAsync(Stream stream)
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