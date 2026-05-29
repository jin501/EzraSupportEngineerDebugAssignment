# Runbook — SupportEngineerChallenge

> Update this file as part of the exercise.

## Service overview
- **Service:** SupportEngineerChallenge.Api
- **Purpose:** Minimal task tracker (create + list tasks)
- **Data store:** SQLite (`app.db` in the API working directory)

## Common commands

**Run locally**
```bash
cd src/SupportEngineerChallenge.Api
dotnet run
```

**Run tests**
```bash
dotnet test
```

## Key endpoints
- `GET /api/tasks?userId={id}&limit={n}`
- `POST /api/tasks`

## Using log artifacts

- **Create-task 500:** Inspect `artifacts/sample_api_log.txt` (or production logs). Look for the `CreateTask request` line — `X-Client-Timestamp present=False` or `length=0` indicates missing/invalid header. The stack trace shows `FormatException` at `DateTime.Parse`.
- **Slow list:** Look for `ListTasks completed` lines with high `elapsedMs` (e.g. `artifacts/sample_slow_list_log.txt`). Correlate `userId` and `limit` with slow requests.

## Troubleshooting checklist (starter)

### “Create task fails with 500”

Symptoms:
- Users report intermittent failures when creating tasks.
- Browser DevTools shows `POST /api/tasks` returning HTTP 500.

Diagnosis:
1. Review `artifacts/sample_api_log.txt` or equivalent production logs.
2. Look for the structured log line:
   `CreateTask request UserId=... X-Client-Timestamp present=False length=0`
3. Review the accompanying stack trace showing:
   `System.FormatException: String '' was not recognized as a valid DateTime`
4. Confirm the exception originates from `DateTime.Parse()` in `TaskEndpoints.cs`.
5. Compare successful vs failed `POST /api/tasks` requests in DevTools if reproducing locally.

Root cause:
- The endpoint was parsing `X-Client-Timestamp` with `DateTime.Parse()` without safely handling missing or invalid values.

Fix:
- Use `DateTime.TryParse()`.
- Default missing timestamp to `DateTime.UtcNow`.
- Return HTTP 400 for malformed timestamps instead of HTTP 500.

Verification:
- Create task with valid timestamp.
- Create task without timestamp.
- Create task with invalid timestamp.
- Confirm no 500s occur.

### “Tasks list is slow”
- Confirm dataset size (seed can be large).
- Inspect how the list endpoint fetches and filters data.
- Review query patterns and database usage.

### “Duplicates / wrong order after refresh”
- Compare API response vs UI rendering.
- Check the UI state update logic during refresh.
- Verify how the list is merged and ordered.

## Verification steps (starter)
- Create tasks from UI and via Swagger.
- Refresh tasks repeatedly; confirm no duplicates and ordering is correct.
- Validate list endpoint returns only requested user's tasks.

## Rollback / mitigation ideas (starter)
- Roll back to last known good version.
- Temporarily disable problematic client behavior (feature flag / UI change).
- Add guardrails (e.g. input validation, error handling) to prevent unhandled exceptions.
