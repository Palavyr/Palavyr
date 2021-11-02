# Palavyr Configuration Manager

The Palavyr Configuration Manager project includes 4 subprojects:

1. [Server](./server/README.md)
2. [Website](./frontend/README.md) // customers will use this to design and program the widget
3. [Widget](./widget/README.md) // customers will use this with their customers

For information on each component follow the links above.

# Overview

The Frontend and the Widget are both served as a static websites from S3 buckets. The frontend is used to configure the chat widget conversations as well as email responses. Each of these sit behind AWS CloudFront which provides SSL certificates DNS is provided by Route53. The frontend and Widget connect to the same API, which uses postgres as a database. Essentially, The frontend is used to configure conversation nodes. These nodes are stored in the database via the API. The widget will then request a node set and execute a conversation.

## TODO

   - I recently tore down the Palavyr infra on AWS to save money and try to redesign things to work in serverless (aws lambda). It would be great if we could get things working 100% serverless to save on operating costs. Otherwise, figuring out a better EC2 situation would be good (staging and prod from a sincle server.) Figure out how to run fully in serverless (for cheap) or figure out how to run staging and prod on a single ec2 instance. Load balancer provides ssl.
   -  We should use something like localstack in dev. (It spins up aws services locally in a dev environment) Doing so gets us around having to use aws during development.
   -  Extract the duplicated code in the chat bot desiger and replace it with the newly integrated widget code base. A nuanced refactor job.
   -  Get someone to sponsor the project or pay for the product. That would be cool. No pressure though. At the very least this is a really fun project to work on for funzies.


# Setup Guide

Follow this guide to get set up with development.


1. Download and install postgres 14

    https://www.postgresql.org/download/windows/


2. Use the download to create a postgres server - use default settings


3. Clone PalavyrPdfServer to user/code

    `git clone git@github.com:Palavyr/PalavyrPdfServer.git`

4. Build and Start the pdf server by using

   `npm start` (to use serverless offline) OR `npm run start-ui` (to use a webpack build and dev server).

5. Clone ConfigurationManager to user/code

    `git clone git@github.com:Palavyr/Configuration-Manager.git`

6. Step in to ConfigurationManager/server/utilities and run

        `./SetDevelopmentalSecrets.ps1`

7. Start the DB migrator

    `dotnet run --project Palavyr.Data.Migrator`

8.  Build the server (in an IDE or using `dotnet build`)

9.  Run the server (via IDE or using `dotnet run --project Palavyr.API`)

10. Step into ConfigurationManager/ui and build and run both the widget and the website using `npm start`, and in a separate window using `npm run start-widget`

11. Navigate to `localhost:8080` and create an account to begin developing

Thanks for your interest in contributing!




### Stages

There are two stages - Staging and Production.

Each component sites behind a staging subdomain which is deployed to first before packages are deployed to production. Staging deployments are automatic, whereas production deployments require manual intervention. Staging deployments can be broken, however at no point should production deployments be broken! These deployments are sacred! Only releases validated in staging shall pass!

### Build

Team city has been configured to build each component. Currently, Team City is configured on Paul's Regex desktop and pings github ever 10 seconds to look for changes (this should probably be brought up to around 60 seconds...). There are 3 build agents installed on the machine. The following are the basic steps to install a new build agent

[StackOverflow link to some responses](https://stackoverflow.com/questions/1789212/running-multiple-teamcity-agents-on-the-same-computer)
[Quick link to a tutorial](https://handcraftsman.wordpress.com/2010/07/20/multiple-teamcity-build-agents-on-one-server/)

1. Use the installer to install the agent. Install it to C:\TeamCity\BuildAgent1 (or 2...)
2. Provide the build url (localhost:8210 in my case) and Agent port (e.g. 9061)
3. Before you click the 'OK' button, edit the build agent config file `C:\TeamCity\BuildAgent1\launcher\conf\wrapper.conf`
4. Modify the following by adding a number for the build agent:
```
    # Title
    wrapper.console.title=TeamCity Build Agent 1 <-increment this number!

    # Name of the service
    wrapper.ntservice.name=TCBuildAgent1 <-increment this number!

    # Display name of the service
    wrapper.ntservice.displayname=TeamCity Build Agent 1 <-increment this number!

    # Description of the service
    wrapper.ntservice.description=TeamCity Build Agent Service 1 <-increment this number!
```
5. Complete the installation.

The build chain is configured to automatically push packages to Octopus Deploy and create a new release.

**Port**
To navigate to team city's UI, on REGEX you can navigate to http://localhost:8210. Team city is a windows service that runs as a background service. It is not necessary to visit


### Deploy with Octopus Deploy

We use Octopus Deploy to deploy to releases to the staging and production servers.

[Octopus Deploy Cloud](https://palavyr.octopus.app/)

Octopus deploy derives the release count from the semantic version baked into the package names that are pushed from Team City.

**Bumping release versions**
If we want to bump the major or minor version of the application, we need to do this manually in team city. Later we can consider using git versions later, but for now we manually do this in Team City.

Patch version are incremented automatically by team city.

**Octopus release versions**
As stated, the release version comes from Team City. Therefore, we should NOT create releases manually in Octopus Deploy. If we need to manually create a release for some reason, we should rename the current release using a sem ver compatible postfix (e.g. -pre or 0.0.1-post). Otherwise, Team City package numbers will provide the release number.

**Server predeployment service halt**
If we don't stop IIS before we deploy, we will encounter a file lock on the .dlls that contain the application. To prevent this file lock, we introduce a step in the deployment processess that triggers the web server to shut down until a startup signal is received (octopus deploy is configured to automatically start the IIS service after the API is installed). To do this, we use the following step in the deployment pipeline BEFORE the API is deployed to IIS on windows server:

    cd "C:\www\palavyrServer"
    try {
        New-Item -Path '.\App_Offline.htm' -ItemType File
    } catch {
        Write-Host "App_Offline.htm already exists on this target in the install dir"
    }

This is a simple script that tries to create an empty file called `App_Offline.htm`. When this file is present, IIS will stop the website pointing at this custom directory. Its a bit hacky, but its commonly used. [Reference](https://stackoverflow.com/questions/49660923/why-is-iis-worker-process-locking-a-file)

## Databases

This application uses Postgres. To manage database migrations, we use DBUP. DBUP is implemented as a separeate console app embedded into the server as its own project. When the project is deployed to staging or production, the following startup script runs in Octopus Deploy as a post-deployment script:

    write-host "---------"
    $OctopusParameters["Octopus.Action.Package.InstallationDirectoryPath"]
    write-host "---------"

    cd $OctopusParameters["Octopus.Action.Package.InstallationDirectoryPath"]

    Write-Host "Setting the current environment variable for running the migrator in #{Palavyr.Environment}"
    $env:ASPNETCORE_ENVIRONMENT=$OctopusParameters["Octopus.Environment.Name"]
    set ASPNETCORE_ENVIRONMENT=$OctopusParameters["Octopus.Environment.Name"]

    Write-Host "Starting the migrator..."
    $databaseProcess = Start-Process Palavyr.Data.Migrator.exe -PassThru -Wait -NoNewWindow
    exit $databaseProcess.ExitCode

    Write-Host "Migrations completed!"

This will bring the postgres database up to speed and apply any new migrations. This is run every time we deploy, so it acts as a kind of history of database structure state.

For details on how to write a code-first migration using EF Core, see the [Server Readme](./server/README.md)

# Development startup

To begin development, run `cleanSetup.ps1` followed by `startServices.ps1`. This will retrieve and install all of the dependencies for development, and then start the necessary services. The API server should be started using a configuration from either Jetbrains Rider or Visual Studio 2019 - however if you intend to only develop on the frontend, you can provide an optional flag to `startServices.ps1 `.

    ./startService.ps1 -runServer

This will run dotnet run to run the server application. (TODO)

To manually refresh the database, you can use curl:

    curl --location \
         --request PUT 'https://localhost:5001/api/setup' \
         --header 'action: secretDevAccess' \
         --header 'accountId: dashboardDev'


# Staging and Production Servers

**This section will outline, in more or less order, of how to configure a windows server**

Deployment is not entirely automated (It is mostly automated!). We currently use AWS EC2 to host Windows Server instances

    AMI Name: Windows_Server-2019-English-Full-Base-2020.08.12

There are two, one for production and one for staging. This infrastructure is currently static, however in the future we may dynamically create this infrastructure using a provider such as terraform. For now, if we start up a new isntance, there is some configuration required.

Microsoft provides a [guide](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1) on configuration. We add a few extra things for our specific solution.

This is the current 3rd party software being used:

[Win-Acme](https://www.win-acme.com/) (to create certificates on prem)

    win-acme.v2.1.10.896.x86.pluggable

[Dotnet Hosting](https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-aspnetcore-3.1.8-windows-hosting-bundle-installer)

    dotnet-hosting-3.1.8-win.exe

[FireFox Browser](https://www.mozilla.org/en-US/firefox/new/) (For using pgAdmin with Postgres)

    Firefox Installer.exe

[Node](https://nodejs.org/en/) (For express)

    node-v12.18.4-x64.msi

[Octopus Tentacle](https://octopus.com/downloads/tentacle) (For deployments)

    Octopus.Tentacle.6.0.0-x64.msi

[Postgres](postgresql-13.0-1-windows-x64.exe) (Database. Don't use the stacks, but include pgAdmin, the server, and cli tools)

    postgresql-13.0-1-windows-x64.exe


**Application data folder**
The Application data folder sits outside of the application and persists user data. The directory is regularly backed up using Hangfire. This directory needs to be created up front:

    C:\PalavyrData

... and the PERMISSIONS need to be set manually (until I can automate this.) You know the drill, right click, properties, edit permissions, and give full read/write permissions to all users. Otherwise the API server won't be able to delete transient files such as pdf previews after they written to S3.

<img src="./assets/DataInstallation.PNG" alt="drawing" width="300"/>
<img src="./assets/DirectoryPermissions.PNG" alt="drawing" width="300"/>


**IIS Configuration**
There is some configuration to perform on the Windows server with regard to setting up IIS, but its pretty minimal. The majority of configuration is performed via [this guide from microsoft](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1).

Once IIS is enabled, various SSL options should be enabled.

**Internet access**
Open Server Manager, and select the Local Server. In the main window, there is an option IE Enhanced Security Configuration. All this needs to be turned off.

<img src="./assets/EnableInternet.PNG" alt="drawing" width="300"/>

The following images demonstrate the current settings be used in staging and production:

#### Server Roles

<img src="./assets/IISRolesImage1.PNG" alt="drawing" width="300"/>
<img src="./assets/IISRolesImage2.PNG" alt="drawing" width="300"/>

#### Server Features

<img src="./assets/IISFeaturesImage1.PNG" alt="drawing" width="300"/>
<img src="./assets/IISFeaturesImage2.PNG" alt="drawing" width="300"/>


# Create the website

In IIS, we simply need to create a new website. Don't worry about SSL yet, just create the website as follows:

<img src="./assets/CreateWebsite1.PNG" alt="drawing" width="300"/>

In order to secure using SSL, install a certificate. (The S3 buckets are secured via cloudfront using the certificate manager.)


# Installing Certificates
Once the website is made, youcan install a certificate using Win-Acme to install certificates on the server. Basically download the software (In one instance I needed to download a slightly older version and use the 32bit x86 version for this to work) and just follow the prompt.

So long as the DNS has been set up correctly and outbound traffic is allowed, it should create and install a certificate.

Once the certificate is install, retrieve the thumbprint and save it as a variable in Octopus Deploy. Deploying to IIS requires bindings (well its optional, but it should be done), and to do that, you can choose and externally managed store (the server) and provide a thumbprint for Octopus to find and verify

#### Retrieve the Certificate thumbprint
This can be done in powershell.

    Get-ChildItem -path cert:\LocalMachine\Root
    # (root, or whichever. Run cert:\LocalMachine to get a list)

You may have to look in a few places.

Octopus will apply the bindings, you can confirm the bindings in the IIS manager after the first deploy.

#### Enable SSL on the box

On the IIS manager, you can select the website and enable ssl and the IP bindings.

<img src="./assets/EnableSSL1.PNG" alt="drawing" width="300"/>
<img src="./assets/EnableSSL2.PNG" alt="drawing" width="300"/>
<img src="./assets/EnableSSL3.PNG" alt="drawing" width="300"/>

### Making IIS always running for Hangfire. I followed these settings in IIS

https://docs.hangfire.io/en/latest/deployment-to-production/making-aspnet-app-always-running.html


## Products

Products are stored in stripe. We currently keep the product ID in the frontend. Should put this in the server.
METADATA: MUST BE SET

