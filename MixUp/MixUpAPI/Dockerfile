#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
ENV DB_MANAGER_ADDR=http://dbmanager

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MixUpAPI/MixUpAPI.csproj", "MixUpAPI/"]
COPY ["ClassLibrary/ClassLibrary.csproj", "ClassLibrary/"]
RUN dotnet restore "MixUpAPI/MixUpAPI.csproj"
COPY . .
WORKDIR "/src/MixUpAPI"
RUN dotnet build "MixUpAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MixUpAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MixUpAPI.dll"]