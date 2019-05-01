FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["NLemos.Api.Gateway/NLemos.Api.Gateway.csproj", "NLemos.Api.Gateway/"]
RUN dotnet restore "NLemos.Api.Gateway/NLemos.Api.Gateway.csproj"
COPY . .
WORKDIR "/src/NLemos.Api.Gateway"
RUN dotnet build "NLemos.Api.Gateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NLemos.Api.Gateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NLemos.Api.Gateway.dll"]
