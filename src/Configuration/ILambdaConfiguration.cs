using Microsoft.Extensions.Configuration;

namespace awscsharp.Configuration
{
    public interface ILambdaConfiguration
    {
        IConfigurationRoot Configuration { get; }
    }
}
