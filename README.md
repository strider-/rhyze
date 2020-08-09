# Rhyze - Streaming Music Service

[![Build Status](https://dev.azure.com/strideriidx/Rhyze/_apis/build/status/strider-.rhyze?branchName=master)](https://dev.azure.com/strideriidx/Rhyze/_build/latest?definitionId=2&branchName=master)

## Getting Started

Running `docker-compose up` will start the MSSQL Server and Azurite, the open source Azure storage emulator. 

After you've confirmed the containers are running, navigate to the `scripts` directory and run `0-create-database.sh` 
(you may need to run `chmod +x 0-create-database.sh` before hand.)  This will create the application database in the docker 
container.

See individual project READMEs for further detail

## Project Layout

```
./
 requests            - Postman request / environment collections
 scripts             - Scripts to ease the pain of getting your env running
 src                 - Source code for projects
    /Rhyze.API       - Web API project
    /Rhyze.Core      - Core foundation (interfaces, models, etc)
    /Rhyze.Functions - Post-upload processing functions
    /Rhyze.Data      - Data access layer
    /Rhyze.Services  - Business logic
    /Rhyze.Tests     - Unit and integration tests
 vol                 - Docker volume mount root
    /sql             - MSSQL volume
    /azurite         - Storage emulator volume
```
