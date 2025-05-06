namespace NotificationService.Domain.Events;

public class ChannelCreatedEvent
{
    public Guid ChannelId { get; set; }
    public required string Name { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime Timestamp { get; set; }
}