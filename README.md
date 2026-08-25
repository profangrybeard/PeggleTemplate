# PeggleTemplate

**GAME 220: Core Principles Programming — SCAD**

A complete, working Peggle-like prototype. You are not building this game from scratch. You are going to **read** it, understand it, and then change how it behaves.

Think of it like an English class. Before you write, you learn to read.

---

## Before You Start

Install these in order. The order matters.

| Tool | Version | Notes |
|------|---------|-------|
| **Unity Hub** | Latest | Download from [unity.com/download](https://unity.com/download) |
| **Unity Editor** | **6000.5.9f1** | Install *through Unity Hub*. Other versions may break the project. |
| **Git LFS** | Latest | [git-lfs.com](https://git-lfs.com) — **install this BEFORE you clone** |
| **GitHub Desktop** | Latest | [desktop.github.com](https://desktop.github.com) |
| **Visual Studio 2022** | Community | Include the *Game development with Unity* workload |

> **Git LFS is not optional.** This project stores images, audio, and fonts through Git LFS. If you clone before installing it, those files arrive as small text placeholders instead of real assets, and the project will look broken. If that happens: install Git LFS, then run `git lfs pull`.

---

## Setup

### 1. Fork this repository

Click **Fork** at the top right of this page. This makes your own copy under your GitHub account. You will submit *your fork*, so don't skip this.

### 2. Clone your fork

In GitHub Desktop: **File → Clone Repository**, pick your fork, choose a local folder.

Avoid folders synced by OneDrive, Dropbox, or Google Drive. They fight with Unity and cause file-lock errors.

Or from a terminal:

```bash
git lfs install && git clone https://github.com/YOUR-USERNAME/PeggleTemplate.git
```

### 3. Open the project in Unity

1. Open **Unity Hub → Add → Add project from disk**
2. Select the folder you just cloned
3. Make sure the editor version reads **6000.5.9f1**, then open it

First open takes several minutes — Unity is importing and compiling everything. This is normal. Let it finish.

### 4. Open the game scene

In the **Project** window, go to `Assets/Scenes/` and double-click **`Peggle_Prototype_01.unity`**.

If you see an empty scene with just a camera and a light, you opened `SampleScene` by mistake. Open the right one.

### 5. Press Play

You should see a grid of orange and blue pegs, a launcher at the top, and a bucket sliding along the bottom.

---

## How to Play

| Input | Action |
|-------|--------|
| **Move mouse** | Aim the launcher |
| **Left click** or **Space** | Launch a ball |

**The rules:**
- You start with **10 balls**
- Hitting any peg scores points
- Hitting several pegs with one ball multiplies your score — the combo resets every shot
- Clear all **25 orange pegs** to win
- Land a ball in the moving bucket to get that ball back
- Run out of balls before clearing the orange pegs and it's game over

---

## What Now?

Read **[STUDENT_GUIDE.md](STUDENT_GUIDE.md)**. It walks you through the code week by week and tells you what to look for.

The short version of your assignment:

1. **Read** the scripts in `Assets/Scripts/`
2. **Annotate** them — the code is full of comments that ask questions. Answer them in your own comments.
3. **Modify** one behavior. Not a sprite. A *behavior*.
4. **Explain** what you changed and why it feels different to play
5. **Push** to your fork and submit the link

**Grading:** Annotation quality (25%) + Behavioral modification (35%) + Design justification (25%) + It runs (15%)

Swapping sprites and calling it done is worth 15% at best. The grade is in the reading.

---

## The Scripts

Each file has one job. You should be able to guess what's inside before you open it.

| File | Its one job |
|------|-------------|
| `GameManager.cs` | Game state, round flow, win and lose conditions |
| `BallLauncher.cs` | Aiming and launching input |
| `BallBehavior.cs` | What a ball does after it's launched |
| `PegBehavior.cs` | How one peg reacts to being hit |
| `BucketBehavior.cs` | The moving bucket at the bottom |
| `ScoreManager.cs` | Score and combo math |
| `UIController.cs` | Putting numbers on the screen |

---

## When Something Breaks

**Missing or placeholder sprites** — Git LFS wasn't installed before cloning. Run `git lfs pull`.

**Bright magenta objects** — That's a shader that doesn't match the render pipeline. This
project uses the Universal Render Pipeline. Select the object, look at its Material, and
switch it to a URP shader.

**"The type or namespace name 'TMPro' could not be found"** — Let Unity finish importing, then **Assets → Reimport All**.

**Console errors the moment you press Play** — Read the error. It names a file and a line number. Double-click it. That habit is most of this class.

**Nothing happens when you click** — Check that you're in `Peggle_Prototype_01`, not `SampleScene`.

**Unity won't open the project** — Confirm the editor version is 6000.5.9f1 in Unity Hub.

Still stuck? Bring the exact error text to class or office hours. "It doesn't work" is not an error message.
