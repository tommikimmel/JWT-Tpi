# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiamos todo el código
COPY . .
WORKDIR "/app/JWT-Tpi"

# Compilamos el proyecto en modo Release
RUN dotnet publish -c Release -o /out

# Etapa final: imagen ligera de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiamos lo compilado desde la etapa anterior
COPY --from=build /out .

# Ejecutamos la app
ENTRYPOINT ["dotnet", "JWT-Tpi.dll"]
