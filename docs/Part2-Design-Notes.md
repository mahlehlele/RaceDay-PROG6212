# RaceDay Part 2 design notes

The API uses ASP.NET Core controllers and Entity Framework Core. The six database entities are Users, Events, Categories, Enrolments, Results and EventRoutes, matching the Part 1 feedback. A session cookie stores UserId and Role after successful login. Passwords are hashed with ASP.NET Core PasswordHasher. Organiser actions check both the role and ownership of the event; participant enrolment and results endpoints are limited to the signed-in participant.

The project uses SQL Server through the connection string in `src/RaceDay.Api/appsettings.json`. It uses EF Core Code-First migrations, so run the migrations and database update commands in the README against your SQL Server instance before using database endpoints.

**Important:** compare the route names and request fields with the exact Part 1 endpoint plan in your own GitHub repository before submission. The lecturer's brief says Part 2 should closely match the approved plan.
