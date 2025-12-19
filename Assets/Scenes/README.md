# Scenes

Unity scene files that contain the game's levels and menus.

## Expected Files

| Scene | Purpose |
|-------|---------|
| `MainGame.unity` | The primary gameplay scene with launcher, pegs, and bucket |

## Scene Contents (MainGame)

The main scene should contain:
- **Main Camera** — Orthographic camera for 2D view
- **Launcher** — At top center, player aims and fires from here
- **Pegs** — Arranged in a pattern across the play area
- **Bucket** — At bottom, moves side to side
- **UI Canvas** — Score display, balls remaining, game messages
- **GameManager** — Empty GameObject holding the GameManager script
- **ScoreManager** — Empty GameObject holding the ScoreManager script

## Questions to Consider

- Why is GameManager an empty GameObject instead of attached to something visible?
- What determines the order that objects appear in front of or behind each other?
