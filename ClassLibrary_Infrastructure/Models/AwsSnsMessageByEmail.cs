using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_Infrastructure.Models;

public class AwsSnsMessageByEmail
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Subject { get; set; }
    public required string Message { get; set; }
}
