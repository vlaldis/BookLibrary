using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Dto;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    public List<Book>? BorrowedBooks { get; set; }
}