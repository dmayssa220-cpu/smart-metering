namespace SmartMetering.Api.Models;

public enum AlertSeverity { Info, Warning, Critical }

public class Alert
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MeterId { get; set; }
    public Meter? Meter { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Acknowledged { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
