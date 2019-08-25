using System.IO;
using Microsoft.Extensions.Configuration;

namespace awscsharp.Configuration
{
    public class LambdaConfiguration : ILambdaConfiguration
    {
        public static IConfigurationRoot Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        IConfigurationRoot ILambdaConfiguration.Configuration => Configuration;
    }
}
