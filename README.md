# Palavyr Configuration Manager


### IT IS VERY IMPORTANT THAT YOU DO NOT RUN THE SERVER USING SUDO - THIS WILL OVERRIDE ALL ENVIRONMENTAL VARIABLES 


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


    
## Database Migrations


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
