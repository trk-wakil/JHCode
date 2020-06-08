using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ThirdApp.Helpers
{
    public class FilesManager
    {



        //TODO handle Directory here
        private string imagesLocation = "C://Coding//JHChallange//Images//";

        public List<string> GetImagesAsStr(int numOfImages = 0)
        {
            var imagesAsStr = new List<string>();

            //TODO handle when numOfImages != 0

            var theFiles = Directory.GetFiles(imagesLocation, "*.png");
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