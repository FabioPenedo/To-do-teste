# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia csproj e restaura
COPY *.csproj .
RUN dotnet restore

# Copia tudo e builda
COPY . .
RUN dotnet publish -c Release -o /app/publish

# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "To-do-teste.dll"]
