# Configuration Manager FrontEnd

The Frontend for the configuration manager is where users can configure their conversations, as well as the responses that are sent. Users can configure decision trees, provide detailed or itemized fee estimates, and configure the emails that sent in reponse to a successful convrsation.

Count Lines: https://github.com/AlDanial/cloc/releases/tag/1.88
# Setup and dev

The frontend requires [node.js](https://nodejs.org/en/) and npm to install. With this, you can run:

    npm install

This will install dependencies needed.

The frontend has a custom webpack configuration that works well with storybook. To start the webpack dev server, run:

    npm start

Alternatively, if you'd like to start all services together including this one, navigate up one directory and run:

    ./startServices.ps1

This will run the frontend and express server.

The front end webpack server runs on port 8080.


## Checking the production build

To check the production build locally, build the distribution:

    npm run build

and then, from the dist directory, run:

    http-server

`http-server` should be globally installed (`npm install -g http-server`).

# Hosting

The frontend is hosted using AWS S3 static website hosting. SSL is provided via CloudFront and the Certificate Manager and DNS is provided by Route53. Records of how this is configured is currently in Notes.md.

For compatibility with react router and webpack, ensure that both the target and error files point to index.html. Cloudfront should also redirect 403 and 404 errors to 200, with redirects to index.html. React router history will take care of the rest.

# Storybook

Storybook is currently used to visually test the components. Soon jest testing will be implemented. To run storybook, do:

    npm run storybook

Testing is currently scant and needs to soon become a major focus.

# Viewing SVGs on windows
https://github.com/tibold/svg-explorer-extension/releases



### S3 bucket settings

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


### JWT Bearer Auth with SSL ONLY

https://devblogs.microsoft.com/aspnet/jwt-validation-and-authorization-in-asp-net-core/
https://www.thecodebuzz.com/create-generate-jwt-token-asp-net-core-example/


GREAT video on jwt
https://www.youtube.com/watch?v=vWkPdurauaA&ab_channel=DotNetCoreCentral

https://www.youtube.com/watch?v=vWkPdurauaA&ab_channel=DotNetCoreCentral
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/samples?view=aspnetcore-3.1
https://stackoverflow.com/questions/47809437/how-to-access-current-httpcontext-in-asp-net-core-2-custom-policy-based-authoriz

https://referbruv.com/blog/posts/implementing-custom-authentication-scheme-and-handler-in-aspnet-core-3x
https://www.c-sharpcorner.com/article/asp-net-core-web-api-creating-and-validating-jwt-json-web-token/
https://docs.microsoft.com/en-us/aspnet/core/security/authorization/simple?view=aspnetcore-3.1
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1
https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-3.1

https://medium.com/mickeysden/react-and-google-oauth-with-net-core-backend-4faaba25ead0
https://stackoverflow.com/questions/58758198/does-addjwtbearer-do-what-i-think-it-does
https://medium.com/@M3rken/asp-net-core-supporting-multiple-authorization-route-branching-cad3ab632410
https://developers.google.com/identity/sign-in/web/server-side-flow#python
https://wildermuth.com/2018/04/10/Using-JwtBearer-Authentication-in-an-API-only-ASP-NET-Core-Project
https://stackoverflow.com/questions/40988238/sending-the-bearer-token-with-axios

## Dependency injection full
https://auth0.com/blog/dependency-injection-in-dotnet-core/