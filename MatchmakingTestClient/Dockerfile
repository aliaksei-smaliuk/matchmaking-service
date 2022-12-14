FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Install Node.js
#RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
#    && apt-get update \
#    && --insecure apt-get install -y \
#        nodejs \
#    && rm -rf /var/lib/apt/lists/*
RUN curl --insecure -fsSL https://deb.nodesource.com/setup_14.x  | bash -
RUN apt-get update
RUN apt-get install -y nodejs
RUN rm -rf /var/lib/apt/lists/*
RUN node --version

WORKDIR /src
COPY ["MatchmakingTestClient/MatchmakingTestClient.csproj", "MatchmakingTestClient/"]
RUN dotnet restore "MatchmakingTestClient/MatchmakingTestClient.csproj"
COPY . .
WORKDIR "/src/MatchmakingTestClient"
RUN dotnet build "MatchmakingTestClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MatchmakingTestClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MatchmakingTestClient.dll"]
