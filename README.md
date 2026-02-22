JSON-Backed Authentication Demo Application


Full Technical Documentation (Mono Repository)

1. Executive Summary
This project is a full-stack authentication demo built using ASP.NET Core (.NET 8) and React (Vite
+ TypeScript). The application demonstrates secure authentication without a database by using a
JSON-based user store, BCrypt password hashing, JWT token generation, and role-based
authorization. The repository is structured as a mono-repo containing both backend and frontend
systems.

2. Repository Structure
backend/
 AuthDemo.Api/
 Controllers/
 Services/
 Models/
 App_Data/users.json
 Program.cs
frontend/
 auth-demo-ui/
 src/
 api/
 auth/
 pages/
 styles/
 .env
The backend exposes RESTful authentication endpoints. The frontend consumes the API and
enforces role-based route protection. Both applications run independently during development.

3. Technology Stack
Backend: - .NET 8 - ASP.NET Core Web API - JWT Bearer Authentication - BCrypt.Net-Next -
Swagger (OpenAPI)
Frontend: - React 18 - TypeScript - Vite - Axios - React Router DOM

5. Backend Setup
cd backend/AuthDemo.Api
dotnet restore
dotnet run
API URL:
https://localhost:7238
Swagger UI:
https://localhost:7238/swagger

6. Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
 c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
 {
 Name = "Authorization",
 Type = SecuritySchemeType.Http,
 Scheme = "bearer",
 BearerFormat = "JWT",
 In = ParameterLocation.Header
 });
 c.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
 {
 new OpenApiSecurityScheme
 {
 Reference = new OpenApiReference
 {
 Type = ReferenceType.SecurityScheme,
 Id = "Bearer"
 }
 },
 new string[] {}
 }
 });
});

7. JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
 options.MapInboundClaims = false;
 options.TokenValidationParameters = new TokenValidationParameters
 {
 ValidateIssuer = true,
 ValidateAudience = true,
 ValidateIssuerSigningKey = true,
 ValidateLifetime = true,
 ClockSkew = TimeSpan.FromSeconds(30)
 };
 });

8. CORS Configuration
builder.Services.AddCors(opt =>
{
 opt.AddPolicy("DevCors", p =>
 p.WithOrigins("http://localhost:5173")
 .AllowAnyHeader()
 .AllowAnyMethod());
});

9. Authentication Flow
1. User submits credentials.
2. 2. Backend validates password using BCrypt.
3. JWT issued
containing 'sub' and 'role'.
4. Token stored in localStorage.
5. /auth/me validates session.
6.
Role-based routing enforced.
7. Logout clears token and redirects.

9. JSON User Store
backend/AuthDemo.Api/App_Data/users.json
{
 "users": [
 { "username": "admin", "passwordHash": "$2a$11$...", "role": "admin" },
 { "username": "user1", "passwordHash": "$2a$11$...", "role": "user" }
 ]
}

10. Git Mono-Repo Workflow
git init
git add .
git commit -m "Initial mono-repo setup"
git branch -M main
git remote add origin <repo-url>
git push -u origin main

11. Acceptance Criteria (Verified)
- Admin redirected to /admin - User redirected to /user - Role protection enforced - JWT required for
protected endpoints - Logout clears session - Session persists on refresh - JSON-based
authentication (no database)
