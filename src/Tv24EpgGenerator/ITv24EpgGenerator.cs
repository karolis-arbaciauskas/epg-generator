using System.Threading.Tasks;

namespace awscsharp.Tv24EpgGenerator
{
    public interface ITv24EpgGenerator
    {
        Task GenerateTVProgram();
    }
}