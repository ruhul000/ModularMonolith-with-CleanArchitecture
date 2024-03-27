# ModularMonolith-with-CleanArchitecture
![Control Hub API](https://github.com/ruhul000/ModularMonolith-with-CleanArchitecture/assets/38735317/4691495d-adda-43bc-b31a-9de80e76193b)

ControlHub is a modular monolithic architecture project built with C#, .NET 8, MSSQL, Azure, and Git Actions.

### **TOOLS, TECHNOLOGIES AND DESIGN PATTERN USED** 
1. .Net 8
2. Rest API
3. Open API - Swagger
4. Entity Framework Core
5. Middleware
6. Error Handling
7. Auto Mapper
8. Factory Pattern
9. Repository Pattern
10. Result Pattern
11. API versioning
12. Fluent Validation
13. Dependency Injection (DI)
14. Structured Logging using Serilog
15. CORS (Cross-Origin Resource Sharing)
16. JWT Authentication and Authorization
17. Global Exception Handling Middleware 

### **NUGET PACKAGES** 
1. Newtonsoft.Json
2. Serilog.AspNetCore
3. Microsoft.Extensions.Logging
4. Swashbuckle.AspNetCore
5. Microsoft.EntityFrameworkCore
6. Microsoft.EntityFrameworkCore.SqlServer
7. Microsoft.AspNetCore.Mvc.Versioning
8. Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
9. AutoMapper.Extensions.Microsoft.DependencyInjection
10. Microsoft.AspNetCore.Authentication.JwtBearer
11. FluentValidation.DependencyInjectionExtensions

#### **PROJECT STRUCTURE**
The project is structured into the following main components:

- **ControlHub.API:** The main entry point of the application. Contains controllers, middleware, and configuration related to API endpoints.
- **ControlHub.Modules:** Contains modular components of the application organized by domain concerns.
	- **Common:** Common utilities and helper classes used across modules.
	- **UserAccess:** Module responsible for user management and authentication.
	- **UserAccess.Application:** Application layer containing services and DTOs.
	- **UserAccess.Domain:** Domain layer containing domain models and factories.
	- **UserAccess.Infrastructure:** Infrastructure layer containing repositories and database context.

#### **PROJECT TREE**
<pre>
src
 ├─ ControlHub.API
 │  ├─ Controllers
 │  │  ├─ UserAccessController.cs
 │  ├─ Middleware
 │  │  └─ GlobalExceptionHandlingMiddleware.cs
 │  ├─ appsettings.Development.json
 │  ├─ appsettings.json
 │  ├─ ConfigureSwaggerOptions.cs
 │  ├─ ControlHub.API.csproj
 │  ├─ ControlHub.API.csproj.user
 │  └─ ControlHub.API.http
 ├─ ControlHub.Scripts
 │  ├─ DBScript.sql
 │  ├─ Seed.sql
 ├─ ControlHub.Modules
 │  ├─ Common
 │  │  └─ Helper
 │  │     ├─ Extensions
 │  │     │  ├─ AuthExtensions.cs
 │  │     │  └─ CommonExtensions.cs
 │  │     ├─ Services
 │  │     │  ├─ EmailSender.cs
 │  │     │  └─ IEmailSender.cs
 │  │     ├─ Utilities
 │  │     │  ├─ Error.cs
 │  │     │  └─ Result.cs
 │  │     └─ DependencyInjection.cs
 │  └─ UserAccess
 │     ├─ UserAccess.Application
 │     │  ├─ Errors
 │     │  │  └─ UserAccessErrors.cs
 │     │  ├─ Services
 │     │  │  ├─ AuthenticationService.cs
 │     │  │  ├─ IAuthenticationService.cs
 │     │  │  ├─ IUserService.cs
 │     │  │  └─ UserService.cs
 │     │  └─ DependencyInjection.cs
 │     ├─ UserAccess.Domain
 │     │  ├─ Factories
 │     │  │  ├─ FactoryMapper
 │     │  │  │  ├─ IMappingFactory.cs
 │     │  │  │  ├─ MappingFactory.cs
 │     │  │  │  └─ UserProfile.cs
 │     │  │  ├─ IUserFactory.cs
 │     │  │  └─ UserFactory.cs
 │     │  ├─ Models
 │     │  │  ├─ AuthInformation.cs
 │     │  │  ├─ JWTSettings.cs
 │     │  │  ├─ User.cs
 │     │  │  ├─ UserLoginRequest.cs
 │     │  │  ├─ UserRequest.cs
 │     │  │  └─ UserResposne.cs
 │     │  └─ DependencyInjection.cs
 │     └─ UserAccess.Infrastructure
 │        ├─ Repositories
 │        │  ├─ IRefreshTokenRepository.cs
 │        │  ├─ IUnitOfWork.cs
 │        │  ├─ IUserRepository.cs
 │        │  ├─ RefreshTokenRepository.cs
 │        │  ├─ UnitOfWork.cs
 │        │  └─ UserRepository.cs
 │        ├─ DependencyInjection.cs
 │        ├─ UserAccess.Infrastructure.csproj
 │        └─ UserAccessDBContext.cs
 └─ ControlHub.API.sln
</pre>

#### **PREREQUISITES**
Before running the project, ensure you have the following installed:

- .NET 8 SDK
- SQL Server or Azure SQL Database
- Azure account for deployment (if applicable)

#### **GETTING STARTED**
1. Clone the repository:
    <pre>git clone https://github.com/ruhul000/ModularMonolith-with-CleanArchitecture.git</pre>
2. Set up your database connection string in **appsettings.json** or **appsettings.Development.json** files.
3. Navigate to the **SqlScript** directory and run **DataBaseScript.sql**, **Sproc.sql**, **Seed.sql**.
4. Navigate to the **ControlHub.API **directory and run the project.

#### DEPLOYMENT
To deploy the application to Azure, follow these steps:

1. To deploy the application to Azure, follow these steps:
2. Set up an Azure App Service and SQL Database.
3. Configure the connection string in Azure App Service settings.
4. Push changes to the main branch.
5. Set up continuous integration/continuous deployment (CI/CD) using Git Actions.

**CONTRIBUTING**

Contributions are welcome!

**LICENSE**

This project is licensed under the MIT License.
