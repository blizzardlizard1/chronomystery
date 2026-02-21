# Chrono Mystery Script Glossary

Use this README file to find out where certain scripts are, what they do, and  how to organize our stuff.

## Interactions

Interactable objects will be the core of most of our puzzles. Every Interactable object needs the **Interaction.cs** script. I also recommend attaching the InteractableUtils and ObjectSync scripts. 

While not in this folder, **PlayerInteractions.cs**, is also needed to connect the player with the interaction system.

### Interactable.cs

Contains two types of interactions:

1. Wheel-based multiple selection
2. Old system where there is one interaction

The legacy system was created by Abdi  and allows object interaction when you are within proximity. However, we wanted the player to be able to choose from multiple options which is why it was refactored to support that.

The new Interactable script contains the following Serializable fields:

**Supported Actions.**

This is a structure defined in **WheelAction.cs**. The WheelAction is what direction of the wheel the action will be when the player interacts with the object. Since we are splitting the wheel into 4 options, there is a top, right, left, and bottom. It's designed to allow any UnityEvent to be used in these 4 options.

**See section on WheelAction.cs for more info**

For things to actually happen when a player chooses an option, make sure to put a function in the corresponding On___Action. 

**If you want to use the old interaction system, leave the SupportedActions array empty. The Mirrors still use the old interaction.**

### InteractableWheel.cs

This handles the logic for displaying the Option Wheel and sends the chosen action to the Interactable script to execute. This just needs to be attached to the Canvas "OptionWheel" object.

### InteractableUtils.cs / Pickup.cs

This is the refactored version of the Pickup script that Abdi made. Decided to repurpose it to include other common Interactable functions like picking stuff up and destroying objects.

### WheelAction.cs

Includes an enum for all 4 possible options in the OptionWheel (Top, Left, Right, Bottom) as well as a struct with this information:

1. Action - [Top, Right, Left, Bottom], Decides where on the wheel the associated action goes
2. Name - The name of the Interaction (will appear on the wheel as such)
3. Required Item - Optional field. You can leave this empty but put the name of the Item needed to do this action. If the player doesn't have the required item, it will appear gray on the wheel.
4. Dialogue - Flavor text that appears when the player completes this action.

You can edit these 4 parameters in the Inspector of any **Interactable** objects.


## Inventory Scripts

The scripts that handle Inventory are InventorySlot.cs, InventoryUI.cs, andPlayerInventory.cs. I'm not going to expand on these because you wrote them.

## Player Scripts

### PlayerController.cs 

Handles the player movement and allows it to feel natural with the isometric perspective. Pretty self-explanatory.

### PlayerInteraction.cs

Bridges player input with the interaction system.

### PlayerSpawner.cs

This should be attached to an empty game object. It basically just needs to exist in the scene and helps spawn the player where it needs to go when we switch between time periods. It uses the SpawnPositions attached to Mirrors to decide where the player spawns when they time travel.

### SmoothCameraFollow.cs

Allows our isometric camera to follow the player as they move around the map.

## Time Travel Related

### MirrorManager.cs

Attach to an empty GameObject. Connects mirrors in different states using a mirrorID. Corresponding mirrors will map to each other. Add a SpawnPosition and make the mirror interactable and you are all set. I use 'mirror-A' as an id. So the mirror in the past and the mirror in the future both need the id 'mirror-A'

### ObjectSync.cs

This is one script apart of a 2 script team that handles syncing objects across time periods. ObjectSync checks every object saved in SceneStateManager's hashmap and deletes them from the scene. Make sure to destroy objects using the special ObjectSync destroy.

### SceneStateManager.cs

Singleton that keeps track of all objects the player has destroyed/moved. It does this using a hashmap of the names of the objects the player has interacted with.

### TimeSwitch.cs

This is the primary script responsible for time travel. These are connected to the mirrors and in the inspector you should set portalID, spawn location, and whether the mirror is open or not. Uses a coroutine for transitions.

## Archive

## Other

For scripts where I don't really know how to categorize yet.

### DialogueDisplay.cs

This is the script that we can reuse for all dialogue-related implementations. All the flavor text that appears when you interact with an object uses this script.