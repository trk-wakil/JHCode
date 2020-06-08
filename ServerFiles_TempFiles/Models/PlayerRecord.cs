using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThirdApp.Models
{
    public class PlayerRecord
    {
        public string playerId = "";
        public int bestScore = 0;
        public int bestScoreNumberOfCards = 0;
        public int totalGamesPlayedCount = 0;
        public int totalGamesWonCount = 0;

        public GameState currentActiveGame = null;
    }
}