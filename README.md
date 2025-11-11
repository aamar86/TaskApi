# Tasky Clean Architecture (EF Core SQL Server + Base Repository)

## Structure
- Tasky.Domain — Entities
- Tasky.Application — Interfaces, DTOs, Services, ProblemException
- Tasky.Infrastructure — BaseRepository, EfTaskRepository, DbContext
- Tasky.WebApi — Minimal API host (Swagger, DI, error handling)

## Run
```bash
dotnet restore
dotnet build Tasky-CleanArch-BaseRepo-Sln.sln

# Add migrations & update DB:
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate   --project Tasky.Infrastructure/Tasky.Infrastructure.csproj   --startup-project Tasky.WebApi/Tasky.WebApi.csproj   --output-dir Persistence/Migrations

dotnet ef database update   --project Tasky.Infrastructure/Tasky.Infrastructure.csproj   --startup-project Tasky.WebApi/Tasky.WebApi.csproj

# Run API
dotnet run --project Tasky.WebApi/Tasky.WebApi.csproj
# Swagger: http://localhost:5000/swagger
```
