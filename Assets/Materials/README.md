# Materials

Unity materials for special visual effects on 2D sprites.

## Expected Files

| Material | Purpose |
|----------|---------|
| `PegGlow.mat` | Additive glow effect for hit pegs |
| `BallTrail.mat` | Trail renderer material for ball |

## Why Materials in a 2D Game?

Even in 2D, materials control how sprites are rendered:
- **Additive blending** — Makes sprites glow by adding light
- **Multiply blending** — Darkens underlying pixels
- **Custom shaders** — Special effects like outlines or distortion

## Default Sprite Material

Most sprites use Unity's default `Sprites-Default` material. Only create custom materials when you need special rendering behavior.
