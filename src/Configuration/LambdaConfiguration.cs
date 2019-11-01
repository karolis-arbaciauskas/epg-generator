using System.IO;
using Microsoft.Extensions.Configuration;

namespace EpgGenerator.Configuration
{
    public class LambdaConfiguration : ILambdaConfiguration
    {
        public static IConfigurationRoot Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        IConfigurationRoot ILambdaConfiguration.Configuration => Configuration;
    }
}