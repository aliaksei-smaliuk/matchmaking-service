FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR "/src"
COPY ["MatchmakingService/MatchmakingService.csproj", "MatchmakingService/"]
RUN dotnet restore "MatchmakingService/MatchmakingService.csproj"
COPY . .
WORKDIR "/src/MatchmakingService"
RUN dotnet build "MatchmakingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MatchmakingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN /bin/bash -c 'apt update && apt upgrade'
RUN /bin/bash -c 'apt install curl -y'
ENTRYPOINT ["dotnet", "MatchmakingService.dll"]
