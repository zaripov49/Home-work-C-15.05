using Domain.ApiResponse;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(BookService bookService)
{
    [HttpPost]
    public async Task<Response<string>> CreateBookAsync(Books books)
    {
        var result = await bookService.CreatBookAsync(books);
        return result;
    }

    [HttpGet]
    public async Task<Response<List<Books>>> GetAllBooksAsync()
    {
        var result = await bookService.GetAllBooksAsync();
        return result;
    }

    [HttpGet("{id:int}")]
    public async Task<Response<Books?>> GetBookAsync(int id)
    {
        var result = await bookService.GetBookAsync(id);
        return result;
    }

    [HttpPut]
    public async Task<Response<string>> UpdateBookAsync(Books books)
    {
        var result = await bookService.UpdateBookAsync(books);
        return result;
    }

    [HttpDelete("{id:int}")]
    public async Task<Response<string>> DeleteBookAsync(int id)
    {
        var result = await bookService.DeleteBookAsync(id);
        return result;
    }

    [HttpGet("GetBookMaxCountAvailableCopies")]
    public async Task<Response<Books?>> GetBookMaxCountAvailableCopiesAsync()
    {
        return await bookService.GetBookMaxCountAvailableCopiesAsync();
    }

    [HttpGet("GetAllBooksReturnDateIsNull")]
    public async Task<Response<List<Books>>> GetAllBooksReturnDateIsNullAsync()
    {
        return await bookService.GetAllBooksReturnDateIsNullAsync();
    }

    [HttpGet("GetBooksOneCountAvailableCopies")]
    public async Task<Response<List<Books>>> GetBooksOneCountAvailableCopiesAsync()
    {
        return await bookService.GetBooksOneCountAvailableCopiesAsync();
    }

    [HttpGet("GetCountIdOne")]
    public async Task<Response<int>> GetCountIdOneAsync()
    {
        return await bookService.GetCountIdOneAsync();
    }

    [HttpGet("GetBookMaxGenre")]
    public async Task<Response<Books?>> GetBookMaxGenreAsync()
    {
        return await bookService.GetBookMaxGenreAsync();
    }
}
