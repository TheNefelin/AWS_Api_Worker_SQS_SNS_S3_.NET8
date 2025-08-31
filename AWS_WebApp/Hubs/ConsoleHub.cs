using Microsoft.AspNetCore.SignalR;

namespace AWS_WebApp.Hubs;

public class ConsoleHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", "╔═══════════════════════════════════════════════════════════╗");
        await Clients.Caller.SendAsync("ReceiveMessage", "║                    DONATION PROCESSOR                     ║");
        await Clients.Caller.SendAsync("ReceiveMessage", "║                AWS SQS S3 Worker Console                  ║");
        await Clients.Caller.SendAsync("ReceiveMessage", "╚═══════════════════════════════════════════════════════════╝");
        await Clients.Caller.SendAsync("ReceiveMessage", $"Iniciado a las: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        await Clients.Caller.SendAsync("ReceiveMessage", "Worker ejecutandose en segundo plano");
        await base.OnConnectedAsync();
    }

    public async Task SendMessageAsync(string message)
    {
        // Envía a todos los clientes conectados
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
