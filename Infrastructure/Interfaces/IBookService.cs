using Domain.ApiResponse;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBookService
{
    public Task<Response<string>> CreatBookAsync(Books books);
    public Task<Response<List<Books>>> GetAllBooksAsync();
    public Task<Response<Books?>> GetBookAsync(int id);
    public Task<Response<string>> UpdateBookAsync(Books books);
    public Task<Response<string>> DeleteBookAsync(int id);
    public Task<Response<Books?>> GetBookMaxCountAvailableCopiesAsync();
    public Task<Response<List<Books>>> GetAllBooksReturnDateIsNullAsync();
    public Task<Response<List<Books>>> GetBooksOneCountAvailableCopiesAsync();
    public Task<Response<int>> GetCountIdOneAsync();
    public Task<Response<Books?>> GetBookMaxGenreAsync();
}
