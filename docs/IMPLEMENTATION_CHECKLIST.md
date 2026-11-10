# Part 2 completion checklist

Use this checklist while working in Visual Studio Code. The ZIP is a working project package, not the final assessment submission; the official brief requires a pushed GitHub repository and an unlisted video.

- [ ] Confirm the code is consistent with the actual Part 1 endpoint plan and SQL/ERD in your repository. If a field or route differs, align it before submission.
- [ ] Install .NET 8 SDK and SQL Server/LocalDB.
- [ ] Update the connection string for the local SQL Server instance.
- [ ] Run restore, build, EF Core migration/database update, and tests locally.
- [ ] Test registration, duplicate email, invalid role, login failure, login success and logout.
- [ ] Test Organiser event CRUD and ensure one Organiser cannot edit another Organiser's event.
- [ ] Test Participant enrolment, duplicate entry, category belonging to another event, own enrolments and own results.
- [ ] Test that unauthenticated requests return 401 and wrong-role requests return 403.
- [ ] Verify all endpoints and request/response examples in Swagger.
- [ ] Push to the lecturer-provided repository, verify the workflow is detected, and fix any build/test failures.
- [ ] Make at least 20 meaningful commits over the real development process.
- [ ] Add a screenshot of the actual green workflow run to the README.
- [ ] Record and upload your own-voice unlisted YouTube demonstration, then replace the README placeholder.
- [ ] Include the AI-use disclosure required by the assessment brief.
