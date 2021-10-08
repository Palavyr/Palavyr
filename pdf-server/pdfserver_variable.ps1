
$tempDir = "templambda";
$packageFile = "palavyr.pdfserver.zip"
$unpackedSettings = ".\${tempDir}\palavyr-pdf.server.js"
# unzip the package
Expand-Archive $packageFile -DestinationPath $tempDir -Force

#########################################################################
(Get-Content $unpackedSettings -Raw) -replace '#%{Pdf.Server.Port}%#', '#{Pdf.Server.Port}' | Out-File $unpackedSettings -Force

##########################################################################

# Rezip the lambda package

Compress-Archive "${tempDir}/*" -DestinationPath $packageFile -Force

Remove-Item "${tempDir}" -Force -Recurse

