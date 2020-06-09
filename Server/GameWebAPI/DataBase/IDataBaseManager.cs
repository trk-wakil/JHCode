using GameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWebAPI.DataBase
{
    public interface IDataBaseManager
    {

        GameState getActiveGame();

        void StoreActiveGame(GameState gameState);

        void DeleteActiveGame();

        PlayerRecord GetPlayerRecord();

        void StorePlayerRecord(PlayerRecord playerRecord);
    }
}
