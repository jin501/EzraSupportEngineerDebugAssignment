# Customer Reports (Context)

## Report 1 — Create task 500s
> "I click **Add** and sometimes it just errors. If I try again a few seconds later it works."

## Report 2 — Slow lists
> "My task list takes a long time to load. My coworker says it's fine."

## Report 3 — Duplicates / ordering weirdness
> "After I refresh, some tasks show up twice. Also the order looks random sometimes."

## Report 4 — 500 in production (can't reproduce locally)
> "We're seeing 500 errors on task creation in production. We can't reproduce in our environment. Engineering has attached a log snippet from a failed request — see **artifacts/sample_api_log.txt**. Please use the logs to identify the cause and fix it."

Notes:
- We don't have perfect reproduction steps.
- Assume this is an app with multiple users, and some have a lot of tasks.
- **At least one issue (e.g. the create-task 500) must be diagnosed using the provided log artifacts** — inspect stack traces, request context, and structured log lines to find the root cause.
