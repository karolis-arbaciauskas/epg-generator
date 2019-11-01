using Amazon.S3;
using awscsharp;
using awscsharp.Configuration;
using awscsharp.HttpFactoryClient;
using awscsharp.Models;
using awscsharp.S3Uploader;
using awscsharp.Tv24EpgGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwsDotnetCsharp
{
  public class Handler
  {
    public static void Main()
    {
      var services = ConfigureServices(LambdaConfiguration.Configuration);
      var serviceProvider = services
          .BuildServiceProvider();
      serviceProvider.GetService<Service>().Run();
    }

    private static IServiceCollection ConfigureServices(IConfiguration root)
    {
      IServiceCollection services = new ServiceCollection();
      services
          .Configure<Tv24Config>(options => root.GetSection("tv24Config").Bind(options))
          .Configure<AwsConfig>(options => root.GetSection("AwsConfig").Bind(options))
          .AddDefaultAWSOptions(root.GetAWSOptions())
          .AddTransient<IHttpFactoryClient, HttpFactoryClient>()
          .AddAWSService<IAmazonS3>()
          .AddTransient<ITv24EpgGenerator, Tv24EpgGenerator>()
          .AddTransient<IS3Uploader, S3Uploader>()
          .AddHttpClient()
          .AddTransient<Service>();

      return services;
    }
  }
}