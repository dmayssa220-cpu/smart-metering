using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMetering.Api.Data;
using SmartMetering.Api.Models;

namespace SmartMetering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MetersController : ControllerBase
{
    private readonly AppDbContext _db;
    public MetersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Meter>>> GetAll()
        => await _db.Meters.AsNoTracking().ToListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Meter>> GetById(Guid id)
    {
        var meter = await _db.Meters.Include(m => m.Readings)
                                     .Include(m => m.Alerts)
                                     .FirstOrDefaultAsync(m => m.Id == id);
        return meter is null ? NotFound() : Ok(meter);
    }

    [HttpPost]
    public async Task<ActionResult<Meter>> Create(Meter meter)
    {
        _db.Meters.Add(meter);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = meter.Id }, meter);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Meter input)
    {
        var meter = await _db.Meters.FindAsync(id);
        if (meter is null) return NotFound();

        meter.SerialNumber = input.SerialNumber;
        meter.Type = input.Type;
        meter.Status = input.Status;
        meter.SiteName = input.SiteName;
        meter.Latitude = input.Latitude;
        meter.Longitude = input.Longitude;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var meter = await _db.Meters.FindAsync(id);
        if (meter is null) return NotFound();

        _db.Meters.Remove(meter);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
