using AWS_WebApp.Hubs;
using ClassLibrary_Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace AWS_WebApp.Services;

public class ConsoleNotifier : IConsoleNotifier
{
    private readonly IHubContext<ConsoleHub> _hubContext;

    public ConsoleNotifier(IHubContext<ConsoleHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendConsoleMessage(string content)
    {
        var formattedMessage = $"[{DateTime.Now:HH:mm:ss}] {content}";
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", formattedMessage);
    }
}
