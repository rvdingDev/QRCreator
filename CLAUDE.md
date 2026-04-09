# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
dotnet build QRCreator/QRCreator.csproj
dotnet run --project QRCreator/QRCreator.csproj
```

Solution file is `QRCreator.slnx` (.NET 10 XML format, not legacy `.sln`).

## Tech Stack

- .NET 10 (`net10.0-windows`), `<LangVersion>preview</LangVersion>`
- WPF-UI 4.x (Fluent Design) — `xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"`
- CommunityToolkit.Mvvm 8.x — `[ObservableProperty]`, `[RelayCommand]` source generators
- SkiaSharp 3.x — note: 3.x removed `SKPaint.FilterQuality`, use `SKSamplingOptions` instead
- QRCoder 1.x — `QRCodeGenerator` and `QRCodeData` are NOT `IDisposable`; `ModuleMatrix` is `List<BitArray>`

## Architecture

**Rendering pipeline:** `QRCoder (bool[,] matrix)` → `QrRenderer.Render()` → `SKBitmap` → `SkiaInterop.ToBitmapSource()` → WPF `Image` binding

**Key design decisions:**
- `ICellRenderer` abstracts cell/finder drawing for extensibility (new shapes, SVG export)
- `QrRenderer` is static, detects finder pattern regions (7x7 at three corners) automatically
- Logo uses transparent masking (cells under logo area are skipped, not overdrawn)
- Preview uses `SKBitmap → PNG → BitmapSource` (not SKElement), so preview and export share the same pipeline
- ECC level is fixed at H (30%) — not user-configurable
- URL input debounces 300ms; all other property changes trigger immediate regeneration

**ViewModel:** `QrDesignViewModel` owns all state. Property change hooks (`partial void On*Changed`) trigger `RegenerateQr()`. Color palette buttons use code-behind click handlers in `MainWindow.xaml.cs` that set ViewModel properties directly.

## WPF-UI Controls

Use WPF-UI controls: `ui:FluentWindow`, `ui:TitleBar`, `ui:Card`, `ui:InfoBar`, `ui:Button`, `ui:TextBox`. Standard WPF `Slider` and `ToggleButton` get Fluent styling automatically via `ControlsDictionary`. Theme switching uses `ApplicationThemeManager.Apply()`.

## Converters

- `EnumToBooleanConverter` — for ToggleButton groups bound to enum properties
- `IntToBooleanConverter` — for ExportScale (int) ToggleButton group
- `ColorToHexConverter` — two-way `Color` ↔ `#RRGGBB` string
