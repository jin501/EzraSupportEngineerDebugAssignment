# Incident Summary (Fill in)

**Title:**  Intermittent 500 Errors During Task Creation
**Date:**  5/29/2026
**Severity:**  Medium (Partial Functionality Impact)

## Impact
* Users intermittently experienced failures when creating new tasks.
* Affected requests returned HTTP 500 instead of successfully creating a task.
* Existing tasks could still be viewed and listed successfully.
* Impact was limited to task creation functionality.

## Detection
* Issue was initially reported through customer feedback indicating that task creation would "sometimes fail with a 500."
* The issue was reproduced locally using the application UI.
* Browser DevTools showed failed POST /api/tasks requests returning HTTP 500.
* The provided log artifact (`artifacts/sample_api_log.txt`) showed `X-Client-Timestamp present=False length=0` immediately before a `System.FormatException` originating from `DateTime.Parse()` in `TaskEndpoints.cs`.

## Timeline (UTC)
- 14:30 — Reviewed customer reported issue and launched the application locally.
- 14:33 — Reproduced intermittent HTTP 500 errors while creating tasks through the UI.
- 14:35 — Inspected browser Network tab and confirmed failures were isolated to `POST /api/tasks`.
- 14:38 — Reviewed application logs and stack traces generated during failed requests.
- 14:41 — Identified exception originating from `TaskEndpoints.cs` during timestamp parsing.
- 14:44 — Confirmed root cause was unsafe use of `DateTime.Parse()` on the optional `X-Client-Timestamp` header.
- 14:48 — Implemented defensive validation using `DateTime.TryParse()` and added fallback behavior.
- 14:53 — Restarted the application and performed manual verification testing.
- 14:57 — Confirmed task creation succeeds with valid, missing, and invalid timestamp scenarios.
- 15:00 — Incident considered resolved and documented.

## Root cause
The task creation endpoint attempted to parse the X-Client-Timestamp request header using DateTime.Parse() before validating that the header contained a valid value.

When the header was missing, empty, or malformed, DateTime.Parse() threw an exception, causing the request to fail with HTTP 500. The endpoint did not gracefully handle invalid timestamp input.

This conclusion was confirmed using the provided production log artifact, which showed a missing timestamp header followed by a `FormatException` during timestamp parsing.

## Mitigation / resolution
* Replaced DateTime.Parse() with DateTime.TryParse().
* Added validation for invalid timestamp values.
* Added a fallback to DateTime.UtcNow when the timestamp header is not provided.
* Preserved existing functionality for valid timestamp values.

Final fix:

* Safe timestamp parsing using DateTime.TryParse().
* Default timestamp generation using server UTC time when no client timestamp is supplied.

## Verification
* Verified task creation succeeds with a valid timestamp.
* Verified task creation succeeds when the X-Client-Timestamp header is absent.
* Verified invalid timestamp values return HTTP 400 instead of HTTP 500.
* Verified newly created tasks are persisted and appear correctly in the task list.
* Confirmed no new exceptions were generated during manual testing.

## Follow-ups / action items
- [ ] Add automated tests covering missing, invalid, and valid timestamp scenarios.
- [ ] Add request validation coverage for task creation endpoints.
- [ ] Add monitoring/alerting for elevated HTTP 500 rates on POST /api/tasks.
- [ ] Review other endpoints for unsafe parsing or unvalidated user input.


## Additional Finding: Slow Task List Performance

Impact:
- Users with larger task datasets experienced significantly slower response times.

Detection:
- Diagnosed using `artifacts/sample_slow_list_log.txt`, which showed requests returning 200 records taking substantially longer than smaller requests.

Root Cause:
- The endpoint retrieved all tasks from the database before applying filtering, sorting, and limits in memory.

Resolution:
- Moved filtering, sorting, and limiting into the database query so only the required rows are retrieved.

Verification:
- Verified generated SQL contains `WHERE`, `ORDER BY`, and `LIMIT`.
- Local testing showed task list requests completing in single-digit milliseconds after the change.