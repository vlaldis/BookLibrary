using BookLibrary.Dto;

namespace BookLibrary.Services.Abstractions;

public interface IBookService
{
    Task<Book> CreateBook(Book book);
    Task<Book> GetBook(int id);
    Task<Book> UpdateBook(int id, Book book);
    Task DeleteBook(int id);
    Task<Book> BorrowBook(int bookId, int userId);
    Task<Book> ReturnBook(int bookId);
}
