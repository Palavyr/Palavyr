# Palavyr Chat Bot Designer and Customer Widget

The Frontend for the configuration manager is where users can configure their conversations, as well as the responses that are sent once a customer completes their enquiry via the widget. Users can configure decision trees, provide detailed or itemized fee estimates, and configure the emails that are sent in reponse to a successful convrsation.

Count Lines: https://github.com/AlDanial/cloc/releases/tag/1.88


### Project Organization

Both the Chat Bot Designer and the customer facing widget are built from this project. There are a lot of types that need to be shared somehow between the widget and the designer, so instead of pulling those types out into a separate project (yikes) I decided to instead merge the designer and widget projects together. You'll notice that there are two separate sets of webpack configuration - this allows you to specify two entry points for each project. Webpack will take of sorting out which dependencies need to be bundled.

Two sepearate sets of run and build commands are present in `package.json`.

# Setup

The frontend requires [node.js](https://nodejs.org/en/) and npm to install. With this, you can run:

    npm install

This will install dependencies needed.


To start the Chat Bot Designer website, you can run:

    npm start

To start the Widget, you can run (open a new tab in your terminal):

    npm run start-widget

For the designer to run correctly, you will also need the pdf server to be running. That server is housed in a public repo:

    https://github.com/Palavyr/PalavyrPdfServer

Clone this repo, and start the server as a local microservice.


Alternatively, if you'd like to start all services together including this one, navigate up one directory and run:

    ./startServices.ps1

This will run the frontend and express server.



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

Storybook is currently used to visually test some of the components. Soon jest testing will be implemented. To run storybook, do:

    npm run storybook

Heads up - this might be broken at the moment. There have been a lot of structural changes to Palavyr and I  haven't updated Storybook in a while.

Testing in general is currently scant.
