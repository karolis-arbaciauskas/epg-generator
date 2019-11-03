using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using EpgGenerator.Models;
using EpgGenerator.S3Uploader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace TV24Generator.Tests
{
    public class S3UploaderTests
    {
        private readonly IS3Uploader _s3Uploader;
        private readonly Mock<IAmazonS3> _client;

        public S3UploaderTests()
        {
            _client = new Mock<IAmazonS3>();

            var services = ConfigureService();
            _s3Uploader = services.GetRequiredService<IS3Uploader>();
        }

        private IServiceProvider ConfigureService()
        {
            var config = new Dictionary<string, string>
            {
                {"BucketName", "TestBucket"},
                {"FileName", "TestFileName"}
            };

            var configRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            return new ServiceCollection()
                .Configure<AwsConfig>(options => configRoot.Bind(options))
                .AddDefaultAWSOptions(configRoot.GetAWSOptions())
                .AddSingleton(_client.Object)
                .AddTransient<IS3Uploader, S3Uploader>()
                .BuildServiceProvider();

        }

        [Fact]
        public async Task ShouldUploadFile_to_S3Bucket()
        {
            await _s3Uploader.WritingAnObjectAsync("xml");
            _client.Verify(i => i.PutObjectAsync(It.IsAny<PutObjectRequest>(),It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task ShouldThrowS3Exception_if_UploadToS3BucketFails()
        {
            const string error = "error";
            _client.Setup(i => i.PutObjectAsync(
                    It.IsAny<PutObjectRequest>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new AmazonS3Exception("error"));

            Assert.ThrowsAsync<AmazonS3Exception>(() => _s3Uploader.WritingAnObjectAsync("xml"));
        }
        
        [Fact]
        public async Task ShouldThrowException_if_UploadToS3BucketFails()
        {
            const string error = "error";
            _client.Setup(i => i.PutObjectAsync(
                    It.IsAny<PutObjectRequest>(),
                    It.IsAny<CancellationToken>()
                ))
                .Throws(new Exception("error"));

            Assert.ThrowsAsync<Exception>(() => _s3Uploader.WritingAnObjectAsync("xml"));
        }
    }
}