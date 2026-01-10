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

Podaci se čuvaju u Docker volumenu (`postgres-data`) i ostaju sačuvani nakon gašenja kontejnera.

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