# Lost Monkey — Game Design Document (v0.1)

> Status: **MVP scoping**. This document captures the agreed direction for the
> first playable version. It is intentionally small — anything not listed under
> "MVP scope" is explicitly out of scope for v1.

## Pitch

A small monkey wakes up separated from its troop, somewhere in a ruined jungle.
It must cross a series of short levels — climbing, jumping and swinging on vines
— to find its way back to its tree.

## Design pillars

1. **Movement feels great first.** This is a platformer; the jump and the vine
   swing are the core of the experience. Everything else is secondary.
2. **Short sessions.** Levels are 1–3 minutes and easy to pick back up.
3. **Gentle, non-punishing difficulty.** Generous checkpoints, no lives system.

## MVP scope

### Mechanics (nothing more for v1)

| Mechanic | Detail |
|----------|--------|
| Movement | Run + jump, tuned with **coyote time** and **jump buffering** for good feel |
| Vine swing | The single signature mechanic tied to the "monkey" theme |
| Collectibles | Bananas (score), 3 per level, purely optional |
| Obstacles | Spikes, moving platforms, falls — **no AI enemies in the MVP** |
| Checkpoints | At least one per level; no punishing lives system |
| Level goal | Reach the exit portal / tree |

**Explicitly out of MVP:** combat, AI enemies, multiple power-ups, voice-over,
cutscenes, multiplayer, procedural generation.

### Content

- **One biome only** (ruined jungle) to stay visually coherent without
  multiplying assets.
- **6–8 short levels**, the last introducing one notable variation (e.g. a
  collapsing jungle or a timer).
- Main menu + level select + end-of-level screen.

### MVP "done when"

- [ ] Player controller feels good (run, jump, coyote time, jump buffer).
- [ ] Vine swing implemented and fun.
- [ ] 6–8 playable levels with checkpoints and a reachable exit.
- [ ] Bananas collectible and counted.
- [ ] Main menu → level select → play → end screen loop is complete.
- [ ] One art pass + basic SFX/music.
- [ ] A build is produced by CI and playable on at least one target platform.

## Technical stack

- **Engine:** Unity (2D). Use the current LTS release.
- **Physics/level:** `Rigidbody2D` + `Tilemap` for fast level design.
- **Camera:** Cinemachine (smooth follow, zero custom camera code).
- **Architecture:** deliberately simple for this scope — a handful of
  `MonoBehaviour`s (`PlayerController2D`, `VineSwing`, `Collectible`,
  `Checkpoint`, `LevelManager`). No ECS, no custom event bus in v1.
- **Placeholder art:** free packs (e.g. Kenney.nl jungle/platformer) for
  prototyping before investing in custom art.

## Suggested repository layout

```
Assets/
  Scripts/
    Player/      # PlayerController2D, VineSwing, ...
    Core/        # LevelManager, Checkpoint, Collectible, ...
  Scenes/        # MainMenu, Level_01 ... Level_08, LevelComplete
  Art/           # sprites, tilesets (Git LFS)
  Audio/         # SFX + music (Git LFS)
  Prefabs/       # player, collectible, checkpoint prefabs
  Settings/      # input actions, render/quality settings
```

## Solo dev roadmap (~6–8 weeks, part-time)

1. **Week 1** — Character controller prototype (run, jump, coyote time) in an
   empty scene, no art.
2. **Week 2** — Vine/swing mechanic + Cinemachine camera.
3. **Weeks 3–4** — Level design of the 6–8 levels with Tilemap + placeholders.
4. **Week 5** — Checkpoints, collectibles, UI (menu, level select, end screen).
5. **Week 6** — Real art pass + audio (jump/swing SFX + royalty-free ambient
   music).
6. **Weeks 7–8** — Playtests, polish (particles, light screen shake, feedback
   SFX), final build.

## DevOps (see `.github/workflows/build.yml` and `README.md`)

- **Git LFS** for all binary assets from the first content commit (already
  configured in `.gitattributes`).
- **CI build** via GitHub Actions + `game-ci/unity-builder` (requires a Unity
  license secret — see README).
- **Distribution** of playtest builds through itch.io.
- **Playtest loop:** a weekly itch.io build + a short feedback form.

## Open questions to resolve before production

These are default assumptions, to confirm as the concept firms up:

- Target platform(s): **assumed PC (WebGL for easy playtest sharing)**; mobile
  would change input (touch controls) and build targets.
- Art direction: pixel art vs. vector/hand-drawn — affects tooling and asset
  pipeline.
- Whether the vine swing is grid-anchored (fixed vine points) or free-rope
  physics — affects controller complexity.
