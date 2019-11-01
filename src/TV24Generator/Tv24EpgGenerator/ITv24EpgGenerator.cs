using System.Threading.Tasks;

namespace EpgGenerator.Tv24EpgGenerator
{
    public interface ITv24EpgGenerator
    {
        Task GenerateTVProgram();
    }
}