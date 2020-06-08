using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ThirdApp.Helpers
{
    public class ImageFilesManager : IImageFilesManager
    {

        public List<string> GetAllImagesFromDirectory(string imagesDir)
        {
            var imagesAsStr = new List<string>();

            var theFiles = Directory.GetFiles(imagesDir, "*.png");
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

    }
}