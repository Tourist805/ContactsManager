# Use the official .NET 8 SDK image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ContactsApi.csproj"
RUN dotnet build "ContactsApi.csproj" -c Release -o /app/build
RUN dotnet publish "ContactsApi.csproj" -c Release -o /app/publish

# Final stage: set up the runtime environment
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ContactsApi.dll"]
