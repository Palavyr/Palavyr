using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.ImageServices
{
    public class ImageSaver : IImageSaver
    {
        private readonly IConfiguration configuration;
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly IS3Saver s3Saver;
        private readonly IConfigurationRepository configurationRepository;
        private readonly DashContext dashContext;
        private readonly IAccountIdTransport accountIdTransport;

        public ImageSaver(
            IConfiguration configuration,
            IS3KeyResolver s3KeyResolver,
            IS3Saver s3Saver,
            IConfigurationRepository configurationRepository,
            DashContext dashContext,
            IAccountIdTransport accountIdTransport)
        {
            this.configuration = configuration;
            this.s3KeyResolver = s3KeyResolver;
            this.s3Saver = s3Saver;
            this.configurationRepository = configurationRepository;
            this.dashContext = dashContext;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<FileLink> SaveImage(IFormFile imageFile, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetUserDataBucket();
            
            
            var newImage = Image.CreateImageRecord(imageFile.FileName, s3KeyResolver, accountIdTransport.AccountId);

            await s3Saver.StreamObjectToS3(userDataBucket, imageFile, newImage.S3Key);

            // add to the databases
            await dashContext.Images.AddAsync(newImage);
            await dashContext.SaveChangesAsync(cancellationToken);

            return FileLink.CreateS3Link(imageFile.FileName, newImage.ImageId, newImage.S3Key);
        }
    }
}