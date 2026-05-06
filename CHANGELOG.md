# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0] - unreleased

### Added
- Standalone Windows desktop application — self-contained executable, no installer or runtime required
- Daily log creation with automatic carry-forward of "What is next" items from the previous log
- One-click access to the latest log in a configured external text editor
- Time tracking per work category with a live elapsed timer
- Automatic log creation when starting a work session if no log exists for today
- Correct handling of sessions that span midnight — time split at 00:00 and attributed to the respective days
- Tracking session persisted to `state.json` so a running timer survives application restarts
- `config.json` for configuring the data root folder, external editor, and named work categories
- Plain Markdown log files at `<dataRoot>/YYYY/MM/YYYY-MM-DD.md` — readable by external tools without any export step
