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
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\", filePath);
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
        public static async Task<string> SpaceLogo(IFormFile file, string SpaceName)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    var fileName = SpaceName + "_Logo"+ ext;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\resources\\SpacePhotos", fileName);//NeedCompletePath
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileSrteam);
                    }
                    return ".\\resources\\SpacePhotos\\" + fileName;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        public static async Task<string> SpaceCover(IFormFile file, string SpaceName)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    var fileName = SpaceName + "_Cover" + ext;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\resources\\SpacePhotos", fileName);//NeedCompletePath
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileSrteam);
                    }
                    return ".\\resources\\SpacePhotos\\" + fileName;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        public static async Task<bool> Save_FileAsync(IFormFile file,string filePath)
        {
            try
            {
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileSrteam);
                }
                return true;
            }
            catch(Exception err)
            {
                return false;
            }
            return false;
        }
        public static async Task<string> userPic(IFormFile file, int userId)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string ext = Path.GetExtension(file.FileName);
                    var fileName ="Profile_"+userId.ToString()+ ext;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\resources\\Profiles", fileName);//NeedCompletePath
                    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileSrteam);
                    }
                    return ".\\resources\\Profiles\\" + fileName;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

    }
}
