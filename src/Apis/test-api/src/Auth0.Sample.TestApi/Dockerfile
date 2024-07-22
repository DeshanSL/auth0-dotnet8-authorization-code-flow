#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Apis/test-api/src/Auth0.Sample.TestApi/Auth0.Sample.TestApi.csproj", "src/Apis/test-api/src/Auth0.Sample.TestApi/"]
RUN dotnet restore "./src/Apis/test-api/src/Auth0.Sample.TestApi/Auth0.Sample.TestApi.csproj"
COPY . .
WORKDIR "/src/src/Apis/test-api/src/Auth0.Sample.TestApi"
RUN dotnet build "./Auth0.Sample.TestApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth0.Sample.TestApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth0.Sample.TestApi.dll"]