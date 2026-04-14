namespace MzansiBuilds.Models;

public class CollaborationRequest
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string FromUser { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

}