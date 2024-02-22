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
    internal class FileService
    {


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
    }
}
