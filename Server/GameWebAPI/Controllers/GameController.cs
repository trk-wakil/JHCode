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
            var result = _gameManager.FlipCard(cardId);

            return result;
        }



        //Note: EndGame request from client always means player ended the game. The win condition is determined here.
        [Route("api/Game/EndGame/")]
        [HttpGet]
        public PlayerRecord EndGame()
        {
            var playerRecord = _dbManager.GetPlayerRecord();
            playerRecord.gamesPlayed++;

            _gameManager.EndGame();

            _dbManager.StorePlayerRecord(playerRecord);

            return playerRecord;

        }



    }
}
