FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CityVilleDotnet.Api/CityVilleDotnet.Api.csproj", "CityVilleDotnet.Api/"]
COPY ["CityVilleDotnet.Common/CityVilleDotnet.Common.csproj", "CityVilleDotnet.Common/"]
COPY ["CityVilleDotnet.Domain/CityVilleDotnet.Domain.csproj", "CityVilleDotnet.Domain/"]
COPY ["CityVilleDotnet.Persistence/CityVilleDotnet.Persistence.csproj", "CityVilleDotnet.Persistence/"]

RUN dotnet restore "CityVilleDotnet.Api/CityVilleDotnet.Api.csproj"

COPY . .

WORKDIR "/src/CityVilleDotnet.Api"
RUN dotnet build "CityVilleDotnet.Api.csproj" -c Release -o /app/build

RUN dotnet publish "CityVilleDotnet.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CityVilleDotnet.Api.dll"]