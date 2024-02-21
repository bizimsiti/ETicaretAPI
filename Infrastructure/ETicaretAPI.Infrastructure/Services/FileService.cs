using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    internal class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try { 
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None,1024 * 1024, useAsync:false);
                return true;
            }
            catch(Exception ex) {

                throw ex;
            }


        }

        async Task<string> FileRenameAsync(string path, string fileName)
        {


             string newFileName =await Task.Run<string>(() =>
            {
                string extention = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string today = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_fff");
                return $"{NameOperation.NameEditor(oldName)}-{today}{extention}";
            });
         
            
            return  newFileName;
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath), path);
            if(!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            List<bool> results = new();

            List<(string fileName, string path)> datas = new();
            foreach(IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath,file.FileName);
                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\{fileNewName}"));
                results.Add(result);
            }
            if(results.TrueForAll(r => r.Equals(true))) {
                return datas;
            }
            return null;
        }
    }
}
