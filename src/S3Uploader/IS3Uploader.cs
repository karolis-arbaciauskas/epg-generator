using System.Threading.Tasks;

namespace awscsharp.S3Uploader
{
    public interface IS3Uploader
    {
        Task WritingAnObjectAsync(string generatedEpg);
    }
}