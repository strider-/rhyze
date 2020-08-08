# Rhyze.Functions

Post-upload processing Azure Functions to retrieve and persist track metadata

-------

## Technology Stack
* C# 8.0
* Azure Functions v3
* Project Dependencies
  * [Rhyze.Core](../Rhyze.Core/README.md)
  * [Rhyze.Data](../Rhyze.Data/README.md)
  * [Rhyze.Services](../Rhyze.Services/README.md)

## Getting Started

### Prerequisites
* The Docker containers specified in the [README.md](../../README.md) repository root should be running via `docker-compose`.

**NOTE: Visual Studio will attempt to launch the Windows based Azure Storage Emulator when the Functions project starts. I've yet to find a way
to disable this, but canceling the launch & starting the project again seems to work for a short while before VS tries to start it again.**

Create a `local.settings.json` file in the project root and populate it accordingly:
```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    }
}
```

Should you need to run both the API project and the Functions project for development / debugging, follow these steps:
* Right click the solution in Visual Studio
* Click Properties at the bottom of the menu
* Under the Common Properties section, click on Startup Project
* Click the Multiple Projects radio button and set the API & Functions project to Start.
* Click OK

When you start debugging the project, both the API and Functions project should open their own console windows.