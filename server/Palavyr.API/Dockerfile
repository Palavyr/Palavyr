#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["Palavyr.API/Palavyr.API.csproj", "Palavyr.API/"]
COPY ["Palavyr.EmailService/Palavyr.EmailService.csproj", "Palavyr.EmailService/"]
COPY ["Palavyr.Data/Palavyr.Data.csproj", "Palavyr.Data/"]
COPY ["Palavyr.Domain/Palavyr.Domain.csproj", "Palavyr.Domain/"]
COPY ["Palavyr.Common/Palavyr.Common.csproj", "Palavyr.Common/"]
COPY ["Palavyr.Background/Palavyr.Background.csproj", "Palavyr.Background/"]
COPY ["Palavyr.PDFService/Palavyr.PDFService.csproj", "Palavyr.PDFService/"]
RUN dotnet restore "Palavyr.API/Palavyr.API.csproj"
COPY . .
WORKDIR "/src/Palavyr.API"
RUN dotnet build "Palavyr.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Palavyr.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/windows/servercore:ltsc2019
RUN dism.exe /online /enable-feature /all /featurename:iis-webserver /NoRestart

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Palavyr.API.dll"]