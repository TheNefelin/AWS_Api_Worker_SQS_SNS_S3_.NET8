using System.Text.Json.Serialization;

namespace ClassLibrary_Infrastructure.Models;

public class AwsSqsDonationMessage
{
    [JsonPropertyName("Email")]
    public string Email { get; set; }

    [JsonPropertyName("Amount")]
    public decimal Amount { get; set; }
}
