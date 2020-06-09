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

        void DeleteActiveGame();

        PlayerRecord GetPlayerRecord();

        void StorePlayerRecord(PlayerRecord playerRecord);


        //TODO ActiveGameCards logic will replace GameState
        ActiveGameCards GetActiveGameCards();


        void StoreActiveGameCards(ActiveGameCards activeGameCards);

    }
}
