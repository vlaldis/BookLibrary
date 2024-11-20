using Microsoft.Extensions.Logging;

namespace BookLibrary.Quartz.Services;

public class FakeEmailService(ILogger<FakeEmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        logger.LogInformation("Sending email to {recipient}, with {subject} and {body}", recipient, subject, body);
        await Task.Yield();
    }
}
