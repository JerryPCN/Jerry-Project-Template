using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace JerryPlat.Utils.Helpers
{
    public static class QrCodeHelper
    {
        public static Bitmap Create(string strContent, int pixelsPerModule = 9, string strIconPath = "")
        {
            Bitmap icon = string.IsNullOrEmpty(strIconPath) ? null : GetBitmap(strIconPath);
            return Create(strContent, pixelsPerModule, icon);
        }

        public static Bitmap Create(string strContent, int pixelsPerModule = 9, Bitmap icon = null)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(strContent, QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(pixelsPerModule, Color.Black, Color.White, icon);
            return qrCodeImage;
        }

        public static Bitmap GetBitmap(string strImagePath)
        {
            if (strImagePath.Contains("http"))
            {
                return GetBitmapFromNet(strImagePath);
            }

            return GetBitmapFromLocal(strImagePath);
        }

        /**
         * @param uri：图片的本地url地址
         * @return Bitmap；
         */

        public static Bitmap GetBitmapFromLocal(string strImagePath)
        {
            if (string.IsNullOrEmpty(strImagePath))
            {
                return null;
            }

            if (!strImagePath.Contains(":"))
            {
                strImagePath = strImagePath.ToMapPath();
            }

            return new Bitmap(strImagePath);
        }

        /**
         * @param uri：图片的本地url地址
         * @return Bitmap；
         */

        public static Bitmap GetBitmapFromNet(string strImageUrl)
        {
            try
            {
                using (Stream stream = HttpHelper.GetStream(strImageUrl))
                {
                    return new Bitmap(stream);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(aPhotoUrl + "获取失败");
                return null;
            }
        }
    }
}