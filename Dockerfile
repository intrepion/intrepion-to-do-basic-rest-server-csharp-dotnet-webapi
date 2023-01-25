FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

COPY ToDoApp.sln .
COPY ToDoApp.Tests/*.csproj ./ToDoApp.Tests/
COPY ToDoApp.WebApi/*.csproj ./ToDoApp.WebApi/
RUN dotnet restore

COPY ToDoApp.Tests/. ./ToDoApp.Tests/
COPY ToDoApp.WebApi/. ./ToDoApp.WebApi/
WORKDIR /source/ToDoApp.WebApi
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "ToDoApp.WebApi.dll"]
