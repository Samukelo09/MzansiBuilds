namespace MzansiBuilds.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Stage { get; set; } = "Concept"; // e.g., MVP, Beta, Live
    public string SupportRequired { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string OwnerUsername { get; set; } = string.Empty;
}