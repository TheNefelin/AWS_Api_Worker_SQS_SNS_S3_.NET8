
using ClassLibrary_Application.Services;
using ClassLibrary_Infrastructure.Services;

namespace AWS_WebApp.Services;

public class SqsBackgroundService : BackgroundService
{
    private readonly ILogger<SqsBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConsoleNotifier _consoleNotifier;
    
    public SqsBackgroundService(ILogger<SqsBackgroundService> logger, IServiceProvider serviceProvider, IConsoleNotifier consoleNotifier)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _consoleNotifier = consoleNotifier;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consoleNotifier.SendConsoleMessage("🚀 SQS Background Service iniciado");
        await _consoleNotifier.SendConsoleMessage("📡 Conectando a SQS con Long Polling...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Crear scope para servicios scoped
                using var scope = _serviceProvider.CreateScope();
                var sqsService = scope.ServiceProvider.GetRequiredService<IAwsSqsService>();

                await _consoleNotifier.SendConsoleMessage("🔍 Consultando SQS (Long Polling 20s)...");

                // Long Polling - SQS espera hasta 20 segundos
                var messages = await sqsService.ReceiveMessagesAsync(
                    maxMessages: 10,
                    waitTimeSeconds: 20
                );

                if (messages.Any())
                {
                    await _consoleNotifier.SendConsoleMessage($"📨 ¡{messages.Count} mensaje(s) recibido(s)!");
                    await _consoleNotifier.SendConsoleMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                    foreach (var message in messages)
                    {
                        // Mostrar detalles del mensaje
                        await _consoleNotifier.SendConsoleMessage($"🆔 Message ID: {message.MessageId}");
                        await _consoleNotifier.SendConsoleMessage($"📝 Body: {message.Body}");

                        // Mostrar atributos si existen
                        if (message.MessageAttributes.Any())
                        {
                            await _consoleNotifier.SendConsoleMessage("📊 Atributos:");
                            foreach (var attr in message.MessageAttributes)
                            {
                                await _consoleNotifier.SendConsoleMessage($"   • {attr.Key}: {attr.Value.StringValue}");
                            }
                        }

                        // Aquí puedes agregar lógica de procesamiento
                        await _consoleNotifier.SendConsoleMessage("⚙️ Procesando mensaje...");

                        // Simular procesamiento
                        await Task.Delay(500, stoppingToken);

                        await _consoleNotifier.SendConsoleMessage("✅ Mensaje procesado correctamente");

                        // Eliminar mensaje de SQS después de procesarlo
                        await sqsService.DeleteMessageAsync(message.ReceiptHandle);
                        await _consoleNotifier.SendConsoleMessage($"🗑️ Mensaje {message.MessageId} eliminado de SQS");
                    }

                    await _consoleNotifier.SendConsoleMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                }
                else
                {
                    await _consoleNotifier.SendConsoleMessage("📭 No hay mensajes nuevos en SQS");
                }

                // Pequeña pausa antes del siguiente ciclo (opcional)
                await Task.Delay(2000, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Cancelación normal
                break;
            }
            catch (Exception ex)
            {
                //await _consoleNotifier.SendConsoleMessage($"❌ Error en SQS Service: {ex.Message}");
                _logger.LogError(ex, "Error en SqsBackgroundService");

                // Pausa más larga en caso de error
                await Task.Delay(10000, stoppingToken);
            }
        }

        await _consoleNotifier.SendConsoleMessage("🛑 SQS Background Service detenido");
    }
}
