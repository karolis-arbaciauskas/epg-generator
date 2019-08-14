using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace awscsharp.Configuration
{
    public class LambdaConfiguration : ILambdaConfiguration
    {
        public IConfiguration Configuration => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
    }
}
