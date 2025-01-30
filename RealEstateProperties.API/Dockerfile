#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:9.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:9.0

FROM $DOTNET_RUNTIME AS base
USER $APP_UID
WORKDIR /app
EXPOSE 977
EXPOSE 731

FROM $DOTNET_SDK AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RealEstateProperties.API/RealEstateProperties.API.csproj", "RealEstateProperties.API/"]
COPY ["RealEstateProperties.Contracts/RealEstateProperties.Contracts.csproj", "RealEstateProperties.Contracts/"]
COPY ["RealEstateProperties.Domain.Entities/RealEstateProperties.Domain.Entities.csproj", "RealEstateProperties.Domain.Entities/"]
COPY ["RealEstateProperties.Domain.Helpers/RealEstateProperties.Domain.Helpers.csproj", "RealEstateProperties.Domain.Helpers/"]
COPY ["RealEstateProperties.Domain/RealEstateProperties.Domain.csproj", "RealEstateProperties.Domain/"]
COPY ["RealEstateProperties.Infrastructure/RealEstateProperties.Infrastructure.csproj", "RealEstateProperties.Infrastructure/"]

COPY ["RealEstateProperties.Tests/RealEstateProperties.Tests.csproj", "RealEstateProperties.Tests/"]

RUN dotnet restore "./RealEstateProperties.API/RealEstateProperties.API.csproj"
COPY . .
WORKDIR "/src/RealEstateProperties.API"
RUN dotnet build "./RealEstateProperties.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RealEstateProperties.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealEstateProperties.API.dll"]
