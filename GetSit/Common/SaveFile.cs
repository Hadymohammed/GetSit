using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GetSit.Common
{
    public class SaveFile
    {
        public static async Task<string> HallPhoto(IFormFile file,string SpaceName,int hallId,int PhotoNumber)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    var fileName =SpaceName+"_"+hallId.ToString()+"_"+PhotoNumber.ToString()+ext;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\resources\\HallPhotos", fileName);//NeedCompletePath
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileSrteam);
                    }
                    return ".\\resources\\HallPhotos\\"+fileName;
                }
                return null;
            }catch(Exception e)
            {
                return null;
            }
            return null;
        }
        public static async Task<string> ServicePhoto(IFormFile file, string SpaceName, int ServiceId, int PhotoNumber)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    var fileName = SpaceName + "_" + ServiceId.ToString() + "_" + PhotoNumber.ToString() + ext;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\resources\\ServicePhotos", fileName);//NeedCompletePath
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileSrteam);
                    }
                    return ".\\resources\\ServicePhotos\\" + fileName;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return true;
                }
            }
            catch (Exception ex)
            {
               return false;
            }
            return false;
        }
    }
}
