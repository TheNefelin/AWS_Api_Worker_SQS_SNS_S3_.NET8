
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
                        using var messageScope = _serviceProvider.CreateScope();
                        var donationProcessor = messageScope.ServiceProvider.GetRequiredService<IDonationProcessor>();
                        await donationProcessor.ProcessDonationAsync(message);
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
                await _consoleNotifier.SendConsoleMessage($"❌ Error en SQS Service: {ex}");
                _logger.LogError(ex, "Error en SqsBackgroundService");

                // Pausa más larga en caso de error
                await Task.Delay(10000, stoppingToken);
            }
        }

        await _consoleNotifier.SendConsoleMessage("🛑 SQS Background Service detenido");
    }
}
