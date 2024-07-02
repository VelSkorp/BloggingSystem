FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BloggingSystem/BloggingSystem.csproj", "BloggingSystem/"]
RUN dotnet restore "./BloggingSystem/BloggingSystem.csproj"
COPY . .
WORKDIR "/src/BloggingSystem"
RUN dotnet build "./BloggingSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BloggingSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_URLS=https://+:8081
ENTRYPOINT ["dotnet", "BloggingSystem.dll"]