FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY RobotCleaner/RobotCleaner.csproj RobotCleaner/
RUN dotnet restore "RobotCleaner/RobotCleaner.csproj"
COPY . .
WORKDIR "/src/RobotCleaner"
RUN dotnet build "RobotCleaner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RobotCleaner.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RobotCleaner.dll"]
