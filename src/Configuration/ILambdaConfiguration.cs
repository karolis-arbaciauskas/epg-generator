using Microsoft.Extensions.Configuration;

namespace EpgGenerator.Configuration
{
    public interface ILambdaConfiguration
    {
        IConfigurationRoot Configuration { get; }
    }
}