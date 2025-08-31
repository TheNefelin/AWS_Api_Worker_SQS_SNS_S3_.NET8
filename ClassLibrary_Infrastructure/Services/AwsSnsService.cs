using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using ClassLibrary_Infrastructure.Config;
using ClassLibrary_Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ClassLibrary_Infrastructure.Services;

public class AwsSnsService : IAwsSnsService
{
    private readonly ILogger<AwsSnsService> _logger;
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly AwsSettings _awsSettings;

    public AwsSnsService(ILogger<AwsSnsService> logger, IAmazonSimpleNotificationService snsClient, AwsSettings awsSettings)
    {
        _logger = logger;
        _snsClient = snsClient;
        _awsSettings = awsSettings;
    }

    public async Task<Subscription?> GetExistingSubscriptionAsync(AwsSnsEmail awsEmail)
    {
        try
        {
            var subscriptionList = await _snsClient.ListSubscriptionsByTopicAsync(new ListSubscriptionsByTopicRequest
            {
                TopicArn = _awsSettings.SnsTopicArn
            });

            var subscription = subscriptionList?.Subscriptions?.FirstOrDefault(s => s.Endpoint == awsEmail.Email);
            _logger.LogDebug("[AwsSnsService] Búsqueda de suscripción para {Email}: {Found}", awsEmail.Email, subscription != null ? "Encontrada" : "No encontrada");

            return subscription;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AwsSnsService] Error buscando suscripción para email: {Email}", awsEmail.Email);
            throw;
        }
    }

    public async Task<SubscribeResponse> SubscribeEmailAsync(AwsSnsEmail awsSnsEmail)
    {
        try
        {
            // Define el filtro para recibir mensajes dirigidos al email o a "all"
            var filterPolicy = new Dictionary<string, List<string>>
            {
                { "target", new List<string> { awsSnsEmail.Email, "all" } }
            };

            // Serializa el filtro a JSON y colócalo en los atributos
            var attributes = new Dictionary<string, string>
            {
                { "FilterPolicy", JsonConvert.SerializeObject(filterPolicy) }
            };

            // Crea la solicitud de suscripción
            var request = new SubscribeRequest
            {
                TopicArn = _awsSettings.SnsTopicArn,
                Protocol = "email",
                Endpoint = awsSnsEmail.Email,
                Attributes = attributes
            };

            var response = await _snsClient.SubscribeAsync(request);
            _logger.LogInformation("[AwsSnsService] Email suscrito a SNS: {Email}, ARN: {SubscriptionArn}", awsSnsEmail.Email, response.SubscriptionArn);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AwsSnsService] Error suscribiendo email a SNS: {Email}", awsSnsEmail.Email);
            throw;
        }
    }

    public async Task<UnsubscribeResponse> UnsubscribeAsync(Subscription subscription)
    {
        try
        {
            var response = await _snsClient.UnsubscribeAsync(subscription.SubscriptionArn);
            _logger.LogInformation("[AwsSnsService] Suscripción cancelada: {SubscriptionArn}", subscription.SubscriptionArn);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AwsSnsService] Error cancelando suscripción: {SubscriptionArn}", subscription.SubscriptionArn);
            throw;
        }
    }

    public async Task<PublishResponse> PublishMassiveMessageAsync(AwsSnsMessage awsSnsMessage)
    {
        var messageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "target",
                    new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = "all"
                    }
                }
            };

        var request = new PublishRequest
        {
            TopicArn = _awsSettings.SnsTopicArn,
            Subject = awsSnsMessage.Subject,
            Message = awsSnsMessage.Message,
            MessageAttributes = messageAttributes
        };

        return await PublishMessageAsync(request);
    }

    public async Task<PublishResponse> PublishMessageByEmailAsync(AwsSnsMessageByEmail awsSnsMessageByEmail)
    {
        var messageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "target",
                    new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = awsSnsMessageByEmail.Email
                    }
                }
            };

        var request = new PublishRequest
        {
            TopicArn = _awsSettings.SnsTopicArn,
            Subject = awsSnsMessageByEmail.Subject,
            Message = awsSnsMessageByEmail.Message,
            MessageAttributes = messageAttributes
        };

        return await PublishMessageAsync(request);
    }

    private async Task<PublishResponse> PublishMessageAsync(PublishRequest publishRequest)
    {
        try
        {
            var response = await _snsClient.PublishAsync(publishRequest);
            _logger.LogInformation("[AwsSnsService] Mensaje publicado en SNS: {MessageId}", response.MessageId);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AwsSnsService] Error publicando mensaje en SNS");
            throw;
        }
    }
}
