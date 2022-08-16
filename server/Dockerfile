# This docker file is very simple -
# - it assumes the serve has been published to `server_build`
# - it just copies the app and then provides an entry point

FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY artifacts/serverbuild/ .

ENV Palavyr_STRIPE__SecretKey ""
ENV Palavyr_AWS__AccessKey ""
ENV Palavyr_AWS__SecretKey ""
ENV Palavyr_AWS__Region ""
ENV Palavyr_AWS__UserDataBucket ""
ENV Palavyr_AWS__PdfUrl ""
ENV Palavyr_ConnectionString ""
ENV Palavyr_Environment ""
ENV Palavyr_JWT__SecretKey ""

ENV Palavyr_AWS__AwsS3ServiceUrl ""
ENV Palavyr_AWS__AwsSESServiceUrl ""

ENTRYPOINT ["dotnet", "Palavyr.API.dll"]