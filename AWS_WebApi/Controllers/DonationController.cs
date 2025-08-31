using ClassLibrary_Application.Models;
using ClassLibrary_Application.Services;
using ClassLibrary_Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace AWS_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DonationController : ControllerBase
{
    private readonly ILogger<DonationController> _logger;
    private readonly IDonationService _donationService;

    public DonationController(ILogger<DonationController> logger, IDonationService donationService)
    {
        _logger = logger;
        _donationService = donationService;
    }

    [HttpPost("subscribe")]
    public async Task<ActionResult<ApiResponse<string>>> Subscribe([FromForm] AwsSnsEmail awsSnsEmail)
    {
        _logger.LogInformation("[DonationsController] - Subscribing email: {Email}", awsSnsEmail.Email);
        var apiResponse = await _donationService.SubscribeEmailAsync(awsSnsEmail);
        return StatusCode(apiResponse.StatusCode, apiResponse);
    }

    [HttpPost("unsubscribe")]
    public async Task<ActionResult<ApiResponse<string>>> Unsubscribe([FromForm] AwsSnsEmail awsSnsEmail)
    {
        _logger.LogInformation("[DonationsController] - Unsubscribing email: {Email}", awsSnsEmail.Email);
        var apiResponse = await _donationService.UnsubscribeEmailAsync(awsSnsEmail);
        return StatusCode(apiResponse.StatusCode, apiResponse);
    }

    [HttpPost("notification")]
    public async Task<ActionResult<ApiResponse<string>>> SendMassNotification(AwsSnsMessage awsSnsMessage)
    {
        _logger.LogInformation("[DonationsController] - Sending mass notification with message: {Message}", awsSnsMessage);
        var apiResponse = await _donationService.SendMassiveNotificationAsync(awsSnsMessage);
        return StatusCode(apiResponse.StatusCode, apiResponse);
    }
}
