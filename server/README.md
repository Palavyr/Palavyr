# Configuration Manager Server

This is the server that powers the Palavyr configuration manager. Its an asp.net core console app written in C# that is intended to run on IIS windows server 2019.

# Setup

Post gers insteall

Refresh the server

```bash
    curl --location --request PUT 'https://localhost:5001/api/setup' --header 'action: secretDevAccess' --header 'accountId: dashboardDev'
```

```powershell
    Invoke-WebRequest -URI https://localhost:5001/api/setup -Header  -Method Post
```







