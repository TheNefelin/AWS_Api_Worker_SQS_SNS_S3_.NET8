namespace ClassLibrary_Infrastructure.Models;

public class AwsSnsMessage
{
    public required string Subject { get; set; }
    public required string Message { get; set; }
}
