# 3D_Tetris

The game has a complete game loop with MainMenu->Gameplay->GameOver->PlayAgain.
Basic controls are: WASD/Arrow Keys to move; Space Key to rotate and Mouse Left Click +
Mouse Movement to control the camera (more commands in the Game’s Main Menu). The
project code is well commented and easy to follow. Given the size of the project, I ended up
using Singletons and Events for most of the tasks. Below are the core classes/objects:
1. <b>Game Manager</b>: keeps count of score, layers and level. It calculates incremental combo
by amount of layers cleared (1,2,3 and tetris).
2. <b>Grid Space</b>: object: made of 5 planes(bottom, north, south, west, east) rendered with
emission grid material for “neon” effect with bloom post processing.
script: keeps the coordinates values of all tetris and ghost pieces inside of it. Also get
new pieces from the pool when needed and “deletes” layers by returning the pieces to
the obj pool again.
3. <b>Tetris Piece</b>: detects user input for movement. Calls Grid Space when it needs to update.
4. <b>Ghost Piece</b>: if the parent piece is disabled, returns itself to the pool.
5. <b>Object Pool</b>: simple but effective Object Pooling for tetris and ghost pieces. Activate and
deactivate pieces to only instantiate additional pieces when it’s really required.
6. <b>UI Handler</b>: listens to Game Manager for score, layer and level increments and handles
eventual necessary UI updates.
7. <b>Audio Player</b>: has two objects: BackgroundMusic for game music loop and
LayerClearedSFX. Used ScriptableObject Game Event to play sound effect when the layer
is cleared.
