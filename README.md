# Atahan_Tanis_CarGame
Used Unity Version: 2020.3.23f (LTS)
(Tested on this version)

Created via default 2d template. No external package was used.

Reference resolution : 1920x1080.

Scenes can be found at Assets/ Scenes.

Editor related scripts can be found at Assets/Editor

Game related scripts can be found at Assets/_Game/Scripts

Game Instructions: 

# -Editor Related-

For every scene, cars' starting and ending positions can be changed from the Ground object at Hierarchy.

After the selection of Ground object, look for GameController script and expand it, if not, via the arrow keys.

There is a blue Select Car text. Below the Select Car text, we can change the current car. After the selection, when we left mouse click to editor scene we can set the positions. The first click set the starting point and the second for ending point, third one will also change again the starting point and goes on... We can't click the outside of the ground and also to obstacles.

Reset button, resets the all positions. 

For obstacles we can spawn the new ones via the prefabs folder that can be found at Assets/_Game/Prefabs/Obstacle and edit them via editor.

There is also a Remember mode that I added myself after seeing different behaviors of cars. When we set true the Remember mode, cars also remember their mistakes.

# -Game Related-

Cars are activating after LeftMouse Click. After the resets we also need to click again.

Car can turn left with the A or left arrow key.

Car can turn right with the D or right arrow key.

When we set the starting position at up cars are starting with looking down and vice versa.

Cars also can't go outside of the Ground and Grounds' scale can editable via editor.


# -Code Related-

State pattern used on Car Controls.
 -CarController,State,IdleState,MovementState,LoopState are related scripts.
 
GameController class is the general controller class.

GameControllerEditor is an editor script.
