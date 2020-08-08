# Rhyze.Services

Business logic utilizing models and implementing contracts defined in the Core project.

-------

## Technology Stack
* C# 8.0
* [TagLib#](https://github.com/mono/taglib-sharp/)
* [Azure.Storage.Blobs](https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/storage/Azure.Storage.Blobs)
* Project Dependencies
  * [Rhyze.Core](../Rhyze.Core/README.md)

## Project Documentation

* `AzureBlobStore : IBlobStore` - Blob management using Azure storage.
* `UploadService : IUploadService` - Business logic for handing audio/image uploads from the API.