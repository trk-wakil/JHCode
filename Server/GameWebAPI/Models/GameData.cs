using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebAPI.Models
{
    public class GameData
    {
        public PlayerRecord playerRecord = new PlayerRecord();
        public GameState gameState = new GameState();
        public int numOfPlayableCards;
    }
}