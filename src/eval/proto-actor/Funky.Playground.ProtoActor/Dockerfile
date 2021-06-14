#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eval/proto-actor/Funky.Playground.ProtoActor/Funky.Playground.ProtoActor.csproj", "eval/proto-actor/Funky.Playground.ProtoActor/"]
RUN dotnet restore "eval/proto-actor/Funky.Playground.ProtoActor/Funky.Playground.ProtoActor.csproj"
COPY . .
WORKDIR "/src/eval/proto-actor/Funky.Playground.ProtoActor"
RUN dotnet build "Funky.Playground.ProtoActor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Funky.Playground.ProtoActor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Funky.Playground.ProtoActor.dll"]