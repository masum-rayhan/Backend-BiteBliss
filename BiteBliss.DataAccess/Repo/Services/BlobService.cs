using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BiteBliss.DataAcces.Repo.IRepo.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.Services;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    public async Task<string> GetBlob(string blobName, string containerName)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        return await Task.FromResult(blobClient.Uri.AbsoluteUri);
    }

    public async Task<string> UploadBlob(string blobName, string containerName, IFormFile file)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = file.ContentType
        };

        var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
        if(result != null)
        {
            return await GetBlob(blobName, containerName);
        }
        return "";
    }

    public async Task<bool> DeleteBlob(string blobName, string containerName)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.DeleteIfExistsAsync();
    }
}
