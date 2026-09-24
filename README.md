# Smart Metering Platform — Squelette de projet

Squelette d'application de supervision de compteurs intelligents, 100% conteneurisé
et basé uniquement sur des outils **gratuits / open-source**.

## Stack

| Couche | Techno |
|---|---|
| Frontend | Angular 18 (standalone), design custom responsive |
| Backend | .NET 8 Web API + Entity Framework Core |
| Base de données | PostgreSQL 16 |
| IA | Micro-service Python/FastAPI (détection d'anomalies + prédiction) |
| Orchestration | Docker Compose |

## Démarrage

Prérequis : Docker + Docker Compose installés.

```bash
docker compose up --build
```

Services exposés :
- Frontend : http://localhost:4200
- Backend (Swagger) : http://localhost:8080/swagger
- Service IA : http://localhost:8000/docs
- PostgreSQL : localhost:5432 (user: `sm_user` / password: `sm_password`)

Au premier démarrage, le backend crée automatiquement le schéma
(`db.Database.EnsureCreated()`) et insère des **données de démonstration**
(8 compteurs, un historique de lecture, une alerte, un utilisateur admin).

### Compte de démonstration

| Email | Mot de passe |
|---|---|
| `admin@smartmetering.local` | `Admin123!` |

Toutes les routes `/api/meters`, `/api/readings`, `/api/alerts` sont protégées
par JWT (`[Authorize]`) ; connectez-vous via `/api/auth/login` ou l'écran de
connexion Angular pour obtenir un token.

⚠️ **Avant tout déploiement réel** : changer la clé `Jwt:Key` dans
`appsettings.json` (ou via une variable d'environnement) et ne jamais la
committer telle quelle.

## Passer à de vraies migrations EF Core (recommandé pour la suite)

`EnsureCreated()` est pratique pour démarrer rapidement mais ne gère pas les
évolutions de schéma. Pour passer à des migrations versionnées (nécessite le
SDK .NET installé en local, hors conteneur) :

```bash
cd backend/SmartMetering.Api
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
```

Puis, dans `Program.cs`, remplacer `db.Database.EnsureCreated();` par
`db.Database.Migrate();`. Le dossier `Migrations/` généré doit être commité.

## Structure du projet

```
smart-metering-app/
├── docker-compose.yml
├── .gitignore
├── backend/                    # API .NET 8
│   ├── Dockerfile
│   ├── .dockerignore
│   └── SmartMetering.Api/
│       ├── Controllers/        # Auth, Meters, Readings, Alerts
│       ├── Models/             # Meter, Reading, Alert, User (+ Dtos/)
│       ├── Data/                # AppDbContext (EF Core) + DbSeeder
│       └── Services/            # JwtTokenService, client HTTP vers le service IA
├── ai-service/                  # Micro-service Python FastAPI
│   ├── Dockerfile
│   ├── .dockerignore
│   └── main.py                  # /anomaly, /predict
├── frontend/                    # Application Angular
│   ├── Dockerfile               # build Node -> nginx
│   ├── .dockerignore
│   ├── nginx.conf
│   └── src/app/
│       ├── core/layout/         # Shell (Sidebar + Topbar), responsive
│       ├── core/services/       # ApiService, AuthService
│       ├── core/guards/         # authGuard
│       ├── core/interceptors/   # authInterceptor (JWT + gestion 401)
│       └── features/            # login, dashboard, meters, alerts
└── database/                     # README (schéma géré par EF Core)
```

## Prochaines étapes suggérées

1. Passer d'`EnsureCreated()` à de vraies migrations EF Core versionnées (voir ci-dessus).
2. Ajouter la gestion des rôles côté UI (masquer certaines actions aux Techniciens, par exemple) et un écran d'inscription/admin des utilisateurs.
3. Remplacer la prédiction naïve du service IA par un modèle ML.NET entraîné ou un modèle Ollama local.
4. Ajouter un pipeline CI (GitHub Actions, gratuit) pour builder et pousser les images Docker.
5. Pour un déploiement cloud gratuit : Azure for Students, Oracle Cloud Always Free, ou Render/Railway (offres gratuites).
