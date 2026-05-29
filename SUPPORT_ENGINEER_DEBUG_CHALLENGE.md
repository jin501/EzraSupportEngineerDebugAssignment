# Support Engineer Debug Challenge

> **PDF:** Open `docs/Support-Engineer-Debug-Challenge.html` in a browser and use File → Print → Save as PDF. See `docs/README.md` for details.

## Overview

This exercise is designed to simulate real Support Engineering work: **triage → reproduce → diagnose → fix safely → communicate clearly**.

You'll work in a small, prebuilt application that has a few realistic "production" issues. Your job is to investigate and ship fixes, plus the operational artifacts we'd expect from someone supporting production software.

- **Timebox:** ~2–4 hours (please don't spend more unless you want to and just note it if you do)
- **AI tools:** Allowed. Please be prepared to explain every change and the reasoning behind it.

---

## Scenario

Assume this app is running in production and you're on call. We've received customer reports:

1. **"Sometimes creating a task fails with a 500."**
2. **"The tasks list is slow for some users."**
3. **"We've seen tasks appear duplicated or out of order after refresh."**
4. **"We're seeing 500 errors on task creation in production but cannot reproduce in our environment. Engineering has attached a log snippet from a failed request (see `artifacts/sample_api_log.txt`). Please use the logs to identify the cause and fix it."**

Not all reports may be accurate and part of the exercise is determining what's real, what's reproducible, and what you'd do next.

**Important:** At least one issue (in particular the create-task 500) is intended to be diagnosed **using the provided log artifacts**. You should inspect stack traces, request context, and structured log lines in `artifacts/` to find the root cause before implementing a fix. The runbook you write should describe how to use logs in production to diagnose these issues.

---

## Repo

**Repository:** https://github.com/AlexFH100/SupportEngineerDebugAssignment

### What's included

- .NET 8 API + SQLite database
- Minimal UI served by the API
- Small test suite (some tests may fail initially)
- Templates for deliverables:
  - `RUNBOOK.md` (update this)
  - `INCIDENT.md` (fill this in)
  - `TICKET.md` (write a follow-up ticket)
- **`artifacts/`** folder with **log snippets and context** (e.g. `sample_api_log.txt`, `sample_slow_list_log.txt`, `customer_report.md`). Use these logs to diagnose at least one of the issues.

### Getting started

**Requirements**

- .NET SDK 8.x

**Run the API + UI**

From the API project directory:

```bash
dotnet restore
dotnet run
```

You'll see output like "Now listening on: …" with the port. It may be 5000 or another port depending on your environment.

**Open the app**

- **UI:** http://localhost:\<PORT>/
- **Swagger:** http://localhost:\<PORT>/swagger

**Run tests**

From the repo root:

```bash
dotnet test
```

---

## Your tasks

### 1) Triage & reproduction

- Get the app running locally
- **Use the provided log artifacts** (e.g. `artifacts/sample_api_log.txt`) to diagnose at least one issue — document what you found in the logs and how it led to the fix
- Determine which issues you can reproduce
- Write clear repro steps for each confirmed issue
- Capture useful evidence (logs, stack traces, screenshots, etc.)

### 2) Root cause analysis

For each confirmed issue, explain briefly:

- What is happening
- Why it is happening (root cause)
- What you considered / ruled out (short)

### 3) Fixes (keep them safe & minimal)

- Implement fixes for the issues you confirm
- Add or update tests where appropriate
- If something is too big to fix in time, document what you found and propose next steps

### 4) Operational thinking: update the runbook

Update `RUNBOOK.md` with:

- **How to diagnose these issues in production** (what to look at, what logs/queries/metrics help — use the structured log lines and sample artifacts as a guide)
- How to verify the fix
- Mitigation / rollback plan if the fix regresses

### 5) Incident summary

Fill in `INCIDENT.md` like an internal incident write-up:

- Impact
- Detection
- Timeline (rough is fine)
- Root cause
- Mitigation/resolution
- Verification
- Follow-up actions

### 6) Create a follow-up ticket

Write `TICKET.md` with one strong follow-up ticket you would file after the incident is resolved. Include:

- Title + description
- Priority/severity and rationale
- Acceptance criteria
- Notes/context (monitoring, tests, refactors, etc.)

---

## Submission (Clone + Link)

Please submit using Clone + Link:

1. Clone this repository into your own GitHub account.
2. Create a branch (any name is fine).
3. Make your changes and commit them.
4. Push the branch to your GitHub account (create a new repo under your account for the push, if needed).
5. Email us:
   - A link to your GitHub repo and the branch you worked on
   - A brief note covering:
     - Which issues you confirmed and fixed
     - **How you used logs to diagnose at least one issue**
     - Any tradeoffs you made
     - What you would do next with more time

Send to: **alex.stiglick@functionhealth.com**

---

## What we evaluate

We're less interested in perfect code and more interested in how you operate. We look for:

- Clear reproduction steps + evidence
- **Use of log artifacts to diagnose issues (stack traces, structured log lines, request context)**
- Hypothesis-driven debugging (not random poking)
- Safe fixes (minimal blast radius, sensible defaults, rollback awareness)
- Verification quality (tests + practical manual verification)
- Operational artifacts (runbook + incident summary + a strong ticket)
- Ability to explain decisions and tradeoffs (including any AI-assisted parts)

---

## Notes

- First run may be noisy because the app seeds data.
- The port may differ from examples — trust the `dotnet run` output.
- If you get blocked on setup, write down what you tried since that's still a useful signal.

## Questions / help

If you're stuck on environment setup or something doesn't make sense, email: **alex.stiglick@functionhealth.com**
