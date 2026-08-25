# The Input System

Read this alongside `BallLauncher.cs`. It is the only script in the project that
listens to the player.

---

## Why This Matters Now

Unity has two input systems. The old one — `Input.GetKeyDown()`, `Input.mousePosition` —
is on its way out. Most tutorials you find online still use it. **They are teaching you a
dying API.**

You are going to learn the new one first, so the old one never becomes a habit you have to
break. If you paste `Input.GetKeyDown(KeyCode.Space)` into this project, it will not
compile. That is deliberate.

Here is the same idea written both ways:

```csharp
// OLD - the code names the hardware directly
bool pressed = Input.GetKeyDown(KeyCode.Space);

// NEW - the code names the INTENT, the hardware is configured elsewhere
bool pressed = launchAction.WasPressedThisFrame();
```

The old line can only ever be the spacebar. The new line is the spacebar, the left mouse
button, a gamepad trigger, or all three — and you change that without touching the code.

---

## Four Words

Almost everything in the new system is one of these:

| Word | What it means | Example |
|------|---------------|---------|
| **Device** | A physical thing the player touches | Mouse, Keyboard, Gamepad |
| **Control** | One part of a device | `leftButton`, `space`, `position` |
| **Binding** | A path pointing at a control | `<Mouse>/leftButton` |
| **Action** | A named thing the player can DO | `Launch`, `AimPosition` |

An **Action** holds a list of **Bindings**. Each **Binding** is a path to a **Control** on
a **Device**.

```
launchAction  ("Launch")
    ├── <Mouse>/leftButton
    └── <Keyboard>/space
```

Both bindings feed one action. `BallLauncher.cs` asks the action one question and both
work. Look at `CheckForLaunchInput()` — one line, two devices.

**The question to sit with:** Why is it better for the code to know about "Launch" than to
know about the spacebar?

---

## Reading the Bindings

Those angle-bracket paths are readable once you know the shape:

```
<Mouse>/leftButton
 ▲       ▲
 │       └── the control on that device
 └────────── the device layout
```

More examples:

| Binding path | Reads as |
|---|---|
| `<Keyboard>/space` | the spacebar |
| `<Mouse>/position` | where the pointer is (a Vector2) |
| `<Gamepad>/buttonSouth` | A on Xbox, X on PlayStation |

That last one is the point of the whole system. `buttonSouth` means "the bottom face
button" on *any* gamepad. You never write a brand name.

---

## Two Kinds of Action

`BallLauncher` uses one of each, and the difference matters.

**Button** — a thing that happens at a moment.

```csharp
bool playerTriggeredLaunchThisFrame = launchAction.WasPressedThisFrame();
```

`WasPressedThisFrame()` is true for exactly one frame, no matter how long the button is
held. It is the new `GetKeyDown`.

**Value** — a thing that always has a reading, like a position or a stick direction.

```csharp
Vector2 pointerScreenPosition = aimPositionAction.ReadValue<Vector2>();
```

`ReadValue<Vector2>()` asks "what is your value right now?" The pointer is always
somewhere, so this always answers.

**The question to sit with:** Would `WasPressedThisFrame()` make any sense on
`aimPositionAction`? Would `ReadValue<Vector2>()` make sense on `launchAction`?

---

## Turning Actions On

This trips up everyone once:

```csharp
private void OnEnable()
{
    aimPositionAction.Enable();
    launchAction.Enable();
}

private void OnDisable()
{
    aimPositionAction.Disable();
    launchAction.Disable();
}
```

**An action reads nothing until it is enabled.** If you add an action and it seems dead,
this is almost always why.

`OnEnable()` runs every time the object switches on — including after a disable. `Start()`
runs once, ever. An action switched off and never switched back on is a bug that only
shows up the second time.

### Experiment
Comment out both lines in `OnEnable()` and press Play. The launcher stops responding
completely — no aiming, no firing. No error appears in the Console. **Nothing tells you
what is wrong.** Get familiar with that silence now; you will meet it again.

---

## Where the Bindings Live

Select the **Launcher** in the Hierarchy and find **Ball Launcher** in the Inspector. The
two actions are there. Click the arrow next to one to open its bindings.

You can change what fires the launcher without opening a single script.

### Experiments

1. Add `<Keyboard>/enter` as a second binding on **Launch**. Play. Both still work.
2. Change **Launch** to `<Keyboard>/escape`. Play. Note that nothing in `BallLauncher.cs`
   changed and the game still works.
3. Delete every binding on **AimPosition**. Play. What happens to aiming — an error, or
   silence?
4. If you have a gamepad, add `<Gamepad>/buttonSouth` to **Launch** and plug it in.

---

## What To Do When Input Breaks

Input failures are usually quiet. Work down this list:

1. **Is the action enabled?** Check `OnEnable()`.
2. **Does the action have a binding?** An action with no bindings is legal and silent.
3. **Is the binding path right?** `<Mouse>/leftbutton` is not `<Mouse>/leftButton`.
4. **Is the type right?** Reading a `Vector2` from a Button action returns zero.
5. **Is the object active?** A disabled GameObject never runs `OnEnable()`.

Open **Window → Analysis → Input Debugger** to watch devices and actions live while the
game runs. It will tell you what Unity is actually seeing.

---

## Questions (Answer in your own comments)

- In `AimTowardPointer()`, why does `ReadValue<Vector2>()` need to become a `Vector3`
  before `ScreenToWorldPoint()` will take it?
- `CheckForLaunchInput()` handles two devices in one line. What would the old system have
  required?
- The launcher's actions are fields on the launcher. What would break if two different
  scripts each had their own `launchAction`?
- Why does this project set **Active Input Handling** to *Input System Package (New)*
  instead of *Both*?
