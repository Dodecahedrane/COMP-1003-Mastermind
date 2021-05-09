# C# Console Mastermind Game for COMP1003 Data Structures and Algorithms Module

## The Game

Mastermind is a code-breaking game for two players. In the original real-world game one player A selects 4 pegs out of 6 colours and puts them in a certain fixed order; multiples of colours are possible (for example, red green red green). His opponent B does not know the colours or order, but has to find out the secret code. To do so, B makes a series of guesses, each evaluated by the first player. A guess consists of an ordered set of colours which B believes is the code. The first player A evaluates the guess and feeds back to B how many positions and colours are correct. A position is correct ("black") if the guess and the secret code have the same colour at it. Additional colours are correct ("white"), if they are in the guess and the code, but not at the same location. For example

                                                       1       2        3        4
                                             secret:  red     green    red     green
                                             guess:   red     blue     green   purple

results in one correct position ("black = 1") for the red peg at position one and one additional correct colour ("white=1") for the green peg in the guess. Note that one "green" in the guess matches only one of the two "greens" in the secret code.

There are versions of mastermind with more or less positions than four, and more or less colours than six. Instead of colours use numbers in your code; ie the codes should be stored in N-element arrays of integers in the range from 1 to M.

## The Task

Write code that implements mastermind with N positions and M colours (encoded by numbers from 1 to M). N and M should be parameters of the code; the code should work for any positive choice of N and M (up to a maximum value of M=9). The code should take the role of player A in the description above, such that the user has to guess a secret code (role B); ask the user for N and M at the beginning; and then enter a main loop that iteratively starts new games until the user indicates an exit-condition (the code may ask the user if he wants to play again).

In a game, the computer generates a random secret according to the chosen values of N and M, and then iteratively asks the user for guesses. A guess would be evaluated and the results printed, i.e., the number of correct positions "black", and number of additional correct 'colours' "white". The program would recognise if the secret code has been correctly identified, print a message, and start again.

Add a "history" to the code that stores the guesses in each step as a queue and prints them out at the beginning of each step for inspection and at the end of the game if the secret code has been broken.

## The Rules

- Must submit a single file
- Can not use any prebuilt data structues besides arrays
- User has unlimited tries to guess the code
