namespace NotificationService.Domain.Events;

public class FileUploadedEvent
{
    public Guid FileId { get; set; }
    public Guid ChannelId { get; set; }
    public Guid UploadedBy { get; set; }
    public string FileName { get; set; }
    public DateTime Timestamp { get; set; }
}