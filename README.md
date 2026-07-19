# Lost Monkey 🐒

A small 2D platformer built in Unity. A monkey, separated from its troop, must
cross a ruined jungle — running, jumping and swinging on vines — to find its way
home.

> **Status:** early scaffolding. See [`GAME_DESIGN.md`](GAME_DESIGN.md) for the
> MVP scope and roadmap.

## Getting started

This repository contains the project scaffolding (folder structure, Git LFS
config, a starter player controller, CI). To turn it into a running project,
open it once with the Unity Editor (current LTS), which will generate the
`ProjectSettings/`, `Packages/` and `.meta` files.

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
  Scripts/Player/   PlayerController2D.cs (starter: run, jump, coyote time, jump buffer)
  Scripts/Core/     gameplay systems (LevelManager, Checkpoint, Collectible, ...)
  Scenes/           MainMenu, Level_01 ... , LevelComplete
  Art/              sprites & tilesets      (Git LFS)
  Audio/            SFX & music             (Git LFS)
  Prefabs/          player / collectible / checkpoint prefabs
  Settings/         input, render, quality settings
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
