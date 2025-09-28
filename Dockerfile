# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution và project files trước
COPY BanHang.sln .
COPY WEB/WEB.csproj WEB/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Entities/Entities.csproj Entities/
COPY UseCase/UseCase.csproj UseCase/

# Restore dependencies
RUN dotnet restore BanHang.sln

# Copy toàn bộ source code
COPY . .

# Build và publish
WORKDIR /src/WEB
RUN dotnet publish -c Release -o /app

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "WEB.dll"]
