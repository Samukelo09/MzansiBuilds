# **MzansiBuilds API - Build in Public Platform**
**MzansiBuilds** is a specialized full-stack API built for the South African developer community. It allows developers to "Build in Public" by sharing their project progress, requesting collaboration, and celebrating milestones.

## **Features**
- **JWT Authentication:** Secure user registration and login system.
- **Project Feed:** Create and browse "Build in Public" projects with real-time status updates.
- **Collaboration System:** A "Raise your hand" feature for developers to request to join projects.
- **JCloud Persistence:** Fully integrated with Azure SQL Database for enterprise-grade data management.
- **Interactive Documentation:** Complete API exploration via Swagger UI with integrated Bearer Token authorization.

---
## **Technical Stack**
- **Framework:** .NET 8 Web API
- **Database:** Azure SQL (Serverless)
- **ORM:** Entity Framework Core (Code-First Migrations)
- **Security:** JSON Web Tokens (JWT)
- **API Documentation::** Swashbuckle / Swagger OAS3

---
## **System Architecture**
The API follows a clean, controller-based architecture with dependency injection for database contexts and configuration management.

---
## **Getting Started**
**1. Prerequisites** 
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or VS Code

**2. Configuration** 
Update the `appsettings.json` file with your Azure SQL connection string and JWT secrets:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:your-server.database.windows.net...;Database=MzansiBuildsDB;..."
},
"Jwt": {
  "Key": "Your_Super_Secret_Key_Here",
  "Issuer": "MzansiBuilds",
  "Audience": "MzansiBuildsUsers"
}
```

**3. Database Migration** 
To sync the schema with your local or cloud database, run:
```
dotnet ef database update
```

---
## **How to Test (Swagger)**
1. Run the application (`F5`).
2. Navigate to `/swagger/index.html`.
3. Register a new user via `/api/Auth/register`.
4. Login via `/api/Auth/login` to receive your JWT token.
5. Click the Authorize button at the top and enter the token.
6. Start creating and managing projects!

---
## **Author**
**Samukelo Mkhize**
- BSc Honours in Computer Science (UKZN)
- Software Developer & AI Engineer Candidate
