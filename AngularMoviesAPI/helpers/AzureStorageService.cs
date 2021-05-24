using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.helpers
{
    public class AzureStorageService : IFileStorageService
    {
        // constructor here to input the azure account
        private string connectionString;
        public AzureStorageService(IConfiguration config)
        {
            connectionString = config.GetConnectionString("AzureStorageConnection");
        }
        // fileRoute here is the file absulate path
        public async Task deleteFile(string fileRoute, string containerName)
        {
            if (fileRoute == null || fileRoute.Length <= 0)
            {
                return;
            }
            var client = new BlobContainerClient(connectionString, containerName);
            await client.CreateIfNotExistsAsync();

            var filename = Path.GetFileName(fileRoute);
            var blob = client.GetBlobClient(filename);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> editFile(string containerName, IFormFile file, string fileRoute)
        {
            await deleteFile(fileRoute, containerName);
            return await saveFile(containerName, file);
        }

        public async Task<string> saveFile(string containerName, IFormFile file)
        {
            // using Azure.Storage.Blobs
            var client = new BlobContainerClient(connectionString,containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            // File.IO, get selected picture's extension like jpg etc
            var extension = Path.GetExtension(file.FileName);   // file extension
            var filename = $"{Guid.NewGuid()}{extension}";      // file name
            var blob = client.GetBlobClient(filename);          // open a Azure blob storage client
            await blob.UploadAsync(file.OpenReadStream());      // start uploading file
            return blob.Uri.ToString();
        }
    }
}
