using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_Infrastructure.Models;

public class AwsSnsEmail
{
    [EmailAddress]
    public required string Email { get; set; }
}
