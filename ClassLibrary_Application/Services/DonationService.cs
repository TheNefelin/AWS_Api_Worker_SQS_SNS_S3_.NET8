using ClassLibrary_Application.Models;
using ClassLibrary_Infrastructure.Models;
using ClassLibrary_Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Application.Services;

public class DonationService : IDonationService
{
    private readonly ILogger<DonationService> _logger;
    private readonly IAwsSnsService _awsSnsService;

    public DonationService(ILogger<DonationService> logger, IAwsSnsService awsSnsService)
    {
        _logger = logger;
        _awsSnsService = awsSnsService;
    }

    public async Task<ApiResponse<string>> SubscribeEmailAsync(AwsSnsEmail awsSnsEmail)
    {
        try
        {
            var subscription = await _awsSnsService.GetExistingSubscriptionAsync(awsSnsEmail);

            if (subscription != null)
            {
                if (subscription.SubscriptionArn == "PendingConfirmation")
                {
                    return new ApiResponse<string>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Solicitud de suscripción ya enviada. Revisa tu correo para confirmar."
                    };
                }

                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Ya estás suscrito a las notificaciones."
                };
            }

            var subscriptionRespone = await _awsSnsService.SubscribeEmailAsync(awsSnsEmail);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Solicitud de suscripción enviada. Revisa tu correo para confirmar.",
                Data = subscriptionRespone.SubscriptionArn
            };
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error en servicio de suscripción para: {Email}", awsSnsEmail.Email);

            return new ApiResponse<string>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<Object>> UnsubscribeEmailAsync(AwsSnsEmail awsSnsEmail)
    {
        try
        {
            var subscription = await _awsSnsService.GetExistingSubscriptionAsync(awsSnsEmail);

            if (subscription == null || subscription.SubscriptionArn == "PendingConfirmation")
            {
                return new ApiResponse<Object>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "No se encontró una suscripción activa para este correo."
                };
            }

            await _awsSnsService.UnsubscribeAsync(subscription);

            return new ApiResponse<Object>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = $"El correo {awsSnsEmail.Email} ha sido desuscrito correctamente."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio de desuscripción para: {Email}", awsSnsEmail.Email);

            return new ApiResponse<Object>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<string>> SendMassiveNotificationAsync(AwsSnsMessage awsSnsMessage)
    {
        try
        {
            var publishResponse = await _awsSnsService.PublishMassiveMessageAsync(awsSnsMessage);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = $"Notificación masiva enviada con éxito [{publishResponse.MessageId}].",
                Data = publishResponse.MessageId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando notificación masiva");

            return new ApiResponse<string>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<string>> SendDonationNotificationAsync(AwsSnsDonation awsSnsDonation)
    {
        try
        {
            var publishResponse = await _awsSnsService.PublishDonationMessageAsync(awsSnsDonation);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = $"Notificación masiva enviada con éxito [{publishResponse.MessageId}].",
                Data = publishResponse.MessageId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando notificación masiva");

            return new ApiResponse<string>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<string>> SendDonationNotificationByEmailAsync(AwsSnsDonation awsSnsDonation)
    {
        try
        {
            var awsSnsMessageByEmail = new AwsSnsMessageByEmail
            { 
                Email = awsSnsDonation.Email,
                Subject = "Donacion",
                Message = awsSnsDonation.Amount.ToString(),
            };  

            var publishResponse = await _awsSnsService.PublishMessageByEmailAsync(awsSnsMessageByEmail);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = $"🎉 ¡Gracias por tu donación! {awsSnsMessageByEmail.Email}",
                Data = publishResponse.MessageId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando recibo de donación a: {Email}", awsSnsDonation.Email);

            return new ApiResponse<string>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }
}
