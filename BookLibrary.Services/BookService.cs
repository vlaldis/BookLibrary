using BookLibrary.DataLayer;
using BookLibrary.Dto;
using BookLibrary.Services.Abstractions;


namespace BookLibrary.Services;

public class BookService(IBookLibraryContext context) : IBookService
{
    public async Task<Book> CreateBook(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> GetBook(int id)
        => await context.Books.FindAsync(id);

    public async Task<Book> UpdateBook(int id, Book book)
    {
        var existingBook = await context.Books.FindAsync(id);
        if (existingBook == null)
            return null;

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Description = book.Description;

        await context.SaveChangesAsync();
        return existingBook;
    }

    public async Task DeleteBook(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book != null)
        {
            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Book> BorrowBook(int bookId, int userId)
    {
        var book = await context.Books.FindAsync(bookId);
        var user = await context.Users.FindAsync(userId);

        if (book == null || user == null || book.IsBorrowed)
            return null;

        book.IsBorrowed = true;
        book.BorrowedByUserId = userId;
        book.BorrowedByUser = user;
        book.BorrowedDate = DateOnly.FromDateTime(DateTime.UtcNow);

        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> ReturnBook(int bookId)
    {
        var book = await context.Books.FindAsync(bookId);

        if (book == null || !book.IsBorrowed)
            return null;

        book.IsBorrowed = false;
        book.BorrowedByUserId = null;
        book.BorrowedByUser = null;
        book.BorrowedDate = null;

        await context.SaveChangesAsync();
        return book;
    }
}

