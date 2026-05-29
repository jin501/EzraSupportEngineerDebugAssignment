# Support Engineer Challenge — Debug & Stabilize

This repo contains a small prebuilt app with a few realistic "production" issues. Your goal is to **triage**, **diagnose**, and **ship safe fixes** with clear communication.

## Scenario

Assume this app is running in production and you're on-call. We’ve received customer reports:

1) “Sometimes creating a task fails with a 500.”
2) “The tasks list is slow for some users.”
3) “We’ve seen tasks appear duplicated or out of order after refresh.”
4) "We're seeing 500 errors on task creation in production but can't reproduce locally. A log snippet is in **artifacts/sample_api_log.txt** — use the logs to identify the cause and fix it."

Not all reports may be accurate — part of the exercise is determining what’s real, what’s reproducible, and what you’d do next.

## What’s here

- `src/SupportEngineerChallenge.Api` — .NET 8 minimal API + SQLite + static UI
- `tests/SupportEngineerChallenge.Tests` — xUnit tests (some may fail)
- `RUNBOOK.md` — **you will update**
- `INCIDENT.md` — **you will fill in**
- `TICKET.md` — **you will write a follow-up ticket**
- `artifacts/` — sample logs + incident report context

## Timebox

Aim for ~2–4 hours. If you go beyond that, please note it.

## Getting started

Requirements:
- .NET SDK 8.x

Run the API + UI:

```bash
cd src/SupportEngineerChallenge.Api
dotnet restore
dotnet run
```

Then open:
- API Swagger: http://localhost:5088/swagger
- UI:         http://localhost:5088/

Run tests:

```bash
dotnet test
```

## Deliverables

Please complete:

1. **Triage & reproduction**  
   - Use the provided log artifacts (e.g. `artifacts/sample_api_log.txt`) to diagnose at least one issue
   - Clear repro steps for each confirmed issue
   - Capture evidence (logs, stack traces, etc.)

2. **Root cause analysis**  
   - Explain what’s happening and why (briefly)

3. **Fixes**  
   - Safe, minimal fixes
   - Add/update tests where appropriate

4. **Operational thinking**  
   - Update `RUNBOOK.md` with diagnosis + verification + rollback/mitigation steps

5. **Incident summary**  
   - Fill in `INCIDENT.md` with impact/timeline/root cause/fix/follow-ups

6. **Follow-up ticket**  
   - Write `TICKET.md` with a high-quality ticket (acceptance criteria, priority, etc.)

## Notes

- You can change anything in this repo (including UI) as long as you explain your choices.
- If you get blocked by setup, write down what you tried and where you got stuck.
