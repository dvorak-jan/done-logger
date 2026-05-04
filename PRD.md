# Product Requirements Document — done-logger

## Problem

Professionals working in corporate environments need a lightweight way to record what they did each day, plan the next day, and track how their time was spent across work categories. Existing tools either require network access, cloud accounts, installation privileges, or are too general-purpose to fit a focused daily logging habit.

## Goals

- Provide a frictionless daily log: open the app, start a work session, jot down activities, done.
- Automatically carry forward unfinished tasks to the next day.
- Track time spent per category without requiring the user to manually calculate totals.
- Keep all data in human-readable files that third-party tools (e.g. AI agents producing period summaries) can consume directly without any export step.

## Non-goals

- No sync, backup, or cloud storage.
- No network access of any kind.
- No reporting or visualisation built into the app — that is left to external tools.
- No multi-user or shared log support.

## Target environment

A single user on a corporate Windows machine. The machine may have restricted software installation policies and no internet access. The app must run as a self-contained executable requiring no pre-installed runtime or admin rights.

## Technology

- **UI:** C# WinForms, .NET self-contained publish (single deployable folder, no runtime dependency on target machine).
- **Data:** Plain Markdown files on the local filesystem.
- **Configuration:** `config.json` alongside the executable.
- **Tracking state:** `state.json` alongside the executable.

---

## Features

### F1 — Configuration

The app reads `config.json` on startup. It defines:

- `dataRoot` — root folder for all log files.
- `editor` — path to the external text editor used to open log files.
- `categories` — ordered list of work categories, each with a `name` and an `isDefault` flag. Exactly one category is the default.

There is no in-app settings UI; the user edits `config.json` directly.

### F2 — Daily log file

One Markdown file per working day, at `<dataRoot>/YYYY/MM/YYYY-MM-DD.md`. The file has a fixed structure:

```
# YYYY-MM-DD

## What I did
<free-form bullet list>

## What is next
<free-form bullet list>

## <Category name>
- XhYYm

...one section per category...
```

The file format is the external interface of the product. External tools rely on this structure and the folder hierarchy to process data by period (day / month / quarter / year).

### F3 — Create New Log

A **Create New Log** button creates today's log file:

- Errors if today's file already exists (no overwrite).
- Finds the most recent previous log, copies its "What is next" lines into the new file's "What is next" section, and appends ` [moved forward]` to each of those lines in the previous file.
- Initialises empty sections for "What I did", "What is next", and one section per configured category.

### F4 — Edit Latest Log

An **Edit Latest Log** button opens the most recent log file (by filename, not filesystem timestamp) in the configured external editor.

### F5 — Time tracking

**Start Work** button:

- Presents a category dropdown pre-selected to the default category.
- Disabled (or shows an error) if tracking is already in progress.
- If no log exists for today, automatically triggers the Create New Log flow first.
- Writes `state.json` with the start timestamp and selected category.

**Stop Work** button:

- Disabled (or shows an error) if no tracking is in progress.
- Calculates elapsed time, rounds to the nearest 5 minutes arithmetically.
- Updates the category's time entry in today's log file.
- Deletes (or clears) `state.json`.

**Midnight handling** (on Stop Work):

If the session spans midnight, the system splits it at 00:00:
1. Rounds and writes time up to midnight into the previous day's log.
2. Triggers the full Create New Log flow for the new day (including carry-forward).
3. Rounds and writes time from midnight to the stop time into the new day's log.

**App restart resilience:**

If `state.json` is present at startup, the app resumes the in-progress session as if it were never stopped. The user can then click Stop Work normally.

---

## Data files reference

| File | Location | Purpose |
|---|---|---|
| `config.json` | Executable folder | User configuration |
| `state.json` | Executable folder | Active tracking session state |
| `YYYY-MM-DD.md` | `<dataRoot>/YYYY/MM/` | Daily work log |

---

## Detailed requirements

See `requirements.md`.
