using System.Threading.Tasks;

namespace EpgGenerator.S3Uploader
{
    public interface IS3Uploader
    {
        Task WritingAnObjectAsync(string generatedEpg);
    }
}