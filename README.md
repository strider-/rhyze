# Rhyze - Streaming Music Service

## Getting Started

Running `docker-compose up` will start the MSSQL Server. After you've confirmed it's running, navigate to the `scripts` directory
and run `0-create-database.sh` (you may need to run `chmod +x 0-create-database.sh` before hand.)  This will create the application 
database in the docker container.

See individual project READMEs for further detail

## Project Layout

```
./
 requests           - Postman request / environment collections
 scripts            - Scripts to ease the pain of getting your env running
 src                - Source code for projects
    /Rhyze.API      - Web API project
    /Rhyze.Core     - Core foundation (interfaces, models, etc)
    /Rhyze.Data     - Data access layer
    /Rhyze.Services - Business logic
 vol                - Docker volume mount root
    /sql            - MSSQL volume
```