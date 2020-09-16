# Configuration Manager Frontend

This UI lets users configure both the chat widget content as well as the resultant fee estimates.










#### Setting up webpack with aliases and typescript
https://medium.com/better-programming/the-right-usage-of-aliases-in-webpack-typescript-4418327f47fa

#### Fixing the Cannot GET /dashboard error
https://ui.dev/react-router-cannot-get-url-refresh/

#### What if I can't find the type definitions for some external library? I have t make my own:
https://www.credera.com/insights/typescript-adding-custom-type-definitions-for-existing-libraries/

#### Missing Microsoft Server!
https://stackoverflow.com/questions/22104739/iis-manager-cannot-verify-whether-the-built-in-account-has-access

#### FTP on Windows server with iis
https://www.youtube.com/watch?v=MSYubvIs8NI&ab_channel=CodewithMarci

#### Getting the website happening
https://www.youtube.com/watch?v=ik3NanW0dAc&ab_channel=CloudLearning


#### AWS Tips on hosting we app on IIS windows server -- best guide realy
https://aws.amazon.com/getting-started/hands-on/host-net-web-app/


#### Turns out hosting asp.net core web api on windows server in aws is a fucking nightmare. Instead azure.
https://medium.com/@lifei.8886196/how-to-deploy-net-core-web-api-to-azure-a127bfb20d09


#### CORS must come AFter useRouting and before UseEndpoints
https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1

#### We also should install the cors module for IIS
https://www.iis.net/downloads/microsoft/iis-cors-module

#### If we use S3, we want SLL?
https://medium.com/@sbuckpesch/setup-aws-s3-static-website-hosting-using-ssl-acm-34d41d32e394


#### THIS MAY FIX CORS
https://stackoverflow.com/questions/59511862/faliure-to-use-cors-in-webapi-in-net-core-3-1

#### NPE> CLOSEST BULLSHIT TO MY PROBLEM:
https://stackoverflow.com/questions/21445885/duplicate-headers-received-from-iis-in-cors-process?rq=1


#### Refreshing my single page react app bundld with webpack gives a 404 - need to map all urls to the index.html
https://hackernoon.com/hosting-static-react-websites-on-aws-s3-cloudfront-with-ssl-924e5c134455


#### Silly boy, you need to specify the error page as the index.html in s3
https://stackoverflow.com/questions/51218979/react-router-doesnt-work-in-aws-s3-bucket


#### How I made my site Secure with SSL in AWS
https://tynick.com/blog/05-30-2019/how-to-create-s3-static-website-with-https-its-so-easy/

#### How I will handle bouncebacks
https://aws.amazon.com/blogs/messaging-and-targeting/handling-bounces-and-complaints/


#### How I installed SSL certificates on to the IIS windows server ( I did this on the server )
https://www.win-acme.com/
https://github.com/win-acme/win-acme


#### How I ran SQL Migrations
1. export the  migration as a script (dotnet ef migrations script --context AccountsContext --output addRego --startup ..\DashboardServer.API\)
2. https://database.guide/5-ways-to-run-sql-script-from-file-sqlite/


#### How I added Culture Info for currency
https://lonewolfonline.net/list-net-culture-country-codes/
https://stackoverflow.com/questions/2453951/c-sharp-double-tostring-formatting-with-two-decimal-places-but-no-rounding


#### How I made IIS Always running
https://docs.hangfire.io/en/latest/deployment-to-production/making-aspnet-app-always-running.html


#### How I got hangfire wrking!
https://www.youtube.com/watch?v=sQyY0xvJ4-o&ab_channel=DotNetCoreCentral


#### Special headers...
// "Server": "Microsoft-IIS/10.0",
// "Access-Control-Allow-Origin": "*",
// "Access-Control-Allow-Headers": "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With",
// "Access-Control-Allow-Methods": "DELETE, POST, GET, OPTIONS, PUT"
// "Access-Control-Allow-Headers": "*",
// "Access-Control-Allow-Methods": "*"


#### Old SQLServer connction strings
"Server": "Server=RegEx;Database=DashboardDev;Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
"ServerAccounts": "Server=RegEx;Database=Accounts;Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
"DynamicTables": "Server=RegEx;Database=DynamicTables;Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",



### Per user sqlite string example
"Sqlite": "Data Source=C:\\ConvoBuilderUserData\\user-1\\database\\user-1.db"



#### Test a Preflight request
curl -H "Origin: https://localhost/" -H "Access-Control-Request-Method: POST" -H "Access-Control-Request-Headers: X-Requested-With" -X OPTIONS --verbose https://server.palavyr.com



#### Old setup for scripts on webpack

"scripts": {
    "start": "set NODE_ENV=development && webpack-dev-server --config ./webpack/webpack.config.dev.js --mode development --open --hot --progress",
    "build": "set NODE_ENV=production webpack --mode production --config ./webpack/webpack.config.prod.js",
    "test": "jest --coverage",
    "storybook": "start-storybook -p 6006 -s public",
    "build-storybook": "build-storybook -s public"
  },



# Bucket Settings

### CORS policy:


<?xml version="1.0" encoding="UTF-8"?>
<CORSConfiguration xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
<CORSRule>
    <AllowedOrigin>*</AllowedOrigin>
    <AllowedMethod>GET</AllowedMethod>
    <AllowedMethod>POST</AllowedMethod>
    <AllowedMethod>PUT</AllowedMethod>
    <MaxAgeSeconds>25000</MaxAgeSeconds>
    <AllowedHeader>*</AllowedHeader>
</CORSRule>
</CORSConfiguration>



### Permissions bucket policy

{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "PublicReadGetObject",
            "Effect": "Allow",
            "Principal": "*",
            "Action": "s3:GetObject",
            "Resource": "arn:aws:s3:::widget.palavyr.com/*"
        }
    ]
}

Endpoint:
http://palavyr.com.s3-website-us-east-1.amazonaws.com




#### Solutions fo
