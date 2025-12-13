namespace AccessControlSystem.Entities;

public class Visitor
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public bool IsCompleted => ExitTime.HasValue;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}