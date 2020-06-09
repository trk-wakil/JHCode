using GameWebAPI.DataBase;
using GameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameWebAPI.Helpers
{
    public class GameManager : IGameManager
    {

        private IDataBaseManager _dbManager;

        public GameManager(IDataBaseManager dataBaseManager)
        {
            _dbManager = dataBaseManager;
        }




        //TODO this replaces CreateNewGame
        public ActiveGameCards SetupNewGame(int numOfCards)
        {
            var imagesStr = ImageFilesHandler.GetAllImagesAsStr();
            numOfCards = (numOfCards == 0 ? imagesStr.Count : numOfCards);
            imagesStr = imagesStr.OrderBy(x => Guid.NewGuid()).Take(numOfCards).ToList();
            var fullDeck = DoubleTheCards(imagesStr);

            var activeGameCards = new ActiveGameCards { cards = fullDeck };

            _dbManager.StoreActiveGameCards(activeGameCards);

            var playerRecord = _dbManager.GetPlayerRecord();
            playerRecord.hasActiveGame = true;
            _dbManager.StorePlayerRecord(playerRecord);

            return activeGameCards;
        }



        public GameState CreateNewGame(int numOfCards =0)
        {
            var imagesStr = ImageFilesHandler.GetAllImagesAsStr();
            numOfCards = (numOfCards == 0 ? imagesStr.Count : numOfCards);
            imagesStr = imagesStr.OrderBy(x => Guid.NewGuid()).Take(numOfCards).ToList();
            var fullDeck = DoubleTheCards(imagesStr);

            var newGame = new GameState
            {
                cards = fullDeck
            };

            return newGame;
        }


        public bool FlipCard(int cardId)
        {
            var results = false;

            var activeGameCards = _dbManager.GetActiveGameCards();
            var playerRecord = _dbManager.GetPlayerRecord();
            playerRecord.currentNumOfFlips++;

            var lastFlippedCardId = playerRecord.lastFlippedCardId;
            var flippedCard = activeGameCards.cards.Find(x => x.id == cardId);

            //No flipped card to compare against
            if (lastFlippedCardId == -1)
            {
                flippedCard.flipped = true;
                playerRecord.lastFlippedCardId = flippedCard.id;
                results = true;
            }
            else
            {
                var lastFlippedCard = activeGameCards.cards.Find(x => x.id == lastFlippedCardId);
                playerRecord.lastFlippedCardId = -1;
                //found a match
                if (lastFlippedCard.img.Equals(flippedCard.img))
                {
                    flippedCard.flipped = true;
                    lastFlippedCard.flipped = true;
                    results = true;
                }
                //No match
                else
                {
                    flippedCard.flipped = false;
                    lastFlippedCard.flipped = false;
                    results = false;
                }
            }

            _dbManager.StorePlayerRecord(playerRecord);
            _dbManager.StoreActiveGameCards(activeGameCards);

            if (results)
            {
                //check for win
                if (!activeGameCards.cards.Any(x => x.flipped))
                {
                    //end game with a win
                    EndGame(true);
                }
            }

            return results;
        }


        public int GetMaxNumOfCards()
        {
            return ImageFilesHandler.GetCountOfAllImages();
        }


        public void EndGame(bool win = false)
        {
            var currentActiveGame = _dbManager.getActiveGame();
            var playerRecord = _dbManager.GetPlayerRecord();

            if (win)
            {
                playerRecord.gamesPlayed++;
                playerRecord.gamesWon++;

                if (playerRecord.bestScore > currentActiveGame.currentNumOfFlips)
                {
                    playerRecord.bestScore = currentActiveGame.currentNumOfFlips;
                    playerRecord.bestScoreNumberOfCards = currentActiveGame.cards.Count;
                }                
            }

            _dbManager.StorePlayerRecord(playerRecord);
            _dbManager.DeleteActiveGame();
        }




        private List<Card> DoubleTheCards(List<string> cardImages)
        {
            var gameCards = new List<Card>();
            int anId = 0;

            foreach (var c in cardImages)
            {
                gameCards.Add(new Card { id = anId++, img = c });
                gameCards.Add(new Card { id = anId++, img = c });
            }

            gameCards = gameCards.OrderBy(x => Guid.NewGuid()).Take(gameCards.Count).ToList();
            return gameCards;
        }



        private List<Card> BuildFullDeck(List<string> cardImages)
        {
            //TODO check this number
            int maxId = (cardImages.Count * 2) -1;

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
            var _rnd = new Random();
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