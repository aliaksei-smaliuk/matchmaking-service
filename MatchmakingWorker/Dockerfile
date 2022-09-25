FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MatchmakingWorker/MatchmakingWorker.csproj", "MatchmakingWorker/"]
RUN dotnet restore "MatchmakingWorker/MatchmakingWorker.csproj"
COPY . .
WORKDIR "/src/MatchmakingWorker"
RUN dotnet build "MatchmakingWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MatchmakingWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MatchmakingWorker.dll"]
