# PDF Converter

## Installation and startup

To install node module dependencies, run the following command in this directory:

    npm install

To start the server, run:

    npm start

This will start the pdf server on port 5600, unless the process.env.PORT variable is set and specifies something different.


## Description
This is a tiny express server that runs alongside the configuration server on port 5603:

    http://localhost:5603


It has but a single route:

    /create-pdf

This route takes a payload that contains a file path, identifier, and html string, and converts & saves it to pdf on disk. If the path provided is valid, the converter will automatically create the necessary path on disk (including directories and subdirectories).

##### Valid Paths

Valid paths must be valid for the OS and include a filename WITH extension. e.g.

    C:\Temp\fakename.pdf

    /home/user/filename.pdf


#### Payload Description
The endpoint expects a payload object this is defined as:

    {
        html: 'html string",
        savePath: "path where a pdf will be saved",
        identifier: "a unique identifier that will be applied to each page of the pdf"
    }

## Send a curl request in powershell to test the server is active

1. Define a body object

```
$Body - ${
    Bucket = "dev-palavyr-previews"
    Key = "test-file"
    Html = "<h1>WOW</h1>"
    Id = "abc234"
    Region = "us-east-1"
    AccessKey = "access-key"
    SecretKey = "secret-key"
    Paper = ""
}
```

1. Invoke a web request

```
    Invoke-WebRequest -URI http://localhost:5603/create-pdf -Body $Body -Method Post
```