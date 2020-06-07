
class MixOrMatch {
    constructor(totalTime, cards) {
        this.cardsArray = cards;
        this.totalTime = totalTime;
        this.timeRemaining = totalTime;
        this.timer = document.getElementById('time-remaining')
        this.ticker = document.getElementById('flips');
    }

    startGame() {
        this.totalClicks = 0;
        this.timeRemaining = this.totalTime;
        this.cardToCheck = null;
        this.matchedCards = [];
        this.busy = true;
        setTimeout(() => {
            this.shuffleCards(this.cardsArray);
            this.countdown = this.startCountdown();
            this.busy = false;
        }, 500)
        this.hideCards();
        this.timer.innerText = this.timeRemaining;
        this.ticker.innerText = this.totalClicks;
    }
    startCountdown() {
        return setInterval(() => {
            this.timeRemaining--;
            this.timer.innerText = this.timeRemaining;
            if(this.timeRemaining === 0)
                this.gameOver();
        }, 1000);
    }
    gameOver() {
        clearInterval(this.countdown);
        this.audioController.gameOver();
        document.getElementById('game-over-text').classList.add('visible');
    }
    victory() {
        clearInterval(this.countdown);
        document.getElementById('victory-text').classList.add('visible');
    }
    hideCards() {
        this.cardsArray.forEach(card => {
            card.classList.remove('visible');
            card.classList.remove('matched');
        });
    }
    flipCard(card) {
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
        return !this.busy && !this.matchedCards.includes(card) && card !== this.cardToCheck;
    }
}


if (document.readyState == 'loading') {
    document.addEventListener('DOMContentLoaded', ready);
} else {
    ready();
}






async function getCurrentGameFromServer() {
    var uri = 'https://localhost:44355/api/Game/GetCurrentGame';
    var data = await (await fetch(uri)).json();

    //if no current game exists, allow only StartNewGame
    if (!data) {
        console.log("NO GAME EXISTS");
        $("#ResumeGameDiv").addClass('disabledDiv');
        $("#NewGameDiv").removeClass('disabledDiv');
    }
    //else allow only ResumeGame or EndGame
    else {
        console.log("found current game");
        $("#ResumeGameDiv").removeClass('disabledDiv');
        $("#NewGameDiv").addClass('disabledDiv');

        //currentGame = new CurrentGame(data.cards, data.currentNumOfFlips, data.lastFlippedCardId);
    }
}




async function initNewGame() {
    await goFetchCards();
    
    let cards = Array.from(document.getElementsByClassName('card'));

    console.log("cards=  " + cards.length);
    let game = new MixOrMatch(500, cards);
    game.startGame();

    cards.forEach(card => {
        card.addEventListener('click', () => {
            game.flipCard(card);
        });
    });
}





async function goFetchCards() {
    var uri = 'https://localhost:44355/api/Game/StartNewGame/0';

    var result = await (await fetch(uri)).json();
    fillGrid(result);    
}


function fillGrid(data) {
    console.log(data[0]);

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
        newCard.classList.add("card");
        newCard.appendChild(cardBack);
        newCard.appendChild(cardFront);
        
        console.log("game_container=  " + game_container);
        game_container.appendChild(newCard);

    }
}


async function ready() {
    let overlays = Array.from(document.getElementsByClassName('overlay-text'));
    ///HERE
    await getCurrentGameFromServer();
    //await goFetchCards();

    // let cards = Array.from(document.getElementsByClassName('card'));

    // console.log("cards=  " + cards.length);
    // let game = new MixOrMatch(100, cards);

    overlays.forEach(overlay => {
        overlay.addEventListener('click', () => {
            overlay.classList.remove('visible');
            //game.startGame();
        });
    });


    // cards.forEach(card => {
    //     card.addEventListener('click', () => {
    //         game.flipCard(card);
    //     });
    // });


}


