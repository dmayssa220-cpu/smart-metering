# Dossier `database/`

Le schéma de base de données est actuellement créé automatiquement au démarrage
du backend via `db.Database.EnsureCreated()` (voir `backend/SmartMetering.Api/Program.cs`),
et peuplé avec des données de démonstration par `Data/DbSeeder.cs`.

Ce dossier est réservé aux scripts SQL manuels (sauvegardes, jeux de données
supplémentaires, scripts de migration ponctuels) une fois que le projet
basculera sur de vraies migrations EF Core (`dotnet ef migrations add ...`),
comme décrit dans le README principal.
