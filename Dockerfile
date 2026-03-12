# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EvaluateItEasily.API/EvaluateItEasily.API.csproj", "EvaluateItEasily.API/"]
COPY ["EvaluateItEasily.Infrastructure/EvaluateItEasily.Infrastructure.csproj", "EvaluateItEasily.Infrastructure/"]
COPY ["EvaluateItEasily.Core/EvaluateItEasily.Core.csproj", "EvaluateItEasily.Core/"]

RUN dotnet restore "EvaluateItEasily.API/EvaluateItEasily.API.csproj"

COPY . .
WORKDIR "/src/EvaluateItEasily.API"
RUN dotnet build "EvaluateItEasily.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EvaluateItEasily.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EvaluateItEasily.API.dll"]