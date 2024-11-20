using BookLibrary.DataLayer;
using BookLibrary.Dto;
using BookLibrary.Quartz.Jobs;
using BookLibrary.Quartz.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace BookLibrary.Quartz.UnitTests.Jobs;

public class DailyJobTests
{
    [Fact]
    public async Task Execute_Should_Send_Email_To_Users_With_Overdue_Books()
    {
        // Arrange
        var dbContextMock = new Mock<IBookLibraryContext>();
        var emailServiceMock = new Mock<IEmailService>();
        var loggerMock = new Mock<ILogger<DailyJob>>();

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var borrowedBooks = new List<Book>
        {
            new Book { Title = "Book 1", Author = "Author 1", IsBorrowed = true, BorrowedDate = DateOnly.FromDateTime(today.AddDays(-30)), BorrowedByUser = new User { Email = "user1@example.com" } },
            new Book { Title = "Book 2", Author = "Author 2", IsBorrowed = true, BorrowedDate = DateOnly.FromDateTime(today.AddDays(-29)), BorrowedByUser = new User { Email = "user2@example.com" } },
            new Book { Title = "Book 3", Author = "Author 3", IsBorrowed = true, BorrowedDate = DateOnly.FromDateTime(today.AddDays(-31)), BorrowedByUser = new User { Email = "user3@example.com" } },
        };

        dbContextMock.Setup(b => b.Books).ReturnsDbSet(borrowedBooks);

        emailServiceMock.Setup(_ => _.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var dailyJob = new DailyJob(dbContextMock.Object, emailServiceMock.Object, loggerMock.Object);

        // Act
        await dailyJob.Execute(null);

        // Assert
        emailServiceMock.Verify(_ => _.SendEmailAsync("user1@example.com", "Notification: Return your book", "Please, return our book tomorrow!"), Times.Never);
        emailServiceMock.Verify(_ => _.SendEmailAsync("user2@example.com", "Notification: Return your book", "Please, return our book tomorrow!"), Times.Once);
        emailServiceMock.Verify(_ => _.SendEmailAsync("user3@example.com", "Notification: Return your book", "Please, return our book tomorrow!"), Times.Never);
    }
}