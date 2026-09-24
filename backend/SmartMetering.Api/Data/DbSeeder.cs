using SmartMetering.Api.Models;

namespace SmartMetering.Api.Data;

// Peuple la base avec un jeu de données de démonstration au premier démarrage.
// Remplacer/compléter par de vraies migrations EF Core (dotnet-ef) en environnement de production.
public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (!db.Users.Any())
        {
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
            var admin = new User
            {
                Email = "admin@smartmetering.local",
                FullName = "Administrateur",
                Role = UserRole.Admin
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");
            db.Users.Add(admin);
        }

        if (!db.Meters.Any())
        {
            var rng = new Random(42);
            var sites = new[] { "Site Nord", "Site Sud", "Zone Industrielle A", "Zone Industrielle B" };
            var meters = new List<Meter>();

            for (int i = 1; i <= 8; i++)
            {
                var meter = new Meter
                {
                    SerialNumber = $"SM-{1000 + i}",
                    Type = (MeterType)(i % 3),
                    Status = i == 3 ? MeterStatus.Faulty : MeterStatus.Active,
                    SiteName = sites[i % sites.Length],
                    Latitude = 36.8 + rng.NextDouble() * 0.2,
                    Longitude = 10.1 + rng.NextDouble() * 0.2
                };
                meters.Add(meter);
            }
            db.Meters.AddRange(meters);
            db.SaveChanges();

            // Historique de lecture de démonstration pour le premier compteur
            var first = meters.First();
            for (int h = 0; h < 24; h++)
            {
                db.Readings.Add(new Reading
                {
                    MeterId = first.Id,
                    Value = 40 + rng.NextDouble() * 25,
                    Timestamp = DateTime.UtcNow.AddHours(-24 + h)
                });
            }

            db.Alerts.Add(new Alert
            {
                MeterId = meters[2].Id,
                Severity = AlertSeverity.Critical,
                Message = "Compteur hors ligne depuis plus de 2 heures."
            });

            db.SaveChanges();
        }
    }
}
