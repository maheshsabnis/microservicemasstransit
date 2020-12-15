#Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS builder
WORKDIR /src
COPY ./ASP_Net_CorePublisher.csproj .
RUN dotnet restore ASP_Net_CorePublisher.csproj
COPY . .
RUN dotnet build ASP_Net_CorePublisher.csproj -c Debug -o /src/out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
COPY --from=builder /src/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "ASP_Net_CorePublisher.dll"]
