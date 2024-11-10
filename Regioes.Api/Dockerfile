FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Regioes.Api/Regioes.Api.csproj", "Regioes.Api/"]
COPY ["Regioes.Core/Regioes.Core.csproj", "Regioes.Core/"]
COPY ["Regioes.Shared/Regioes.Shared.csproj", "Regioes.Shared/"]
COPY ["Regioes.Infra/Regioes.Infra.csproj", "Regioes.Infra/"]
RUN dotnet restore "./Regioes.Api/Regioes.Api.csproj"
COPY . .
WORKDIR "/src/Regioes.Api"
RUN dotnet build "./Regioes.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Regioes.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Regioes.Api.dll"]