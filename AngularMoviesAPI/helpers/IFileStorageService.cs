using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.helpers
{
    public interface IFileStorageService
    {
        Task deleteFile(string fileRoute, string containerName);
        Task<string> saveFile(string containerName, IFormFile file);
        Task<string> editFile(string containerName, IFormFile file, string fileRoute);
    }
}
