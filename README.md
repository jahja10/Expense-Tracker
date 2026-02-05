# Expense Tracker API

REST API application for managing expenses, budgets, transactions, and related entities.

## Technologies

- .NET 8
- ASP.NET Core Web API
- PostgreSQL
- Entity Framework Core
- Docker & Docker Compose
- Swagger (OpenAPI)

## Application Startup (Docker – recommended)

### Prerequisites
- Docker
- Docker Compose

### Start

In the folder containing `docker-compose.yml`, run:

```bash
docker compose up --build
```


The application will be available at:

- API: http://localhost:8080

- Swagger UI: http://localhost:8080/swagger

## Database

PostgreSQL runs inside a Docker container with the following parameters:

- Host: localhost

- Port: 5433

- Database: expense_tracker

- Username: sampleuser

- Password: samplepass

## Authentication and Authorization (JWT)

The application uses JWT (JSON Web Token) for secure user authentication and authorization.

## Features

- User login

- JWT token generation after successful authentication

- Endpoint protection using the [Authorize] attribute

- Role-based authorization (e.g. Admin, User)

- Each user has access only to their own data
(transactions, categories, budgets, etc.)

- Administrator has extended system privileges

## Using the JWT Token

After logging in, the API returns a JWT token which must be included in every protected request via the HTTP header:

Authorization: Bearer {jwt_token}

## Swagger Documentation

The API is documented using Swagger (OpenAPI).
Swagger UI allows inspection of all endpoints, request/response models, and API testing.

Swagger is available at:
http://localhost:8080/swagger

## Application Architecture

The application is organized into layers:

- Domain – business models and rules

- Application – business logic and services

- Infrastructure – database access and external services

- WebApi – HTTP endpoints and application entry point
