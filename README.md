# done-logger

A lightweight Windows desktop application for tracking daily professional work. Logs are plain Markdown files stored locally — no network, no database, no installation required.

> Built by [dvorak-jan](https://github.com/dvorak-jan) with [Claude Code](https://claude.ai/code) (Anthropic).

---

## Installation

1. Download `DoneLogger.exe` from the [Releases](https://github.com/dvorak-jan/done-logger/releases) page.
2. Place it in any folder (e.g. `C:\Tools\DoneLogger\`).
3. In the same folder, create a `config.json` file (see [Configuration](#configuration) below).
4. Run `DoneLogger.exe` — no installer, no admin rights required.

---

## Configuration

Create `config.json` in the same folder as the executable. Use this template:

```json
{
  "dataRoot": "C:\\Users\\YourName\\Documents\\done-logger-data",
  "editor": "C:\\Windows\\System32\\notepad.exe",
  "categories": [
    { "name": "My Project", "isDefault": true },
    { "name": "Admin" }
  ]
}
```

| Field | Description |
|---|---|
| `dataRoot` | Folder where log files are stored |
| `editor` | Path to the text editor opened by "Edit Latest Log" |
| `categories` | List of work categories; exactly one must have `"isDefault": true` |

---

## Usage

### Create New Log

Creates today's log file (`YYYY-MM-DD.md`). Any items in the **What is next** section of the previous log are carried forward into the new one, and removed from the old one.

### Edit Latest Log

Opens the most recent log file in the configured text editor.

### Start Work / Stop Work

Tracks time against the selected category. The elapsed timer is shown while a session is active.

- Time is rounded to the nearest 5 minutes.
- If you forget to stop before midnight, the session is automatically split: time before midnight is written to the previous day's log, and time after midnight to the new day's.
- If no log exists for today when you click **Start Work**, one is created automatically.

A running session survives application restarts — the state is saved in `state.json` alongside the executable.

---

## Data format

Each log is a Markdown file at `<dataRoot>/YYYY/MM/YYYY-MM-DD.md`:

```markdown
# 2026-05-04

## What I did
- Reviewed the quarterly report.
- Fixed the auth regression in the API.

## What is next
- Write release notes.

## My Project
- 5h30m

## Admin
- 1h00m
```

The folder structure and file format are intentionally stable — external tools (e.g. an AI agent summarising a year of work) can read the files directly without any export step.

---

## License

Public domain — see [LICENSE](LICENSE).
