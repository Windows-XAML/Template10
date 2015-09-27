//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Template10.Services.Azure.BlobService
//{
//    public class BlobHelper
//    {
//        public Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient CloudBlobClient { get; private set; }

//        public AzureBlobHelper(string accountName, string accessKey, string groupName, bool useHttps = false)
//        {
//            var credentials = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, accessKey);
//            var storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(credentials, useHttps);
//            this.CloudBlobClient = storageAccount.CreateCloudBlobClient();
//            this.Container = new ContainerLogic(this, groupName);
//            this.Blob = new BlobLogic(this, groupName);
//        }

//        private Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer GetContainer(string containerName)
//        {
//            // cannot be empty
//            if (string.IsNullOrWhiteSpace(containerName))
//                throw new ArgumentNullException("containerName cannot be empty");
//            // must all be lower case
//            if (containerName != containerName.ToLower())
//                throw new ArgumentNullException("containerName must be lower case");
//            // first letter must be number or letter
//            if (!System.Text.RegularExpressions.Regex.IsMatch(containerName, @"[\w\d]"))
//                throw new ArgumentNullException("containerName must start with a number or letter");
//            // only letter, hyphen, number
//            if (!System.Text.RegularExpressions.Regex.IsMatch(containerName, @"^[-\w\d]"))
//                throw new ArgumentNullException("containerName must only contain numbers, letters, and hyphens");
//            return CloudBlobClient.GetContainerReference(containerName);
//        }

//        public ContainerLogic Container { get; private set; }
//        public class ContainerLogic
//        {
//            private AzureBlobHelper Parent { get; set; }
//            private string _groupName;
//            public ContainerLogic(AzureBlobHelper parent, string groupName)
//            {
//                this.Parent = parent;
//                this._groupName = groupName;
//            }

//            public async Task<Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer> GetAsync(string containerName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                try { await container.CreateIfNotExistsAsync(Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Container, new Microsoft.WindowsAzure.Storage.Blob.BlobRequestOptions { }, new Microsoft.WindowsAzure.Storage.OperationContext { }); }
//                catch { System.Diagnostics.Debugger.Break(); }
//                return container;
//            }

//            public async Task<bool> ExistsAsync(string containerName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                return await container.ExistsAsync();
//            }

//            public async Task DeleteAsync(string containerName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                await container.DeleteIfExistsAsync();
//            }

//            public Uri GetSas(Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions permission, TimeSpan expiry, string containerName)
//            {
//                var policy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy
//                {
//                    Permissions = permission,
//                    SharedAccessExpiryTime = DateTime.UtcNow.Add(expiry)
//                };
//                var container = this.Parent.GetContainer(containerName);
//                var signature = container.GetSharedAccessSignature(policy);
//                return new Uri(System.IO.Path.Combine(container.Uri.ToString(), signature));
//            }

//            public async Task<IEnumerable<Microsoft.WindowsAzure.Storage.Blob.IListBlobItem>> ListBlobsAsync(string containerName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                var results = new List<Microsoft.WindowsAzure.Storage.Blob.IListBlobItem>();
//                var continuationToken = default(Microsoft.WindowsAzure.Storage.Blob.BlobContinuationToken);
//                do
//                {
//                    var response = await container.ListBlobsSegmentedAsync(continuationToken);
//                    continuationToken = response.ContinuationToken;
//                    results.AddRange(response.Results);
//                } while (continuationToken != null);
//                return results;
//            }
//        }

//        public BlobLogic Blob { get; private set; }
//        public class BlobLogic
//        {
//            private AzureBlobHelper Parent { get; set; }
//            private string _groupName;
//            public BlobLogic(AzureBlobHelper parent, string groupName)
//            {
//                this.Parent = parent;
//                this._groupName = groupName;
//            }

//            /*
//                Read
//                Container: Not applicable on blob container.
//                Blob: Gives the ability to read the contents of the blob. Also gives the ability to read the metadata and properties of the blob as well.
 
//                Write
//                Container: Gives the ability to upload one or more blobs in blob container. Also gives the ability to update properties, metadata and create snapshots of a blob.
//                Blob: Give the ability to upload a new blob (by the same name) or overwrite an existing blob (by the same name). Also gives the ability to update properties, metadata and create snapshots of a blob.

//                List
//                Container: Lists the blobs in the blob container.
//                Blob: Not applicable on blobs.
 
//                Delete
//                Container: Not applicable on blob container.
//                Blob: Gives the ability to delete the blob.
//            */
//            public Uri GetSas(Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions permission, TimeSpan expiry, string containerName, string blobName)
//            {
//                var policy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy
//                {
//                    Permissions = permission,
//                    SharedAccessExpiryTime = null
//                };
//                var container = this.Parent.GetContainer(containerName);
//                var signature = container.GetSharedAccessSignature(policy);
//                var blob = container.GetBlockBlobReference(blobName);
//                return new Uri(System.IO.Path.Combine(blob.Uri.ToString(), signature));
//            }

//            public async Task UploadAsync(Windows.Storage.StorageFile sourceFile,
//                string containerName, string blobName)
//            {
//                var properties = await sourceFile.GetBasicPropertiesAsync();
//                if (properties.Size > (1024 * 1024 * 64))
//                    throw new Exception("File cannot be larger than 64MB");

//                var container = this.Parent.GetContainer(containerName);
//                var blob = container.GetBlockBlobReference(blobName);
//                await blob.UploadFromFileAsync(sourceFile);
//            }

//            public async Task<Windows.Networking.BackgroundTransfer.UploadOperation> UploadBackgroundAsync(Windows.Storage.StorageFile sourceFile,
//                string containerName, string blobName, Action<Windows.Networking.BackgroundTransfer.UploadOperation> progressCallback = null)
//            {
//                var properties = await sourceFile.GetBasicPropertiesAsync();
//                if (properties.Size > (1024 * 1024 * 64))
//                    throw new Exception("File cannot be larger than 64MB");

//                var permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Write;
//                var container = this.Parent.GetContainer(containerName);
//                var signature = this.GetSas(permissions, TimeSpan.FromMinutes(5), container.Name, blobName);
//                var uploader = new Windows.Networking.BackgroundTransfer.BackgroundUploader
//                {
//                    TransferGroup = Windows.Networking.BackgroundTransfer.BackgroundTransferGroup.CreateGroup(_groupName)
//                };
//                uploader.SetRequestHeader("Filename", sourceFile.Name);
//                var upload = uploader.CreateUpload(signature, sourceFile);
//                if (progressCallback == null)
//                    return await upload.StartAsync();
//                else
//                    return await upload.StartAsync().AsTask(new Progress<Windows.Networking.BackgroundTransfer.UploadOperation>(progressCallback));
//            }

//            public async Task DownloadAsync(Windows.Storage.StorageFile targetFile,
//                string containerName, string blobName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                var blob = container.GetBlockBlobReference(blobName);
//                await blob.DownloadToFileAsync(targetFile);
//            }

//            public async Task<Windows.Networking.BackgroundTransfer.DownloadOperation> DownloadBackgroundAsync(Windows.Storage.StorageFile targetFile,
//                string containerName, string blobName, Action<Windows.Networking.BackgroundTransfer.DownloadOperation> progressCallback = null)
//            {
//                var read = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read;
//                var container = this.Parent.GetContainer(containerName);
//                var signature = this.GetSas(read, TimeSpan.FromMinutes(5), container.Name, blobName);
//                var downloader = new Windows.Networking.BackgroundTransfer.BackgroundDownloader
//                {
//                    TransferGroup = Windows.Networking.BackgroundTransfer.BackgroundTransferGroup.CreateGroup(_groupName)
//                };
//                var download = await downloader.CreateDownloadAsync(signature, targetFile, null);
//                if (progressCallback == null)
//                    return await download.StartAsync();
//                else
//                    return await download.StartAsync().AsTask(new Progress<Windows.Networking.BackgroundTransfer.DownloadOperation>(progressCallback));
//            }

//            public async Task<bool> ExistsAsync(string containerName, string blobName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                var blob = container.GetBlockBlobReference(blobName);
//                return await blob.ExistsAsync();
//            }

//            public async Task DeleteAsync(string containerName, string blobName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                var blob = container.GetBlockBlobReference(blobName);
//                await blob.DeleteIfExistsAsync();
//            }

//            public async Task CopyAsync(string containerName, string blobName, string newName)
//            {
//                var container = this.Parent.GetContainer(containerName);
//                var blob = container.GetBlockBlobReference(blobName);
//                var newBlob = container.GetBlockBlobReference(newName);
//                await blob.StartCopyFromBlobAsync(blob);
//            }
//        }
//    }
//}