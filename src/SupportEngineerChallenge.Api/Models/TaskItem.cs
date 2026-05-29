namespace SupportEngineerChallenge.Api.Models;

public class TaskItem
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public required string Title { get; set; }
    public required string Status { get; set; } // "open" | "done"
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
