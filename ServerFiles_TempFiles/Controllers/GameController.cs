
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ThirdApp.DataBase;
using ThirdApp.Helpers;
using ThirdApp.Models;

namespace ThirdApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "*")]
    public class GameController : ApiController
    {

        IDbManager _dbManager;
        IGameManager _gameManager;
        

        public GameController(IDbManager dbManager, IGameManager gameManager)
        {
            _dbManager = dbManager;
            _gameManager = gameManager;

        }


        
        [Route("api/Game/GetCardCount/")]
        [HttpGet]
        public int GetCardCount()
        {
            var numOfPlayableCards = _gameManager.GetMaxNumOfCards();
            return numOfPlayableCards;
        }


        //TODO will revisit this one later
        [Route("api/Game/GetInitialSettings/")]
        [HttpGet]
        public string GetInitialSettings()
        {
            var numOfPlayableCards = _gameManager.GetMaxNumOfCards();
            var playerRecord = _dbManager.GetPlayerRecord();
            var gameState = _dbManager.getActiveGame();

            
            var results = JsonConvert.SerializeObject(new
            {
                playerRecord,
                //gameState,
                numOfPlayableCards
            });

            return results;
        }




        [Route("api/Game/GetPlayerRecord/")]
        [HttpGet]
        public PlayerRecord GetPlayerRecord()
        {
            var playerRecord = _dbManager.GetPlayerRecord();
            return playerRecord;
        }


        [Route("api/Game/GetCurrentGame/")]
        [HttpGet]
        public GameState GetCurrentGame()
        {
            var gameState = _dbManager.getActiveGame();
            return gameState;
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


        //[Route("api/Game/StartNewGame/")]
        //[HttpGet]
        //public List<Card> StartNewGame(int numOfCards, string playerName)
        //{
        //    if (string.IsNullOrEmpty(playerName))
        //    {
        //        var results = new List<string>();

        //        var newActiveGame = _gameManager.CreateNewGame(numOfCards);
        //        _dbManager.StoreActiveGame(newActiveGame);

        //        newActiveGame.cards.ForEach((c) => { results.Add(JsonConvert.SerializeObject(c)); });

        //        return (newActiveGame.cards);
        //    }
        //}




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
            if(lastFlippedCardId == -1)
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





        [Route("api/Game/EndGame/")]
        [HttpGet]
        public PlayerRecord EndGame()
        {

            var playerRecord = _dbManager.GetPlayerRecord();
            playerRecord.totalGamesPlayedCount++;

            var activeGame = _dbManager.getActiveGame();

            //find out if this was a win game or not
            if (activeGame.cards.TrueForAll(x => x.flipped))
            {
                playerRecord.totalGamesWonCount++;
                if (activeGame.currentNumOfFlips < playerRecord.bestScore)
                {
                    playerRecord.bestScore = activeGame.currentNumOfFlips;
                    playerRecord.bestScoreNumberOfCards = activeGame.cards.Count;
                }
            }

            _dbManager.StorePlayerRecord(playerRecord);
            _dbManager.StoreActiveGame(null);

            return playerRecord;

        }




        // GET: api/Game/5
        [Route("api/Game/TestInputVal/")]
        [HttpPost]
        public string TestInputVal(int numOfCards)
        {
            return "Some Result";
        }

        // POST: api/Game
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Game/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Game/5
        public void Delete(int id)
        {
        }
    }
}
