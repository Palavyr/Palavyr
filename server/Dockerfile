# This docker file is very simple -
# - it assumes the serve has been published to `server_build`
# - it just copies the app and then provides an entry point

FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY artifacts/serverbuild App/
WORKDIR /App


ENV STRIPE__SecretKey ""
ENV AWS__AccessKey ""
ENV AWS__SecretKey ""
ENV AWS__Region ""
ENV AWS__UserDataBucket ""
ENV AWS__PdfUrl ""
ENV ConnectionString ""
ENV Environment ""
ENV JWT__SecretKey ""

ENV AWS__AwsS3ServiceUrl=s3.amazonaws.com
ENV AWS__AwsSESServiceUrl=ses.amazonaws.com

ENTRYPOINT ["dotnet", "Palavyr.API.dll"]