# ControlHubAPI
![ControlHub API](https://github.com/ruhul000/ControlHub.API/assets/38735317/76d26efe-d5c5-44e7-be43-8bd18f3dcf36)

ControlHub is a modular monolithic architecture project built with C#, .NET 8, MSSQL, Azure, and Git Actions.

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
 │     └─ UserAccess.Infrustracture
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
    <pre>git clone https://github.com/yourusername/ControlHub.git</pre>
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
