FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY KanbanApi/KanbanApi.csproj KanbanApi/
RUN dotnet restore KanbanApi/KanbanApi.csproj

COPY . .
WORKDIR /src/KanbanApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Directorio para la base de datos SQLite
RUN mkdir -p /app/data

# Generar migración inicial
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migration
WORKDIR /src
COPY KanbanApi/KanbanApi.csproj KanbanApi/
RUN dotnet restore KanbanApi/KanbanApi.csproj
COPY . .
WORKDIR /src/KanbanApi
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet ef migrations add InitialCreate --output-dir Migrations || true
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=migration /app/publish .
RUN mkdir -p /app/data
EXPOSE 8080
ENTRYPOINT ["dotnet", "KanbanApi.dll"]