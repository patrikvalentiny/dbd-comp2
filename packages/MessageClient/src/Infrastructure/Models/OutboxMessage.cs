namespace MessageClient.Infrastructure.Models;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Subscription { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ProcessedAtUtc { get; set; }
    public bool IsProcessed => ProcessedAtUtc != null;
}