using Amazon.SimpleNotificationService.Model;
using ClassLibrary_Infrastructure.Models;

namespace ClassLibrary_Infrastructure.Services;

public interface IAwsSnsService
{
    public Task<Subscription?> GetExistingSubscriptionAsync(AwsSnsEmail awsSnsEmail);
    public Task<SubscribeResponse> SubscribeEmailAsync(AwsSnsEmail awsSnsEmail);
    public Task<UnsubscribeResponse> UnsubscribeAsync(Subscription subscription);
    public Task<PublishResponse> PublishMassiveMessageAsync(AwsSnsMessage awsSnsMessage);
    public Task<PublishResponse> PublishDonationMessageAsync(AwsSnsDonation awsSnsDonation);
    public Task<PublishResponse> PublishMessageByEmailAsync(AwsSnsMessageByEmail awsSnsMessageByEmail);
}
