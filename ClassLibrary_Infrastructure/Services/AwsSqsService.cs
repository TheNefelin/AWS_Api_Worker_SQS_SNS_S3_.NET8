using Amazon.SQS;
using Amazon.SQS.Model;
using ClassLibrary_Infrastructure.Config;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Infrastructure.Services;

public class AwsSqsService
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
            var request = new ReceiveMessageRequest
            {
                QueueUrl = _awsSettings.SqsUrl,
                MaxNumberOfMessages = maxMessages,
                WaitTimeSeconds = waitTimeSeconds,
                MessageAttributeNames = new List<string> { "All" }
            };

            var response = await _sqsClient.ReceiveMessageAsync(request);
            _logger.LogInformation("[AwsSqsService] Mensajes recibidos de SQS: {MessageCount}", response.Messages.Count);

            return response.Messages;
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
            var deleteRequest = new DeleteMessageRequest
            {
                QueueUrl = _awsSettings.SqsUrl,
                ReceiptHandle = receiptHandle
            };

            await _sqsClient.DeleteMessageAsync(deleteRequest);
            _logger.LogDebug("[AwsSqsService] Mensaje eliminado de SQS exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AwsSqsService] Error eliminando mensaje de SQS");
            throw;
        }
    }
}
