# Lost Monkey — Unity setup & wiring guide

The repository ships **all gameplay code** plus project scaffolding. What remains
is Editor work that cannot be produced outside Unity: opening the project (which
generates `ProjectSettings/` and `.meta` files), then building scenes, prefabs,
tilemaps, art and audio, and wiring script references in the Inspector.

This guide takes you from a fresh clone to a playable vertical slice.

## 0. Prerequisites

- Unity **2022.3 LTS** (matches `Packages/manifest.json`; other versions will
  auto-resolve package versions on first open).
- Modules: **2D**, **WebGL Build Support**.
- **Git LFS** installed (`git lfs install`) before adding binary assets.

## 1. First open

1. Open the project folder in Unity Hub. Unity generates `ProjectSettings/`,
   `Library/`, and `.meta` files.
2. **Commit the generated `ProjectSettings/` folder** (especially
   `ProjectVersion.txt`) — the CI build (`game-ci`) needs it to pick a Unity
   version. `Library/` stays git-ignored.
3. Confirm the scripts compiled with no errors (Console). Run the tests:
   **Window → General → Test Runner → EditMode → Run All** (5 tests should pass).

## 2. Tags & layers (one-time)

- Add a **`Player`** tag and assign it to the player GameObject.
- Add layers **`Ground`** and **`Vine`**. Assign ground/platform colliders to
  `Ground`, and vine anchor objects to `Vine`.

## 3. Bootstrap objects (persistent managers)

In your first scene (e.g. `MainMenu`), create:

- An empty **`GameManager`** GameObject + `GameManager` component.
- An empty **`AudioManager`** GameObject + `AudioManager` component, with an
  `AudioSource` (SFX) assigned; optionally a second `AudioSource` for music and
  the four default clips.

Both are `DontDestroyOnLoad`, so they only need to exist in the first scene.

## 4. Player prefab

1. Create a sprite GameObject, tag it **`Player`**.
2. Add: `Rigidbody2D` (Gravity Scale ~3, Freeze Rotation Z), a `Collider2D`
   (Capsule/Box), `PlayerController2D`, `VineSwing`, `PlayerRespawn`.
3. Add an empty child **`GroundCheck`** at the feet; assign it to
   `PlayerController2D → Ground Check`, and set `Ground Layer = Ground`.
4. On `VineSwing`, set `Vine Layer = Vine`.
5. Save as a prefab in `Assets/Prefabs`.

## 5. A level scene (`Level_01`)

1. New scene `Level_01` in `Assets/Scenes`; add to **Build Settings** (also add
   `MainMenu`, `LevelSelect`, and the other `Level_0X`).
2. Add an empty **`LevelManager`** GameObject + `LevelManager`; set
   `Level Number = 1`.
3. Build the level with a **Tilemap** (ground on the `Ground` layer). Place the
   player prefab at the start.
4. Add gameplay actors (each needs a `Collider2D`; triggers where noted):
   - **Bananas:** sprite + trigger collider + `Collectible`. Place 3.
   - **Checkpoints:** trigger + `Checkpoint`.
   - **Hazards (spikes):** collider + `Hazard`.
   - **KillZone:** a wide trigger below the level + `KillZone`.
   - **Moving platforms:** kinematic `Rigidbody2D` + collider + `MovingPlatform`,
     with 2+ waypoint Transforms assigned.
   - **Vines:** an object on the `Vine` layer at the top of each vine, with a
     small trigger collider (the swing anchor).
   - **Exit (tree):** trigger + `LevelExit`.
5. **Camera:** add a Cinemachine Virtual Camera following the player.

## 6. In-level UI

Create a Canvas with:

- A `Text` for the banana count → `HUDController.Banana Text`.
- A hidden **LevelComplete** panel (result `Text` + Next/Replay/Menu buttons) →
  `LevelCompleteController`. Wire buttons to `OnNext` / `OnReplay` / `OnMenu`.
- A hidden **Pause** panel → `PauseMenuController` (buttons → `Resume` /
  `OnRestart` / `OnMenu`).

## 7. Menu scenes

- **MainMenu:** Canvas + `MainMenuController`; Play → `OnPlay`, Quit → `OnQuit`.
- **LevelSelect:** Canvas + `LevelSelectController`; assign a `buttonContainer`
  (e.g. a Grid Layout Group) and a `Button` prefab (with a child `Text`).

## 8. Play

Press Play from `MainMenu`. Controls (legacy Input): **A/D or ←/→** move,
**Space** jump, **Left Shift** grab/swing a vine (release Shift or press jump to
let go), **Esc** pause.

## What the code already guarantees

- Banana counting, level unlock/persistence (PlayerPrefs), scene routing,
  respawn, and the swing physics are all implemented and (for the pure logic)
  unit-tested.
- Every manager call site null-checks, so partially-wired scenes still run.

## What still needs a human/artist (outside code)

- Scenes, prefabs, tilemaps, level design of the 6–8 levels.
- Art (sprites/tilesets) and audio (SFX/music).
- Tuning pass and playtests.
