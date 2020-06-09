using GameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace GameWebAPI.DataBase
{
    public class DataBaseManager : IDataBaseManager
    {
        private static string ActiveGameXMLFile = HttpContext.Current.Server.MapPath("~") + "ActiveGame.xml";
        private static string PlayerRecordXMLFile = HttpContext.Current.Server.MapPath("~") + "PlayerRecord.xml";





        public GameState getActiveGame()
        {
            var gameState = new GameState();
            var xmlSerializer = new XmlSerializer(typeof(GameState));

            //first time ever
            if (!File.Exists(ActiveGameXMLFile))
            {
                //StoreActiveGame(gameState);
                return null;
            }
            else
            {
                using (FileStream fs = new FileStream(ActiveGameXMLFile, FileMode.Open, FileAccess.Read))
                {
                    gameState = xmlSerializer.Deserialize(fs) as GameState;
                }
            }

            return gameState;
        }


        public void StoreActiveGame(GameState gameState)
        {
            var xmlSerializer = new XmlSerializer(typeof(GameState));

            using (FileStream fs = new FileStream(ActiveGameXMLFile, FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(fs, gameState);
            }
        }


        public void DeleteActiveGame()
        {
            if (File.Exists(ActiveGameXMLFile))
            {
                File.Delete(ActiveGameXMLFile);
            }
        }



        public PlayerRecord GetPlayerRecord()
        {
            var playerRecord = new PlayerRecord();
            var xmlSerializer = new XmlSerializer(typeof(PlayerRecord));

            //First time ever
            if (!File.Exists(PlayerRecordXMLFile))
            {
                StorePlayerRecord(playerRecord);
            }
            else
            {
                using (FileStream fs = new FileStream(PlayerRecordXMLFile, FileMode.Open, FileAccess.Read))
                {
                    playerRecord = xmlSerializer.Deserialize(fs) as PlayerRecord;
                }
            }
            return playerRecord;
        }



        public void StorePlayerRecord(PlayerRecord playerRecord)
        {
            var xmlSerializer = new XmlSerializer(typeof(PlayerRecord));

            using (FileStream fs = new FileStream(PlayerRecordXMLFile, FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(fs, playerRecord);
            }
        }





    }
}