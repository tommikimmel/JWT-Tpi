# Imagen base de .NET para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todo y compilar
COPY . .
WORKDIR "/app/JWT-Tpi"
RUN dotnet publish -c Release -o /out

# Imagen final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "JWT-Tpi.dll"]
