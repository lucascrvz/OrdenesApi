FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["OrdenesApi/OrdenesApi.csproj", "OrdenesApi/"]
RUN dotnet restore "OrdenesApi/OrdenesApi.csproj"

COPY . .
WORKDIR "/src/OrdenesApi"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrdenesApi.dll"]
