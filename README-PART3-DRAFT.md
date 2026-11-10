# RaceDay — Part 3 implementation draft

## Project overview
RaceDay is an event management platform for South African running, walking and cycling events. It has two roles: Organisers manage events, categories, enrolments and results; Participants browse events, enter a category and review their personal results.

## Current implementation
- `src/RaceDay.Api`: existing Part 2 REST API.
- `src/RaceDay.Mvc`: MVC front end which calls the API using `HttpClient`.
- Login and registration forms; the MVC session retains the API session cookie.
- Event browsing and details; Participant event entry; personal enrolment and results views.
- `Dockerfile` and a GitHub Actions build/test workflow.

## Run locally
1. Install .NET 8 SDK and SQL Server.
2. Set `ConnectionStrings:RaceDayDatabase` in `src/RaceDay.Api/appsettings.json` for your SQL Server.
3. Start the API: `dotnet run --project src/RaceDay.Api`.
4. Note the API HTTPS address printed in the terminal.
5. Put that exact address in `src/RaceDay.Mvc/appsettings.json` under `Api:BaseUrl`.
6. In another terminal run `dotnet run --project src/RaceDay.Mvc`.
7. Open the MVC URL printed in the terminal.

Run tests with `dotnet test RaceDay.sln`.

## Important items to complete and verify before submission
- The MVC project is a starter implementation and must be built and tested on the student's computer. Verify the API URL and SQL Server connection.
- Organiser management screens (dashboard, event create/edit/delete, category management, enrolment listing and result entry) still need to be completed and tested against the API.
- Azure Blob Storage is not yet configured. Add the Azure Storage connection string securely as a GitHub secret/local user secret, implement the API upload endpoint, and test banner/profile image uploads. Do not place real credentials in source control.
- The Dockerfile currently publishes the API image. A separate MVC container and a SQL Server service/configuration must be added and tested for a full end-to-end Docker deployment.
- Add a genuine green Actions screenshot after the workflow succeeds.
- Record your own voice presentation and replace the YouTube placeholder with the unlisted video URL.
- Review, understand, run and adapt the code. The assessment brief requires disclosure of AI assistance where used.
