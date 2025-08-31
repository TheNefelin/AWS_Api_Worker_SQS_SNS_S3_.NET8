using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_Infrastructure.Models;

public class AwsSnsDonation
{
    [EmailAddress]
    public required string Email { get; set; }
    public required int Amount { get; set; }
}
