# RealtyHub API — README

Backend en **ASP.NET Core 9 + MongoDB**. Incluye seed de datos con `--seed`.

## Requisitos

- .NET SDK 9
- MongoDB 7 (local o en Docker)
- (Opcional) Docker y Docker Compose

## Variables de entorno (API)

- `AllowedCorsOrigins__0` — ej. `localhosth:3000`
- `ConnectionStrings__Mongo` — ej. `mongodb://localhost:27017` o `mongodb://mongo:27017`
- `Mongo__Database` — ej. `realtyHub`

## Ejecutar local

```bash
# restaurar y compilar
dotnet restore
dotnet build


# Crear índices e insertar datos de ejemplo
dotnet run --project src/Api/Api.csproj -- --seed

# levantar la API
dotnet run --project src/Api/Api.csproj
# => http://localhost:5008
```
