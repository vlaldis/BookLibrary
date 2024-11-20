using BookLibrary.Dto;
using BookLibrary.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdBook = await _bookService.CreateBook(book);
        return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _bookService.GetBook(id);
        if (book == null)
            return NotFound();

        return book;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> UpdateBook(int id, [FromBody] Book book)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedBook = await _bookService.UpdateBook(id, book);
        if (updatedBook == null)
            return NotFound();

        return updatedBook;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        await _bookService.DeleteBook(id);
        return NoContent();
    }

    [HttpPost("{bookId}/borrow/{userId}")]
    public async Task<ActionResult<Book>> BorrowBook(int bookId, int userId)
    {
        var borrowedBook = await _bookService.BorrowBook(bookId, userId);
        if (borrowedBook == null)
            return BadRequest("Book not available or invalid book/user ID");

        return borrowedBook;
    }

    [HttpPost("{bookId}/return")]
    public async Task<ActionResult<Book>> ReturnBook(int bookId)
    {
        var returnedBook = await _bookService.ReturnBook(bookId);
        if (returnedBook == null)
            return BadRequest("Book not borrowed or invalid book ID");

        return returnedBook;
    }
}
