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

# Claude Code Instructions

## Workflow: Implementing GitHub Issues

When asked to "implement new gh issues" or similar, follow this workflow for each open, unassigned issue — one at a time, in order of issue number.

### Per-issue process

1. **Understand the problem**
   - Read the issue title, body, and all comments carefully
   - If anything is ambiguous or underspecified, ask clarifying questions *before* writing any code
   - Do not proceed to implementation until the requirements are clear

2. **Implement**
   - Make all necessary code changes
   - Follow the existing code style and conventions in the repo
   - Keep the scope tight — only what the issue asks for

3. **Update CHANGELOG.md**
   - Add an entry under the appropriate section (e.g. `## Unreleased`)
   - Format: `- <Brief description>`

4. **Update README.md**
   - **Always search README.md for any UI labels, button names, or terminology that the issue changes.**
   - Update any references that are now stale (e.g. a renamed button, a removed option, a new feature that is documented).
   - Skip only if the issue is purely internal with zero user-visible changes.

5. **Commit**
   - Stage all changed files
   - Commit message format: `<prefix>: <short lowercase summary> (issue #<issue_number>)`
     followed by a blank line and one or two sentences explaining what was done and why.
   - Prefix must be one of: `feat:`, `fix:`, `chore:`, `refactor:`, `docs:`
   - The summary after the prefix must start with a lowercase letter.
   - Example: `feat: add retry logic for failed connections (issue #42)`

### Rules
- **One issue = one commit.** Never bundle multiple issues into one commit.
- **No PRs.** Commit directly to the current branch.
- **No closing issues** via commit message keywords (`fixes`, `closes`, etc.) or the GitHub API.
- After finishing one issue, pause and confirm before moving to the next — unless told to process all of them in one go.

### After all issues are done
- Run a fresh publish build into `./publish`:
  ```bash
  cd src/DoneLogger
  dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ../../publish
  ```
