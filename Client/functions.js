




class ActiveGameManager {
    constructor(cards, apiHelper) {
        this.cardsArray = cards;
        this.apiHelper = apiHelper;
        this.ticker = document.getElementById('flips');
    }

    startGame() {
        console.log("startGame");
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
            if (card.getAttribute('flipped') === 'true') {
                card.classList.add('visible');
            }
        });
    }
    flipCard(card) {
        //Do the matching here as well as in server side.
        //but first, alert server of flipped card
        this.apiHelper.sendFlipRequest(card);
        
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
        return !this.busy && !this.matchedCards.includes(card) && card !== this.cardToCheck;
    }
}



function handleElementEnable(elementId, enable) {
    if (enable) {
        $(elementId).removeClass('disabledDiv');
    }
    else {
        $(elementId).addClass('disabledDiv');
    }
}



async function getNumOfPlayableCards() {
    this.baseURL = 'https://localhost:44309/api/Game/';
    var uri = this.baseURL + 'GetMaxPlayableCards/';
    var result = await (await fetch(uri)).json();
    document.getElementById("NumUniqueCards").value = result;
    
    return result;
}



async function getCurrentActiveGame() {
    this.baseURL = 'https://localhost:44309/api/Game/';
    var uri = this.baseURL + 'GetCurrentGame/';
    var result = await (await fetch(uri)).json();


    //if no current game exists, allow only StartNewGame
    if (!result  ||  !result.cards.length ) {
        console.log("NO GAME EXISTS");
        handleElementEnable('#NewGameDiv', true);
        handleElementEnable('#ResumeGameDiv', false);
        handleElementEnable('#EndGameDiv', false);
    }
    //else allow only ResumeGame or EndGame
    else {
        //this.resumeGame();
        console.log("found current game");
        handleElementEnable('#NewGameDiv', false);
        handleElementEnable('#ResumeGameDiv', true);
        handleElementEnable('#EndGameDiv', true);
    }

    console.log('---> ' + result);
    //document.getElementById("NumUniqueCards").value = result;
    
    //return result;
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

    document.getElementById('welcome').addEventListener('click', () => { getCurrentActiveGame(); });

    document.getElementById('NewGameBtn').addEventListener('click', () => { gameManager.beginNewGame(); });
    document.getElementById('ResumeGameBtn').addEventListener('click', () => { gameManager.resumeGame(); });
    document.getElementById('EndGameBtn').addEventListener('click', () => { gameManager.endGame(); });

}

