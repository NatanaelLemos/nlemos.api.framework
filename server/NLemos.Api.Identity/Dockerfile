FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["NLemos.Api.Identity/NLemos.Api.Identity.csproj", "NLemos.Api.Identity/"]
RUN dotnet restore "NLemos.Api.Identity/NLemos.Api.Identity.csproj"
COPY . .
WORKDIR "/src/NLemos.Api.Identity"
RUN dotnet build "NLemos.Api.Identity.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NLemos.Api.Identity.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NLemos.Api.Identity.dll"]
