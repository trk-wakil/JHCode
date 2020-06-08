using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdApp.Helpers
{
    public interface IImageFilesManager
    {
        List<string> GetAllImagesFromDirectory(string imagesDir);
    }
}
