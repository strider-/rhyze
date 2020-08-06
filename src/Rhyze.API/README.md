# Rhyze API

## Getting Started

Set the application secrets

```bash
dotnet user-secrets set "Authentication:Jwt:Issuer" "<your google oauth2 app project name>"
dotnet user-secrets set "Authentication:Jwt:Audience" "https://securetoken.google.com/<your google oauth2 app project name>"
```