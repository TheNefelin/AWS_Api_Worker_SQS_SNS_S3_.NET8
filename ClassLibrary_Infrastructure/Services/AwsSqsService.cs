using Amazon.SQS;
using Amazon.SQS.Model;
using ClassLibrary_Infrastructure.Config;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Infrastructure.Services;

public class AwsSqsService : IAwsSqsService
{
    private readonly ILogger<AwsSqsService> _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly AwsSettings _awsSettings;

    public AwsSqsService(ILogger<AwsSqsService> logger, IAmazonSQS sqsClient, AwsSettings awsSettings)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _awsSettings = awsSettings;
    }

    public async Task<List<Message>> ReceiveMessagesAsync(int maxMessages = 10, int waitTimeSeconds = 20)
    {
        try
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = _awsSettings.SqsUrl,
                MaxNumberOfMessages = maxMessages,
                WaitTimeSeconds = waitTimeSeconds,
                MessageAttributeNames = new List<string> { "All" }
            };

            var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);
            _logger.LogInformation("[AwsSqsService] Mensajes recibidos de SQS: {MessageCount}", receiveMessageResponse.Messages.Count);

            return receiveMessageResponse.Messages;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AwsSqsService] Error recibiendo mensajes de SQS");
            throw;
        }
    }

    public async Task DeleteMessageAsync(string receiptHandle)
    {
        try
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = _awsSettings.SqsUrl,
                ReceiptHandle = receiptHandle
            };

            await _sqsClient.DeleteMessageAsync(deleteMessageRequest);
            _logger.LogDebug("[AwsSqsService] Mensaje eliminado de SQS exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AwsSqsService] Error eliminando mensaje de SQS");
            throw;
        }
    }
}
