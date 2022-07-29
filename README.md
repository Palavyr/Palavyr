# Palavyr


## Overview

Palavyr is a fully containerized web app that allows users to configure a chat bot. The configuration is unique in that it allows users to set pricing strategies which return values that depend on user input. The output of the application is customizable emails with optional attachments and response PDFs that contain pricing details given a user's input.

Anyone can use Palavyr!

# Getting Started

Since the application is dockerized, there are only a couple steps to get going.

1. Ensure you've got a docker daemon installed (You'll be running docker commands)
2. Copy the local.env.template to local.env and set your environment variables
   1. If you are using local stack to simulate AWS you'll have an easier time getting going. Otherwise see Paul about an API key
   2. Stripe is also dockerized. You can use a fake test key.
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