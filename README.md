# Palavyr


## Overview

Palavyr is a fully containerized web app that allows users to configure a chat bot.  The configuration is unique in that it allows users to set pricing strategies which return values that depend on user input. The output of the application is customizable emails with optional attachments and response PDFs that contain pricing details given a user's input.

There are two ways to access the chatbot:
 - Throught the widget app (a full page react app)

In this case, users will load the widget through an IFrame (see palavyr-chat-widget) project. This app uses a backend that provides the entire conversation on the initial request and traverses its tree structure depending on the responses provided by the user.

 - (WIP) directly from the API

In this case, users are free to present widget data however they'd like. The API exposes as backend the provides a single node at a time, depending on what is requested. Users will create an initial request for a convo, and then each subsequent request returns the next node.

Palavyr is intended to be used by any small business that wishes to automate some of their custer engagement.


# Getting Started

Since the application is dockerized, there are only a couple steps to get going.

### 1. Set environment variables on your system

All you need to set is the `ECR_REGISTRY` variable in your system

### 2. Set up the .env.local file

Make a copy of `.env.local.template` and provide the values.



1. Ensure you've got a docker daemon installed (You'll be running docker commands)
2. Copy the local.env.template to local.env and set your environment variables
3. Run `docker compose up`
4. Run the tests to make sure everything works.
5. Get crackin'!






## Dev Tools

#### Github Actions

To test github actions locally:
 - https://github.com/nektos/act

.e.g act pull_request -W ./.github/workflows/server_PR_checks.yml -j build-frontend
This will trigger a pull_request, executing the given yml, and then specifically execute the build-frontend job. Too easy.


## Project organization

Palavyr is a monorepo that includes 2 subprojects:

1. server
1. ui - The ui project contains two react apps
   1. Widget
   2. Configuration UI


## Application architecture

The architecture of Palavyr is as follows:

![Palavyr](./assets/architecture.PNG)

## Legal

By joining the Palavyr Org and cloning this repository, you agree that:

 1. You will not distribute this software under any circumstances without the express permission of Paul Gradie.
 2. If you contribute to the code base, your efforts will be duely recognized if this application every goes on to make any moolah. Cause thats the point yo. :D

## Project Composition breakdown

This analysis uses github.com/AlDanial/cloc

```
-------------------------------------------------------------------------------
Language                     files    blank lines       comments  lines of code
-------------------------------------------------------------------------------
C#                             850          17993            816          62047
TypeScript                     511           3868           1108          33137
JSON                             9              4              0          12529
SVG                             62             16             58           5735
JavaScript                      20             82             48            836
PowerShell                       9            241            127            745
HTML                             8             19              0            458
Sass                            14             88              6            452
Markdown                         4            300              0            386
MSBuild script                   6             20              0            267
XML                              4              4             16             76
Bourne Shell                     1             19             17             32
CSS                              1              0              7             21
-------------------------------------------------------------------------------
SUM:                          1671          32584           2203         116721
-------------------------------------------------------------------------------
```

#### Additional Note

Terraformer import all [nicely](https://www.youtube.com/watch?v=GpjCF4yZU9A&ab_channel=NedintheCloud)
`terraformer import aws --resources="*" --compact --path-pattern "{output}/{provider}" --output hcl`