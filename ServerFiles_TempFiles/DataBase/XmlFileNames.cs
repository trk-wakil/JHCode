using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThirdApp.DataBase
{
    public static class XmlFileNames
    {
        public static string ActiveGameXMLFile = HttpContext.Current.Server.MapPath("~") + "ActiveGame.xml";
        public static string PlayerRecordXMLFile = HttpContext.Current.Server.MapPath("~") + "PlayerRecord.xml";
        public static string AllImagesXMLFile = HttpContext.Current.Server.MapPath("~") + "AllImages.xml";

        public static string AllImagesDirectory = HttpContext.Current.Server.MapPath("~") + "AllImages";
    }
}