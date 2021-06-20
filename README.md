# 1. Identity

Identity Server 4 using MongoDB for Stores.

Live at [identity.mariomenjr.com](https://identity.mariomenjr.com/)

# 2. Samples

By Grant types.

## 2.1 client_credentials

### 2.1.1 Request

By some implementations.

#### 2.1.1.1 HTTP

```http
POST /connect/token HTTP/1.1
Host: identity.mariomenjr.com:443
Content-Type: application/x-www-form-urlencoded
Content-Length: 94

client_id=test.client&client_secret=test.secret&scope=test.scope&grant_type=client_credentials
```

#### 2.1.1.2 cURL

```bash
curl --location --request POST 'https://identity.mariomenjr.com:443/connect/token' \
--header 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'client_id=test.client' \
--data-urlencode 'client_secret=test.secret' \
--data-urlencode 'scope=test.scope' \
--data-urlencode 'grant_type=client_credentials'
```

#### 2.1.1.3 JavaScript - Fetch

```javascript
var myHeaders = new Headers();
myHeaders.append("Content-Type", "application/x-www-form-urlencoded");

var urlencoded = new URLSearchParams();
urlencoded.append("client_id", "test.client");
urlencoded.append("client_secret", "test.secret");
urlencoded.append("scope", "test.scope");
urlencoded.append("grant_type", "client_credentials");

var requestOptions = {
  method: "POST",
  headers: myHeaders,
  body: urlencoded,
  redirect: "follow",
};

fetch("https://identity.mariomenjr.com:443/connect/token", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
```

### 2.1.2 Response

```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjA5RjE4MUUzMzNCMzVBMTM0RTQ3MjE1ODZENjgyRjUwIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2MjQxNjExNDMsImV4cCI6MTYyNDE2NDc0MywiaXNzIjoiaHR0cDovL2lkZW50aXR5Lm1hcmlvbWVuanIuY29tIiwiY2xpZW50X2lkIjoidGVzdC5jbGllbnQiLCJqdGkiOiIyNTc5MEM0QUZGNzQ3NTZDNzM5RjQ4RjEzRkUyMDc2RSIsImlhdCI6MTYyNDE2MTE0Mywic2NvcGUiOlsidGVzdC5zY29wZSJdfQ.sm91IYPIn2O5c1BukDrhJPYMqmFl48f5CBaDdpUCrCdWt9oUKCF_w4etyfEbb7wfb94zzbxOQvdLCVQKshh7abFaA5AGgi9jBDkrpEBIxwlnNcjNAo6GG_W1h9lZ9BdxW3kXSUEvL8h1JQMbWRiaQmJcrJCH0m-Nv2NiRDSJ0rJyes73Aa2IVHJRwQ4WvwdNxRLE5Zg03w9X70_KARN7rdppkZiyEZCoCpR-58DVxlhs6uJuetFIE-sUCpb52xTM3u5-0uZm3VgsP41boc0BfAuSt71RnDhf9-flCoRezAXcBSCXymiaxiUTRETAQsDXSKAwWG33Zb3_m9joEHijVg",
  "expires_in": 3600,
  "token_type": "Bearer",
  "scope": "test.scope"
}
```

# 3. Stores

This implementation uses MongoDB to store Client & Resource data. As of now, the database has to be manually created.

## 3.1 Collection schemas

Feel free to use schemas below.

```json
// Collection name: Clients
{
  "_id": { "$oid": "60bc183aa50e552edc2e4ca6" },
  "name": "test.client",
  "secret": "test.secret",
  "apiScopeIds": [{ "$oid": "60ceb1e8a50e552edc2bc40a" }]
}
```

```json
// Collection name: ApiScopes
{
  "_id": { "$oid": "60ceb1e8a50e552edc2bc40a" },
  "name": "test.scope",
  "displayName": "Test Scope"
}
```

```json
// Collection name: ApiResources
{
  "_id": { "$oid": "60bc327fa50e552edc3296b7" },
  "name": "continuee_api",
  "apiScopeIds": [
    { "$oid": "60bc4fb5a50e552edc374527" },
    { "$oid": "60bc5ae9a50e552edc3a32fd" }
  ]
}
```

```json
// Collection name: IdentityResources

// Pending implementation
```

## 3.2 appsettings.json

Currently, the appsettings.json is being tracked by Git. Be careful when making changes to this file, you don't want your credentials to get into a public repository.

```json
// appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "MongoSettings": {
    // Replace values with your own, without <>
    "ConnectionString": "mongodb+srv://<username>:<password>@<server>/<database>?retryWrites=true&w=majority",

    // Replace values with your own, without <>
    "DatabaseName": "<database>"
  }
}
```

# 4. TODOs

- Externalize IdentityResources
- Implement secrets.json with Docker (Ref: https://blog.matjanowski.pl/2017/11/18/managing-options-and-secrets-in-net-core-and-docker/)

> Version 0.0.0.2