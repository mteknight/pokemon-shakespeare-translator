#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Poketranslator.API/Poketranslator.API.csproj", "Poketranslator.API/"]
RUN dotnet restore "Poketranslator.API/Poketranslator.API.csproj"
COPY . .
WORKDIR "/src/Poketranslator.API"
RUN dotnet build "Poketranslator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Poketranslator.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poketranslator.API.dll"]