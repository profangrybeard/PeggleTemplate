# Audio

Sound effects and music for the game.

## Subfolders

### SFX/
Short sound effects triggered by game events:
- `ball_launch.wav` — When player fires the ball
- `peg_hit_01.wav` through `peg_hit_05.wav` — Peg hit sounds (rising pitch)
- `ball_lost.wav` — Ball falls out of play
- `bucket_catch.wav` — Bucket catches ball for free ball
- `level_complete.wav` — Victory fanfare
- `level_failed.wav` — Out of balls

### Music/
Background music loops:
- `gameplay_loop.ogg` — Main gameplay music

## Why Multiple Peg Hit Sounds?

In Peggle, each consecutive peg hit plays at a higher pitch, creating a musical scale effect. This is called "progressive audio" and makes hitting many pegs feel rewarding.

## Import Settings

- Load Type: Decompress On Load (for short SFX)
- Load Type: Streaming (for music)
- Compression Format: Vorbis for music, PCM for short SFX
