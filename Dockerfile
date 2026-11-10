FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore RaceDay.sln
RUN dotnet publish src/RaceDay.Api/RaceDay.Api.csproj -c Release -o /app/api
RUN dotnet publish src/RaceDay.Mvc/RaceDay.Mvc.csproj -c Release -o /app/mvc

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app
COPY --from=build /app/api .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet","RaceDay.Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS mvc
WORKDIR /app
COPY --from=build /app/mvc .
ENV ASPNETCORE_URLS=http://+:8081
EXPOSE 8081
ENTRYPOINT ["dotnet","RaceDay.Mvc.dll"]
