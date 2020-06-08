using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using ThirdApp.Models;

namespace ThirdApp.Helpers
{
    public class GameManager : IGameManager
    {
        public GameState CreateNewGame(int numOfCards)
        {
            var newGame = new GameState();

            var filesHelper = new FilesManager();
            var imagesStr = filesHelper.GetImagesAsStr(numOfCards);

            //TODO don't like this call done this way here
            newGame.cards = BuildFullDeck(imagesStr);


            return newGame;
        }

        public int GetMaxNumOfCards()
        {
            var filesHelper = new FilesManager();
            var imagesStr = filesHelper.GetImagesAsStr();
            return imagesStr.Count;
        }

        private List<Card> BuildFullDeck(List<string> cardImages)
        {
            //TODO check this number
            int maxId = cardImages.Count * 2;

            var gameCards = new List<Card>();

            foreach (var c in cardImages)
            {   
                //TODO undo repitions
                var anId = FindEmptyId(gameCards, maxId);
                gameCards.Add(new Card { id = anId, img = c });

                //Insert should add a NEW card and not the same c
                anId = FindEmptyId(gameCards, maxId);
                gameCards.Add(new Card { id = anId, img = c });
            }

            gameCards.OrderBy(x => x.id);
            return gameCards;
        }


        private int FindEmptyId(List<Card> cards, int maxId)
        {
            var rnd = new Random();
            var foundIt = false;
            int theId = 0;

            while (!foundIt)
            {
                theId = rnd.Next(maxId);
                foundIt = (!cards.Exists(x => x.id == theId));
            }

            return theId;

        }


    }
}