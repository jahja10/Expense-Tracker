# Expense Tracker API

REST API aplikacija za upravljanje troškovima, budžetima, transakcijama i povezanim entitetima.

## Tehnologije

- .NET 8
- ASP.NET Core Web API
- PostgreSQL
- Entity Framework Core
- Docker & Docker Compose
- Swagger (OpenAPI)

## Pokretanje aplikacije (Docker – preporučeno)

### Preduvjeti
- Docker
- Docker Compose

### Start

U folderu gdje se nalazi `docker-compose.yml` pokrenuti:

```bash
docker compose up --build
```

Aplikacija će biti dostupna na:
- API: http://localhost:8080
- Swagger UI: http://localhost:8080/swagger

## Baza podataka

PostgreSQL se pokreće u Docker kontejneru sa sljedećim parametrima:

- Host: localhost
- Port: 5433
- Database: expense_tracker
- Username: sampleuser
- Password: samplepass

## Autentifikacija i autorizacija (JWT)

Aplikacija koristi JWT (JSON Web Token) za sigurnu autentifikaciju i autorizaciju korisnika.

Funkcionalnosti

- Prijava korisnika (login)

- Generisanje JWT tokena nakon uspješne autentifikacije

- Zaštita endpointa pomoću [Authorize] atributa

- Role-based autorizacija (npr. Admin, User)

- Svaki korisnik ima pristup isključivo vlastitim podacima (transakcije, kategorije, budžeti, itd.)

- Administrator ima proširene privilegije nad sistemom

## Korištenje JWT tokena

Nakon prijave, API vraća JWT token koji je potrebno slati u svakom zaštićenom zahtjevu kroz HTTP header:

Authorization: Bearer {jwt_token}

## Swagger dokumentacija

API je dokumentovan pomoću Swagger (OpenAPI).
Swagger UI omogućava pregled svih endpointa, request/response modela i testiranje API-ja.

Swagger je dostupan na:
http://localhost:8080/swagger

## Arhitektura aplikacije

Aplikacija je organizovana po slojevima:

- Domain – poslovni modeli i pravila
- Application – poslovna logika i servisi
- Infrastructure – pristup bazi i vanjskim servisima
- WebApi – HTTP endpointi i ulazna tačka aplikacije
