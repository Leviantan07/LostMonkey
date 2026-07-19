# Lost Monkey 🐒

A small 2D platformer built in Unity. A monkey, separated from its troop, must
cross a ruined jungle — running, jumping and swinging on vines — to find its way
home.

> **Status:** early scaffolding. See [`GAME_DESIGN.md`](GAME_DESIGN.md) for the
> MVP scope and roadmap.

## What's implemented

All MVP **gameplay code** is written and organised into assemblies:

- **Player:** `PlayerController2D` (run/jump, coyote time, jump buffer, variable
  jump), `VineSwing` (the signature swing mechanic), `PlayerRespawn`.
- **Level systems:** `Collectible` (bananas), `Checkpoint`, `Hazard`, `KillZone`,
  `MovingPlatform`, `LevelExit`, `LevelManager`.
- **Meta / flow:** `GameManager` (scene routing), progression & save system
  (`ProgressData` + PlayerPrefs), `AudioManager`.
- **UI:** main menu, level select (with locking), HUD, pause, level-complete.
- **Tests:** EditMode unit tests for the pure logic (banana counting, level
  progression, save round-trip) under `Assets/Tests/EditMode`.

See [`ARCHITECTURE.md`](ARCHITECTURE.md) for how it all fits together.

## What still needs the Unity Editor

Scenes, prefabs, tilemaps, art, audio and level design **cannot** be produced
outside Unity. Follow [`UNITY_SETUP.md`](UNITY_SETUP.md) to open the project
(which generates `ProjectSettings/` and `.meta` files) and assemble a playable
level by wiring the existing scripts in the Inspector.

## Getting started

Open the project once with the Unity Editor (2022.3 LTS recommended, matching
`Packages/manifest.json`). Unity generates the `ProjectSettings/`, `Library/`
and `.meta` files. Then run the EditMode tests via **Window → General → Test
Runner** and follow [`UNITY_SETUP.md`](UNITY_SETUP.md).

### Prerequisites

- **Unity** (latest LTS) with the **2D** and **WebGL Build Support** modules.
- **Git LFS** — binary assets (art, audio, models) are stored via LFS:

  ```bash
  git lfs install
  git clone https://github.com/Leviantan07/LostMonkey.git
  ```

  If you cloned before installing LFS, run `git lfs pull` afterwards.

### Project layout

```
Assets/
  Scripts/
    Player/         PlayerController2D, VineSwing, PlayerRespawn
    Core/           GameManager, LevelManager, Collectible, Checkpoint, Hazard,
                    KillZone, MovingPlatform, LevelExit, BananaCounter, ...
    Core/Progression/  ProgressData, ISaveStore, PlayerPrefsSaveStore
    UI/             MainMenu, LevelSelect, HUD, Pause, LevelComplete controllers
    Audio/          AudioManager
    LostMonkey.Runtime.asmdef
  Tests/EditMode/   EditMode unit tests (banana counter, progression, save)
  Scenes/           MainMenu, LevelSelect, Level_01 ...   (create in Editor)
  Art/              sprites & tilesets      (Git LFS)
  Audio/            SFX & music             (Git LFS)
  Prefabs/          player / collectible / checkpoint prefabs (create in Editor)
  Settings/         input, render, quality settings
Packages/manifest.json   package dependencies (Cinemachine, 2D, test framework)
```

## Continuous integration

[`.github/workflows/build.yml`](.github/workflows/build.yml) builds a WebGL
player with [game-ci/unity-builder](https://game.ci/).

It needs three repository secrets. Until they are set, the build **self-skips**
with a green result (it does not fail):

| Secret | How to get it |
|--------|---------------|
| `UNITY_LICENSE` | Activate a personal license and copy the contents of the generated `.ulf` file. See the [game-ci activation guide](https://game.ci/docs/github/activation). |
| `UNITY_EMAIL` | Your Unity account email. |
| `UNITY_PASSWORD` | Your Unity account password. |

Add them under **Settings → Secrets and variables → Actions**.

## Distribution / playtesting

Playtest builds are intended to be shared via [itch.io](https://itch.io) — the
WebGL artifact produced by CI can be uploaded there for quick, link-based
playtests.

## License

TBD.
