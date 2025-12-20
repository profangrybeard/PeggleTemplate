# Student Guide: Reading the Peggle Template

You're not building this game — you're reading it. Treat code like a novel: read it, notice patterns, ask questions, then make it your own.

---

## Week 1: The Basics

**Read:** `PegBehavior.cs`, `BallBehavior.cs`

### Quick Reference
- **GameObject:** A thing in your scene (the ball, a peg, the camera)
- **Component:** A behavior attached to a GameObject (script, collider, sprite)
- **Inspector:** The panel where you see and edit a GameObject's components
- **[SerializeField]:** Makes a variable editable in the Inspector

### Things to Notice
- Each script starts with a header comment explaining its one job
- Variables are grouped: REFERENCES, SETTINGS, STATE
- `Update()` runs every frame — Unity calls it automatically
- `OnCollisionEnter2D()` runs when something touches this object

### Experiments
1. Change `pointValueForThisPeg` in the Inspector. Play. What happens?
2. Change `minimumYPositionBeforeBallIsLost` to -2. What happens?
3. Remove a peg's Collider2D component. What happens when the ball reaches it?

### Questions (Answer in your own comments)
- In PegBehavior: Why check `thisPegHasAlreadyBeenHit` before doing anything?
- In BallBehavior: Why check Y position instead of using a trigger at the bottom?

---

## Week 2: Cause and Effect

**Read:** `BallLauncher.cs`, `BucketBehavior.cs`

### Things to Notice
- `Input.mousePosition` gives screen pixels, not world position
- `ScreenToWorldPoint()` converts between the two coordinate systems
- `Instantiate()` creates a new copy of a prefab at runtime
- `-transform.up` gives the direction the launcher is pointing (down)

### Experiments
1. Change `launchForceMultiplier` to 5, then to 25. How does it feel?
2. Change `maximumAimAngleInDegrees` to 30, then to 90. What's the tradeoff?
3. Double `bucketMoveSpeedInUnitsPerSecond`. Is the game easier or harder?
4. In BallLauncher, change `-transform.up` to `transform.up`. What happens? Why?

### Questions (Answer in your own comments)
- In BallLauncher: Why do we need to convert mouse position from screen to world?
- In BucketBehavior: Why use `OnTriggerEnter2D` instead of `OnCollisionEnter2D`?
- Why does the ball use `Instantiate()` but the pegs are already in the scene?

---

## Week 3: Systems Talking

**Read:** `ScoreManager.cs`, `UIController.cs`, `GameManager.cs`

### Things to Notice
- ScoreManager calculates, UIController displays — they don't do each other's jobs
- GameManager is the "boss" — other scripts report to it
- `[SerializeField]` references are dragged in the Inspector to connect scripts
- UIController has no outgoing calls — it only receives information

### Trace This Flow
1. Ball hits peg → PegBehavior calls ScoreManager
2. ScoreManager adds points → ScoreManager calls UIController
3. UIController updates the text on screen

Now trace: What happens when the ball falls off the bottom?

### Questions (Answer in your own comments)
- Why doesn't UIController calculate the score itself?
- Why does GameManager check win/lose AFTER the ball is gone, not when a peg is hit?
- What would break if PegBehavior updated the UI directly?

---

## What to Submit

### Step 1: Fork and Clone
Fork this repository to your own GitHub account. Clone it locally.

### Step 2: Annotate
Answer the question-comments throughout the code. Write your answers as new comments directly below each question.

### Step 3: Modify
Make **one behavioral change** — not just swapping sprites. Examples:
- Ball speeds up each time it hits a peg
- Pegs are worth more points the lower they are on screen
- Bucket changes direction when the ball is launched
- Ball bounces higher after hitting 5+ pegs

### Step 4: Explain
At the top of your modified script, add a comment block:
```csharp
/*
 * MY MODIFICATION: [What you changed]
 * WHY: [What effect this creates for the player]
 * HOW: [Which variables/methods you modified]
 */
```

### Step 5: Push
Commit your changes and push to your fork. Submit the repository link.

---

**Grading:** Annotation quality (25%) + Behavioral modification (35%) + Design justification (25%) + It runs (15%)
