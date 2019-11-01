using Amazon.S3;
using EpgGenerator;
using EpgGenerator.Configuration;
using EpgGenerator.HttpFactoryClient;
using EpgGenerator.Models;
using EpgGenerator.S3Uploader;
using EpgGenerator.Tv24EpgGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TV24ProgramGenerator
{
  public class Program
  {
    public static void Main()
    {
      var services = ConfigureServices(LambdaConfiguration.Configuration);
      var serviceProvider = services
          .BuildServiceProvider();
      serviceProvider.GetService<Service>().Run();
    }

    private static IServiceCollection ConfigureServices(IConfigurationRoot root)
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