# Tales of Alagaesia

## Project Links

- **Playable itch.io Build:** `[https://tbottc1.itch.io/tales-of-alagaesia]`
- **GitHub Repository:** `[https://github.com/tbottc1/Alagaesia]`
- **YouTube Gameplay Demo:** `[https://youtu.be/GljrznkGnUs]`

---

## Game Overview

**Tales of Alagaesia** is a third-person 3D fantasy adventure prototype made in Unity. The player takes the role of a young Rider-in-training completing the final tasks required before becoming bonded with a dragon.

The current build serves as the opening tutorial for a much larger game idea. The player explores a settlement on Vroengard, gathers equipment, interacts with NPC shopkeepers, completes archery training, hunts deer in the mountains, and finally chooses one of three dragon eggs.

The prototype includes a complete beginning, gameplay loop, challenge system, failure condition, and ending. It also includes a main menu, pause menu, inventory and objective screens, NPC shops, resource tracking, archery, simple deer AI, and dragon egg selection.

> **Project note:** This is a non-commercial educational fan prototype inspired by the world and themes of *Eragon* and *The Inheritance Cycle*. It is not affiliated with or endorsed by the official rights holders.

---

## Setting and Concept

The game takes place hundreds of years before the events of *Eragon*, during a time when dragons and Dragon Riders still worked together as warriors, scholars, protectors, and peacekeepers.

The player has trained on Vroengard and must complete one final set of tasks before receiving a dragon egg. These tasks introduce exploration, interaction, shopping, inventory management, ranged combat, hunting, and objective progression.

The egg selected at the end of the tutorial can be emerald, ruby, or sapphire. That choice is intended to determine the color and identity of the dragon that eventually hatches.

---

## Main Objective

The goal of the current build is to finish the Rider training tutorial and choose a dragon egg.

The player must:

1. Begin from the **Tales of Alagaesia** main menu.
2. Open the player menu and review the current objective.
3. Locate and collect the backpack.
4. Visit the Saddler and purchase a saddle.
5. Visit the Fletcher and purchase a bow and arrows.
6. Equip the bow and complete the five-target archery challenge.
7. Travel into the grassy mountain area.
8. Defeat and collect two deer.
9. Return to the hatchery.
10. Choose an emerald, ruby, or sapphire dragon egg.
11. Reach the completion screen and finish the tutorial.

---

## Gameplay Loop

```text
Main Menu
→ Enter the settlement
→ Check the current objective
→ Collect the backpack
→ Purchase the saddle
→ Purchase the bow and arrows
→ Equip the bow
→ Complete the archery range
→ Hunt and collect two deer
→ Return to the hatchery
→ Choose a dragon egg
→ Tutorial completion screen
```

The game uses ordered progression, so later objectives do not become available until the required earlier tasks are completed.

Gold and arrows create a resource-management challenge. Arrows are consumed when fired, and replacements must be purchased from the Fletcher. Missing too many shots can leave the player without enough arrows or gold to finish the hunt, which can trigger the tutorial failure screen.

---

## How to Play

Select **PLAY** on the main menu.

Tutorial guidance appears on the HUD and changes as objectives are completed. Press **TAB** to view inventory items and the current objective. When near an item, NPC, defeated deer, or another interactable object, press **E**.

After purchasing the bow, press **B** to pull it out. Hold the right mouse button to aim and press the left mouse button to fire. Complete the archery challenge before traveling into the mountains to hunt deer.

After collecting two deer, return to the hatchery and select a dragon egg. Confirming the egg choice completes the current tutorial.

Press **ESC** during gameplay to open the pause menu. The pause menu allows the player to resume, return to the main menu, or exit the game.

---

## Controls

| Input | Action |
|---|---|
| **WASD** | Move the player |
| **Mouse** | Control the camera |
| **Left Shift** | Run |
| **C / Left Ctrl** | Crouch |
| **Space** | Jump / restart when prompted |
| **E** | Interact, collect, talk, or purchase |
| **TAB** | Open or close the inventory and objective menu |
| **B** | Pull out or put away the bow |
| **Right Mouse Button** | Aim the bow |
| **Left Mouse Button** | Fire an arrow while aiming |
| **ESC** | Open or close the pause menu |

---

## Main Gameplay Systems

### Exploration and Objectives

The player explores the settlement, shops, archery range, mountains, and hatchery while following a sequence of tutorial objectives. HUD text and the objective menu show the current task.

### Inventory and Resources

The inventory tracks gold, the backpack, saddle, bow, arrow count, dragon egg selection, and progression.

### NPC Shops

The Saddler sells the saddle. The Fletcher sells the bow, starting arrows, and arrow refills. The player must have enough gold for each purchase.

### Archery

The player can equip the bow, enter an over-the-shoulder aiming mode, fire physical arrow projectiles, and hit five unique targets.

### Deer Hunting

Deer wander through the mountain area and flee when the player gets close. The player must defeat them with arrows, approach the defeated animal, and press **E** to collect it.

### Dragon Egg Selection

After completing the hunt, the player returns to the hatchery and chooses an emerald, ruby, or sapphire egg. A confirmation panel prevents accidental selection.

### Win and Failure States

The tutorial is completed after the player chooses an egg.

The player can fail if the deer hunt is unfinished, no arrows remain, no more arrows can be purchased, and no defeated deer remain available to collect.

---

## UI and Menus

The game includes:

- Main menu with **PLAY** and **EXIT**
- Tutorial guidance
- Interaction prompts
- Temporary dialogue and purchase messages
- Gold and arrow displays
- Inventory and objective menus
- Archery target progress
- Deer collection progress
- Dragon egg selection panel
- Tutorial completion and failure screens
- Pause menu with **Resume**, **Main Menu**, and **Exit Game**

---

## Current Ending

The current build ends after the player completes archery training, collects two deer, returns to the hatchery, and chooses a dragon egg.

The selected egg represents the player's future dragon and leads into the next stage of the game.

---

## Future Direction

The next major section will begin with the chosen egg reacting to the player and eventually hatching.

Planned future systems include:

- A dragon egg hatching sequence
- The player's first meeting and bonding scene with the dragon
- The dragon keeping the emerald, ruby, or sapphire color chosen by the player
- Feeding, caring for, and training the hatchling
- The dragon growing from a hatchling into a larger juvenile dragon
- New abilities unlocking as the dragon grows
- Ground-based dragon companionship and commands
- Learning to place and use the saddle
- Riding the dragon for the first time
- Aerial movement and free-flight systems
- Dragon-mounted exploration
- Flying challenges and Rider training
- Mounted combat and dragon abilities
- Additional settlements, islands, caves, mountains, and story areas
- More developed NPC dialogue, quests, and relationships
- A larger story involving the Riders, Vroengard, and threats to the dragons

The long-term goal is for the opening tutorial to lead into a full dragon-rider progression system where the player raises a dragon, develops a bond with it, learns to ride it, and eventually explores the world from the air.

---

## Current Features

- Playable third-person fantasy environment
- Main menu and scene loading
- Pause, resume, return-to-menu, and exit controls
- Third-person movement and camera
- Over-the-shoulder bow aiming
- Inventory and objective menus
- Backpack pickup
- Saddler and Fletcher shop interactions
- Gold and arrow tracking
- Bow equipment and projectile arrows
- Five-target archery challenge
- Wandering and fleeing deer AI
- Deer defeat and collection
- Ordered tutorial progression
- Dragon egg selection
- Completion and failure conditions
- Background music
- Fantasy-themed HUD and menu artwork

---

## External Assets and Resources

The full asset list is included in [`ASSET_CREDITS.md`](ASSET_CREDITS.md).

### Unity Asset Store Packages

| Asset | Creator/Publisher | Use |
|---|---|---|
| [Animals FREE - Animated Low Poly 3D Models](https://assetstore.unity.com/packages/3d/characters/animals/animals-free-animated-low-poly-3d-models-260727) | ithappy | Deer models and animal animations |
| [Starter Assets - ThirdPerson \| URP](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-urp-196526) | Unity Technologies | Third-person controller, input, camera, and movement reference assets |
| [UMA 2](https://assetstore.unity.com/packages/3d/characters/uma-2-35611) | UMA Steering Group | Saddler and Fletcher character generation and wardrobe system |
| [Modular Fantasy Knight Character](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/modular-fantasy-knight-character-276754) | Anton Puzanov | Main player model, armor, and modular pieces |
| [Advanced Foliage Pack 2.0](https://assetstore.unity.com/packages/3d/environments/advanced-foliage-pack-2-0-103151) | NatureManufacture | Grass, plants, and environmental foliage |

### Animations

- [Adobe Mixamo](https://www.mixamo.com/) supplied the upper-body bow animations.
- Clips used: **Draw Arrow**, **Aim Idle**, and **Shoot / Recoil**.
- The clips were configured for a Humanoid rig and blended through an upper-body Avatar Mask.

### Building Textures

- [Poly Haven](https://polyhaven.com/textures/) PBR textures were used for stone, rough wood, and thatch building materials.
- Maps included diffuse/base color, OpenGL normal, roughness, and displacement.
- The exact individual texture-set names were not recorded.

### Original Blender Assets

Created specifically for the project:

- Emerald, ruby, and sapphire dragon eggs
- Saddle
- Bow
- Arrows

### UI Artwork

- The three-dragon main-menu artwork was created with OpenAI image-generation assistance through ChatGPT.
- Fantasy parchment panels and HUD art were also created or refined with ChatGPT/OpenAI assistance.
- Unity UI and TextMeshPro were used to assemble the interface.

### Audio

- Background music is played through the Audio Manager.
- **Before submission:** add the exact track title, creator, source URL, and license to `ASSET_CREDITS.md`.
- No complete third-party sound-effect pack is currently listed.

### Code and Development Assistance

Gameplay systems, scene setup, asset integration, testing, and final design decisions were completed as part of this Unity project.

ChatGPT by OpenAI assisted with script drafting, debugging, Unity setup explanations, gameplay logic, UI planning, and documentation. ChatGPT was not the sole creator. Final integration, testing, modeling, scene construction, and creative decisions were completed by the developer.

---

## Known Limitations

- The dragon does not hatch in the current build.
- Dragon growth, riding, and flying are not implemented yet.
- NPC dialogue and animation are limited.
- Bow hand placement and animation blending could use more polish.
- Deer use simple wandering and fleeing AI.
- More sound effects and environmental ambience are needed.
- The project represents only the opening tutorial of the planned game.

---

## Running the Game

Download the Windows build from itch.io, extract the ZIP file, and run:

```text
Alagaesia.exe
```

Keep the executable, the `_Data` folder, and the included Unity runtime files together.
