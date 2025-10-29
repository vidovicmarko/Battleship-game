How to run:
Option 1: Run from terminal 
1. Make sure .NET SDK is installed
2. Open a terminal
3. Navigate to project folder, one containing .csproj file
4. Run the game with command: dotnet run
5. The game will start automatically

Option 2: Run from code editor
1. Open the project in code editor that supports .NET projects
2. Click the Run button or press Ctrl + F5
3. The game will run in the built-in console

Brief summary:

The program simulates a complete game of Battleship between two computer-controlled players.
Its starts by initializing two 10x10 boards and filling them with empty tiles.

During setup, each player's ships are placed randomly:
- A random coordinate is selected.
- The program checks whether the ship can fit there (no overlap or out of bounds)
- A random direction is chosen and placement is attempted
- If the placement is invalid, another direction is tried until a valid placement is found.

Once the game begins, players take turns firing shots.
Each player starts by searching randomly until they score a hit.
When hit occurs, the player switches into a targeting mode:
- They pick a random direction from that hit and take a shot in that direction
- If they find another hit, they keep shooting in the same direction until the ship is sunk, the edge of the board is reached or a missed tile is hit.
- If the ship isn't sunk when reaching the out of bounds or miss, the player reverses the direction and shoots the other way from the original hit until the ship is destroyed.

To make searching more efficient, the player uses two strategies:
1. Before firing at the tile, the algorithm checks if the smallest remaining ship could fit horizontally or vertically in that area. If the space is too small in both directions, it skips the cell and tries again.
2. During the hunting phase, the player also fires only on every other cell, following a checkerboard pattern. Because no ship is smaller than two tiles, which guarantees that no ship can be between missed shots.

After every shot, the program prints the move and displays the current state of the board, allowing the spectator to visually track progress.
The game continues until one player sinks all the opponent's ships.
At that point, the game announces the winner, prints the total number of turns and displays the final state of both boards.
