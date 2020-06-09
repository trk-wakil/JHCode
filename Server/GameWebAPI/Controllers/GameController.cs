using GameWebAPI.DataBase;
using GameWebAPI.Helpers;
using GameWebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GameWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "*")]
    public class GameController : ApiController
    {

        IDataBaseManager _dbManager;
        IGameManager _gameManager;


        public GameController(IDataBaseManager dbManager, IGameManager gameManager)
        {
            _dbManager = dbManager;
            _gameManager = gameManager;

        }


        
        [Route("api/Game/GetInitialData/")]
        [HttpGet]
        public GameData GetInitialSettings()
        {
            var numOfPlayableCards = _gameManager.GetMaxNumOfCards();
            var playerRecord = _dbManager.GetPlayerRecord();
            var gameState = _dbManager.getActiveGame();

            var settings = new GameData();
            settings.playerRecord = playerRecord;
            settings.gameState = gameState;
            settings.numOfPlayableCards = numOfPlayableCards;

            return settings;

        }




        
        [Route("api/Game/GetCardsForNewGame/{numOfCards:int}")]
        [HttpGet]
        public List<Card> StartNewGame(int numOfCards)
        {
            var results = new List<string>();

            var newActiveGame = _gameManager.CreateNewGame(numOfCards);
            _dbManager.StoreActiveGame(newActiveGame);

            newActiveGame.cards.ForEach((c) => { results.Add(JsonConvert.SerializeObject(c)); });

            return (newActiveGame.cards);
        }



        [Route("api/Game/FlipCard/{cardId:int}")]
        [HttpGet]
        public bool FlipCard(int cardId)
        {
            var results = false;

            var activeGame = _dbManager.getActiveGame();
            activeGame.currentNumOfFlips++;

            var lastFlippedCardId = activeGame.lastFlippedCardId;
            var flippedCard = activeGame.cards.Find(x => x.id == cardId);

            //No flipped card to compare against
            if (lastFlippedCardId == -1)
            {
                flippedCard.flipped = true;
                activeGame.lastFlippedCardId = flippedCard.id;
                results = true;
            }
            else
            {
                var lastFlippedCard = activeGame.cards.Find(x => x.id == lastFlippedCardId);
                activeGame.lastFlippedCardId = -1;
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

            _dbManager.StoreActiveGame(activeGame);


            return results;

        }




    }
}
