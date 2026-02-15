# Expense Tracker API

REST API application for managing personal finances including expenses, transactions, budgets, recurring transactions, and user accounts.

---

##  Tech Stack

- .NET 8
- ASP.NET Core Web API
- PostgreSQL
- Npgsql (raw SQL)
- Docker & Docker Compose
- JWT Authentication
- Swagger (OpenAPI)

---

#  How to Run the Application (Docker – Recommended)

##  Prerequisites

- Docker
- Docker Compose

---

##  Start the Application

Navigate to the folder containing `docker-compose.yml` and run:

docker compose up --build

The application will be available at:

- API → http://localhost:8080  
- Swagger → http://localhost:8080/swagger  

---

#  Database Configuration

PostgreSQL runs inside a Docker container with the following configuration:

- Host: localhost
- Port: 5433
- Database: expense_tracker
- Username: sampleuser
- Password: samplepass

The connection string is automatically injected via environment variables.

---

#  Authentication & Authorization (JWT)

The application uses JWT (JSON Web Token) for secure authentication and authorization.

After successful login or registration, the API returns a JWT token.

All protected endpoints require the token in the HTTP header:

Authorization: Bearer {jwt_token}

---

#  How to Register

Endpoint:
POST /auth/register

Request Body:

{
  "name": "Test User",
  "email": "test@test.com",
  "password": "123456"
}

Response:

- UserId
- Email
- Role
- JWT Token

---

#  How to Login

Endpoint:
POST /auth/login

Request Body:

{
  "email": "jahja.hromadzic@gmail.com",
  "password": "JahjaJahja"
}

---

#  Roles

User  
- Can access only their own data (transactions, categories, budgets, etc.)

Admin  
- Extended system privileges

---

#  Features

- User registration
- User login
- JWT token generation
- Role-based authorization
- CRUD operations for:
  - Transactions
  - Categories
  - Payment Methods
  - Budgets
  - Recurring Transactions
- Soft delete (user deactivation)
- Secure password hashing using BCrypt

---

#  Application Architecture

The application follows a layered architecture:

Domain – Business entities and rules  
Application – Business logic and use cases  
Infrastructure – Database access (Npgsql)  
WebApi – HTTP endpoints and application entry point  

---

#  Swagger Documentation

Swagger UI is available at:

http://localhost:8080/swagger

Swagger allows:
- Viewing all endpoints
- Testing requests
- Inspecting request/response models

---

#  Important Notes

- Unique email constraint is enforced at database level.
- Passwords are securely hashed using BCrypt.
- JWT validation includes issuer, audience, and signing key.
- Database tables must exist before running the application.
