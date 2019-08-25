using Amazon.S3;
using awscsharp;
using awscsharp.Configuration;
using awscsharp.Models;
using awscsharp.Tv24EpgGenerator;
using awscsharp.S3Uploader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using awscsharp.HttpFactoryClient;

namespace AwsDotnetCsharp
{
    public class Handler
    {
        public static void Program()
        {
            var services = ConfigureServices(LambdaConfiguration.Configuration);
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<Service>().Run();

        }

        private static IServiceCollection ConfigureServices(IConfigurationRoot root)
        {
            IServiceCollection services = new ServiceCollection();
            services
                .Configure<Tv24Config>(options => root.GetSection("tv24Config").Bind(options))
                .Configure<AwsConfig>(options => root.GetSection("AwsConfig").Bind(options))
                .AddDefaultAWSOptions(root.GetAWSOptions())
                .AddAWSService<IAmazonS3>()
                .AddHttpClient()
                .AddTransient<IHttpFactoryClient, HttpFactoryClient>()
                .AddTransient<ITv24EpgGenerator, Tv24EpgGenerator>()
                .AddTransient<IS3Uploader, S3Uploader>()
                .AddTransient<Service>();

            return services;
        }
    }
}
