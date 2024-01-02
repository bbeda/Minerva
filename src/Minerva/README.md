## Add Migration
dotnet ef migrations add <<NAME>>  --project ./Minerva.Application/Minerva.Application.csproj --startup-project ./Minerva.WebApp/Minerva.WebApp/Minerva.WebApp.csproj

## Update BD
dotnet ef database update  --project ./Minerva.Application/Minerva.Application.csproj --startup-project ./Minerva.WebApp/Minerva.WebApp/Minerva.WebApp.csproj

## Remove migration
dotnet ef migrations remove  --project ./Minerva.Application/Minerva.Application.csproj --startup-project ./Minerva.WebApp/Minerva.WebApp/Minerva.WebApp.csproj

## Drop BD
dotnet ef database drop  --project ./Minerva.Application/Minerva.Application.csproj --startup-project ./Minerva.WebApp/Minerva.WebApp/Minerva.WebApp.csproj

