
class GameManager {
    constructor() {

        //TODO handle failed connection
        this.apiHelper = new ApiHelper();
        this.playerName = '';
        this.maxCountOfCards;
        this.currentActiveGame;
        this.numberOfAvailableCards;
        
    }


    async getMaxPlayableCards() {
        var result = await this.apiHelper.getMaxPlayableCardsFromServer();
        this.maxCountOfCards = result;
        document.getElementById('NumUniqueCards').setAttribute('max', this.maxCountOfCards);
        document.getElementById('NumUniqueCards').setAttribute('placeholder', this.maxCountOfCards);
    }


    async initGame() {
        var results = await this.apiHelper.getActiveGameFromServer();

        this.currentActiveGame = results;

        //if no current game exists, allow only StartNewGame
        if (!this.currentActiveGame) {
            console.log("NO GAME EXISTS");
            this.handleElementEnable('#NewGameDiv', true);
            this.handleElementEnable('#ResumeGameDiv', false);
            this.handleElementEnable('#EndGameDiv', false);
        }
        //else allow only ResumeGame or EndGame
        else {
            console.log("found current game");
            this.handleElementEnable('#NewGameDiv', false);
            this.handleElementEnable('#ResumeGameDiv', true);
            this.handleElementEnable('#EndGameDiv', true);
        }
        
    }


    resumeGame() {
        this.setupGridAndGame(this.currentActiveGame.cards);
    }


    
    async beginNewGameAsNoOne() {
        this.apiHelper.goFetchCards().then((data) => {
            this.setupGridAndGame(data);
        });

        this.handleElementEnable('#NewGameDiv', false);
        this.handleElementEnable('#ResumeGameDiv', false);
        this.handleElementEnable('#EndGameDiv', true);
    }


    async beginNewGame() {
        this.apiHelper.goFetchCards().then((data) => {
            this.setupGridAndGame(data);
        });

        this.handleElementEnable('#NewGameDiv', false);
        this.handleElementEnable('#ResumeGameDiv', false);
        this.handleElementEnable('#EndGameDiv', true);
    }

    //TODO handle respone
    async endGame() {
        this.apiHelper.endGame();
        var game_container = document.getElementsByClassName('game-container')[0];
        //game_container.innerHTML = "";

        this.handleElementEnable('#NewGameDiv', true);
        this.handleElementEnable('#ResumeGameDiv', false);
        this.handleElementEnable('#EndGameDiv', false);

    }

    
    setupGridAndGame(cardsFromServer) {

        this.fillGrid(cardsFromServer);

        let cards = Array.from(document.getElementsByClassName('card'));
        console.log("cards=  " + cards.length);
        this.currentActiveGame = new ActiveGameManager(this.playerName, cards, this.apiHelper);
        this.currentActiveGame.startGame();

        cards.forEach(card => {
            card.addEventListener('click', () => {
                this.currentActiveGame.flipCard(card);
            });
        });
    }

    
    fillGrid(data) {
        console.log(data[0]);

        var game_container = document.getElementsByClassName('game-container')[0];
        //game_container.innerHTML = "";

        for (var i=0; i < data.length; i++) {
            var cardData = data[i];
                
            var cardBack = document.createElement("div");
            cardBack.classList.add("card-back");
            cardBack.classList.add("card-face");

            var imgBack = document.createElement("img");
            imgBack.classList.add("spider");
            imgBack.src = "Assets/Spider.png";
            cardBack.appendChild(imgBack);
            
            var cardFront = document.createElement("div");
            cardFront.classList.add("card-front");
            cardFront.classList.add("card-face");

            var imgFront = document.createElement("img");
            imgFront.classList.add("card-value");
            imgFront.src = "data:image/png;base64," + cardData.img;
            cardFront.appendChild(imgFront);

            var newCard = document.createElement("div");
            newCard.setAttribute("id", cardData.id);
            newCard.classList.add("card");
            newCard.appendChild(cardBack);
            newCard.appendChild(cardFront);
            
            console.log("game_container=  " + game_container);
            game_container.appendChild(newCard);

        }
    }


    handleElementEnable(elementId, enable) {
        if (enable) {
            $(elementId).removeClass('disabledDiv');
        }
        else {
            $(elementId).addClass('disabledDiv');
        }
    }



}




class ApiHelper {
    constructor() {
        this.baseURL = 'https://localhost:44355/api/Game/';
        this.cardsFromServer;
        this.maxNumOfCardsAllowed = 0;
    }

    async getMaxPlayableCardsFromServer() {
        var uri = this.baseURL + 'GetCardCount';
        var data = await (await fetch(uri)).json();
        return data;
    }


    async getActiveGameFromServer() {
        var uri = this.baseURL + 'GetCurrentGame';
        var data = await (await fetch(uri)).json();
        return data;
    }

        
    async goFetchCards(numOfUniqueCards) {
        //TODO use the operation in fetch and allow using the variable
        var uri = this.baseURL + 'GetCardsForNewGame/0';
        var result = await (await fetch(uri)).json();
        return result;
    }

    
    sendFlipRequest(card) {
        var uri = this.baseURL + 'FlipCard/' + card.id;
        //Think about the response.. do we need it?
        //also, insert id in the fetch operation
        fetch(uri)
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                console.log('from Server:  ' +data);
            });
    }

    endGame() {
        //TODO consider asyn call here
        var uri = this.baseURL + 'EndGame';
        fetch(uri)
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                console.log('from Server:  ' +data);
            });
    }
}



class ActiveGameManager {
    constructor(playerName, cards, apiHelper) {
        this.playerName = playerName;
        this.cardsArray = cards;
        this.apiHelper = apiHelper;
        //TODO ticker is score. Maybe allow passing it here on resumed game
        this.ticker = document.getElementById('flips');
    }

    startGame() {
        this.totalClicks = 0;
        this.timeRemaining = this.totalTime;
        this.cardToCheck = null;
        this.matchedCards = [];
        this.hideCards();
        this.ticker.innerText = this.totalClicks;
    }
    victory() {
        clearInterval(this.countdown);
        document.getElementById('victory-text').classList.add('visible');
    }
    //TODO exclude previously uncovered card when hiding on resume game
    hideCards() {
        this.cardsArray.forEach(card => {
            card.classList.remove('visible');
            card.classList.remove('matched');
        });
    }
    flipCard(card) {
        //Do the matching here as well as in server side.
        //but first, alert server of flipped card
        if (this.playerName) {
            this.apiHelper.sendFlipRequest(card);
        }
        
        console.log("CLICKED");
        if(this.canFlipCard(card)) {
            this.totalClicks++;
            this.ticker.innerText = this.totalClicks;
            card.classList.add('visible');

            if(this.cardToCheck) {
                this.checkForCardMatch(card);
            } else {
                this.cardToCheck = card;
            }
        }
        else { console.log('canFlipCard = false'); }
    }
    checkForCardMatch(card) {
        if(this.getCardType(card) === this.getCardType(this.cardToCheck))
            this.cardMatch(card, this.cardToCheck);
        else 
            this.cardMismatch(card, this.cardToCheck);

        this.cardToCheck = null;
    }
    cardMatch(card1, card2) {
        this.matchedCards.push(card1);
        this.matchedCards.push(card2);
        card1.classList.add('matched');
        card2.classList.add('matched');
        if(this.matchedCards.length === this.cardsArray.length)
            this.victory();
    }
    cardMismatch(card1, card2) {
        this.busy = true;
        setTimeout(() => {
            card1.classList.remove('visible');
            card2.classList.remove('visible');
            this.busy = false;
        }, 1000);
    }
    //TODO not sure I need this
    shuffleCards(cardsArray) { // Fisher-Yates Shuffle Algorithm.
        for (let i = cardsArray.length - 1; i > 0; i--) {
            let randIndex = Math.floor(Math.random() * (i + 1));
            cardsArray[randIndex].style.order = i;
            cardsArray[i].style.order = randIndex;
        }
    }
    getCardType(card) {
        return card.getElementsByClassName('card-value')[0].src;
    }
    canFlipCard(card) {
        console.log(!this.busy);
        console.log(!this.matchedCards.includes(card));
        console.log(card !== this.cardToCheck);
        return !this.busy && !this.matchedCards.includes(card) && card !== this.cardToCheck;
    }
}









if (document.readyState == 'loading') {
    document.addEventListener('DOMContentLoaded', ready);
} else {
    ready();
}




async function ready() {

    let gameManager = new GameManager();
    await gameManager.getMaxPlayableCards();
    await gameManager.initGame();

    let overlays = Array.from(document.getElementsByClassName('overlay-text'));    
    overlays.forEach(overlay => {
        overlay.addEventListener('click', () => {
            overlay.classList.remove('visible');
        });
    });

    document.getElementById('PlayAsGuestBtn').addEventListener('click', () => { gameManager.beginNewGameAsNoOne(); });

    document.getElementById('PlayAsMeBtn').addEventListener('click', () => { gameManager.beginNewGame(); });
    document.getElementById('ResumeGameBtn').addEventListener('click', () => { gameManager.resumeGame(); });
    document.getElementById('EndGameBtn').addEventListener('click', () => { gameManager.endGame(); });

}



function MiscApiTest() {
    var uri = 'https://localhost:44355/api/Game/TestInputVal/';
    //var uri = 'https://localhost:44355/api/Game/GetCardCount/';
    //var uri = 'https://localhost:44355/api/Game/GetCardsForNewGame/';
    var requestData = {numOfCards: 3};
        
    var req = {
        method: 'POST',
        mode: 'no-cors', // no-cors, *cors, same-origin
        headers: {
          'Content-Type': 'application/json'   //,
          //'Content-Type': 'application/x-www-form-urlencoded'
        }  //,
        //body: JSON.stringify(requestData)
    };

    fetch(uri, req)
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                console.log('from Server:  ' +data);
            });

    // fetch(uri, req)
    //     .then((response) => {
    //         return response.json()
    //     })
    //     .then((output) => {
    //         console.log('from Server:  ' +output);
    //     });
            

}



function tryAjax() {
    var uri = 'https://localhost:44355/api/Game/TestInputVal/';
    var requestData = {numOfCards: 3};


    $.ajax({
            type: "POST",
            //crossdomain: true,
            headers: { 'Access-Control-Allow-Origin': '*' },
            url: uri,
            //dataType: "json",
            data: JSON.stringify(requestData),
            success: function (result) {
                console.log(result)
            },
            error: function (xhr, status, err) {
                console.error(xhr, status, err);
            }
        });
}


/*********
 * 
 * 
 


 $.ajax({
            type: "POST",
            crossdomain: true,
            url: "http://localhost:1415/anything",
            dataType: "json",
            data: JSON.stringify({
                anydata1: "any1",
                anydata2: "any2",
            }),
            success: function (result) {
                console.log(result)
            },
            error: function (xhr, status, err) {
                console.error(xhr, status, err);
            }
        });




// Example POST method implementation:
async function postData(url = '', data = {}) {
    // Default options are marked with *
    const response = await fetch(url, {
      method: 'POST', // *GET, POST, PUT, DELETE, etc.
      mode: 'cors', // no-cors, *cors, same-origin
      cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
      credentials: 'same-origin', // include, *same-origin, omit
      headers: {
        'Content-Type': 'application/json'
        // 'Content-Type': 'application/x-www-form-urlencoded',
      },
      redirect: 'follow', // manual, *follow, error
      referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
      body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    return response.json(); // parses JSON response into native JavaScript objects
  }
  
  postData('https://example.com/answer', { answer: 42 })
    .then(data => {
      console.log(data); // JSON data parsed by `response.json()` call
    });


    ******************************/