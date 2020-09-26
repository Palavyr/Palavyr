# Palavyr Configuration Manager

## Setup

This repo is two part:

1. The Server backend (written in C#)
2. The frontend portal (written in typescript/react)
---

#### Starting the listening tentacle on the ubuntu server
sudo /opt/octopus/tentacle/Tentacle service --install --start --instance "palavyrUbuntuTentacle"


    
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
