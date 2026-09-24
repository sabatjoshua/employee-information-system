FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

LABEL org.opencontainers.image.source="https://github.com/sabatjoshua/employee-information-system"

WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/EmployeeInformationSystem.API/EmployeeInformationSystem.API.csproj", "EmployeeInformationSystem.API/"]
COPY ["src/EmployeeInformationSystem.Application/EmployeeInformationSystem.Application.csproj", "EmployeeInformationSystem.Application/"]
COPY ["src/EmployeeInformationSystem.Domain/EmployeeInformationSystem.Domain.csproj", "EmployeeInformationSystem.Domain/"]
COPY ["src/EmployeeInformationSystem.Infrastructure/EmployeeInformationSystem.Infrastructure.csproj", "EmployeeInformationSystem.Infrastructure/"]
COPY ["src/EmployeeInformationSystem.Persistence/EmployeeInformationSystem.Persistence.csproj", "EmployeeInformationSystem.Persistence/"]

RUN dotnet restore "EmployeeInformationSystem.API/EmployeeInformationSystem.API.csproj"

COPY src/ .

WORKDIR "/src/EmployeeInformationSystem.API"

RUN dotnet build "EmployeeInformationSystem.API.csproj" \
    -c Release \
    -o /app/build \
    --no-restore

FROM build AS publish

RUN dotnet publish "EmployeeInformationSystem.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EmployeeInformationSystem.API.dll"]