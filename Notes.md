################################### FRONTEND WITH SERVER ON IIS ###########################################

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



################################################# LINUX ########################################################

## Super user in pgadmin 
https://chartio.com/learn/postgresql/create-a-user-with-pgadmin/


### IT IS VERY IMPORTANT THAT YOU DO NOT RUN THE SERVER USING SUDO - THIS WILL OVERRIDE ALL ENVIRONMENTAL VARIABLES 

### IIS Is locknig a file... heres what you can do
https://stackoverflow.com/questions/49660923/why-is-iis-worker-process-locking-a-file

## Setup

This repo is two part:

1. The Server backend (written in C#)
2. The frontend portal (written in typescript/react)
---

#### Starting the listening tentacle on the ubuntu server
sudo /opt/octopus/tentacle/Tentacle service --install --start --instance "palavyrUbuntuTentacle"


# Working on linux to set up postgress and finding processes on ports and killing them
 https://appuals.com/how-to-kill-process-on-port/



    # get the process associated with a given port
    sudo lsof -i:5000

    # example output from above
    COMMAND   PID USER   FD   TYPE DEVICE SIZE/OFF NODE NAME
    dotnet  53198 root  199u  IPv4 146484      0t0  TCP localhost:5000 (LISTEN)
    dotnet  53198 root  201u  IPv6 146489      0t0  TCP ip6-localhost:5000 (LISTEN)

    # kill a process on that port
    sudo kill {PID}

    # list ports that are in use
    sudo ss -tulwn

### Running kestrel as a service
https://swimburger.net/blog/dotnet/how-to-run-aspnet-core-as-a-service-on-linux

Once the service config is installed to 

    /etc/systemd/system/PalavyrServer.service

    sudo cp PalavyrServer.service /etc/systemd/system/PalavyrServer.service
    sudo systemctl daemon-reload

To query the status of the service:

    systemctl status PalavyrServer

To enable automatic restart of the service in event of a crash:

    sudo systemctl enable AspNetSite




#### Getting postgres to work and also attaching to postgres on EC2 from local 
https://www.shubhamdipt.com/blog/postgresql-on-ec2-ubuntu-in-aws/

    sudo apt-get update && sudo apt-get -y upgrade
    sudo apt-get install postgresql postgresql-contrib​

    # go into psql
    sudo -u postgres psql
    postgres=#\password​

    # Edit pg_hba.conf in vim
    sudo vim /etc/postgresql/10/main/pg_hba.conf

    # Near bottom of file after local rules, add rule (allows remote access):
    host    all             all             0.0.0.0/0               md5

    # save file​

    # Edit config in vim
    sudo vim /etc/postgresql/10/main/postgresql.conf

    # Change line 59 to listen to external requests:
    listen_address='*'
    
    # save file​

    # Restart postgres server
    sudo /etc/init.d/postgresql restart​

### Set up ftp with ubuntu EASY
https://www.youtube.com/watch?v=Qxs7CYguo70&ab_channel=MicrowaveSam


### Certificates
https://www.rushtime.in/softwares/download-openssl-for-windows-free/
I was able to get a free certificate from zero SSL - it came in three parts (ca_bundle, certificate.crt, private.key)

I converted this to a .pfx using openssl.exe from

command: 
    pkcs12 -export -out C:\Users\paule\code\palavyr\server.palavyr.com\ready_certificate.pfx -inkey C:\Users\paule\code\palavyr\server.palavyr.com\private.key -in C:\Users\paule\code\palavyr\server.palavyr.com\certificate.crt

This gave me a cert that i stored in ocotpus deploy. There is a password. hint: ducks are rub. how many days of christmas.


    
################## OLD MANUAL Database Migrations ##############################################


Palavyr uses a (poorly) designed ORM database scheme implemented using EF Core. The following details how database migrations can be handled.

For reference, I used this website: [Automating ef core sql migraitons](https://www.huuhka.net/automating-net-ef-core-sql-migration-script-creation/)

#### Modifying the database and creating a migration

The Database Schema is defined in the Palavyr.Domain. This contains all of the table columns and their types. The Database Contexts are defined in Palavyr.Data. This contains the context definitions

If we update an existing table or database, we change the schema. If we add a database or a table to a database we add a context.

    NOTE: We currenly rely on sqlite, which prevents some actions from being performed. For example we cannot:

    - Rename Columns
    - Change Types?
    - Change table names?

Once you've made your changes, you can apply local changes using visual studio (doing this via cli is too annoying,but it can be done).

In the package manager console, execute:

    add-migration migrationName -Context ContextName

The context Name is e.g. DashContext or AccountsContext. You need to do this for each context you've touched.

Next, update the database using:

    update-database -Context ContextName

### Taking your migration to production

Taking this production can be done by creating an sql script that can be run aganist the (BACKED UP) production database. BACK UP THE DB BEFORE YOU DO THIS IN CASE YOU EFF UP.

To create the script, navigate to the we can run the following command:

    dotnet ef migrations script --startup-project <PROJECT> --Context <CONTEXT> --output <PATH> [--idempotent]

    NOTE: --idempotent is not availabe for sqlite dbs!

So running the command FROM the Palavyr.Data directory, a real command might be:

    dotnet ef migrations script --startup-project ..\Palavyr.API\ --context AccountsContext --output .\MigrationScripts\AddSubscriptions.sql

Finally, FTP this script up to the server where the databases live and run:
(For hints on how to run, [look here](https://database.guide/5-ways-to-run-sql-script-from-file-sqlite/))

    #/powershell

    ## COPY THE DATABASE TO A BACKUP LOCATION BEFORE RUNNING THIS ##
    Rename the coppied Db to DBName_pre_MigrationScriptName.db

    Run:
    cat AddSubscriptions.sql | sqlite3 Accounts.db

Take the migration script and rename to follow the current pattern (#_Scriptname.sql)


This is currently how I know how to safely apply migrations.




######################### DOCKER ###################################


# Steps I took to dockerize
1. https://www.youtube.com/watch?v=f0lMGPB10bM&ab_channel=LesJackson
2. https://stackoverflow.com/questions/45270598/how-i-can-dockerize-my-web-api-on-windows
    docker pull microsoft/aspnetcore

3. https://www.red-gate.com/simple-talk/sysadmin/containerization/asp-net-core-with-gitops-dockerizing-an-api-on-aws-ec2/

#### What are those <none> images?
https://www.projectatomic.io/blog/2015/07/what-are-docker-none-none-images/


#### Aother asp net core app with docker tutorial
https://medium.com/@lucaslohrmann/create-a-asp-net-core-web-app-with-docker-5df9e2cc17c7

#### Good Guidance on iis (a reverse proxy) in front of asp.net core (runs on kestrel, but doesn't auto handle ssl)
https://weblog.west-wind.com/posts/2016/jun/06/publishing-and-running-aspnet-core-applications-with-iis#:~:text=It's%20not%20hosted%20inside%20of,this%20self%2Dhosted%20server%20instance.


#### Some good docs on dockerfile for windows:
https://docs.microsoft.com/en-us/virtualization/windowscontainers/manage-docker/manage-windows-dockerfile

#### THE HOLY GRAIL FOR WINDOW IIS DOCKERFILES!
https://github.com/MicrosoftDocs/Virtualization-Documentation/tree/master/windows-container-samples

### Docker hub for server core iis
https://hub.docker.com/_/microsoft-windows-servercore-iis

### How to make a sleep command:

    RUN ping 127.0.0.1 -n 6 > nul



### How to clear the docker cache
https://forums.docker.com/t/how-to-delete-cache/5753

    S) alias docker_clean_images='docker rmi $(docker images -a --filter=dangling=true -q)'

    B) alias docker_clean_ps='docker rm $(docker ps --filter=status=exited --filter=status=created -q)'

If you want to remove ALL of your cache, you first have to make sure all containers are stopped and removed, since you cannot remove an image in use by a container. So something similar

    docker kill $(docker ps -q)
    docker_clean_ps
    docker rmi $(docker images -a -q)`


### What are the steps then?

1. pull windows server core iss, sdk 3.1, asp.net core 3.1
2. Expose port 80 and 443
3. create a working directory call `/src` where we can copy the solution filse
4. create a directory called inetpub/wwwroot where the api build will go
5. build the soln
6. publish the soln't
7. push the image


### How to build your image

Ive successfully built an image that contains asp.net core 3.1. I used the .net sdk image to run the build
commands.

    docker build -t paulegradie-server .

This was actually incorrect I think. It should have been:

    docker build -t paulegradie/paulegradie-server .

#### How to retag the image and push

So I created my image, and I ran the build command, but then I couldn't push. I followed the above tutorials and first thought I was meant to run this:

    # WRONG COMMAND
     docker push docker.io/paulegradie/paulegradie-server

So after a bit of reading I came across:
https://stackoverflow.com/questions/41984399/denied-requested-access-to-the-resource-is-denied-docker

Basically when you create the image name, it needs the username attached if you're gonna push it to docker hub, so
you can basically rename it using the `docker tag` command:

    docker tag paulegradie-server paulegradie/paulegradie-server
    docker push paulegradie/paulegradie-server

Then you can make sure you're logged in:

    docker login

(Then provide your credentials)


#### How to remove a docker image

Docker provides a single command that will clean up any resources — images, containers, volumes, and networks — that are dangling (not associated with a container):

    docker system prune

To additionally remove any stopped containers and all unused images (not just dangling images), add the -a flag to the command:

    docker system prune -a

##### Removing Docker Images
Remove one or more specific images
Use the docker images command with the -a flag to locate the ID of the images you want to remove. This will show you every image, including intermediate image layers. When you’ve located the images you want to delete, you can pass their ID or tag to docker rmi:

List:

    docker images -a

Remove:
    docker rmi Image Image


#### That was ubuntu, how about IIS?

For a container that has an windows server core image with iis and the application installed inside it, we need a slightly different dockerfile.

At the beginning of our dockerfile, we can add:

    FROM microsoft/iis:10.0.14393.206
    SHELL ["powershell"]