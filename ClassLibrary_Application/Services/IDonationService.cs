using ClassLibrary_Application.Models;
using ClassLibrary_Infrastructure.Models;

namespace ClassLibrary_Application.Services;

public interface IDonationService
{
    public Task<ApiResponse<string>> SubscribeEmailAsync(AwsSnsEmail awsSnsEmail);
    public Task<ApiResponse<Object>> UnsubscribeEmailAsync(AwsSnsEmail awsSnsEmail);
    public Task<ApiResponse<string>> SendMassiveNotificationAsync(AwsSnsMessage awsSnsMessage);
    public Task<ApiResponse<string>> SendDonationNotificationByEmailAsync(AwsSnsMessageByEmail awsSnsMessageByEmail);
}
