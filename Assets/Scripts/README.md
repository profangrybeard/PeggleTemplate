# Scripts

C# scripts that control game behavior. Each script has one job.

## All Scripts

| Script | Responsibility |
|--------|----------------|
| `GameManager.cs` | Overall game state and round flow |
| `BallLauncher.cs` | Player aiming and ball launching input |
| `BallBehavior.cs` | Ball physics and lifetime |
| `BucketBehavior.cs` | Moving bucket that catches balls |
| `PegBehavior.cs` | Individual peg collision response |
| `ScoreManager.cs` | Score tracking with combo multiplier |
| `UIController.cs` | Display layer for score, balls remaining, etc. |

## Reading Order for Students

Read the scripts in this order. Each one builds on concepts from the previous.

### 1. Start with PegBehavior.cs
**Why first?** It's the simplest — just responds to one thing (collision).
- Notice how it only does ONE thing when hit
- Look for the enum at the top — what is it for?
- Find the "huh?" moment: why use a flag instead of destroying immediately?

### 2. Then BallBehavior.cs
**Why second?** It shows what happens after the ball is launched.
- Notice it checks position every frame
- Find the "huh?" moment: why check Y position instead of using a trigger?

### 3. Then BucketBehavior.cs
**Why third?** Simple movement with simple trigger detection.
- Notice the pattern: move, check boundary, flip direction
- Find the "huh?" moment: why trigger instead of collision?

### 4. Then BallLauncher.cs
**Why fourth?** Now see how the ball gets INTO the scene.
- Notice the auto-aiming pattern (no mouse needed)
- Find the "huh?" moment: why separate angle from force?

### 5. Then ScoreManager.cs
**Why fifth?** See how hitting pegs becomes points.
- Notice the combo multiplier — what does it do?
- Find the "huh?" moment: why track pegs hit THIS shot?

### 6. Then UIController.cs
**Why sixth?** See how data becomes visible.
- Notice it ONLY displays — never calculates
- Find the "huh?" moment: why doesn't it calculate anything?

### 7. Finally GameManager.cs
**Why last?** This is the most complex — it connects everything.
- Notice how other scripts call INTO this one
- Find the "huh?" moment: why check win/lose AFTER the ball is gone?

## How Scripts Talk to Each Other

```
BallLauncher ──────► GameManager ◄────── BallBehavior
                          │
                          ▼
PegBehavior ─────► ScoreManager ──────► UIController
     │
     └──────────► GameManager

BucketBehavior ───► GameManager
```

Arrows point toward the script being called. Notice: UIController has NO outgoing arrows.
