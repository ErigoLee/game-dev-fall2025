# Assignment 1 - Unity Review: Pacman
**Branch version:** `Assign1`</br>
This branch contains the Pacman project for **Assignment 1 (Unity Review)**. </br>
https://github.com/ErigoLee/game-dev-fall2025-Pacman/tree/Version1</br>

**Game Description**
- The enemy moves using NavMesh and is coded to avoid obstacles.
- When the player collects a green item, they power up for a certain period of time, freezing the enemies. During this state, if the player collides with enemies, the enemies are destroyed and the player earns points.
- The player is coded based on the previously learned character controller.
- The game is cleared when all points are collected.
- If the player collides with enemies while not in the frozen state, their heart count decreases. After three collisions, the game is over.

**Play the Game** </br>
https://erigolee.github.io/game-dev-fall2025-Pacman/Pacman_Builds/


# Assignment - 2 OOP Inventory
**Branch version:** `Assign2`</br>

https://github.com/ErigoLee/game-dev-fall2025-Pacman/tree/Version2?tab=readme-ov-file

This branch contains the Pacman project for **Assignment 2 OOP Inventory**. </br>
**Play the Game** </br>
https://erigolee.github.io/game-dev-fall2025-Pacman/Pacman_version2_Builds/

**Game Description**
- Implemented an abstract Item class and created derived classes such as AppleItem and OrangeItem, which can be collected into the inventory.
- Added an Observer pattern in GameManager so that when the player collects inventory items or frozen enemies, they are added to the inventory.
- Integrated the Observer pattern in GameManager to update game UI and play sound effects when the player collects coins, obtains a big reward, or takes damage from enemies.
- Designed enemy behavior patterns using the IEnemyState interface, creating multiple state classes such as WaitState and AttackState.

## License 
- All **source code** in this repository is licensed under the [MIT License](./LICENSE).
- Some code and assets are adapted from **Game Development II course materials**.
  Details are listed below.
- Third-party **assets** (models, textures, sounds, fonts, etc.) remain under their original licenses.
  They may **not be licensed for redistribution or in-game use** in this repository.  
  Please check the original source pages for specific license terms.

### Assets References
> Note: Third-party assets remain under their original licenses.
> They may **not be licensed for redistribution or in-game use** in this repository.  
> Please review each source page for license terms before redistribution or in-game use.


- **RGRPG Essentials Sound Effects - FREE!** </br>
  Source: https://assetstore.unity.com/packages/audio/sound-fx/rpg-essentials-sound-effects-free-227708

- **Match 3d Object Pack: Fruits & Vegetables** </br>
  Source: https://assetstore.unity.com/packages/3d/props/food/match-3d-object-pack-fruits-vegetables-284706

- **Reference code from “Unity INVENTORY: A Definitive Tutorial** </br>
  Source: https://www.youtube.com/watch?v=oJAE6CbsQQA&t=509s
