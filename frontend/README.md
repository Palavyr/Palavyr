# Configuration Manager FrontEnd

The Frontend for the configuration manager is where users can configure their conversations, as well as the responses that are sent. Users can configure decision trees, provide detailed or itemized fee estimates, and configure the emails that sent in reponse to a successful convrsation.

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


# Hosting

The frontend is hosted using AWS S3 static website hosting. SSL is provided via CloudFront and the Certificate Manager and DNS is provided by Route53. Records of how this is configured is currently in Notes.md.

For compatibility with react router and webpack, ensure that both the target and error files point to index.html. Cloudfront should also redirect 403 and 404 errors to 200, with redirects to index.html. React router history will take care of the rest.

# Storybook

Storybook is currently used to visually test the components. Soon jest testing will be implemented. To run storybook, do:

    npm run storybook

Testing is currently scant and needs to soon become a major focus.


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
