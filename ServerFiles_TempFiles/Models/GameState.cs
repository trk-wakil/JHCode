using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThirdApp.Models
{
    public class GameState
    {

        public int currentNumOfFlips = 0;
        public int lastFlippedCardId = -1;
        public List<Card> cards = new List<Card>();

        public int countOfFlippedCards = 0;




    }
}