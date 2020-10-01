# PDF Converter

## Installation and startup

To install node module dependencies, run the following command in this directory:
    
    npm install

To start the server, run:

    npm start  

This will start the pdf server on port 5600, unless the process.env.PORT variable is set and specifies something different.  


## Description
This is a tiny express server that runs alongside the configuration server on port 5600:

    http://localhost:5603


It has but a single route:

    /create-pdf


#### Payload Description
The endpoint expects a payload object this is defined as:

    {
        html: 'html string",
        savePath: "path where a pdf will be saved",
        identifier: "a unique identifier that will be applied to each page of the pdf"
    }

{ html: "html string", savePath: "path where a pdf will be saved", identifier: "a unique identifier that will be applied to each page of the pdf" }
## Send a curl request in powershell to test the server is active

#### 1. Define a body object
$Body = @{
    html = '<h1>WOW</h1>'
    path = 'C:\Temp\Test.pdf'
    id = '234-234-234'
}

#### 2. Invoke a web request
Invoke-WebRequest -URI http://localhost:5603/create-pdf -Body $Body -Method Post