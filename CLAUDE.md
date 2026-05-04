# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

**done-logger** is a standalone Windows desktop application for tracking professional work. It is intentionally offline-only — no network access, no external services, no installer dependencies. The target environment is a corporate Windows machine that may have internet or software restrictions.

## Technology

- **UI:** C# WinForms, .NET 8, `net8.0-windows`
- **Data storage:** Plain Markdown files on the local filesystem. No database.
- **Config:** `config.json` alongside the executable (edited manually by the user).
- **Tracking state:** `state.json` alongside the executable (written/deleted by the app).

## Build and run

```bash
# From repo root
cd src/DoneLogger

# Run in development (requires config.json in the output folder)
dotnet run

# Debug build
dotnet build

# Self-contained single-file executable for distribution
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o <output-folder>
```

Before running, place a `config.json` in the executable's folder. Use `config.json.example` as the template.

## Project structure

```
src/DoneLogger/
  Models/           AppConfig, Category, TrackingState (JSON-serialisable)
  Services/
    ConfigService           loads and validates config.json
    LogService              all log file operations (create, carry-forward, time update)
    TimeTrackingService     start/stop, 5-min rounding, midnight split
  Forms/
    MainForm.cs / .Designer.cs   single application window
  Program.cs                wires up services, shows startup errors
  config.json.example       template for first-time setup
```

## Data format and folder structure

Daily log files are stored at `<dataRoot>/YYYY/MM/YYYY-MM-DD.md`:

```markdown
# 2026-05-04

## What I did
- ...

## What is next
- ...

## My Project
- 6h30m

## Admin
- 1h00m
```

The file format and folder hierarchy are a public interface — external tools (e.g. AI agents summarising a year of work) read them directly. Do not change the structure without considering downstream consumers.

## Key constraints

- **No network.** The app must function with no internet or intranet access.
- **No installation.** Runs as a self-contained executable; no runtime on the target machine required.
- **Data locality.** All data stays on the local machine.
