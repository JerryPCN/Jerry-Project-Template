using System;
using System.IO;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public static class FileHelper
    {
        public static bool IsValid(this HttpPostedFileBase file)
        {
            return true;
        }

        public static string SaveTo(this HttpPostedFileBase file, string strCategory = "")
        {
            if (file == null)
            {
                return "";
            }

            if (string.IsNullOrEmpty(strCategory))
            {
                strCategory = "Upload/" + DateTime.Now.ToString("yyyy-MM-dd");
            }

            string strFilePath = "/File/" + strCategory;
            string strMapPath = strFilePath.ToMapPath();
            strMapPath.Create();
            file.SaveAs(strMapPath + "/" + file.FileName);
            return strFilePath + "/" + file.FileName;
        }

        public static void Create(this string strPath)
        {
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
        }

        public static string ToMapPath(this string strPath)
        {
            return HttpContext.Current.Server.MapPath(strPath);
        }
    }
}