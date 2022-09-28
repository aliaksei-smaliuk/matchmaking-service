# Matchmaking POC
## Repository contains 3 services implementation
- matchmaking-service
- matchmaking-worker
- matchmaking-test-client

## Infrastructure
Infrastructure contains components:
- matchmaking-service (ASP.NET)
- matchmaking-worker (ASP.NET)
- matchmaking-test-client (ASP.NET)
- kafka
- Redis

## Build and execution
To build or rebuild use command
```
docker compose build
```
To deploy all services localy inside docker compose use command
> Kafka container is starting slowly. Please wait before starting request generation. 
```
docker compose up
```
To stop and delete all containers
```
docker compose down
```
Solution with all services could be opened using Visual Studio or Rider.

## Documentation
More details could be found in documentation.
[PRD](https://docs.google.com/document/d/16n2lYSyLASt-VRa8XY4FUwTf1Wf7NWV-CCn1hlMVGVc/edit?usp=sharing)
[HLD/SDD](https://docs.google.com/document/d/10II0gt0qYYWAHpStQo0PKnZrcwFXQPZ-N9eHPl1svNY/edit?usp=sharing)
