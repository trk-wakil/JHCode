using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebAPI.Models
{
    public class PlayerRecord
    {

        public int bestScore = 0;
        public int bestScoreNumberOfCards = 0;
        public int gamesPlayed = 0;
        public int gamesWon = 0;

        public bool hasActiveGame = false;
        public int currentNumOfFlips = 0;
        public int lastFlippedCardId = -1;
    }
}