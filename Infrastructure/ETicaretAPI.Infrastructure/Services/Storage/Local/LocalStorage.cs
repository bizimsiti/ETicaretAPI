﻿using ETicaretAPI.Application.Abstractions.Storage.Local;
using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteAsync(string path, string fileName)
        {
            File.Delete($"{path}\\{fileName}");
        }

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f=>f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
            =>File.Exists($"{path}\\{fileName}");
        
        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        async Task<string> FileRenameAsync(string path, string fileName)
        {


            string newFileName = await Task.Run<string>(() =>
            {
                string extention = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string today = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_fff");
                return $"{NameOperation.NameEditor(oldName)}-{today}{extention}";
            });


            return newFileName;
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath), path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                string newFileName =await FileRenameAsync(path, file.Name);
                await CopyFileAsync($"{uploadPath}\\{newFileName}", file);
                datas.Add((newFileName, $"{path}\\{newFileName}"));
            }
            
            return datas;
        }
    }
}
