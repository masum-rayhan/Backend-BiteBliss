﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.IRepo.Services;

public interface IBlobService
{
    Task<string> GetBlob(string blobName, string containerName);
    Task<string> UploadBlob(string blobName, string containerName, IFormFile file);
    Task<bool> DeleteBlob(string blobName, string containerName);
}
