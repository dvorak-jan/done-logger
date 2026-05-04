# Daily Activity & Time Logging System — Requirements
 
## 1. Work Categories
 
1.1. The system shall support a configurable list of **work categories** (e.g. "My Project", "Admin").
 
1.2. The number of categories shall not be fixed; the system must work with any number of categories ≥ 1.
 
1.3. One category shall be designated as the **default category** (used when no category is explicitly specified during time tracking).
 
---

## 2. Configuration File

2.0. Configuration is stored in `config.json`, located in the same folder as the application executable.

2.1. `config.json` shall contain the following fields:

- **dataRoot** — absolute path to the folder under which the `YYYY/MM/` log structure is maintained.
- **editor** — absolute path to the external text editor executable used by the 'Edit Latest Log' action.
- **categories** — ordered list of work category objects, each with:
  - **name** — category name, used as the `## ` section heading in log files.
  - **isDefault** — boolean; exactly one category shall have this set to `true`.

---
 
## 3. Log File Format
 
3.1. Each daily log is a Markdown file named `YYYY-MM-DD.md`, stored in `<dataRoot>/YYYY/MM/`.
 
3.2. A daily log file shall have the following structure (in order):
 
```
# YYYY-MM-DD
 
## What I did
<free-form bullet list of activities>
 
## What is next
<free-form bullet list of planned tasks>
 
## <Category 1 name>
<time entry, e.g. "- 6h30m">
 
## <Category 2 name>
<time entry>
 
... (one section per configured category)
```
 
3.3. Time entries under category sections use the format `- XhYYm` (e.g. `- 8h00m`, `- 1h30m`).

3.4. All sections are delimited by `## ` headings. Content within a section is free-form text (typically Markdown bullet lists).
 
---
 
## 4. Creating a New Daily Log

4.0. UI has a button with 'Create New Log' text that triggers actions described below.

4.1. The new log file shall be named using the **current date** in `YYYY-MM-DD` format.
 
4.2. **Guard: no overwrite** — If a file with today's date already exists, abort with an error message and do not overwrite it.
 
4.3. **Task carry-forward** — When a new log is created:
  - a) Find the most recent existing log file.
  - b) Extract all lines from the "What is next" section of that previous log.
  - c) Insert those lines into the new log's "What is next" section.
  - d) In the **previous** log file, annotate each line in the "What is next" section by appending ` [moved forward]`.
 
4.4. The new file shall contain sections for "What I did", "What is next", and one section per configured category (using each category's name as the section heading).
 
---
 
## 5. Editing the Latest Log

5.0. UI has a button with 'Edit Latest Log' text that triggers actions described below.
 
5.1. Open the most recent `.md` file (most recent by name, not date/time of saving) in the configured external text editor.
 
---
 
## 6. Time Tracking
 
6.0. Time tracking uses the current day record to keep track of ongoing activities. Individual records in the "What I did" section have neither time duration nor category assigned.

6.1. UI has a button that makes it possible to start tracking time (Start Work button). Times are rounded to 5 mins arithmetically (2 mins = 0 mins, 3 mins = 5 mins). When clicking the button, the user can select a category from a dropdown, the default one being preselected. There is an error message (or the button is greyed out) if time tracking is already in progress. If no log exists for today, the Start Work action shall automatically trigger the Create New Log flow (see section 4) before starting the timer.

6.2. UI has a button that makes it possible to stop tracking time (Stop Work button). Times are again rounded and there is an error message if no time is being tracked and the button is clicked (or the button is greyed out).

6.3. Upon clicking 'Stop Work', if the elapsed period crosses midnight, the system shall:
  - a) Split the work period at midnight.
  - b) Complete the previous day's log with time up to midnight (updating its category section).
  - c) Trigger the full Create New Log flow (see section 4) for the new day, including carry-forward.
  - d) Record the remaining time (from midnight to stop) in the new day's log under the same category.

6.4. Upon the completion of each work period, the respective category's accumulated time in the current log file is updated.

6.5. Active tracking state is persisted in a `state.json` file stored alongside `config.json`. This allows the program to be stopped and restarted without losing the running timer. The file stores at minimum: whether tracking is active, the start timestamp (full date and time), and the selected category. The file is written on Start Work and deleted (or cleared) on Stop Work.
