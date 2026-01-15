# Mapsui.Maui MapControl Memory Leak Repro

This repository contains a **minimal MAUI repro project** demonstrating a memory leak in `Mapsui.Maui.MapControl`
(version **5.0.2**) on **WinUI, Android and iOS**.

After navigating to a page containing `MapControl` and leaving it, the page is collected,
but `MapControl` and its underlying `SKGLView` remain alive.

---

## Affected versions
- Mapsui.Maui: **5.0.2**
- Mapsui: **5.0.2**
- MAUI: <TO_FILL>
- .NET: <TO_FILL>
- SkiaSharp: <TO_FILL>

---

## How to reproduce

1. Clone this repository
2. Build and run on **WinUI / Android / iOS**
3. From the first page, tap **"Open Map Page"**
4. Navigate back
5. Trigger `VisualLeakCheckQueue.Monitor()`
6. Observe that `MapControl` and `SKGLView` are not collected

---

## Leak detector output

### WinUI

MapPage => ✅ Collected

    Grid => ✅ Collected

        MapControl => 💦 Leak

            SKGLView => 💦 Leak


### Android

[0:] MapPage => ✅ Collected
[0:] - Grid => ✅ Collected
[0:] - MapControl => 💦 Leak
[0:] - SKGLView => 💦 Leak


### iOS

[0:] MapPage => ✅ Collected
[0:] - Grid => ✅ Collected
[0:] - MapControl => 💦 Leak
[0:] - SKGLView => 💦 Leak


---

## Notes
- The leak occurs even when navigating back and forcing GC.
- The repro uses a custom weak-reference based leak detector (`VisualLeakCheckQueue`).
- A fix has been implemented in a fork and will be proposed via Pull Request.

---

## Expected behavior
After leaving the page and running GC, `MapControl` and `SKGLView` should be collected.
