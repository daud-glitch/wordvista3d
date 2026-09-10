# WordVista 3D: Crossword Adventure

[![Build Android APK](https://github.com/daud-glitch/wordvista3d/actions/workflows/build-android.yml/badge.svg)](https://github.com/daud-glitch/wordvista3d/actions/workflows/build-android.yml)

An original 3D crossword puzzle adventure game for Android and Unity.
> *"Every word brings a beautiful 3D world to life."*

---

## Highlights

- **250 Validated Levels**: Progressive difficulty across 8 scenic worlds.
- **8 Scenic 3D Worlds**: Green Valley, Mystic Forest, Crystal Lake, Desert Kingdom, Snow Valley, Tropical Island, Sky Kingdom, and Aurora World.
- **Dynamic Environmental Transformation**: Solving words and completing levels awakens 3D world elements (blooming wildflowers, flowing rivers, bridges, ancient temple pillars, and glowing crystals).
- **Smooth Letter Wheel**: Radial stone wheel supporting 3 to 7 letters, smooth drag tracking, connection lines, backtracking, and shuffle.
- **Dimensional Crossword Grid**: Responsive mobile layout with solve bounce animations and particle shimmer.
- **Coins, Hints & Bonus Words**: Reveal Letter (25), Smart Hint (40), Reveal Word (100), and bonus word coin rewards.
- **Offline Campaign & Daily Puzzles**: 100% offline playability with calendar-based daily challenges.
- **Relaxing Audio**: Calming ambient music tracks and tactile game sound effects.
- **Android Target**: Locked portrait orientation, ARM64 target, GLES3, and URP quality profiles.

---

## How to Play in Unity

1. Clone or open the repository folder `WordVista3D` in **Unity 6** (or stable Unity 2022/2023).
2. Open `Assets/Scenes/Bootstrap.unity` or `MainMenu.unity`.
3. Hit **Play**.

---

## Automated Tests

Run the test suite from the terminal:
```bash
dotnet test WordVista.Tests/WordVista.Tests.csproj
```
All 250 levels, crossword intersections, and game logic pass with 100% success.
