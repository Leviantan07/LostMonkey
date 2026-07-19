# Lost Monkey — Code Architecture

All gameplay is implemented in C# under `Assets/Scripts/`, split into four
namespaces / folders. The code is intentionally simple (per the design doc:
"a handful of MonoBehaviours, no ECS, no custom event bus").

## Assemblies

- `LostMonkey.Runtime` (`Assets/Scripts/LostMonkey.Runtime.asmdef`) — all game code.
- `LostMonkey.EditModeTests` (`Assets/Tests/EditMode/…asmdef`) — EditMode tests,
  Editor-only, references the runtime assembly + NUnit.

## Namespaces & responsibilities

### `LostMonkey.Core`
| Type | Responsibility |
|------|----------------|
| `GameManager` | Persistent (`DontDestroyOnLoad`) singleton. Owns `ProgressData`, handles all scene routing (`LoadLevel`, `LoadNextLevel`, `LoadMainMenu`, `LoadLevelSelect`, `ReloadCurrentLevel`). |
| `LevelManager` | One per gameplay scene. Counts bananas (via `BananaCounter`), raises `BananasChanged` / `LevelCompleted` events, notifies `GameManager` on completion. |
| `BananaCounter` | Pure C# banana bookkeeping (unit-tested). |
| `Collectible` | A banana; on player trigger, calls `LevelManager.CollectBanana()` and self-destructs. |
| `Checkpoint` | On player trigger, sets the player's respawn point once. |
| `Hazard` / `KillZone` | Kill-on-contact / fall-catcher; call `PlayerRespawn.Die()`. |
| `MovingPlatform` | Waypoint ping-pong movement; carries riders by per-frame delta. |
| `LevelExit` | The goal; on player trigger, calls `LevelManager.CompleteLevel()`. |
| `GameConstants` | Scene names, level count, player tag, helpers. |

### `LostMonkey.Core.Progression`
| Type | Responsibility |
|------|----------------|
| `ProgressData` | Pure C# model of unlocked levels + best banana counts (unit-tested). |
| `ISaveStore` | Persistence abstraction. |
| `PlayerPrefsSaveStore` | JSON-over-PlayerPrefs implementation used in game. |

### `LostMonkey.Player`
| Type | Responsibility |
|------|----------------|
| `PlayerController2D` | Run + jump with coyote time, jump buffering, variable jump height. Yields control to `VineSwing` via `ControlEnabled`. |
| `VineSwing` | Grab a vine (layer-based), swing via a runtime `DistanceJoint2D`, release with momentum. |
| `PlayerRespawn` | Death + instant respawn at the last checkpoint (non-punishing). |

### `LostMonkey.UI`
| Type | Responsibility |
|------|----------------|
| `HUDController` | Shows the live banana count. |
| `MainMenuController` | Play / Quit. |
| `LevelSelectController` | Builds locked/unlocked level buttons from `ProgressData`. |
| `LevelCompleteController` | End-of-level panel (Next / Replay / Menu). |
| `PauseMenuController` | Escape-to-pause overlay. |

### `LostMonkey.Audio`
| Type | Responsibility |
|------|----------------|
| `AudioManager` | Persistent SFX/music hub (`PlayJump`, `PlayCollect`, `PlayDeath`, `PlayCheckpoint`, `PlayMusic`). |

## Wiring contract (how objects find each other)

- **Singletons found by static `Instance`:** `GameManager`, `AudioManager`
  (persistent), `LevelManager` (per-scene). All call sites null-check, so a
  scene works even if a manager is absent (with reduced behaviour).
- **Events:** `LevelManager.BananasChanged` → `HUDController`;
  `LevelManager.LevelCompleted` → `LevelCompleteController`.
- **Player tag:** actors detect the player with `CompareTag("Player")`
  (`GameConstants.PlayerTag`). The player object must have the `Player` tag.
- **Layers:** ground detection uses a `groundLayer` mask; vine grabbing uses a
  `vineLayer` mask. Both are assigned in the Inspector.

See [`UNITY_SETUP.md`](UNITY_SETUP.md) for the exact Editor steps.
