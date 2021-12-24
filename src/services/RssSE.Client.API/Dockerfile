#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./services/RssSE.Client.API/RssSE.Client.API.csproj", "services/RssSE.Client.API/"]
COPY ["./building blocks/RssSE.Core/RssSE.Core.csproj", "building blocks/RssSE.Core/"]
COPY ["./building blocks/RssSE.WebApi.Core/RssSE.WebApi.Core.csproj", "building blocks/RssSE.WebApi.Core/"]
COPY ["./building blocks/RssSE.MessageBus/RssSE.MessageBus.csproj", "building blocks/RssSE.MessageBus/"]

RUN dotnet restore "services/RssSE.Client.API/RssSE.Client.API.csproj"
COPY . .
WORKDIR "services/RssSE.Client.API"
RUN dotnet build "RssSE.Client.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RssSE.Client.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RssSE.Client.API.dll"]