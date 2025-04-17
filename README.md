# DEPLOY

dotnet new list

dotnet new globaljson --sdk-version 9 --output ./src

dotnet new web --name DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API -o ./src/DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API --language C# --no-https

dotnet new sln --name DEPLOY.OpenAPI.Scalar.Swagger.ReDoc --output ./src

dotnet sln .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.sln  add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Microsoft.AspNetCore.OpenApi

Do the following in the Startup.cs file:

````csharp
builder.Services.AddOpenApi();
````

````csharp
app.MapOpenApi();
````

Now you can open the URL:
````
https://localhost:5001/openapi/v1.json
````

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Asp.Versioning.Mvc.ApiExplorer


dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Scalar.AspNetCore

configuracao da api

...


Inicio da configuracao do banco de dados

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Microsoft.EntityFrameworkCore.InMemory

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore


Swagger

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Swashbuckle.AspNetCore

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Swashbuckle.AspNetCore.ReDoc

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Swashbuckle.AspNetCore.SwaggerUI


Scalar

dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Scalar.AspNetCore

---
Você pode gerar automaticamente o documento OpenAPI do seu projeto durante o build. Esse recurso é útil para integrar o OpenAPI em ferramentas de desenvolvimento ou pipelines de CI/CD.

Como funciona:

Adicione o pacote Microsoft.Extensions.ApiDescription.Server ao seu projeto.
Por padrão, o documento será gerado na pasta obj, mas você pode mudar o local. Para gerar o documento na raiz do projeto, adicione isso ao seu .csproj:
````xml
<PropertyGroup>
  <OpenApiDocumentsDirectory>./</OpenApiDocumentsDirectory>
</PropertyGroup>
````
dotnet add .\src\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.API.csproj package Microsoft.Extensions.ApiDescription.Server

---
New SLN

cd .\src\ && dotnet sln migrate && dotnet build .\DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.slnx