# Palavyr Server :D

# Getting started

### Set your environmental variables
All env vars are prefixed with 'Palavyr__'

Palavyr__ConnectionString=
Palavyr__AWS_SecretKey=
Palavyr__AWS_AcessKey=
Palavyr__AWS_Region=
Palavyr__STRIPE_SecretKey=
Palavyr__STRIPE_WebhookKey=
Palavyr__AWSDATA_UserDataBucket=

Palavyr__JWT_SecretKey=
Palavyr__JWT_Secret=
Palavyr__JWT_Issuer=
Palavyr__JWT_Audience=
Palavyr__JWT_Expiration=
Palavyr__JWT_RefreshExpiration=

PALAVYR__PDFSERVER_Url=
PALAVYR__Environment=


## OLD APPSETTINGS.JSON

{
"Serilog": {
"MinimumLevel": {
"Default": "Information",
"Override": {
"Microsoft": "Warning",
"Microsoft.Hosting.Lifetime": "Information"
}
},
"WriteTo": [
{
"Name": "File",
"Args": {
"path": "./logs/log-.txt",
"rollingInterval": "Day"
}
}
]
},
"AllowedHosts": "*",
"ConnectionString": "",
"AWS": {
"SecretKey": "",
"AccessKey": "",
"Region": "",
"ProfilesLocation": ""
},
"Stripe": {
"SecretKey": "",
"WebhookKey": "whsec_c4ef2b671e5305562067e3a8241bc4591ea8d8b528ffb71c203b2df3eb843303"
},
"JWTSecretKey": "SomeSecretKeyagasgasghery336356345345",
"Previews": "dev-palavyr-previews",
"Userdata": "palavyr-user-data-development",
"jwtTokenConfig": {
"secret": "1234567890123456789",
"issuer": "https://mywebapi.com",
"audience": "https://mywebapi.com",
"accessTokenExpiration": 20,
"refreshTokenExpiration": 60
},
"Pdf.Server.Port": "",
"Pdf.Server.Host": "https://pn81y1hx02.execute-api.us-east-1.amazonaws.com/production",
"Pdf.Server.Apikey": "",
"Palavyr.Server.Environment": "Development"
}

