# RaceDay — Part 2 REST API

**Module:** PROG6212 Programming 2B (2026)  
**Project:** RaceDay event management API  
**Technology:** C#, ASP.NET Core Web API, Entity Framework Core, SQL Server, session cookies, Swagger and xUnit.

## What is included

- REST endpoints for registration, login, logout and session status.
- Password hashing using ASP.NET Core `PasswordHasher<User>`; passwords are never stored as plain text.
- Session cookie storing the authenticated user ID and role.
- Organiser and Participant checks at the API level.
- EF Core SQL Server Code-First data model: `Users`, `Events`, `Categories`, `Enrolments`, `Results` and `EventRoutes`.
- Event/category/enrolment/result CRUD operations appropriate to the roles.
- Swagger in Development mode.
- xUnit API tests and a GitHub Actions build-and-test workflow.

## Roles

- **Organiser:** creates, edits and deletes their own events; creates event categories and routes; views enrolments for their own events; records results for participants entered in their events.
- **Participant:** views events, updates their own profile, enters an event with a category, views their enrolments and views their own results.

Public event browsing is allowed. Protected endpoints require a session. A Participant cannot perform Organiser operations, and an Organiser cannot use Participant-only enrolment/result views.

## Folder structure

```text
RaceDay-Part2/
├── .github/workflows/part2-ci.yml
├── src/RaceDay.Api/
│   ├── Controllers/
│   ├── Data/RaceDayDbContext.cs
│   ├── Models/
│   ├── Program.cs
│   └── appsettings.json
├── tests/RaceDay.Tests/
├── RaceDay.sln
└── README.md
```

## Run using Visual Studio Code

1. Install the **.NET 8 SDK**, SQL Server (or SQL Server LocalDB), and the C# Dev Kit extension in Visual Studio Code.
2. Extract this project folder somewhere convenient, for example `Documents/RaceDay-Part2`.
3. Open Visual Studio Code and choose **File → Open Folder**, then select `RaceDay-Part2`.
4. Open `src/RaceDay.Api/appsettings.json`. Change `ConnectionStrings:RaceDayDatabase` if your SQL Server instance uses a different name. Do not commit real passwords or secrets.
5. Open **Terminal → New Terminal** at the project root.
6. Run `dotnet --info` and confirm the installed SDK is version 8.x.
7. Run `dotnet restore RaceDay.sln`.
8. Run `dotnet build RaceDay.sln` and resolve any errors before continuing.
9. Create the database schema using EF Core. Install the tool if required with `dotnet tool install --global dotnet-ef`, then run:
   `dotnet ef migrations add InitialCreate --project src/RaceDay.Api --startup-project src/RaceDay.Api`
   `dotnet ef database update --project src/RaceDay.Api --startup-project src/RaceDay.Api`
10. Start the API with `dotnet run --project src/RaceDay.Api`.
11. Open the HTTPS URL printed in the terminal and add `/swagger` to open Swagger UI. Swagger is enabled in Development mode.
12. Register an Organiser and a Participant through `POST /api/auth/register`, then use `POST /api/auth/login`. The client must retain the `.RaceDay.Session` cookie between requests. In Swagger, use **Try it out** and check the response; for persistent cookie testing, a browser or a cookie-aware API client may be easier.
13. Run tests from the root with `dotnet test RaceDay.sln`.

### Database note

The API uses EF Core Code-First and the model follows the six Part 1 entities described in the assessment feedback. Run migrations against your own SQL Server instance before testing database-backed endpoints. The SQL connection string is a local-development example and may need to be changed for your computer.

## Endpoints

| Method | Route | Access | Purpose |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Register an Organiser or Participant |
| POST | `/api/auth/login` | Public | Verify credentials and create session |
| POST | `/api/auth/logout` | Logged in | Clear the session |
| GET | `/api/auth/session` | Logged in | Return current session status |
| GET / PUT | `/api/profile` | Logged in | Read or update own profile |
| GET | `/api/events` | Public | List events |
| GET | `/api/events/{id}` | Public | View event, categories and routes |
| POST | `/api/events` | Organiser | Create an event |
| PUT / DELETE | `/api/events/{id}` | Organiser (owner) | Update or delete own event |
| GET | `/api/categories?eventId={id}` | Public | List categories, optionally for one event |
| POST | `/api/categories` | Organiser (event owner) | Create a category |
| PUT / DELETE | `/api/categories/{id}` | Organiser (event owner) | Update or delete a category |
| POST | `/api/enrolments` | Participant | Enter an event and select a category |
| GET | `/api/enrolments/mine` | Participant | View own enrolments |
| GET | `/api/enrolments/event/{eventId}` | Organiser (event owner) | View event enrolments |
| DELETE | `/api/enrolments/{id}` | Participant (owner) | Cancel an enrolment if no result exists |
| POST / PUT | `/api/results` and `/api/results/{id}` | Organiser (event owner) | Record or update a result |
| GET | `/api/results/mine` | Participant | View own results |
| GET / POST / DELETE | `/api/eventroutes` and `/api/eventroutes/{id}` | Public GET; Organiser writes | Read and manage route information |

## GitHub Actions and submission checklist

The workflow is saved at `.github/workflows/part2-ci.yml` and builds the solution and runs tests on pushes and pull requests. To use it, commit and push the whole project to the lecturer-provided GitHub repository. Confirm the workflow is detected and reaches a green check. The workflow cannot run until the files are pushed to GitHub.

## Video presentation

**Unlisted YouTube video:** `ADDED UNLISTED YOUTUBE LINK 
