namespace ClassLibrary_Application.Services;

public interface IConsoleNotifier
{
    Task SendConsoleMessage(string content);
}
