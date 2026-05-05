# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0] - unreleased

### Added
- Standalone Windows desktop application (WinForms, .NET 8, self-contained — no installer or runtime required)
- **Create New Log** — creates today's Markdown log file; carries forward all items from the previous log's "What is next" section
- **Edit Latest Log** — opens the most recent log file in the configured external text editor
- **Start Work / Stop Work** — tracks time against a selected category with a live elapsed timer; time is rounded to the nearest 5 minutes
- Automatic log creation on Start Work if no log exists for today
- Midnight crossing handling — a session that spans midnight is split at 00:00 and time is attributed correctly to each day
- Session persistence via `state.json` — a running timer survives application restarts
- `config.json` for configuring the data root folder, external editor, and work categories (one designated as default)
- Daily log files stored as plain Markdown at `<dataRoot>/YYYY/MM/YYYY-MM-DD.md` — human-readable and consumable by external tools without any export step
- Custom checkmark application icon

### Changed
- "What is next" section is removed from the previous log after its items are carried forward to the new log (rather than being left in place with annotations)
