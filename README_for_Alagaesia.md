# Dragon Rider Prototype

## Project Overview

This project is an original 3D fantasy game prototype built in Unity. The game is inspired by the world and themes of *Eragon*, but it is being developed as a personal/class project with its own gameplay systems, environment, and progression structure.

The current prototype focuses on the opening sequence of a future Dragon Rider adventure. The player begins in a fantasy starting area where they must gather supplies and visit local craftsmen before they would eventually be ready to receive a dragon egg.

At the moment, the dragon egg and dragon are not fully part of the playable game yet. I need more time to create and polish the dragon in Blender, since I have been using Blender for many of my custom models/assets. Because of that, the current playable version ends after the player buys the bow and arrows from the Fletcher.

---

## Setting

The game takes place roughly **300 years before the rebellion of Galbatorix**. At this point in history, dragons and Dragon Riders still live in harmony and work together for the betterment of society. The Riders are respected protectors, scholars, warriors, and peacekeepers who help guide the land through their bond with dragons.

The player character has been training to become a Dragon Rider. Before receiving a dragon egg, they must complete a few final preparation tasks. These tasks include gathering a backpack, buying a saddle, and purchasing a bow and arrows.

The opening area is meant to represent an early Dragon Rider settlement where the player prepares for the larger journey that would come later in the full version of the game.

---

## Current Gameplay Goal

The player’s current goal is to prepare for their future dragon-riding journey.

The player must:

1. Open the menu with **TAB**.
2. Locate and pick up the backpack.
3. Visit the Saddler and purchase a saddle.
4. Visit the Fletcher and purchase a bow and arrows.

Once the player purchases the bow and arrows from the Fletcher, the current prototype ends. In a future version, the next step would be receiving or choosing a dragon egg, but that part is not currently implemented.

---

## Current Gameplay Loop

The current playable loop is:

1. Spawn in the starting fantasy environment.
2. Read the tutorial guidance message.
3. Press **TAB** to open the player menu.
4. Locate the backpack.
5. Press **E** near the backpack to pick it up.
6. Follow the objective/tutorial guidance to the Saddler.
7. Buy the saddle.
8. Follow the objective/tutorial guidance to the Fletcher.
9. Buy the bow and arrows.
10. Reach the current end of the prototype.

---

## Controls

| Input | Action |
|---|---|
| **WASD** | Move |
| **Mouse** | Camera control |
| **Left Shift** | Run |
| **C / Left Ctrl** | Crouch |
| **Space** | Jump |
| **E** | Interact / Pick up / Buy |
| **TAB** | Open or close the player menu |

---

## Current Project Status

### Completed / Working So Far

- Created a 3D Unity project.
- Built a fantasy-style outdoor environment.
- Imported and used fantasy foliage/environment assets.
- Imported a modular fantasy knight character model.
- Customized the player character materials so the character no longer appears as a plain gray model.
- Fixed material shader issues, including pink/magenta material problems.
- Added a controllable third-person player character.
- Added a Character Controller-based movement system.
- Added walking/running animation support through an Animator.
- Added a working third-person camera follow system.
- Added a player inventory system.
- Added gold/currency tracking.
- Added story item tracking:
  - Backpack
  - Saddle
  - Bow
  - Arrows
- Added a backpack pickup object.
- Converted the backpack to an **E-interaction** instead of automatic pickup.
- Added a Saddler shop interaction.
- Added a Fletcher shop interaction.
- Added purchase logic for the saddle.
- Added purchase logic for the bow and arrows.
- Added a player menu that opens with **TAB**.
- Added Inventory and Objective menu buttons.
- Fixed cursor locking/unlocking while the menu is open.
- Added objective tracking through the game objective system.
- Added tutorial guidance messages.
- Added background music through an Audio Manager.
- Added an animation event receiver to prevent footstep animation event errors.

---

## UI Systems

The game currently includes a basic UI system with:

- Player menu panel.
- Inventory button.
- Objective button.
- Tutorial guidance text.
- Objective updates based on player progress.

The player menu opens with **TAB**. When the menu is open, the cursor is unlocked so the player can interact with the UI. When the menu closes, the cursor returns to normal gameplay control.

The tutorial text guides the player through the early prototype:

1. Press **TAB** to open the menu.
2. Find the backpack.
3. Visit the Saddler.
4. Visit the Fletcher.
5. End the current prototype after buying the bow and arrows.

---

## Inventory and Progression

The player inventory currently tracks:

- Gold
- Backpack
- Saddle
- Bow
- Arrows
- Dragon egg placeholder status

Even though the inventory script has a dragon egg variable for future use, the dragon egg is not currently part of the playable build. It is planned for a later version once I have more time to create and polish the dragon/egg-related assets in Blender.

The current progression is:

```text
Start
→ Open menu
→ Pick up backpack
→ Buy saddle
→ Buy bow and arrows
→ Prototype ends
```

---

## Shops

### Saddler

The Saddler is the first shop the player visits after collecting the backpack. The player can interact with the Saddler and buy a saddle if they have enough gold.

### Fletcher

The Fletcher is the second shop. After purchasing the saddle, the player is guided to the Fletcher to buy a bow and arrows.

After the player purchases the bow and arrows, the current version of the game is considered complete.

---

## Audio

The project now includes a basic Audio Manager for background music.

Current audio features:

- Background music plays when the scene starts.
- The music loops during gameplay.
- The Audio Manager is set up so more sounds can be added later.

Planned future audio includes:

- Backpack pickup sound.
- Shop purchase sound.
- Interaction sound.
- Footstep sounds.
- Dragon egg discovery sound.
- Ambient forest or settlement sounds.

---

## Animation

The starting player character now has an Animator setup so the model no longer glides around the world without movement animations.

Current animation work includes:

- Animator component assigned to the character.
- Runtime Animator Controller assigned.
- Movement script sends animation parameters such as speed and grounded status.
- Basic walking/running animation support.
- Animation event receiver added for footstep animation events.

Future animation polish could include:

- Better idle animations.
- Jump animation.
- Crouch animation.
- Interaction animation.
- Pickup animation.
- Shop interaction animation.
- Bow holding or bow idle animation.

---

## Known Issues / Notes

- The dragon egg is not currently implemented in the playable prototype.
- The game currently ends after the player buys the bow and arrows from the Fletcher.
- A future version will add the dragon egg sequence once the dragon and egg assets are created or polished in Blender.
- Some systems are still prototype versions and could use more polish.
- Player movement and camera movement work, but could be improved with more advanced camera collision and free-look controls.
- Some animations are still basic and may need better transitions later.
- The environment is playable, but additional landmarks, paths, and signs would help guide the player more naturally.
- Footstep sounds are not fully implemented yet, but the animation event issue has been handled.

---

## Required Assignment Checklist

### 1. Create or continue an original 3D game project in Unity

- **Status:** Complete
- **Notes:** The project has an original fantasy Dragon Rider-style direction with its own opening sequence and gameplay progression.

### 2. Create a playable 3D world, level, terrain, arena, dungeon, track, or environment

- **Status:** Complete
- **Notes:** A playable 3D fantasy environment has been created using terrain, foliage, and imported assets.

### 3. Add a controllable player character, vehicle, object, or avatar

- **Status:** Complete
- **Notes:** The player controls a fantasy knight character using a third-person Character Controller setup.

### 4. Add a working camera system that follows, views, or supports the player during gameplay

- **Status:** Complete
- **Notes:** A third-person camera follow system is working and supports the player during gameplay.

### 5. Add at least three interactive, collectible, usable, or gameplay-related objects

- **Status:** Complete
- **Objects included:**
  - Backpack
  - Saddle
  - Bow and arrows
  - Shop interactions

### 6. Add at least one interaction system

- **Status:** Complete
- **Notes:** The game uses an **E-interaction** system for the backpack and shop purchases.

### 7. Add basic UI feedback

- **Status:** Complete
- **UI included:**
  - Player menu
  - Inventory section
  - Objective section
  - Tutorial guidance text
  - Objective/progression feedback

### 8. Add basic audio or placeholder audio

- **Status:** Complete
- **Notes:** Background music has been added through an Audio Manager.

### 9. Begin adding original design choices so the project is not an exact copy of the project built in class

- **Status:** Complete
- **Original design choices:**
  - Dragon Rider-inspired fantasy setting.
  - Player preparation sequence before receiving a future dragon egg.
  - Backpack, saddle, bow, and arrows as story progression items.
  - Saddler and Fletcher shop interactions.
  - Custom fantasy environment.
  - Blender-created/customized assets planned for future dragon content.

### 10. Make sure the project runs without major errors that prevent the player from testing the scene

- **Status:** Complete
- **Notes:** The player can move, use the camera, open the menu, collect the backpack, buy the saddle, buy the bow/arrows, and complete the current prototype.

---

## Development Task List

### Completed Tasks

- [x] Create a playable 3D fantasy environment.
- [x] Add a controllable third-person player.
- [x] Add a working third-person camera.
- [x] Add a player inventory system.
- [x] Track gold/currency.
- [x] Track backpack, saddle, bow, and arrows.
- [x] Add backpack object.
- [x] Convert backpack pickup to press-**E** interaction.
- [x] Add Saddler shop.
- [x] Add Fletcher shop.
- [x] Add saddle purchase interaction.
- [x] Add bow and arrows purchase interaction.
- [x] Add player menu.
- [x] Add inventory/objective UI buttons.
- [x] Fix cursor behavior while menu is open.
- [x] Add objective progression.
- [x] Add tutorial guidance text.
- [x] Add background music.
- [x] Add basic player animation support.
- [x] Fix animation event receiver issue.

### Future Tasks

- [ ] Create or polish the dragon egg asset.
- [ ] Create a dragon model in Blender.
- [ ] Add dragon egg selection.
- [ ] Add a dragon hatching or bonding sequence.
- [ ] Add boat/dock area for leaving the starting zone.
- [ ] Add more sound effects.
- [ ] Add better footstep audio.
- [ ] Add more polished player animations.
- [ ] Add free-look camera behavior.
- [ ] Add camera collision.
- [ ] Improve terrain paths and landmarks.
- [ ] Add more environmental storytelling.
- [ ] Add NPC dialogue.
- [ ] Add more final objective/end-game feedback.

---

## Development Log

### Current Build Notes

- The player character can move around the 3D environment.
- The camera follows the player correctly.
- The player can press **TAB** to open the menu.
- The inventory and objective menu buttons work.
- The cursor correctly unlocks when the menu is open.
- The tutorial text guides the player through the current sequence.
- The backpack is collected by pressing **E** near it.
- The Saddler shop works.
- The Fletcher shop works.
- The player inventory updates after purchases.
- Background music plays through the Audio Manager.
- The player character now has basic animation support.
- The current prototype ends after the player buys the bow and arrows.

---

## Design Notes

This project is currently being developed as a class prototype and personal fantasy game concept. The goal is to build a playable opening sequence that introduces the player to the world, gives them a clear objective, and sets up the larger idea of becoming a Dragon Rider.

The dragon egg and dragon are planned future features, but they are not included in the current playable build. I need more time to create and polish the dragon in Blender, especially because many of the custom visual pieces for this project are being created or adjusted through Blender.

Future versions may rename locations, factions, and story elements if the project is developed beyond a school/fan prototype.
