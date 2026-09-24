using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMetering.Api.Data;
using SmartMetering.Api.Models;
using SmartMetering.Api.Services;

namespace SmartMetering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReadingsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly AiServiceClient _ai;

    public ReadingsController(AppDbContext db, AiServiceClient ai)
    {
        _db = db;
        _ai = ai;
    }

    [HttpGet("meter/{meterId:guid}")]
    public async Task<ActionResult<IEnumerable<Reading>>> GetForMeter(Guid meterId)
        => await _db.Readings.Where(r => r.MeterId == meterId)
                              .OrderBy(r => r.Timestamp)
                              .ToListAsync();

    // Ingestion d'une nouvelle mesure + vérification d'anomalie via le service IA.
    [HttpPost]
    public async Task<IActionResult> Ingest(Reading reading)
    {
        var meter = await _db.Meters.FindAsync(reading.MeterId);
        if (meter is null) return NotFound("Compteur introuvable.");

        _db.Readings.Add(reading);
        await _db.SaveChangesAsync();

        var history = await _db.Readings
            .Where(r => r.MeterId == reading.MeterId)
            .OrderByDescending(r => r.Timestamp)
            .Take(30)
            .Select(r => r.Value)
            .ToListAsync();

        var anomaly = await _ai.DetectAnomalyAsync(history, reading.Value);
        if (anomaly is { IsAnomaly: true })
        {
            _db.Alerts.Add(new Alert
            {
                MeterId = reading.MeterId,
                Severity = AlertSeverity.Warning,
                Message = $"Valeur anormale détectée ({reading.Value:F2}), score={anomaly.Score:F2}"
            });
            await _db.SaveChangesAsync();
        }

        return Ok(new { reading, anomaly });
    }
}
