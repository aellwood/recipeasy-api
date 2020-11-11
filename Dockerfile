# NuGet restore
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY Recipeasy.Api.Tests/*.csproj Recipeasy.Api.Tests/
COPY Recipeasy.Api/*.csproj Recipeasy.Api/
RUN dotnet restore
COPY . .

# testing
FROM build AS testing
WORKDIR /src/Recipeasy.Api
RUN dotnet build
WORKDIR /src/Recipeasy.Api.Tests
RUN dotnet test

# publish
FROM build AS publish
WORKDIR /src/Recipeasy.Api
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "Recipeasy.Api.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Recipeasy.Api.dll