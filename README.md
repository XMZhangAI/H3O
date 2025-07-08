
# H3O – 2-D Force-Feedback Rehabilitation Trainer

**H3O** is a Windows/WinForms application written in **C#** for upper-limb motor-rehabilitation experiments.
It drives a planar XY stage, records **two-axis force data at 20 kHz**, and guides a patient through pre-defined or user-defined trajectories (circle, square, triangle, round-trip lines) with both **active** and **passive** training modes.
The software talks to an **MP4623-series data-acquisition card** to read force sensors and to generate the step-pulse outputs that move the motors.([raw.githubusercontent.com][1])

```text
H3O
│
├── C#
│   ├── Test.sln
│   └── Test
│       ├── FrmMain.cs
│       ├── HardwareOpt.cs
│       ├── MP4623.cs
│       ├── bin/ …               (pre-built binaries & driver)
│       └── icon/ …
└──  .DS_Store                     (macOS artefacts)
```
---

## Key Features

| Category                            | Details                                                                                                                                                                                                      |
| ----------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **High-rate sampling**              | 20 kHz, 12-bit A/D via `MP4623_FAD` → `MP4623_FRead` driver calls.([raw.githubusercontent.com][2])                                                                                                           |
| **Trajectory library**              | Circle (`zdyuan`), square (`zdfang`), triangle (`zdsanjiao`), point-to-point (`zdzx`) and round-trip (`zdwangf`) routines – each rendered in real time on a `Chart` control.([raw.githubusercontent.com][3]) |
| **Active vs. passive modes**        | *Active*: patient force must exceed a configurable threshold before the stage advances.<br>*Passive*: stage drags the limb along the path at constant speed.([raw.githubusercontent.com][3])                 |
| **Force scaling & visual feedback** | Vector force components are displayed live; MathNet Numerics is used for statistics (e.g., running means).([raw.githubusercontent.com][2])                                                                   |
| **Data logging**                    | Each micro-segment’s theoretical and actual displacement is appended to a log (`sb`) for later analysis.                                                                                                     |
| **Driver abstraction**              | All low-level MP4623 P/Invoke signatures are isolated in `MP4623.cs`, making hardware substitution easier.([raw.githubusercontent.com][1])                                                                   |

---

## Hardware Requirements

| Item                     | Notes                                                                   |
| ------------------------ | ----------------------------------------------------------------------- |
| **MP4623 DAQ card**      | Analogue ±5 V / ±10 V range, 2-channel A/D, 16-bit DIO                  |
| **XY translation stage** | Step-per-motor or servo-motor pair; driven by 5 V TTL pulse/dir outputs |
| **2-axis force sensor**  | Calibrated in mV; connected to MP4623 analogue inputs                   |
| **Windows 10/11 PC**     | Visual Studio 2019/2022, .NET Framework 4.8                             |

---

## Quick Start

### 1 · Clone & build

```bash
git clone https://github.com/XMZhangAI/H3O.git
cd H3O/C#
# Open Test.sln in Visual Studio
```

> Ensure **`MP4623.dll`** is in `C#\Test\bin\Debug` or on your `%PATH%` before you run the program.([github.com][4])

### 2 · Hardware setup

1. Install the MP4623 Windows driver (supplied by the vendor).
2. Connect the DAQ card to the force sensor outputs and motor drivers.
3. Verify that `MP4623_CAL()` completes without errors (shown in the status bar).

### 3 · Running a session

| Action                    | UI element                                                                 |
| ------------------------- | -------------------------------------------------------------------------- |
| **Active circle tracing** | `主动走圆` button (button 10)                                                  |
| **Passive square**        | `被动走方` button (button 14)                                                  |
| **Emergency stop**        | `急停` button (button 13)                                                    |
| **Trajectory program**    | Enter sequence like `y03f02w01…` in the program textbox, then press **执行** |

During execution the live force vector, instantaneous deviation, and path traces (ideal vs. actual) are plotted. All data are saved to the current log file when **保存** is pressed.

---

## Directory Structure

```text
C#/
└── Test/
    ├── FrmMain.cs        # Main UI + trajectory logic
    ├── HardwareOpt.cs    # High-level card operations & helpers
    ├── MP4623.cs         # P/Invoke wrapper for MP4623 DLL
    ├── Properties/…      # Resources & settings
    └── bin/Debug         # Pre-built EXE, DLLs, driver stubs
```

---

## Extending H3O

1. **Add a new trajectory**
   Implement a routine in `FrmMain.cs` that appends theoretical points to the chart, then calls `stepline()` or `stepline1()` to stream step commands.
2. **Change sampling rate**
   Adjust `HardwareOpt.nSampleFrequency`; the `tdata` parameter in `MP4623_FAD` is derived from it.
3. **Swap hardware**
   Replace `MP4623.cs` with your own P/Invoke layer, keeping method names identical.

---

## License

The original code has no explicit license. Until clarified, treat it as **“all rights reserved”** and request permission before commercial use.

---

## Acknowledgements

This project reportedly received a **50 000 RMB XbotPark Songshan-Lake Innovation Grant** (松山湖创业投资).

---

*Last updated – 7 Jul 2025*

[1]: https://raw.githubusercontent.com/XMZhangAI/H3O/1a6a5f35e77cc20253c4cb953f8dc79378434e1f/C%23/Test/MP4623.cs "raw.githubusercontent.com"
[2]: https://raw.githubusercontent.com/XMZhangAI/H3O/1a6a5f35e77cc20253c4cb953f8dc79378434e1f/C%23/Test/HardwareOpt.cs "raw.githubusercontent.com"
[3]: https://raw.githubusercontent.com/XMZhangAI/H3O/1a6a5f35e77cc20253c4cb953f8dc79378434e1f/C%23/Test/FrmMain.cs "raw.githubusercontent.com"
[4]: https://github.com/XMZhangAI/H3O/commit/1a6a5f35e77cc20253c4cb953f8dc79378434e1f "c# · XMZhangAI/H3O@1a6a5f3 · GitHub"
