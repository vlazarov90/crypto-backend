# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN dotnet tool install --global dotnet-ef

# Copy the project file and restore dependencies
COPY ["CryptoDashboard.Api.csproj", "CryptoDashboard.Api/"]
RUN dotnet restore "CryptoDashboard.Api/CryptoDashboard.Api.csproj" -r linux-arm64

# Copy the entire source code and build the application
WORKDIR "/src/CryptoDashboard.Api"
COPY . .
RUN dotnet build "CryptoDashboard.Api.csproj" -c Release -o /app/build 

# Publish the app
FROM build AS publish
RUN dotnet publish "CryptoDashboard.Api.csproj" -c Release -o /app/publish --no-self-contained

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CryptoDashboard.Api.dll"]