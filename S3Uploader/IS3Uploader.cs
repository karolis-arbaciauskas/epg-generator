using System.Threading.Tasks;
using System.Xml.Linq;

namespace awscsharp.S3Uploader
{
    public interface IS3Uploader
    {
        Task WritingAnObjectAsync(string generatedEpg);
    }
}
