FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
# FROM mcr.microsoft.com/windows/servercore:ltsc2019 AS OS

WORKDIR /src
COPY . .

RUN dotnet restore "Palavyr-Server.sln"

WORKDIR "/src/Palavyr.API"
RUN dotnet build "Palavyr.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Palavyr.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Palavyr.API/Palavyr.API.dll"]



### Load the application into the iis server directory
FROM mcr.microsoft.com/windows/servercore/iis

RUN powershell -NoProfile -Command Remove-Item -Recurse C:\inetpub\wwwroot\*

WORKDIR /inetpub/wwwroot

COPY content/ .