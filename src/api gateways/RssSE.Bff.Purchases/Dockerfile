#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./api gateways/RssSE.Bff.Purchases/RssSE.Bff.Purchases.csproj", "api gateways/RssSE.Bff.Purchases/"]
COPY ["./building blocks/RssSE.Core/RssSE.Core.csproj", "building blocks/RssSE.Core/"]
COPY ["./building blocks/RssSE.WebApi.Core/RssSE.WebApi.Core.csproj", "building blocks/RssSE.WebApi.Core/"]
COPY ["./building blocks/RssSE.MessageBus/RssSE.MessageBus.csproj", "building blocks/RssSE.MessageBus/"]

RUN dotnet restore "api gateways/RssSE.Bff.Purchases/RssSE.Bff.Purchases.csproj"
COPY . .
WORKDIR "api gateways/RssSE.Bff.Purchases"
RUN dotnet build "RssSE.Bff.Purchases.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RssSE.Bff.Purchases.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RssSE.Bff.Purchases.dll"]