using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThirdApp.Models;

namespace ThirdApp.DataBase
{
    public interface IDbManager
    {

        GameState getActiveGame();

        void StoreActiveGame(GameState gameState);

        PlayerRecord GetPlayerRecord();

        void StorePlayerRecord(PlayerRecord playerRecord);



    }
}