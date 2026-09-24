namespace SmartMetering.Api.Models;

public enum MeterType { Electricity, Water, Gas }
public enum MeterStatus { Active, Inactive, Faulty }

public class Meter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SerialNumber { get; set; } = string.Empty;
    public MeterType Type { get; set; }
    public MeterStatus Status { get; set; } = MeterStatus.Active;
    public string SiteName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;

    public ICollection<Reading> Readings { get; set; } = new List<Reading>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
