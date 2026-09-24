using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMetering.Api.Data;

namespace SmartMetering.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AlertsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool onlyUnacknowledged = false)
    {
        var query = _db.Alerts.Include(a => a.Meter).AsQueryable();
        if (onlyUnacknowledged) query = query.Where(a => !a.Acknowledged);
        return Ok(await query.OrderByDescending(a => a.CreatedAt).ToListAsync());
    }

    [HttpPost("{id:guid}/acknowledge")]
    public async Task<IActionResult> Acknowledge(Guid id)
    {
        var alert = await _db.Alerts.FindAsync(id);
        if (alert is null) return NotFound();
        alert.Acknowledged = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
