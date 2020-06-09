using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace GameWebAPI.Helpers
{
    public class ImageFilesHandler
    {
        private static string _imagesLocation = HttpContext.Current.Server.MapPath("~") + "Images";


        public static List<string> GetAllImagesAsStr()
        {
            var imagesAsStr = new List<string>();

            var theFiles = Directory.GetFiles(_imagesLocation, "*.png");
            foreach (var f in theFiles)
            {
                using (Image image = Image.FromFile(f))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        var base64String = Convert.ToBase64String(imageBytes);
                        imagesAsStr.Add(base64String);
                    }
                }
            }

            return imagesAsStr;
        }

        public static int GetCountOfAllImages()
        {
            return Directory.GetFiles(_imagesLocation, "*.png").Length;
        }
    }
}