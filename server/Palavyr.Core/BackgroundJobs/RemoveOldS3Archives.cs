using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace Palavyr.Core.BackgroundJobs
{
    public class RemoveOldS3Archives : IRemoveOldS3Archives
    {
        private IAmazonS3 S3Client { get; }
        private readonly ILogger<RemoveOldS3Archives> _logger;

        public RemoveOldS3Archives(IAmazonS3 s3Client, ILogger<RemoveOldS3Archives> logger)
        {
            S3Client = s3Client;
            _logger = logger;
        }

        public async Task RemoveS3Objects()
        {
            var staleObjects = await ListS3Archives();
            var keyVersions = new List<KeyVersion>();
            foreach (var staleObject in staleObjects)
            {
                var keyVersion = new KeyVersion() {Key = staleObject.Key};
                keyVersions.Add(keyVersion);
            }

            var request = new DeleteObjectsRequest()
            {
                BucketName = Utils.ArchivesBucket,
                Objects = keyVersions
            };
            try
            {
                var response = await S3Client.DeleteObjectsAsync(request);
                _logger.LogInformation($"Successfully Deleted objects. Http status Response: {response.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Could not delete: {ex.Message}");
            }
        }

        private async Task<List<S3Object>> ListS3Archives()
        {
            var request = new ListObjectsRequest()
            {
                BucketName = Utils.ArchivesBucket,
                Prefix = Utils.SnapshotsDir
            };
            var response = await S3Client.ListObjectsAsync(request);
            var sortedObjects = response
                .S3Objects
                .Where(x => x.Key.ToString().EndsWith(".zip"))
                .OrderByDescending(x => x.LastModified)
                .ToList();

            var databaseArchives = sortedObjects.Where(x => x.Key.Contains(Utils.Databases)).ToList();
            var userdataArchives = sortedObjects.Where(x => x.Key.Contains(Utils.UserData)).ToList();

            if (databaseArchives.Count != userdataArchives.Count)
            {
                _logger.LogCritical("Number of ");
            }

            var dbArchiveDelList = new List<S3Object>();
            var userDataArchiveDelList = new List<S3Object>();

            for (var index = 0; index < databaseArchives.Count; index++)
            {
                var dbObject = databaseArchives[index];
                if (index >= Utils.MinNumArchives)
                {
                    dbArchiveDelList.Add(dbObject);
                }
            }

            for (var index = 0; index < userdataArchives.Count; index++)
            {
                {
                    var userObject = userdataArchives[index];
                    if (index >= Utils.MinNumArchives)
                    {
                        userDataArchiveDelList.Add(userObject);
                    }
                }
            }

            var objectsToRemove = new List<S3Object>();
            objectsToRemove.AddRange(userDataArchiveDelList);
            objectsToRemove.AddRange(dbArchiveDelList);

            return objectsToRemove;
        }
    }
}