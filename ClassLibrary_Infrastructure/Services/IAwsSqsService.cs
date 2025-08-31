using Amazon.SQS.Model;

namespace ClassLibrary_Infrastructure.Services;

public interface IAwsSqsService
{
    Task<List<Message>> ReceiveMessagesAsync(int maxMessages = 10, int waitTimeSeconds = 20);
    Task DeleteMessageAsync(string receiptHandle);
}
