# Fonts

Font files for UI text rendering.

## Expected Files

- `GameFont.ttf` — Main font for scores and UI text
- `TitleFont.ttf` — Decorative font for game title (optional)

## Using Fonts in Unity

1. Import the .ttf or .otf file into this folder
2. Unity automatically creates a Font asset
3. Assign the font to TextMeshPro or legacy Text components

## TextMeshPro Note

For crisp text at any size, use TextMeshPro (included in Unity):
1. Window > TextMeshPro > Font Asset Creator
2. Select your font file
3. Generate a Font Asset
4. Use TMP_Text components instead of legacy Text
