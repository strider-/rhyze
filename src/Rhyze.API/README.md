# Rhyze.API

Web API for uploading, streaming & updating music metadata.

-------

## Technology Stack
* C# 8.0
* ASP.NET Core 3.1 (WebAPI)
* [MediatR](https://github.com/jbogard/MediatR)
* [Google Identity Platform](https://developers.google.com/identity/) for IaaS.
* Project Dependencies
  * [Rhyze.Core](../Rhyze.Core/README.md)
  * [Rhyze.Data](../Rhyze.Data/README.md)
  * [Rhyze.Services](../Rhyze.Services/README.md)

## Getting Started

### Prerequisites
* The Docker containers specified in the [README.md](../../README.md) repository root should be running via `docker-compose`.

In this project's root folder, set the application secrets for JWT authentication:
```bash
$ dotnet user-secrets set "Authentication:Jwt:Issuer" "<your google oauth2 app project name>"
$ dotnet user-secrets set "Authentication:Jwt:Audience" "https://securetoken.google.com/<your google oauth2 app project name>"
```

If you're using Postman, import the request and environment collections found in the `requests` folder in the root of
this repository. You'll need to populate your own values for the following environment variables:
* `user.email`
* `user.password`
* `rhyze.auth.apiKey` (Firebase API auth key)

To authenticate with the API, make sure the aforementioned values are populated and correct, then run the `POST verifyPassword` request in the `Rhyze/Auth` folder. If successful, you should be able to run the `GET /me` request and see your user information. If/when the token expires and you get `401` back from the API, run `POST refreshToken` to update your credentials.