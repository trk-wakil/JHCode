
My initial thoughts..
It was actually rather exciting to work on this excercise.. So exciting in fact that I got carried away with ideas and tried to implement too many things before fully completing the main requirements. In the end, I ended up aborting much of my original plans in order to keep a simple working version.

I tend to be a perfectionist when I have full control of the project, like in this case. That was probably a mistake to learn from. I am still interested in doing further work on this game so I will most likely pick it up again in the coming weeks.


The Client-side:
perhaps the one area that I had so much ambitions about and yet the one part that frustrated me the most. I put together at least two different versions before this one. I was using TypeScript with classes and they were working fine but the biggest part that was missing was the animations and the logic of flipping the cards. In the end, I came across an online tutorial that did this game fully in Javascript and it had great animations and designs. I took the codes pretty much as they are and implemented my changes on top of them to add the API calls. I was planning to add my own changes on the designs but in the end, I realize that an open-source code is not something to feel guilty about. What really matters is understanding it enough to apply them as needed. I am glad I came across that code because I was very glad to make use of it.

One of the biggist obstacles I faced that I still want to get back to is calling the API in the correct format. The endpoints work fine when tested via Postman. I am able to send JSON data and get back responses. However, from the Javascript codes, it seems that adding an options param in the fetch method is a bit more tricky. I came across that issue later in the process and spent many hours trying different solutions but with little success. That issue held me back signeficantly.


The DataBase:
it turned out I didn't have SQL server or any RDBM installed on my personal laptop and so my initial choice was to use simple XML files to hold the data only as a first choice in order to get me going. My plan was to get back to that part at a later point to use and actual light weight RDBM. But I ended up spinning my wheels too much with the client side that I ran out of time. Luckily, I implemented the DataBase abstraction well enough I believe that I can still easily get back to that part.
Another mistake I made with the database part is that I ended up changing the Model classes in order to better fit the XML files structure. For example, instead of thinking of data models as player VS game, I thought of it as data that needs to have a short-term life (cards in an active game) and data that we could use even after the game is done. This actually made things easier for calling the enpoints without losing the data.



In the end, I loved going through this excercise. It reminded me of times when I had full control over the projects I was working on where I brainstormed many ideas and explored different solutions just for the sake of curiosity of finding out if I can solve the problem in different ways. Clearly what I missed here is that I had a deadline to meet!