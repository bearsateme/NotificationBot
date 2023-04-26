FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NotificationBot/NotificationBot.csproj", "NotificationBot/"]
RUN dotnet restore "NotificationBot/NotificationBot.csproj"
COPY . .
WORKDIR "/src/NotificationBot"
RUN dotnet build "NotificationBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationBot.dll"]
