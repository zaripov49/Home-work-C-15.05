using System.Net;
using Dapper;
using Domain.ApiResponse;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class BookService(DataContext context) : IBookService
{
    public async Task<Response<string>> CreatBookAsync(Books books)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"INSERT INTO Books (Title, Genre, PublicationYear, TotalCopies, AvailableCopies)
                            VALUES (@Title, @Genre, @PublicationYear, @TotalCopies, @AvailableCopies);";
            var result = await connection.ExecuteAsync(cmd, books);
            if (result == 0)
            {
                return new Response<string>("Some thing went wrong", HttpStatusCode.InternalServerError);
            }
            return new Response<string>(null, "Book successfully created");
        }
    }

    public async Task<Response<List<Books>>> GetAllBooksAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Books";
            var result = await connection.QueryAsync<Books>(cmd);
            return new Response<List<Books>>(result.ToList(), "Books retrieved successfuly");
        }
    }

    public async Task<Response<Books?>> GetBookAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Books Where id = @id";
            var result = await connection.QueryFirstOrDefaultAsync<Books>(cmd, new { id = id });
            if (result == null)
            {
                return new Response<Books?>("Book not found", HttpStatusCode.NotFound);
            }
            return new Response<Books?>(result, "Book successfully retrieved");
        }
    }

    public async Task<Response<string>> UpdateBookAsync(Books books)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Update Books set 
                        title = @title, genre = @genre, publicationYear = @publicationYear, totalCopies = @totalCopies, availableCopies = @availableCopies";
            var result = await connection.ExecuteAsync(cmd, books);
            if (result == 0)
            {
                return new Response<string>("Some thing went wrong", HttpStatusCode.InternalServerError);
            }
            return new Response<string>(null, "Book successfully updated");
        }
    }

    public async Task<Response<string>> DeleteBookAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Delete from Books Where id = @id";
            var result = await connection.ExecuteAsync(cmd, new { id = id });
            if (result == 0)
            {
                return new Response<string>("Some thing went wrong", HttpStatusCode.InternalServerError);
            }
            return new Response<string>(null, "Book successfully deleted");
        }
    }

    public async Task<Response<Books?>> GetBookMaxCountAvailableCopiesAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = "Select * from Books Order By AvailableCopies desc Limit 1";
            var result = await connection.QuerySingleOrDefaultAsync<Books>(cmd);
            return new Response<Books?>(result, "Successfully");
        }
    }

    public async Task<Response<List<Books>>> GetAllBooksReturnDateIsNullAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = @"Select * from Books as b
                        Join Borrowings br on b.id = br.BookId
                        Where ReturnDate is null";
            var result = await connection.QueryAsync<Books>(cmd);
            return new Response<List<Books>>(result.ToList(), "Successfuly");
        }
    }

    public async Task<Response<List<Books>>> GetBooksOneCountAvailableCopiesAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = @"Select * from Books Where TotalCopies < 2";
            var result = await connection.QueryAsync<Books>(cmd);
            return new Response<List<Books>>(result.ToList(), "Successfuly");
        }
    }

    public async Task<Response<int>> GetCountIdOneAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = @"select count(*) from Books
                            where id not in (
	                        select BookId from Borrowings)";
            var result = await connection.ExecuteScalarAsync<int>(cmd);
            return new Response<int>(result, "Successfuly");
        }
    }

    public async Task<Response<Books?>> GetBookMaxGenreAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = @"Select Genre, count(*) from Books 
                            Group by Genre 
                            Having count(Genre) = (
	                        select count(Genre) as genre from Books
	                        order by genre desc Limit 1)";
            var result = await connection.QuerySingleOrDefaultAsync<Books>(cmd);
            return new Response<Books?>(result, "Succesfuly");
        }
    }
}
