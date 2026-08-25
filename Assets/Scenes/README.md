# Scenes

Unity scene files that contain the game's levels and menus.

## Files

| Scene | Purpose |
|-------|---------|
| `Peggle_Prototype_01.unity` | **The game.** Open this one. |
| `SampleScene.unity` | Unity's empty starter scene. Nothing in it. Ignore it. |

If you press Play and nothing happens, check the tab at the top of the Scene view. You are
probably in `SampleScene`.

## Scene Contents (Peggle_Prototype_01)

- **Main Camera** — Orthographic camera for 2D view
- **MANAGERS** — Empty parent, purely for organization. Holds:
  - **GameManager** — Round flow, win and lose conditions
  - **ScoreManager** — Score and combo math
  - **UIController** — Writes numbers onto the Canvas
- **Launcher** — At top center, player aims and fires from here
- **OrganizedPegs** — Empty parent holding all 50 pegs in a 10 × 5 grid
- **Bucket** — At bottom, moves side to side
- **LeftWall / RightWall** — Keep the ball in the play area
- **Canvas** — Score display, balls remaining, game over panel
- **EventSystem** — Unity adds this automatically with any Canvas

## About Those Empty GameObjects

`MANAGERS` and `OrganizedPegs` have no components beyond a Transform. They exist only to
keep the Hierarchy readable. Collapsing `OrganizedPegs` hides 50 rows.

## Questions to Consider

- Why is GameManager an empty GameObject instead of attached to something visible?
- What determines the order that objects appear in front of or behind each other?
- The pegs are children of `OrganizedPegs`, which sits at a non-zero position. What does
  that do to each peg's *local* position versus its *world* position?
