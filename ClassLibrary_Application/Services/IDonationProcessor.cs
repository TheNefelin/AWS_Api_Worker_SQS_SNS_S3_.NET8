using Amazon.SQS.Model;

namespace ClassLibrary_Application.Services;

public interface IDonationProcessor
{
    Task ProcessDonationAsync(Message sqsMessage);
}
