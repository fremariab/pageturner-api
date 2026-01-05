# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PageTurner.Api/PageTurner.Api.csproj", "PageTurner.Api/"]
RUN dotnet restore "PageTurner.Api/PageTurner.Api.csproj"
COPY . .
WORKDIR "/src/PageTurner.Api"
RUN dotnet build "PageTurner.Api.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "PageTurner.Api.csproj" -c Release -o /app/publish

# Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Render uses the PORT environment variable
ENV ASPNETCORE_URLS=http://+:10000 
ENTRYPOINT ["dotnet", "PageTurner.Api.dll"]