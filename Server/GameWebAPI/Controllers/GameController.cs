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

        private IDataBaseManager _dbManager;
        private IGameManager _gameManager;


        public GameController(IDataBaseManager dbManager, IGameManager gameManager)
        {
            _dbManager = dbManager;
            _gameManager = gameManager;

        }



        //EndGame request from client always means player ended the game. The win condition is determined here.
        [Route("api/Game/EndGame/")]
        [HttpGet]
        public PlayerRecord EndGame()
        {
            var playerRecord = _gameManager.EndGame();
            return playerRecord;
        }


        [Route("api/Game/GetPlayerRecord/")]
        [HttpGet]
        public PlayerRecord GetPlayerRecord()
        {
            var currentPlayerRecord = _dbManager.GetPlayerRecord();
            return currentPlayerRecord;
        }


        [Route("api/Game/GetMaxPlayableCards/")]
        [HttpGet]
        public int GetMaxPlayableCards()
        {
            var numOfPlayableCards = _gameManager.GetMaxNumOfCards();
            return numOfPlayableCards;
        }


        [Route("api/Game/SetupNewGame/{numOfCards:int}")]
        [HttpGet]
        public List<Card> SetupNewGame(int numOfCards)
        {
            var results = new List<string>();
            var newActiveGame = _gameManager.SetupNewGame(numOfCards);
            newActiveGame.cards.ForEach((c) => { results.Add(JsonConvert.SerializeObject(c)); });
            return (newActiveGame.cards);
        }


        [Route("api/Game/FlipCard/{cardId:int}")]
        [HttpGet]
        public bool FlipCard(int cardId)
        {
            var result = _gameManager.FlipCard(cardId);
            return result;
        }


        [Route("api/Game/GetCurrentGameCards/")]
        [HttpGet]
        public List<Card> GetCurrentGameCards()
        {
            var results = new List<string>();
            var currentGame = _dbManager.GetActiveGameCards();
            currentGame.cards.ForEach((c) => { results.Add(JsonConvert.SerializeObject(c)); });
            return (currentGame.cards);
        }



    }
}
