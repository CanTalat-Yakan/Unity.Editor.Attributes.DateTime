# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

# DateTime Attribute

> Quick overview: Inspector-only attributes to edit dates and times with friendly controls. Use `[Date]` on a `Vector3Int` to pick Year/Month/Day, and `[Time]` on a `float` to pick Hour/Minute/Second with a 0–24h slider.

A tiny pair of PropertyDrawers that make editing date and time values easier directly in the Inspector. Date provides Year/Month/Day dropdowns with leap‑year handling; Time provides Hour/Minute/Second selectors plus a slider.

![screenshot](Documentation/Screenshot.png)

## Features
- Date picker `[Date]`
  - Works on `Vector3Int`
  - Year (1900–2100), Month (1–12), Day (auto‑clamped for month/year, leap‑year aware)
- Time picker `[Time]`
  - Works on `float`
  - Hour/Minute/Second pickers with inline 0–24 hours slider
  - Float field stores hours as a decimal (e.g., 13.5 = 13:30:00)
- Inspector‑only, no runtime overhead

## Requirements
- Unity Editor 6000.0+ (Editor-only; attributes live in Runtime for convenience)
- Enum Drawer module (used for enum popup UI)
  - Package: `Unity.Editor.Drawer.Enum` (provides `EnumDrawer.EnumPopup`)

Tip: If the enum popup doesn’t render, make sure the Enum Drawer module is installed.

## Usage
Date (Year/Month/Day)

```csharp
using UnityEngine;
using UnityEssentials;

public class DateExample : MonoBehaviour
{
    // x = Year (index), y = Month (1–12), z = Day (1–31)
    [Date]
    public Vector3Int Birthday;
}
```

Time (0–24 hours)

```csharp
using UnityEngine;
using UnityEssentials;

public class TimeExample : MonoBehaviour
{
    // Stores hours as a decimal between 0 and 24
    [Time]
    public float StartTime; // e.g., 9.5 = 09:30:00
}
```

Combined example

```csharp
public class Schedule : MonoBehaviour
{
    [Date] public Vector3Int Date; // Year/Month/Day
    [Time] public float Time;      // 0–24 hours
}
```

## How It Works
- Date
  - Expects a `Vector3Int` and renders three dropdowns: Year, Month, Day
  - Validates Day against Month and Year (leap‑year aware)
  - Internally stores the selection back into the `Vector3Int`
- Time
  - Expects a `float` storing hours in [0, 24]
  - Renders Hour/Minute/Second selectors and a 0–24h slider
  - Converts H/M/S to a single float: `h + m/60 + s/3600`

## Notes and Limitations
- Supported field types
  - `[Date]`: `Vector3Int` only
  - `[Time]`: `float` only (current drawer targets float; `Vector3Int` time is not supported in this version)
- Year range: 1900–2100
- Serialization detail: The Date field stores its components in a single `Vector3Int`; the drawer handles leap years and clamps Day as needed
- Time precision: Seconds are supported in the picker; the `float` value reflects the exact time as fractional hours
- Editor‑only: These drawers affect Inspector rendering; they don’t change runtime behavior

## Files in This Package
- `Runtime/DateAttribute.cs` – `[Date]` attribute marker
- `Runtime/TimeAttribute.cs` – `[Time]` attribute marker
- `Editor/DateDrawer.cs` – Date picker drawer (Year/Month/Day, leap‑year handling)
- `Editor/TimeDrawer.cs` – Time picker drawer (H/M/S + 0–24h slider)
- `Runtime/UnityEssentials.DateTimeAttribute.asmdef` – Runtime assembly definition
- `Editor/UnityEssentials.DateTimeAttribute.Editor.asmdef` – Editor assembly definition

## Tags
unity, unity-editor, attribute, propertydrawer, date, time, datetime, inspector, ui, tools, workflow
