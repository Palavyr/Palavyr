using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Services.ImageServices
{
    public class ImageSaver : IImageSaver
    {
        private readonly IConfiguration configuration;
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly IS3Saver s3Saver;
        private readonly DashContext dashContext;
        private readonly ILinkCreator linkCreator;

        public ImageSaver(
            IConfiguration configuration,
            IS3KeyResolver s3KeyResolver,
            IS3Saver s3Saver,
            DashContext dashContext,
            ILinkCreator linkCreator)
        {
            this.configuration = configuration;
            this.s3KeyResolver = s3KeyResolver;
            this.s3Saver = s3Saver;
            this.dashContext = dashContext;
            this.linkCreator = linkCreator;
        }

        public async Task<FileLink> SaveImage(string accountId, IFormFile imageFile, CancellationToken cancellationToken)
        {
            var userDataBucket = configuration.GetUserDataBucket();
            var newImage = Image.CreateImageRecord(imageFile.FileName, s3KeyResolver, accountId);

            await s3Saver.StreamObjectToS3(userDataBucket, imageFile, newImage.S3Key);
            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(newImage.S3Key, userDataBucket, DateTime.Now.AddDays(6.5));

            // add to the databases
            await dashContext.Images.AddAsync(newImage);
            await dashContext.SaveChangesAsync(cancellationToken);

            return FileLink.CreateLink(imageFile.FileName, preSignedUrl, newImage.SafeName);
        }
    }
}