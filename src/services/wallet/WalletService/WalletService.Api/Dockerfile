# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy entire repository (context is repo root)
COPY . .

# Restore
RUN dotnet restore src/services/wallet/WalletService/WalletService.Api/WalletService.Api.csproj

# Publish
RUN dotnet publish src/services/wallet/WalletService/WalletService.Api/WalletService.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "WalletService.Api.dll"]
