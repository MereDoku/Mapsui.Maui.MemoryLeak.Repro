# Mapsui.Maui.MemoryLeak.Repro
This repository contains a **minimal MAUI repro project** demonstrating a memory leak in `Mapsui.Maui.MapControl` (version **5.0.2**) on **WinUI, Android and iOS**.  After navigating to a page containing `MapControl` and leaving it, the page is collected, but `MapControl` and its underlying `SKGLView` remain alive.
