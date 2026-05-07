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

The application has two tabs: **Work** for everyday use and **Advanced** for less frequent operations.

### Work tab

**Edit Latest Log** — opens the most recent log file in the configured text editor.

**Start Work / Stop Work** — tracks time against the selected category. The elapsed timer is shown while a session is active.

- Time is rounded to the nearest 5 minutes.
- If you forget to stop before midnight, the session is automatically split: time before midnight is written to the previous day's log, and time after midnight to the new day's.
- If no log exists for today when you click **Start Work**, one is created automatically.

A running session survives application restarts — the state is saved in `state.json` alongside the executable.

### Advanced tab

**Create New Log** — creates today's log file (`YYYY-MM-DD.md`). Any items in the **What is next** section of the previous log are carried forward into the new one, and removed from the old one. (This also happens automatically when **Start Work** finds no log for today.)

**Custom time** — enter a time in `HH:mm` format and use the **Start Work** / **Stop Work** buttons here to record a session that started or ended at a specific time rather than the current moment. Useful for logging work retrospectively or in advance.

**Open log by date** — select any date from the dropdown (only dates with an existing log file are listed) and click **Open Log** to open it in the configured editor.

### Summary tab

Generates a Markdown summary for a chosen date range and opens it in the configured editor.

- Set the **From** and **To** dates manually and click **Create Summary**, or use one of the quick-select buttons: **Today**, **Yesterday**, **This Week**, **This Month**, **Last Week**, **Last Month**, **This Year**, **Last Year**.
- Quick-select buttons are disabled when no log data exists for that period.
- The summary aggregates all **What I did** items from each day in the range, shows time totals per category, and adds an overall **Total**.
- The output is written to `summary.md` in the same folder as the executable, overwriting any previous summary.

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
