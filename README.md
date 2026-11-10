# RaceDay — Programming 2B

RaceDay is a South African road running, walking and cycling event management system.

## Roles
- **Organiser:** manages their own events, categories, enrolments and participant results.
- **Participant:** browses events, enters an event category, views enrolments and personal results.

## Project folders
- `src/RaceDay.Api` — ASP.NET Core REST API and EF Core database access.
- `src/RaceDay.Mvc` — MVC front end that calls the API.
- `tests/RaceDay.Tests` — xUnit API tests.
- `.github/workflows` — automated build and test workflows.
- `docs` — project notes and implementation checklist.

## Run
Install the .NET 8 SDK and SQL Server. Configure the connection string in `src/RaceDay.Api/appsettings.json`. Start the API using `dotnet run --project src/RaceDay.Api`, then set the exact API URL in `src/RaceDay.Mvc/appsettings.json` and run `dotnet run --project src/RaceDay.Mvc`.

Run automated tests with `dotnet test RaceDay.sln`.

## Part 3 status
This repository includes an initial MVC interface and Docker configuration, but it is not yet a verified, complete Part 3 submission. See `README-PART3-DRAFT.md` for remaining tasks. The Azure Blob Storage upload workflow and full Organiser management pages still require implementation and testing.

## Presentation
Add your own unlisted YouTube demonstration link here after recording and uploading the video.
