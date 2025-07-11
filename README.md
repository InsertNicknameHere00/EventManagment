# Event Management System

A full-stack web application for managing events built with ASP.NET Core 8.0 Razor Pages and Web API.

## 🚀 Features

- User authentication with JWT tokens
- Event creation and management
- Event discovery and filtering
- Responsive design with Bootstrap 5
- Real-time validation
- Secure password handling
- Role-based access control

## 🔧 Prerequisites

- .NET 8.0 SDK
- SQL Server 2019 or later (or SQL Server LocalDB)
- Visual Studio 2022

## ⚙️ Installation

### Setup

1. Clone the repository: git clone https://github.com/InsertNicknameHere00/EventManagment cd EventManagement

2. Update the connection string in `EventManagmentBackend/appsettings.json`:

3. Update the API base URL in `Program.cs` if needed: builder.Services.AddHttpClient("BackendAPI", client => { client.BaseAddress = new Uri("https://localhost:7286"); });

4. Open Package Manager Console and run migrations: cd EventManagmentBackend dotnet ef database update

5. Select "Configure Startup projects" on the solution and set it to run multiple projects:
   - EventManagmentBackend
   - EventManagmentFrontend

The API (Backend) will be available at `https://localhost:7286`

## 🔐 Authentication

The system uses JWT tokens for authentication:
1. Register a new account at `/Register`
2. Login at `/Login` to receive a JWT token
3. Token is automatically stored in localStorage
4. Protected routes require a valid token

## 🎯 API Endpoints

 Authentication
- `POST /api/Auth/Register` - Register new user
- `POST /api/Auth/Login` - Login user

 Events
- `GET /api/Events` - Get all events
- `GET /api/Events/{id}` - Get event by ID
- `POST /api/Events` - Create new event (requires auth)
- `PUT /api/Events/{id}` - Update event (requires auth)
- `DELETE /api/Events/{id}` - Delete event (requires auth)

## 🔒 Security Features

- Password hashing using BCrypt
- JWT token authentication
- Cross-Origin Resource Sharing (CORS) configuration
- Form validation on both client and server
- SQL injection protection via Entity Framework
- XSS protection
- HTTPS enforcement

## 🚦 Development Environment

For local development:
1. Set both projects as startup projects in Visual Studio
2. Configure CORS settings if needed
3. Use SQL Server LocalDB for development

## 🌐 Deployment

1. Update connection strings for production
2. Configure production JWT settings
3. Build both projects

## 📚 Dependencies

Backend:
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.17)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.6)
- BCrypt.Net-Next (4.0.3)
- Swashbuckle.AspNetCore (6.4.0)

Frontend:
- Bootstrap (5.3.0)
- Font Awesome (6.4.0)
- jQuery Validation (1.19.5)

## 🤝 Contributing (Optional)

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request (Optional, if you really want to :) )
