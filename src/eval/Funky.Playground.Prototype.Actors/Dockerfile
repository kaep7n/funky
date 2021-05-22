#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eval/Funky.Playground.Prototype.Actors/Funky.Playground.Prototype.Actors.csproj", "eval/Funky.Playground.Prototype.Actors/"]
RUN dotnet restore "eval/Funky.Playground.Prototype.Actors/Funky.Playground.Prototype.Actors.csproj"
COPY . .
WORKDIR "/src/eval/Funky.Playground.Prototype.Actors"
RUN dotnet build "Funky.Playground.Prototype.Actors.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Funky.Playground.Prototype.Actors.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Funky.Playground.Prototype.Actors.dll"]