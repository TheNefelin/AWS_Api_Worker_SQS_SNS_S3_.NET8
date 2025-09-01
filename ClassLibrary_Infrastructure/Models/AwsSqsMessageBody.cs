using Amazon.SQS.Model;
using System.Text.Json.Serialization;

namespace ClassLibrary_Infrastructure.Models;

public class AwsSqsMessageBody
{
    [JsonPropertyName("Type")]
    public string Type { get; set; }

    [JsonPropertyName("Subject")]
    public string Subject { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; }

    [JsonPropertyName("MessageAttributes")]
    public Dictionary<string, MessageAttributeValue> MessageAttributes { get; set; }
}
