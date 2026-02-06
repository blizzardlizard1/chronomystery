# Time Switch Mechanics

## Overview

The core method that allows the player to time travel in our game is through entering magical mirrors. Mirrors will usually only be a one-way trip, requiring players to pay attention to destinations of mirrors.

## How they work

In order to connect mirrors in two separate Unity scenes, we give a Mirror object a "mirror-ID". Two corresponding mirrors should have the same mirror ID. 

The script attached to the mirror-object attempts to find another mirror with the same mirror-ID. Every scene has an empty object with a MirrorManager script that contains the target mirror-ID. 

The PlayerSpawner script is how we know where to place the player once they exit the portal.

The TimeSwitch script is used to initiate the scene transition.

## How to setup

Place down a mirror in two scenes you want to connect. Ideally you should try to make the two scenes look similar so that when the player travels through the mirror, it looks like they are in the same place, but at a different time. 

In both the scenes that want to implement time switching, make sure to include these 3 things:

1. An empty game object called MirrorManager with the MirrorManager.cs script attached
2. An empty game object called PlayerSpawner with the PlayerSpawner.cs script attached
3. An empty game object (can be called anything like SpawnPositionA) which will simply represent the position the player spawns at after going through the mirror. Make sure to place it in front of the mirror. 

Finally, you just need to attach the TimeSwitch.cs script to each mirror and assign the correct parameters. 

* Destination name should be the literal name of the Unity scene you want to travel to
* portal ID is some kind of string that matches two portals in different scenes. This can be anything as long as its consistent and the same for both mirrors.
* spawnPoint is where the player spawns when they come out of the mirror. Remember that empty game object we called "SpawnPositionA"? This goes here
* Finally there's a boolean paremeter called "open?" which basically allows us to make a mirror unable to be travelled through. This allows us to create one-way mirrors.
