# Animations

Unity animation clips and animator controllers.

## Expected Files

| Animation | Purpose |
|-----------|---------|
| `PegHit.anim` | Peg flash/glow when struck by ball |
| `PegFade.anim` | Peg fading out after being hit |
| `LauncherAim.anim` | Subtle movement on the launcher |
| `ScorePop.anim` | Score numbers appearing and floating up |
| `BucketBob.anim` | Bucket's side-to-side movement |

## Animation vs Code

Some movement in this game is code-driven (like the ball's physics). Animations are used for:
- Visual feedback that doesn't affect gameplay
- Predictable, repeating motions
- UI flourishes

## Questions to Consider

- Why might the bucket use an animation instead of code for movement?
- What's the difference between an Animation clip and an Animator Controller?
