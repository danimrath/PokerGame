# GameEvaluator
Game Library to evaluate poker hands
PokerHandEvaluator
    // Implements an object that can evaluate poker hands.
    // Constructor can take a series of strings representing players and their hands.
    // Provides methods to Add, Evaluate and clear the players.
    // ToString will print the list of players and their best hands.
    // Player objects can be created externally and compared to each other if desired.
    
Data Validation: I am assuming no duplicate cards are being passed in. Duplicates within a hand or between hands will not cause issues, they will just not be called out for cheating.

OutPut: calling EvaluateHands will print the name of the winner(s) as well as their winning hand(s). 
Writing the PokerHandEvaluator object to the console will will print all players and their hands.
