#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/api gateways/RssSE.Bff.Purchases/RssSE.Bff.Purchases.csproj", "src/api gateways/RssSE.Bff.Purchases/"]
RUN dotnet restore "src/api gateways/RssSE.Bff.Purchases/RssSE.Bff.Purchases.csproj"
COPY . .
WORKDIR "/src/src/api gateways/RssSE.Bff.Purchases"
RUN dotnet build "RssSE.Bff.Purchases.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RssSE.Bff.Purchases.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RssSE.Bff.Purchases.dll"]