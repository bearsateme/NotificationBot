FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WhoGivesACluck.csproj", ""]
RUN dotnet restore "WhoGivesACluck.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "WhoGivesACluck.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WhoGivesACluck.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhoGivesACluck.dll"]
