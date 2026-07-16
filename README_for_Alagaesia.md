# Dragon Rider Prototype

## Project Overview

This project is an original 3D fantasy game prototype built in Unity. It is inspired by the world and themes of *Eragon*, but it is being developed as a personal/class project with its own gameplay systems, environment, progression, and future story ideas.

The current build focuses on the player's final training before they are considered ready to become a Dragon Rider. The player gathers supplies, visits local craftsmen, completes an archery challenge, and hunts deer in the grassy mountains.

The playable section now has a complete beginning and ending. After the player finishes the archery range and collects two deer, a completion screen appears saying that the tutorial is complete and that the player is ready to get their dragon.

The next major part of the game will focus on **recovering a dragon egg**. After the egg is recovered, the story will eventually continue into the egg hatching and the beginning of the player's bond with their dragon.

---

## Setting

The game takes place roughly **300 years before the rebellion of Galbatorix**. Dragons and Dragon Riders still live and work together, and the Riders are respected as protectors, scholars, warriors, and peacekeepers.

The player character has been training to become a Dragon Rider. Before they can begin the next stage of that journey, they must prove that they are prepared by gathering equipment and completing basic training around the settlement.

The opening area is meant to represent an early Dragon Rider settlement on Vroengard. It currently includes shops, an archery range, mountain terrain, grassy hunting areas, and locations used for the tutorial objectives.

---

## Current Gameplay Goal

The player's current goal is to complete their final training and prove that they are ready to begin searching for a dragon egg.

The player must:

1. Open the menu with **TAB**.
2. Locate and collect the backpack.
3. Visit the Saddler and purchase a saddle.
4. Visit the Fletcher and purchase a bow and arrows.
5. Equip and use the bow.
6. Hit all five targets at the archery range.
7. Travel into the grassy mountains.
8. Hunt and collect two deer.
9. Reach the tutorial completion screen.

After the second deer is collected, the game displays:

```text
Tutorial completed!

You are now ready to get your dragon.

To Be Continued....
```

The player can then press **SPACE** to restart the game.

---

## Current Gameplay Loop

The current playable loop is:

```text
Start
→ Open the menu
→ Collect the backpack
→ Buy the saddle
→ Buy the bow and arrows
→ Complete the archery range
→ Hunt and collect two deer
→ Tutorial completion screen
→ Press SPACE to restart
```

This gives the current prototype a complete start-to-finish objective instead of ending immediately after the Fletcher shop.

---

## Controls

| Input | Action |
|---|---|
| **WASD** | Move |
| **Mouse** | Control the camera |
| **Left Shift** | Run |
| **C / Left Ctrl** | Crouch |
| **Space** | Jump / Restart after completion |
| **E** | Interact, collect, or buy |
| **TAB** | Open or close the player menu |
| **B** | Equip or put away the bow |
| **Right Mouse Button** | Aim the bow |
| **Left Mouse Button** | Fire an arrow while aiming |

---

## New Since the Previous Build

A large amount of gameplay has been added since the earlier version of the project.

### Bow and Arrow System

- The bow can now be equipped and put away.
- The player can aim using an over-the-shoulder camera.
- Arrows are fired toward the center of the camera.
- Fired arrows use Rigidbody-based projectile movement.
- Arrows can stick into objects after hitting them.
- A visible arrow appears on the bow while the player is aiming.
- The player's upper body uses draw, aim, and firing animations.
- The player rotates toward the aiming direction while using the bow.

### Arrow Resource System

- The inventory now tracks the exact number of arrows.
- Firing an arrow reduces the arrow count.
- The player receives arrows when purchasing the bow.
- The Fletcher can sell arrow refills.
- The tutorial tells the player to return to the Fletcher when they run out.
- Limited arrows and limited gold create a resource-management challenge.

### Archery Range

- An archery range has been added to the environment.
- The player must hit five separate targets.
- Each target only counts once.
- The game tracks archery progress.
- After all five targets are hit, the objective changes to deer hunting.

### Deer Hunting System

- Animated deer have been added to the mountain area.
- Deer switch between idle, wandering, and fleeing behavior.
- Deer run away when the player gets too close.
- Deer can be defeated by arrows.
- The player can approach a defeated deer and press **E** to collect it.
- The objective system tracks the number of deer collected.
- The player must collect two deer to finish the tutorial.

### Deer Spawning System

- A deer spawning zone has been added to the grassy mountain area.
- The spawner keeps at least five living deer in the area.
- Deer are spawned on valid ground using downward raycasts.
- The deer remain connected to the mountain hunting zone instead of spawning across the entire map.

### Completion and Restart

- A full-screen tutorial completion panel has been added.
- The game pauses when the completion screen appears.
- The screen fades in using a Canvas Group.
- The player can press **SPACE** to reload the current scene and restart.

---

## Current Project Status

### Completed / Working So Far

- Created a playable 3D fantasy environment.
- Added terrain, mountains, foliage, shops, and an archery area.
- Added a controllable third-person player.
- Added walking, running, crouching, and jumping.
- Added a third-person follow camera.
- Added an over-the-shoulder aiming camera.
- Imported and customized a modular fantasy knight character.
- Fixed character material and shader problems.
- Added player locomotion animations.
- Added upper-body bow animations.
- Added a usable bow and projectile arrows.
- Added arrow count and gold tracking.
- Added a backpack pickup interaction.
- Added Saddler and Fletcher shop interactions.
- Added bow, saddle, arrow, and backpack inventory tracking.
- Added arrow refill purchases.
- Added a five-target archery challenge.
- Added wandering and fleeing deer AI.
- Added deer hunting and collection.
- Added a deer spawn zone and population system.
- Added tutorial hints that update throughout the full objective sequence.
- Added inventory and objective menu pages.
- Added a completion screen and restart flow.
- Added background music through an Audio Manager.
- Added an animation event receiver for footstep and landing animation events.

---

## Interaction Systems

The game currently includes several different types of interaction:

1. **Item pickup:** The player presses **E** to collect the backpack.
2. **Shop interaction:** The player presses **E** near NPC shopkeepers to buy the saddle, bow, and arrows.
3. **Usable weapon:** The player equips, aims, and fires the bow.
4. **Target interaction:** Arrows activate archery targets when they hit them.
5. **Deer collection:** The player presses **E** near defeated deer to collect them.
6. **Menu interaction:** The player opens the menu and selects the inventory or objective sections.
7. **Completion and restart:** The completion screen appears after the final objective and allows the scene to restart.

---

## Challenge Systems

The current prototype includes at least three challenge systems:

### Archery Accuracy

The player must aim and hit five different targets. Hitting the same target repeatedly does not increase progress.

### Limited Resources

The player has limited gold and a limited number of arrows. Missing too many shots may require returning to the Fletcher and purchasing more arrows.

### Deer AI and Hunting

Deer wander through the mountains and flee when the player approaches. This makes them harder to line up and shoot than the stationary archery targets.

### Ordered Progression

The tutorial objectives must be completed in order. The player gathers equipment before moving into archery training and then deer hunting.

---

## UI Systems

The game currently includes:

- Tutorial guidance text.
- Interaction prompts.
- Temporary gameplay messages.
- Gold tracking.
- Arrow count tracking.
- Inventory display.
- Objective display.
- Archery target progress.
- Deer collection progress.
- Tutorial completion screen.
- Restart instructions.

The player menu opens with **TAB**. The cursor unlocks while the menu is open and locks again when the player returns to gameplay.

The tutorial guidance currently walks the player through:

1. Opening the menu.
2. Finding the backpack.
3. Visiting the Saddler.
4. Visiting the Fletcher.
5. Completing the archery range.
6. Hunting two deer in the grassy mountains.
7. Becoming ready for the next part of the story.

---

## Inventory and Progression

The player inventory currently tracks:

- Gold
- Backpack
- Saddle
- Bow
- Arrow count
- Dragon egg placeholder status
- General objective progress

Other systems track:

- Number of archery targets hit
- Whether archery training is complete
- Number of deer collected
- Whether the tutorial has been completed

The dragon egg variable is still a placeholder for the next stage of development. The next major gameplay section will involve recovering the egg instead of simply receiving it immediately.

---

## Shops

### Saddler

The Saddler is the first shop the player visits after collecting the backpack. The player can purchase a saddle if they have enough gold.

### Fletcher

The Fletcher sells the player's bow and starting arrows. The player can also return to the Fletcher later to buy more arrows if they run out during the archery range or deer hunt.

The shops work as progression points, but they will need more dialogue, animation, and visual detail later.

---

## Audio

The project currently includes a basic Audio Manager for background music.

Current audio features:

- Background music plays during gameplay.
- The music loops while the player completes the tutorial.
- The project has support for footstep and landing animation events.
- The deer and target systems include optional places for hit or collection sounds.

Audio still needs more work. Future additions should include:

- Footstep sounds for different surfaces.
- Bow draw and bow firing sounds.
- Arrow impact sounds.
- Target hit sounds.
- Deer movement or reaction sounds.
- Backpack pickup sound.
- Shop purchase sound.
- UI button sounds.
- Forest, mountain, and settlement ambience.
- Dragon egg recovery and hatching audio.

---

## Animation

Current animation work includes:

- Idle, walk, run, crouch, and jump support for the main character.
- Animator parameters controlled by the movement script.
- An upper-body Avatar Mask for bow animations.
- Bow draw animation.
- Bow aiming idle animation.
- Bow firing/recoil animation.
- Animated deer movement.
- Footstep animation event handling.

The animations are working, but they are still one of the areas that need the most polish. The main character needs smoother transitions, better hand placement, better bow alignment, and more natural interaction animations.

---

## Current Ending

The prototype no longer ends after buying the bow.

The current ending happens after the player:

1. Hits all five archery targets.
2. Hunts and collects two deer.
3. Completes the tutorial.

A completion screen tells the player that they are ready to get their dragon and displays **To Be Continued....**

This ending leads directly into the planned egg recovery section.

---

## Next Major Story and Gameplay Phase

The next major section of the game will be **dragon egg recovery**.

Instead of the player immediately choosing or receiving an egg, the player will have to take part in a mission or sequence where the egg is located and recovered. This should make the dragon egg feel more important and give the player a stronger connection to it.

Possible parts of the egg recovery section include:

- Receiving information about a missing or endangered egg.
- Talking to Riders, guards, villagers, or other NPCs.
- Traveling to a new area or dangerous part of the island.
- Following clues or completing smaller objectives.
- Reaching the egg through exploration, combat, a puzzle, or an environmental challenge.
- Recovering the egg and returning it to safety.
- Beginning the bonding process between the player and the dragon.

After the egg recovery section, development will move toward:

- The egg reacting to the player.
- The egg beginning to crack.
- A hatching sequence.
- The first appearance of the player's dragon.
- Early bonding and training systems.
- Choosing or confirming the dragon's color and appearance.

The dragon itself will require more time because I plan to create or heavily customize the model in Blender.

---

## Main Areas That Need Improvement

The game is playable from beginning to end, but several areas still need a lot of improvement before it feels like a larger and more polished game.

### NPCs

NPCs will be one of the biggest areas of focus going forward.

Current NPCs mainly act as shop interaction points. They still need:

- Dialogue systems.
- Names and personalities.
- Better idle and talking animations.
- Facial movement or expression where possible.
- More NPCs throughout the settlement.
- Riders, guards, villagers, workers, and trainers.
- Story conversations related to the egg recovery mission.
- NPC schedules or simple wandering behavior.
- Better shopkeeper reactions and purchase feedback.

### Main Character

The main character works, but still needs more polish.

Planned improvements include:

- Better movement animation blending.
- Better bow placement in the hands.
- Better hand and arm alignment while aiming.
- Improved jumping and crouching animations.
- Pickup and interaction animations.
- More detailed character materials.
- More customization options.
- Better camera collision and movement.
- Additional equipment that appears on the character.
- A stronger visual identity for the player character.

### Environment

The world is playable, but it still needs more detail and environmental storytelling.

Planned improvements include:

- More grass in the mountain areas.
- More trees, bushes, rocks, flowers, and ground details.
- Better paths between major objectives.
- Signs pointing toward important locations.
- More props around the shops and settlement.
- Better lighting and shadows.
- Improved skybox, fog, and atmosphere.
- More landmarks so the player can navigate naturally.
- Better mountain boundaries and collision.
- Additional areas for the egg recovery section.
- More original Blender-created environment assets.

### HUD

The current HUD gives the player the required information, but it still looks like a prototype.

Planned improvements include:

- A cleaner fantasy-themed layout.
- Better fonts and icons.
- Separate icons for gold, arrows, and important items.
- Less screen clutter.
- Better positioning of tutorial hints and temporary messages.
- Improved objective progress display.
- Better aiming feedback or a crosshair.
- Animated UI transitions.
- Sound effects when the HUD updates.
- A more polished completion screen.

### Inventory Screen

The inventory currently displays tracked items and progress, but it needs a major visual and functional update.

Planned improvements include:

- Item slots instead of mostly text.
- Item icons for the backpack, saddle, bow, arrows, and future egg.
- Item descriptions.
- Better organization into equipment, supplies, and story items.
- Highlighting or selecting items.
- Showing equipped items.
- Improved arrow and gold displays.
- A parchment or fantasy-style layout.
- Better controller and mouse navigation.
- Future dragon-related information after the egg is recovered.

---

## Known Issues / Notes

- The dragon egg recovery and hatching sections are not implemented yet.
- The dragon model still needs to be created or polished in Blender.
- Bow and upper-body animations work but still need alignment and blending improvements.
- The aiming camera is working but may still need smoothing and collision improvements.
- Deer use simple wandering and fleeing AI and may still run into difficult terrain or objects.
- The environment needs more paths, signs, grass, props, and landmarks.
- Several sound effects are still missing.
- The HUD and inventory screen are functional but still visually basic.
- NPCs need dialogue, animations, and more personality.
- Some Unity Editor graph errors may appear in the Console even though they have not prevented the game from running.
- More full playthrough testing is still needed after every major update.

---

## Current Assignment Checklist

### Original 3D Game

- **Status:** Complete
- The project has its own fantasy setting, objectives, progression, shops, archery system, deer hunt, and planned dragon story.

### Playable 3D World

- **Status:** Complete
- The game includes terrain, mountains, a settlement, shops, an archery range, and a grassy hunting area.

### Controllable Player and Camera

- **Status:** Complete
- The player can walk, run, crouch, jump, use a third-person camera, and switch into an over-the-shoulder aiming view.

### Five Gameplay-Related Objects

- **Status:** Complete
- Backpack
- Saddle
- Bow
- Arrows
- Archery targets
- Deer
- Shopkeepers

### Three Interaction Systems

- **Status:** Complete
- Item pickup
- Shop purchases
- Bow use
- Target activation
- Deer collection
- Menu interaction

### Progress Tracking

- **Status:** Complete
- Gold
- Arrow count
- Inventory items
- Archery target progress
- Deer collection progress
- Objective state

### Clear UI Feedback

- **Status:** Complete
- Inventory
- Objectives
- Tutorial hints
- Interaction prompts
- Gold and arrows
- Target progress
- Deer progress
- Completion screen

### Three Challenge Systems

- **Status:** Complete
- Archery accuracy
- Limited arrows and gold
- Fleeing deer AI
- Ordered progression

### Clear Gameplay Goal

- **Status:** Complete
- The player must prepare their equipment, complete archery training, and collect two deer.

### Ending and Restart Flow

- **Status:** Complete
- The game displays a completion screen and allows the player to press **SPACE** to restart.

### Audio

- **Status:** Complete, but needs improvement
- Background music is working. More sound effects and ambience are planned.

### Visual Polish

- **Status:** In progress
- Materials, character visuals, terrain, foliage, and UI styling are present, but the environment and interface still need more detail.

### Full Playthrough

- **Status:** Working
- The current game can be played from the opening tutorial through the archery challenge, deer hunt, ending screen, and restart.

---

## Development Task List

### Completed Tasks

- [x] Create a playable 3D fantasy environment.
- [x] Add a controllable third-person player.
- [x] Add a working third-person camera.
- [x] Add a player inventory system.
- [x] Track gold and arrows.
- [x] Track the backpack, saddle, and bow.
- [x] Add the backpack pickup interaction.
- [x] Add Saddler and Fletcher shop interactions.
- [x] Add saddle, bow, and arrow purchases.
- [x] Add arrow refills.
- [x] Add the player menu.
- [x] Add inventory and objective pages.
- [x] Fix cursor behavior while the menu is open.
- [x] Add objective progression.
- [x] Add tutorial guidance text.
- [x] Add background music.
- [x] Add player locomotion animations.
- [x] Add bow draw, aim, and fire animations.
- [x] Add a usable bow and projectile arrows.
- [x] Add an aiming camera.
- [x] Add five archery targets.
- [x] Track unique target hits.
- [x] Add deer AI.
- [x] Add deer hunting and collection.
- [x] Add a deer spawning zone.
- [x] Keep at least five living deer in the hunting area.
- [x] Add the tutorial completion screen.
- [x] Add scene restart with **SPACE**.

### Next Tasks

- [ ] Improve NPC dialogue and behavior.
- [ ] Add more NPCs around the settlement.
- [ ] Improve the main character model, materials, and animations.
- [ ] Improve bow and hand alignment.
- [ ] Add more grass, trees, rocks, signs, and props.
- [ ] Improve the HUD layout and styling.
- [ ] Rebuild the inventory screen with item slots and icons.
- [ ] Add more sound effects and ambience.
- [ ] Design the dragon egg recovery mission.
- [ ] Create or polish the dragon egg asset.
- [ ] Create the dragon model in Blender.
- [ ] Add the egg recovery sequence.
- [ ] Add the egg bonding and hatching sequence.
- [ ] Add the first dragon interaction and training systems.
- [ ] Continue testing the game from start to finish.

---

## Development Log

### Current Build Notes

- The player can move through the full 3D environment.
- The third-person camera and aiming camera are working.
- The player can open the menu with **TAB**.
- The inventory and objective pages work.
- Tutorial hints update throughout the entire playable sequence.
- The player can collect the backpack with **E**.
- The Saddler and Fletcher shops work.
- The player can equip, aim, and fire the bow.
- Arrow count decreases when arrows are fired.
- The Fletcher can sell additional arrows.
- The archery range tracks five unique targets.
- After archery training, the player is directed to the grassy mountains.
- At least five deer spawn in the hunting area.
- Deer wander and flee from the player.
- Defeated deer can be collected with **E**.
- The player must collect two deer.
- The completion screen appears after the final deer is collected.
- Pressing **SPACE** restarts the scene.
- Background music plays through the Audio Manager.

---

## Design Notes

This project is currently being developed as both a class prototype and a larger personal fantasy game idea. The current goal has been to build a playable opening tutorial that introduces the world, teaches the player how to interact with objects and shops, introduces ranged combat, and gives the player a complete objective.

The next part will move beyond basic training and into the story of recovering a dragon egg. That section will eventually lead to the egg hatching and the player meeting their dragon for the first time.

A large amount of future development will focus on making the world and characters feel more alive. The biggest priorities will be the NPCs, the main character, the environment, the HUD, and the inventory screen. These systems already work at a basic level, but they need more detail, stronger visual design, and better interaction before the game feels complete.

Future versions may also rename locations, factions, and story elements if the project continues beyond a school or fan prototype.
