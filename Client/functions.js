
/***
 * TODO list:
 * add error messages for failed connections
 * work more on the fethc methods
 * use classes and constancts, especially for the API address
 * clear the grid on game ending
 * add a faq section to help explain the steps
 * move the button divs around better
 * display player scores
 */



let bestScore;
let bestScoreNumberOfCards;
let gamesPlayed;
let gamesWon;

let numOfFlips =-1;
let lastFlippedCardId = -1


class ActiveGameManager {
    constructor(cards) {
        this.cardsArray = cards;
        this.ticker = document.getElementById('flips');

        this.lastFlippedCardIdFromServer;
        this.numberOfFlipsFromServer;
    }

    startGame() {
        console.log("startGame");
        this.totalClicks = 0;
        this.timeRemaining = this.totalTime;
        this.cardToCheck = null;
        this.matchedCards = [];
        this.hideCards();
        if (this.numberOfFlipsFromServer > -1) {
            this.ticker.innerText = numOfFlips;
            this.totalClicks = numOfFlips;
        }
        else {
            this.ticker.innerText = this.totalClicks;
        }       
        

    }
    victory() {
        gamesPlayed++;
        gamesWon++;
        if (bestScore > numOfFlips) {
            bestScore = numOfFlips;
            bestScoreNumberOfCards = this.cardsArray.length;
        }

        document.getElementById('victory-text').classList.add('visible');
        
    }
    hideCards() {
        this.cardsArray.forEach(card => {
            console.log('what is flipped:  ' +card.getAttribute('flipped'));
            // card.classList.remove('visible');
            // card.classList.remove('matched');    
            if (card.getAttribute('flipped') == 'true') {
                card.classList.add('visible');
                if (card.getAttribute('id') == this.lastFlippedCardIdFromServer) {
                    this.cardToCheck = card;
                }
                else {
                    this.matchedCards.push(card);
                }
            }
            else { 
                card.classList.remove('visible');
                card.classList.remove('matched');    
                console.log('flipped says false'); 
            }
        });
    }
    flipCard(card) {
        //Do the matching here as well as in server side.
        //but first, alert server of flipped card
        sendFlipRequest(card);
        
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
    getCardType(card) {
        return card.getElementsByClassName('card-value')[0].src;
    }
    canFlipCard(card) {
        return !this.busy && !this.matchedCards.includes(card) && card !== this.cardToCheck;
    }
}


function showScore() {
    document.getElementById('player-score').innerText = 'Score:';
}


function handleElementEnable(elementId, enable) {
    if (enable) {
        $(elementId).removeClass('disabledDiv');
    }
    else {
        $(elementId).addClass('disabledDiv');
    }
}




function sendFlipRequest(card) {
    var uri = this.baseURL + 'FlipCard/' + card.id;
    //Think about the response.. do we need it?
    fetch(uri);
        // .then((response) => {
        //     return response.json();
        // })
        // .then((data) => {
        //     console.log('from Server:  ' +data);
        // });
}


async function getNumOfPlayableCards() {
    this.baseURL = 'https://localhost:44309/api/Game/';
    var uri = this.baseURL + 'GetMaxPlayableCards/';
    var result = await (await fetch(uri)).json();
    document.getElementById("NumUniqueCards").value = result;
    
    return result;
}



async function getPlayerRecord() {
    this.baseURL = 'https://localhost:44309/api/Game/';
    var uri = this.baseURL + 'GetPlayerRecord/';
    var result = await (await fetch(uri)).json();

    //if no current game exists, allow only StartNewGame
    if (result.hasActiveGame == false) {
        console.log("NO GAME EXISTS");
        handleElementEnable('#NewGameDiv', true);
        handleElementEnable('#ResumeGameDiv', false);
        handleElementEnable('#EndGameDiv', false);

        numOfFlips = -1;
        lastFlippedCardId = -1;
    }
    //else allow only ResumeGame or EndGame
    else {
        console.log("found current game");
        handleElementEnable('#NewGameDiv', false);
        handleElementEnable('#ResumeGameDiv', true);
        handleElementEnable('#EndGameDiv', true);

        numOfFlips = result.currentNumOfFlips;
        lastFlippedCardId = result.lastFlippedCardId;

    }

    
    bestScore = result.bestScore;
    bestScoreNumberOfCards = result.bestScoreNumberOfCards;
    gamesPlayed = result.gamesPlayed;
    gamesWon = result.gamesWon;

}


async function resumeGame() {
    this.baseURL = 'https://localhost:44309/api/Game/';
    var uri = this.baseURL + 'GetCurrentGameCards/';
    var result = await (await fetch(uri)).json();

    handleElementEnable('#EndGameDiv', true);

    setupGridAndGame(result);
}


async function setupNewGame() {

    var numOfUniqueCards = document.getElementById("NumUniqueCards").value;
    console.log('numOfUniqueCards=  ' + numOfUniqueCards );

    //TODO use the operation in fetch and allow using the variable
    var uri = this.baseURL + 'SetupNewGame/' + numOfUniqueCards;
    var result = await (await fetch(uri)).json();

    numOfFlips = -1;
    lastFlippedCardId = -1;

    handleElementEnable('#EndGameDiv', true);

    setupGridAndGame(result);
}


async function endCurrentGame() {
    var uri = this.baseURL + 'EndGame/';
    var result = await (await fetch(uri)).json();

    handleElementEnable('#NewGameDiv', true);
    handleElementEnable('#ResumeGameDiv', false);
    handleElementEnable('#EndGameDiv', false);

    numOfFlips = -1;
    lastFlippedCardId = -1;
    gamesPlayed++;

    document.getElementById('game-ended-text').classList.add('visible');

    console.log(result);
}


function setupGridAndGame(cardsFromServer) {

    console.log(cardsFromServer);

    this.fillGrid(cardsFromServer);

    let cards = Array.from(document.getElementsByClassName('card'));
    this.currentActiveGame = new ActiveGameManager(cards);

    this.currentActiveGame.lastFlippedCardIdFromServer = lastFlippedCardId;
    this.currentActiveGame.numberOfFlipsFromServer = numOfFlips;

    this.currentActiveGame.startGame();

    cards.forEach(card => {
        card.addEventListener('click', () => {
            this.currentActiveGame.flipCard(card);
        });
    });
}


function fillGrid(data) {

    var game_container = document.getElementsByClassName('game-container')[0];

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
        //add attribute flipped
        newCard.setAttribute("flipped", JSON.stringify(cardData.flipped));
        newCard.classList.add("card");
        newCard.appendChild(cardBack);
        newCard.appendChild(cardFront);

        game_container.appendChild(newCard);
    }
}






if (document.readyState == 'loading') {
    document.addEventListener('DOMContentLoaded', ready);
} else {
    ready();
}




async function ready() {

    await getNumOfPlayableCards();
    
    let overlays = Array.from(document.getElementsByClassName('overlay-text'));    
    overlays.forEach(overlay => {
        overlay.addEventListener('click', () => {
            overlay.classList.remove('visible');
        });
    });

    document.getElementById('welcome').addEventListener('click', () => { getPlayerRecord(); });

    document.getElementById('NewGameBtn').addEventListener('click', () => { setupNewGame(); });
    document.getElementById('ResumeGameBtn').addEventListener('click', () => { resumeGame(); });
    document.getElementById('EndGameBtn').addEventListener('click', () => { endCurrentGame(); });

}

