using BookLibrary.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BookLibrary.DataLayer
{
    public interface IBookLibraryContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<User> Users { get; set; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}