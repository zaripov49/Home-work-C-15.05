using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class BookService : IBookService
{
    private readonly DataContext context = new DataContext();

    public async Task<string> CreatBookAsync(Books books)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
                            VALUES (@Title, @Genre, @PublicationYear, @TotalCopies, @AvailableCopies);";
            var result = await connection.ExecuteAsync(cmd, books);
            if (result > 0)
            {
                return "Created Book successfully";
            }
            return "Created Book not successfully";
        }
    }

    public async Task<List<Books>> GetAllBooksAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Books";
            var result = await connection.QueryAsync<Books>(cmd);
            return result.ToList();
        }
    }

    public async Task<Books?> GetBookAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Books Where id = @id";
            var result = await connection.QueryFirstOrDefaultAsync<Books>(cmd, new {id = id});
            return result;
        }
    }

    public async Task<string> UpdateBookAsync(Books books)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Update Books set 
                        title = @title, genre = @genre, publicationYear = @publicationYear, totalCopies = @totalCopies, availableCopies = @availableCopies";
            var result = await connection.ExecuteAsync(cmd, books);
            if (result > 0)
            {
                return "Updated Book successfully";
            }
            return "Updated Book not successfully";
        }
    }

    public async Task<string> DeleteBookAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Delete from Books Where id = @id";
            var result = await connection.ExecuteAsync(cmd, new {id = id});
            if (result > 0)
            {
                return "Deleted Book successfully";
            }
            return "Deleted Book not successfully";
        }
    }

}
