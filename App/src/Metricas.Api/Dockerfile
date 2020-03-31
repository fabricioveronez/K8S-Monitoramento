FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Metricas.Api/Metricas.Api.csproj", "Metricas.Api/"]
RUN dotnet restore "Metricas.Api/Metricas.Api.csproj"
COPY . .
WORKDIR "/src/Metricas.Api"
RUN dotnet build "Metricas.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Metricas.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Metricas.Api.dll"]
