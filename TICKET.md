# Follow-up Ticket

**Title:** Add request validation and regression test coverage for task creation endpoint
**Priority:** P2
**Owner:** Engineering

## Description

The task creation endpoint previously returned HTTP 500 when the optional `X-Client-Timestamp` header was missing or invalid. The immediate issue has been resolved by replacing unsafe timestamp parsing with validation and fallback behavior.

To reduce the likelihood of similar incidents, add automated validation and regression test coverage for task creation requests. Monitoring should also be added so unexpected increases in task creation failures can be detected quickly.

## Acceptance criteria

* [ ] Add automated tests covering:
  * Valid timestamp
  * Missing timestamp
  * Invalid timestamp
* [ ] Verify missing timestamps do not return HTTP 500.
* [ ] Verify invalid timestamps return HTTP 400.
* [ ] Add monitoring for elevated failure rates on `POST /api/tasks`.
* [ ] Document expected behavior for optional request headers.

## Notes / context

* Relevant endpoint: `TaskEndpoints.cs`
* Root cause was unsafe use of `DateTime.Parse()` on an optional request header.
* Consider adding centralized request validation to reduce duplicated validation logic across endpoints.
* Consider adding alerts when task creation failure rates exceed expected thresholds.
