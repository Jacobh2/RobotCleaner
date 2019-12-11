FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY RobotCleaner/RobotCleaner.csproj RobotCleaner/
RUN dotnet restore "RobotCleaner/RobotCleaner.csproj"
COPY . .
WORKDIR "/src/RobotCleaner"
RUN dotnet build "RobotCleaner.csproj" -c Release -o /app/build

FROM build as tester
WORKDIR /tests
COPY RobotCleaner.Test/RobotCleaner.Test.csproj RobotCleaner.Test/
RUN dotnet restore "RobotCleaner.Test/RobotCleaner.Test.csproj"
COPY . .
WORKDIR "/tests/RobotCleaner.Test"
RUN dotnet build "RobotCleaner.Test.csproj" -c Release -o /app/tests/build;
RUN dotnet vstest /app/tests/build/RobotCleaner.Test.dll