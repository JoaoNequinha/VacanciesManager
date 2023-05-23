#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

## PROD CONFIG
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY /app .
ENTRYPOINT ["dotnet", "Dashboard.API.dll"]

## DEV CONFIG
#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["Services/Dashboard/Dashboard.API.csproj", "."]
#RUN dotnet restore "Dashboard.API.csproj"
#COPY "Services/Dashboard/" .
#WORKDIR /src
#RUN dotnet publish "Dashboard.API.csproj" -c release -o ./app --no-restore
#
#FROM base AS final
#WORKDIR /app
#COPY /app .
#ENTRYPOINT ["dotnet", "Dashboard.API.dll"]