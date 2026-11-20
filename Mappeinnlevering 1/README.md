# Bruktbørsen API

REST-API for en markedsplass for brukte ting, laget i ASP.NET Core Web API (.NET 10) med EF Core,
SQLITE og ASP.NET Core Identity.

## Kjøre prosjektet
1. Klon repoet og åpne "Mappeinnlevering 1.slnx" i Rider eller Visual Studio
2. Kjør prosjektet "Mappeinnlevering 1", eller bruk terminalen:

cd "Mappeinnlevering 1"
dotnet run
3. Databasen ("bruktborsen.db") opprettes, migreres og fylles med testdata automatisk ved oppstart
4. API-et kjører på "http://localhost:5221"
5. API-dokemntasjonen Scalar ligger på "http://localhost:5221/scalar"

## Testbrukere

| E-post         | Passord   | Rolle |
|----------------|-----------|--|
| admin@brukt.no | Admin123! | Admin|
| ola@brukt.no   | User123!  | User|
| kari@brukt.no  | User123!  | User|
| per@brukt.no   | User123!  | User|

Logg inn på "POST /api/auth/login" og bruk "accessToken" fra svaret som Bearer-token

## Kjøre testene
dotnet test


# Gruppemedlemmer
- Dat Le
- Lars Adrian Helvin