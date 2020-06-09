using GameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebAPI.Helpers
{
    public class GameManager : IGameManager
    {

        Random _rnd = new Random();


        public GameState CreateNewGame(int numOfCards =0)
        {
            var imagesStr = ImageFilesHandler.GetAllImagesAsStr();

            if (numOfCards != 0)
                imagesStr = imagesStr.OrderBy(x => Guid.NewGuid()).Take(numOfCards).ToList();

            var fullDeck = BuildFullDeck(imagesStr);

            var newGame = new GameState
            {
                cards = fullDeck
            };


            return newGame;
        }


        public void FlipCard(int cardId)
        {
            throw new NotImplementedException();
        }


        public int GetMaxNumOfCards()
        {
            return ImageFilesHandler.GetCountOfAllImages();
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
            var foundIt = false;
            int theId = 0;

            while (!foundIt)
            {
                theId = _rnd.Next(maxId);
                foundIt = (!cards.Exists(x => x.id == theId));
            }

            return theId;

        }
    }
}