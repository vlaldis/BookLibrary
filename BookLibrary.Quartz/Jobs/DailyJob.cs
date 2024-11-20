using BookLibrary.DataLayer;
using BookLibrary.Quartz.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BookLibrary.Quartz.Jobs;


public class DailyJob(IBookLibraryContext dbContext, IEmailService emailService, ILogger<DailyJob> logger) : IJob
{
    private const string EmailSubject = "Notification: Return your book";
    private const string EmailBody = "Please, return our book tomorrow!";

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Daily job executed at: " + DateTime.Now);

        var today = DateTime.Today;
        var thirtyDaysFromToday = today.AddDays(30);
        var tomorrow = DateOnly.FromDateTime(today.AddDays(1));

        var booksToReturn = await dbContext.Books
            .Include(_ => _.BorrowedByUser)
            .Where(_ => _.IsBorrowed)
            .Where(_ => _.BorrowedDate.HasValue)
            .Where(_ => _.BorrowedDate!.Value.AddDays(30) == tomorrow)
            .ToListAsync();

        // Print the list of books to return

        logger.LogInformation("Books to return: {books}", string.Join("\n", booksToReturn.Select(_ => $"- {_.Title} by {_.Author}")));

        var usersToNotify = booksToReturn.Select(_ => _.BorrowedByUser);
        var sendEmailTasks = usersToNotify.Select(_ => emailService.SendEmailAsync(_.Email, EmailSubject, EmailBody)).ToArray();

        await Task.WhenAll(sendEmailTasks);

        foreach (var task in sendEmailTasks)
        {
            if(task.IsFaulted)
                logger.LogError("Failed to send email {error}", task.Exception);
         }
    }
}
