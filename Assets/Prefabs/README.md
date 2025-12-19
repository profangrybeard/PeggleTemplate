# Prefabs

## What Is a Prefab?

A **prefab** is a saved GameObject that lives in your Project folder, not in a scene. Think of it as a **template** or **master copy** that you can stamp into scenes as many times as you want.

### The Art Tool Comparison

If you've used any of these, you already understand prefabs:

| Tool | Feature | Prefab Equivalent |
|------|---------|-------------------|
| **Photoshop** | Smart Object | Edit the source, all instances update |
| **Illustrator** | Symbol | One master, many instances on artboard |
| **After Effects** | Precomp | Nested composition used multiple times |
| **Figma** | Component | Master component with instances |
| **Procreate** | — | No equivalent (this is why Procreate feels limiting for complex work) |

The core idea: **Change the master once, update everywhere.**

---

## The Three Big Concepts

### 1. Inheritance: Parent and Child Relationship

When you drag a prefab into a scene, you create an **instance**. That instance **inherits** everything from the prefab:

```
Peg_Blue.prefab (THE MASTER - lives in Project folder)
    │
    ├── Peg_Blue (Instance 1 - lives in Scene)
    ├── Peg_Blue (Instance 2 - lives in Scene)
    ├── Peg_Blue (Instance 3 - lives in Scene)
    └── ... (as many as you want)
```

**What gets inherited:**
- The sprite/visual appearance
- All components (scripts, colliders, rigidbodies)
- All settings on those components
- Child objects (objects nested inside)

**The magic:** If you open the prefab and change the sprite, ALL instances in ALL scenes update automatically.

**The question to sit with:** Why is this better than copying and pasting a GameObject 50 times?

---

### 2. Overrides: When an Instance Becomes Unique

Sometimes you want ONE instance to be different. Unity allows **overrides** — changes that only affect that specific instance.

```
Peg_Blue.prefab
    │
    ├── Peg_Blue (Instance 1) ─── uses prefab position
    ├── Peg_Blue (Instance 2) ─── uses prefab position
    └── Peg_Blue (Instance 3) ─── OVERRIDE: moved to (5, 3, 0)
```

In the Inspector, overridden values appear in **bold**. This tells you "this instance is different from the master."

**Common overrides in this project:**
- Position (each peg is in a different spot)
- PegType (same prefab base, different type setting)
- References (each peg needs to know about the ScoreManager in THIS scene)

**The question to sit with:** If you override the sprite on one instance, then change the prefab's sprite, what happens to that instance?

---

### 3. Instancing: Creating Copies at Runtime

So far we've talked about placing prefabs in the Scene view by hand. But prefabs have another superpower: **spawning them from code while the game is running.**

This is called **instantiation** (creating an instance).

```csharp
// This line creates a NEW copy of the ball prefab
GameObject newBall = Instantiate(ballPrefabToSpawn, spawnPosition, Quaternion.identity);
```

**What happens when this runs:**
1. Unity looks at the prefab (the master copy)
2. Unity creates a brand new GameObject with all the same components
3. Unity places it at the position you specified
4. The new object is now independent — it exists in the scene

**In this project:**
- The **Ball** prefab gets instantiated every time the player shoots
- The **Pegs** are placed by hand in the editor (not instantiated)
- The **Launcher** and **Bucket** are placed by hand (one of each)

**The question to sit with:** Why do we instantiate the ball but place the pegs by hand?

---

## Local Space vs. World Space

This concept trips up EVERYONE the first time. Here's the key insight:

### World Space: The Absolute Grid

Imagine your scene has a giant invisible grid. The center is (0, 0, 0). This is **world space** — the absolute coordinates of everything.

```
World Space
    │
    │    (0, 5, 0) ← Launcher lives here
    │
────┼──────────────── (0, 0, 0) is the center
    │
    │    (0, -5, 0) ← Bucket lives here
    │
```

When you look at a GameObject's Transform in the Inspector, you're usually seeing **world position** — where it exists on this absolute grid.

### Local Space: Relative to Parent

Now imagine you have a Launcher, and the Launcher has a child object called "SpawnPoint" (where balls come out).

```
Launcher (world position: 0, 5, 0)
    └── SpawnPoint (local position: 0, -0.5, 0)
```

The SpawnPoint's **local position** is (0, -0.5, 0). That means:
- "I am 0 units to the left/right of my parent"
- "I am 0.5 units BELOW my parent"
- "I am 0 units in front/behind my parent"

**The world position** of SpawnPoint is (0, 4.5, 0) — calculated by adding parent + local.

### Why Does This Matter?

When the Launcher rotates to aim, the SpawnPoint rotates WITH it — because it's a child. The SpawnPoint's **local** position stays the same (0, -0.5, 0), but its **world** position changes as the Launcher turns.

```
Launcher pointing straight down:
    SpawnPoint world position: (0, 4.5, 0)

Launcher rotated 45° to the right:
    SpawnPoint world position: (0.35, 4.65, 0) ← it moved!
```

**The question to sit with:** If you want the ball to spawn where the SpawnPoint is, should you use `spawnPoint.localPosition` or `spawnPoint.position`?

---

## Prefab Structure for This Project

### Ball.prefab

```
Ball
├── Sprite Renderer (the visual)
├── Circle Collider 2D (so it can hit things)
├── Rigidbody 2D (so gravity works)
└── BallBehavior.cs (the script)
```

**Key settings:**
- Rigidbody2D → Gravity Scale: affects how fast it falls
- Circle Collider 2D → Radius: size of collision area
- Tag: must be set to "Ball" (so pegs can identify it)

**The question:** Why does the ball need a Rigidbody2D but the pegs don't?

---

### Peg_Blue.prefab (base for all peg types)

```
Peg_Blue
├── Sprite Renderer (the visual)
├── Circle Collider 2D (so ball can hit it)
└── PegBehavior.cs (the script)
```

**Key settings:**
- PegBehavior → whatTypeOfPegIsThis: Blue
- PegBehavior → pointValueForThisPeg: 100
- No Rigidbody2D (pegs don't move from physics)

**The question:** If pegs don't have Rigidbody2D, how does collision detection still work?

---

### Peg_Orange.prefab, Peg_Green.prefab, Peg_Purple.prefab

These are nearly identical to Peg_Blue. The only differences:
- Different sprite (color)
- Different `whatTypeOfPegIsThis` setting
- Different `pointValueForThisPeg` value

**The question:** Should these be separate prefabs, or should there be ONE peg prefab with overrides? What are the tradeoffs?

---

### Launcher.prefab

```
Launcher
├── Sprite Renderer (the cannon visual)
├── SpawnPoint (empty child object)
│   └── Transform only (marks where ball appears)
└── BallLauncher.cs (the script)
```

**Key settings:**
- SpawnPoint is a **child object** with local position (0, -0.5, 0)
- BallLauncher → positionWhereBallSpawns: drag SpawnPoint here

**The question:** Why use an empty child object for spawn position instead of just spawning at the Launcher's position?

---

### Bucket.prefab

```
Bucket
├── Sprite Renderer (the bucket visual)
├── Box Collider 2D (set as TRIGGER)
└── BucketBehavior.cs (the script)
```

**Key settings:**
- Box Collider 2D → Is Trigger: TRUE (this is critical!)
- BucketBehavior → boundary positions for movement

**The question:** What happens if Is Trigger is FALSE? Try it.

---

## Creating a Prefab (Step by Step)

### Method 1: Scene to Project (most common)

1. Build your GameObject in the Scene
2. Add all components (sprite, collider, scripts)
3. Configure all settings
4. Drag the GameObject from Hierarchy into the Prefabs folder
5. Unity creates a .prefab file
6. The object in your scene is now an **instance** of that prefab

### Method 2: Empty Prefab (less common)

1. Right-click in Prefabs folder → Create → Prefab
2. Double-click to open in Prefab Mode
3. Build everything inside Prefab Mode
4. Save and close

**The question:** When would Method 2 be better than Method 1?

---

## Editing Prefabs

### Option A: Edit One Instance (be careful!)

1. Select an instance in the Scene
2. Make changes
3. Click "Overrides" dropdown in Inspector
4. Choose "Apply All" to push changes back to prefab

**Warning:** This applies YOUR overrides to the master. All other instances will get these changes too.

### Option B: Open Prefab Mode (safer)

1. Double-click the prefab in Project window
2. Scene view changes to show ONLY the prefab
3. Make your changes
4. Click the back arrow to exit Prefab Mode
5. All instances everywhere update automatically

**The question:** If you're working on a team, why might Option B cause fewer problems?

---

## Common Mistakes to Avoid

### Mistake 1: Forgetting to Apply Changes

You modify an instance, it looks perfect, you save the scene. But you never applied changes to the prefab. Next time you open the scene, your changes are there — but they won't appear in other scenes or new instances.

**Fix:** Always check the "Overrides" dropdown after modifying an instance.

### Mistake 2: Breaking Prefab Links

If you delete a component that the prefab has, or rename things incorrectly, you can "break" the prefab connection. The object becomes a regular GameObject, losing all prefab benefits.

**Fix:** When you see "(Missing Prefab)" in the Hierarchy, something went wrong. Undo immediately.

### Mistake 3: Circular References in Prefabs

A prefab cannot reference a specific object in a scene. Why? Because the prefab might be used in MANY scenes, and that specific object doesn't exist in all of them.

**This is why** our pegs have [SerializeField] references that you fill in PER INSTANCE:
```csharp
[SerializeField] private ScoreManager scoreManagerReference;
```

The prefab has this field, but it's empty. Each instance in each scene gets the reference dragged in manually.

**The question:** What would happen if the prefab could store a scene reference?

---

## Questions for Reflection

After reading this, sit with these questions:

1. You have 47 blue pegs in your level. You realize the collision area is too small. How do you fix all 47 at once?

2. You want ONE specific peg to be worth 500 points instead of 100. The rest should stay at 100. How?

3. The ball spawns at the wrong angle after you rotate the launcher. Should you change the ball's spawn position in world space or local space?

4. You create a new peg type (Gold, worth 1000 points). Should you duplicate an existing prefab or create from scratch? What are the tradeoffs?

5. Your bucket isn't catching balls — they pass right through. What setting did you probably forget?
