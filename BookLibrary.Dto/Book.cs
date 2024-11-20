using System.ComponentModel.DataAnnotations;


namespace BookLibrary.Dto
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public bool IsBorrowed { get; set; }

        public int? BorrowedByUserId { get; set; }
        
        public User BorrowedByUser { get; set; }

        public DateOnly? BorrowedDate { get; set; }
    }
}

