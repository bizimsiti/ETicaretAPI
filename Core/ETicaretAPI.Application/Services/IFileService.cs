
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services
{
    public interface IFileService
    {
        Task<bool> CopyFileAsync(string path,IFormFile file);
        Task<List<(string fileName, string path)>> UploadAsync(string path,IFormFileCollection files);
    }
}
