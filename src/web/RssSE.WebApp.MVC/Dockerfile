#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./web/RssSE.WebApp.MVC/RssSE.WebApp.MVC.csproj", "web/RssSE.WebApp.MVC/"]
COPY ["./building blocks/RssSE.Core/RssSE.Core.csproj", "building blocks/RssSE.Core/"]
COPY ["./building blocks/RssSE.WebApi.Core/RssSE.WebApi.Core.csproj", "building blocks/RssSE.WebApi.Core/"]
RUN dotnet restore "web/RssSE.WebApp.MVC/RssSE.WebApp.MVC.csproj"
COPY . .
WORKDIR "web/RssSE.WebApp.MVC"
RUN dotnet build "RssSE.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RssSE.WebApp.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RssSE.WebApp.MVC.dll"]