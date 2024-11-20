using BookLibrary.Dto;
using Microsoft.EntityFrameworkCore;


namespace BookLibrary.DataLayer;

public class BookLibraryContext : DbContext, IBookLibraryContext
{
    public BookLibraryContext(DbContextOptions<BookLibraryContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
}
