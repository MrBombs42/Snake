# SnakeGame

same-screen multiplayer Snake game [1], where each player had
a block to pick and an enemy AI snake to dispute that block, both respawn
when picked or die, respectively, as long as its player is alive.

The blocks, when picked by the snake, enter in front of the snake as the
head, multiple blocks can be picked at once if they are inline in the same
direction of the snake's movement.

Each block slows down the snake a bit, as it gets loaded, but they also
accumulate benefits, for instance:

* ENGINE POWER - moves faster, it more than compensates for the load;

* TIME TRAVEL - when the snake dies the whole game returns to the moment
  immediately before it got the newest time travel block, but the block 
  is not there on the board anymore;

* BATTERING RAM - if you hit an opponent with the head of your snake, it
  disables one of your battering ram blocks and the block the head touched
  on the other snake, allowing it to pass through that disabled block.

All other things that the head may hit, and are not loose blocks on the board,
may potentially kill the snake.

In the beginning, the game instructs the player to press two letters or
numbers [2] for 1 second. There can be as many players as there are keys available. 
The movement is relative to thedirection the snake is moving -- i.e.,
pressing left makes the snake turn 90 degrees to its left -- 
and the snake can't go straight back in one turn,
must make two turns in the same direction.
