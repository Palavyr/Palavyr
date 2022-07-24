# This docker file is very simple -
# - it assumes the serve has been published to `server_build`
# - it just copies the app and then provides an entry point

FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY server_build/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "Palavyr.API.dll"]