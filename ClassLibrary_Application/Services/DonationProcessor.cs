using Amazon.SQS.Model;
using ClassLibrary_Infrastructure.Models;
using ClassLibrary_Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ClassLibrary_Application.Services;

public class DonationProcessor : IDonationProcessor
{
    private readonly IConsoleNotifier _consoleNotifier;
    private readonly ILogger<DonationProcessor> _logger;
    private readonly IAwsSqsService _sqsService;

    public DonationProcessor(IConsoleNotifier consoleNotifier, ILogger<DonationProcessor> logger, IAwsSqsService sqsService)
    {
        _consoleNotifier = consoleNotifier;
        _logger = logger;
        _sqsService = sqsService;
    }


    public async Task ProcessDonationAsync(Message sqsMessage)
    {
        try
        {
            await _consoleNotifier.SendConsoleMessage($"🆔 Message ID: {sqsMessage.MessageId}");

            // Deserializar el cuerpo del mensaje SQS
            var sqsBody = JsonSerializer.Deserialize<AwsSqsMessageBody>(sqsMessage.Body);

            if (sqsBody == null)
            {
                await _consoleNotifier.SendConsoleMessage("❌ Error: No se pudo deserializar el cuerpo del mensaje");
                return;
            }

            await _consoleNotifier.SendConsoleMessage($"📋 Subject: {sqsBody.Subject}");

            // Deserializar el mensaje interno de la donación
            var donation = JsonSerializer.Deserialize<AwsSqsDonationMessage>(sqsBody.Message);

            if (donation == null)
            {
                await _consoleNotifier.SendConsoleMessage("❌ Error: No se pudo deserializar el mensaje de donación");
                return;
            }

            // Mostrar información de la donación
            await _consoleNotifier.SendConsoleMessage($"📧 Email: {donation.Email}");
            await _consoleNotifier.SendConsoleMessage($"💰 Amount: ${donation.Amount}");

            // Mostrar atributos si existen
            if (sqsMessage.MessageAttributes != null && sqsMessage.MessageAttributes.Any())
            {
                await _consoleNotifier.SendConsoleMessage("📊 Atributos:");
                foreach (var attr in sqsMessage.MessageAttributes)
                {
                    await _consoleNotifier.SendConsoleMessage($"   • {attr.Key}: {attr.Value.StringValue}");
                }
            }

            // Procesar la donación (lógica de negocio aquí)
            await _consoleNotifier.SendConsoleMessage("⚙️ Procesando donación...");
            await Task.Delay(500); // Simular procesamiento

            await _consoleNotifier.SendConsoleMessage("✅ Donación procesada correctamente");

            // Eliminar mensaje de SQS después de procesarlo
            if (!string.IsNullOrEmpty(sqsMessage.ReceiptHandle))
            {
                await _sqsService.DeleteMessageAsync(sqsMessage.ReceiptHandle);
                await _consoleNotifier.SendConsoleMessage($"🗑️ Mensaje {sqsMessage.MessageId} eliminado de SQS");
            }
        }
        catch (Exception ex)
        {
            await _consoleNotifier.SendConsoleMessage($"❌ Error procesando donación: {ex.Message}");
            _logger.LogError(ex, "Error en DonationProcessor");
            throw;
        }
    }
}
