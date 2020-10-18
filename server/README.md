# Configuration Manager Server

This is the server that powers the Palavyr configuration manager. Its an asp.net core console app written in C# that is intended to run on IIS windows server 2019.

# Setup

Postgres install

Refresh the server

```bash
    curl --location --request PUT 'https://localhost:5001/api/setup' --header 'action: secretDevAccess' --header 'accountId: dashboardDev'
```

```powershell
    Invoke-WebRequest -URI https://localhost:5001/api/setup -Header  -Method Post
```


### Updating tools

##### update dotnet-ef

    dotnet tool update --global dotnet-ef --version 3.1.0

#### Getting EF CORE TO REROLL MIGRATIONS
https://stackoverflow.com/questions/54443131/force-dbup-to-rerun-new-scripts-during-development