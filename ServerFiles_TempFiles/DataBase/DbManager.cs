using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ThirdApp.Helpers;
using ThirdApp.Models;

namespace ThirdApp.DataBase
{
    public class DbManager : IDbManager
    {

        private IImageFilesManager _imageFilesManager;

        public DbManager(IImageFilesManager imageFilesManager)
        {

            //TODO.. let's think about this one later
            _imageFilesManager = imageFilesManager;
            BuildImagesDb();

        }



        public GameState getActiveGame()
        {
            var gameState = new GameState();
            var xmlSerializer = new XmlSerializer(typeof(GameState));

            if (!File.Exists(XmlFileNames.ActiveGameXMLFile))
            {
                return null;
            }
            else
            {
                using (FileStream fs = new FileStream(XmlFileNames.ActiveGameXMLFile, FileMode.Open, FileAccess.Read))
                {
                    gameState = xmlSerializer.Deserialize(fs) as GameState;
                }
            }

            return gameState;
        }


        public void StoreActiveGame(GameState gameState)
        {
            var xmlSerializer = new XmlSerializer(typeof(GameState));

            using (FileStream fs = new FileStream(XmlFileNames.ActiveGameXMLFile, FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(fs, gameState);
            }
        }



        public PlayerRecord GetPlayerRecord()
        {
            var playerRecord = new PlayerRecord();
            var xmlSerializer = new XmlSerializer(typeof(PlayerRecord));

            if (!File.Exists(XmlFileNames.PlayerRecordXMLFile))
            {
                return null;
            }
            else
            {
                using (FileStream fs = new FileStream(XmlFileNames.PlayerRecordXMLFile, FileMode.Open, FileAccess.Read))
                {
                    playerRecord = xmlSerializer.Deserialize(fs) as PlayerRecord;
                }
            }

            return playerRecord;
        }



        public void StorePlayerRecord(PlayerRecord playerRecord)
        {
            var xmlSerializer = new XmlSerializer(typeof(PlayerRecord));

            using (FileStream fs = new FileStream(XmlFileNames.PlayerRecordXMLFile, FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(fs, playerRecord);
            }
        }



        private void BuildImagesDb()
        {
            List<string> allImagesStr = _imageFilesManager.GetAllImagesFromDirectory(XmlFileNames.AllImagesDirectory);

            if (!allImagesStr.Any()) return;

            var xmlSerializer = new XmlSerializer(typeof(Card));

            using (FileStream fs = new FileStream(XmlFileNames.AllImagesXMLFile, FileMode.Create, FileAccess.Write))
            {
                int i = 0;
                foreach(var image in allImagesStr)
                {
                    xmlSerializer.Serialize(fs, new Card { id = i++, flipped= false, img= image });
                }
            }
        }
    }
}