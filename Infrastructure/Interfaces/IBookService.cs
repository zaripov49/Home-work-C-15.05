using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBookService
{
    public Task<string> CreatBookAsync(Books books);
    public Task<List<Books>> GetAllBooksAsync();
    public Task<Books?> GetBookAsync(int id);
    public Task<string> UpdateBookAsync(Books books);
    public Task<string> DeleteBookAsync(int id);
}
