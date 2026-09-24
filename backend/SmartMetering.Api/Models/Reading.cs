namespace SmartMetering.Api.Models;

public class Reading
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MeterId { get; set; }
    public Meter? Meter { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
