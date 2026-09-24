# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY SmartMetering.Api/*.csproj ./SmartMetering.Api/
RUN dotnet restore ./SmartMetering.Api/SmartMetering.Api.csproj
COPY SmartMetering.Api/. ./SmartMetering.Api/
WORKDIR /src/SmartMetering.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SmartMetering.Api.dll"]
