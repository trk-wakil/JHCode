using GameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebAPI.Helpers
{
    public interface IGameManager
    {
        GameState CreateNewGame(int numOfCards);

        int GetMaxNumOfCards();

        bool FlipCard(int cardId);

        void EndGame(bool win=false);
    }
}