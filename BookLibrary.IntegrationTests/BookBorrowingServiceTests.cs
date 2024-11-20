using BookLibrary.DataLayer;
using BookLibrary.Dto;
using BookLibrary.Services;
using BookLibrary.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BookLibrary.IntegrationTests;

public class BookBorrowingServiceTests
{
    const string DefaultConnection = "Server=localhost;Database=BookLibrary;User=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;Encrypt=False;trusted_connection=false;";
    
    [Fact]
    public async Task BorrowBook_Should_Update_Book_Status_And_Add_Borrowing_History()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<BookService>>();
        var serviceProvider = new ServiceCollection()
            .AddDbContext<IBookLibraryContext, BookLibraryContext>(options =>
                options.UseSqlServer(DefaultConnection))
            .AddTransient<IBookService, BookService>()
            .AddSingleton<ILogger<BookService>>(loggerMock.Object)
            .BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookLibraryContext>();
            var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();

            var book = new Book { Title = "Test Book", IsBorrowed = false, Author = "SampleAuthor", Description = "Very nice book" };
            dbContext.Books.Add(book);
            var user = new User { Name = "User 1", Email = "User1@exampleemail.com", };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var userId = await dbContext.Users.Select(_ => _.Id).FirstOrDefaultAsync();
            // Act
            await bookService.BorrowBook(book.Id, userId);

            // Assert
            var updatedBook = dbContext.Books.Find(book.Id);
            Assert.True(updatedBook.IsBorrowed);

            var borrowedBook = dbContext.Books.FirstOrDefault();
            Assert.NotNull(borrowedBook);
            Assert.Equal(userId, borrowedBook.BorrowedByUserId);
        }
    }
}
