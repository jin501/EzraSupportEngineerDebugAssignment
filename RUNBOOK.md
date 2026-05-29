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

Symptoms:
- Some users report task lists loading significantly slower than others.
- Performance degradation becomes more noticeable for users with larger task counts.

Diagnosis:
1. Review `artifacts/sample_slow_list_log.txt`.
2. Compare `elapsedMs` across different users and limits.
3. Check whether filtering and limiting are being performed in the database query or in application memory.
4. Enable SQL logging and verify generated SQL contains:
   - `WHERE UserId = ...`
   - `ORDER BY`
   - `LIMIT`

Evidence:
- The provided artifact showed:
  - `user-001 limit=50 elapsedMs=312`
  - `user-002 limit=50 elapsedMs=289`
  - `user-015 limit=200 elapsedMs=1847`
- This suggested the endpoint was processing more data than necessary before applying filters.

Root cause:
- The endpoint loaded the entire Tasks table into memory using `ToListAsync()` before filtering by user and applying limits.
- As data volume increased, response times increased unnecessarily.

Fix:
- Moved filtering, ordering, and limiting into the database query.
- Retained `AsNoTracking()` for read-only operations.

Verification:
- Confirm generated SQL contains `WHERE`, `ORDER BY`, and `LIMIT`.
- Re-run task list requests and compare elapsed times.
- Verify only the requested user's tasks are returned.

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

## Testing Notes

- API project builds and runs successfully.
- Test project compilation issues were resolved (`using Xunit`).
- Test execution remains blocked by a FluentAssertions runtime dependency resolution issue (`FluentAssertions 6.12.0`) despite package restore and reinstall attempts.
- Core fixes were verified manually through the UI and API endpoints.