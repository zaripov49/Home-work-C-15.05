using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(BookService bookService)
{
    [HttpPost]
    public async Task<string> CreateBookAsync(Books books)
    {
        var result = await bookService.CreatBookAsync(books);
        return result;
    }

    [HttpGet]
    public async Task<List<Books>> GetAllBooksAsync()
    {
        var result = await bookService.GetAllBooksAsync();
        return result;
    }

    [HttpGet("{id:int}")]
    public async Task<Books?> GetBookAsync(int id)
    {
        var result = await bookService.GetBookAsync(id);
        return result;
    }

    [HttpPut]
    public async Task<string> UpdateBookAsync(Books books)
    {
        var result = await bookService.UpdateBookAsync(books);
        return result;
    }

    [HttpDelete("{id:int}")]
    public async Task<string> DeleteBookAsync(int id)
    {
        var result = await bookService.DeleteBookAsync(id);
        return result;
    }

    [HttpGet("GetBookMaxCountAvailableCopies")]
    public async Task<Books?> GetBookMaxCountAvailableCopiesAsync()
    {
        return await bookService.GetBookMaxCountAvailableCopiesAsync();
    }

    [HttpGet("GetAllBooksReturnDateIsNull")]
    public async Task<List<Books>> GetAllBooksReturnDateIsNullAsync()
    {
        return await bookService.GetAllBooksReturnDateIsNullAsync();
    }

    [HttpGet("GetBooksOneCountAvailableCopies")]
    public async Task<List<Books>> GetBooksOneCountAvailableCopiesAsync()
    {
        return await bookService.GetBooksOneCountAvailableCopiesAsync();
    }

    [HttpGet("GetCountIdOne")]
    public async Task<int> GetCountIdOneAsync()
    {
        return await bookService.GetCountIdOneAsync();
    }

    [HttpGet("GetBookMaxGenre")]
    public async Task<Books?> GetBookMaxGenreAsync()
    {
        return await bookService.GetBookMaxGenreAsync();
    }
}
