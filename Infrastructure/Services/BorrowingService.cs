using System.Net;
using Dapper;
using Domain.ApiResponse;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class BorrowingService(DataContext context) : IBorrowingService
{
    public async Task<Response<List<Borrowings>>> GetAllBorrowingsAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Borrowings";
            var result = await connection.QueryAsync<Borrowings>(cmd);
            return new Response<List<Borrowings>>(result.ToList(), "Successfully");
        }
    }

    public async Task<Response<Borrowings?>> GetBorrowingByMemberId(int memberId)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Borrowings Where memberId = @memberId";
            var result = await connection.QueryFirstOrDefaultAsync<Borrowings>(cmd, new { memberId = memberId });
            return new Response<Borrowings?>(result, "Successfuly");
        }
    }

    public async Task<Response<string>> CreateBorrowingAsync(Borrowings borrowings)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string bookCommand = $@"Select * from books Where id = @id";
            var book = await connection.QueryFirstOrDefaultAsync<Books>(bookCommand, new { id = borrowings.BookId });
            if (book == null)
            {
                return new Response<string>("Book not found", HttpStatusCode.NotFound);
            }

            if (book.AvailableCopies <= 0)
            {
                return new Response<string>("Kniga v daniy moment ne svobodnyi", HttpStatusCode.NotFound);
            }

            if (borrowings.BorrowDate >= borrowings.DueDate)
            {
                return new Response<string>("data выдачи книг не может быть позже даты обратно вырнуть", HttpStatusCode.NotFound);
            }

            string borrowingCommand = $@"Insert into Borrowings(bookId, memberId, borrowDate, dueDate) values (@bookId, @memberId, @borrowDate, @dueDate)";
            var result = await connection.ExecuteAsync(borrowingCommand, borrowings);

            if (result == 0)
            {
                return new Response<string>("Borrowing not created", HttpStatusCode.NotFound);
            }

            var updateBookCommand = "Update books set availableCopies = availableCopies - 1 where id = @id";
            await connection.ExecuteAsync(updateBookCommand, new { id = borrowings.BookId });

            return new Response<string>(null, "Borrowing created");
        }
    }

    public async Task<Response<string>> ReturnBookAsync(int borrowingId)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string borrowingCommand = "Select * from borrowings Where id = @id";
            var borrowing = await connection.QueryFirstOrDefaultAsync<Borrowings>(borrowingCommand, new { id = borrowingId });
            if (borrowing == null)
            {
                return new Response<string>("Borrowing not found", HttpStatusCode.NotFound);
            }
            borrowing.ReturnDate = DateTime.Now;
            if (borrowing.ReturnDate > borrowing.DueDate)
            {
                var days = borrowing.ReturnDate.Day - borrowing.DueDate.Day;
                borrowing.Fine = days * 10;
            }

            string updateBorrowingCommand = "Update borrowings Set ReturnDate = @returnDate, Fine = @fine Where id = @id";
            var result = await connection.ExecuteAsync(updateBorrowingCommand, borrowing);
            if (result == 0)
            {
                return new Response<string>("Borrowing not updated", HttpStatusCode.NotFound);
            }

            string updateBooksCommand = "Update Book Set AvailableCopies = AvailableCopies + 1 where id = @id";
            await connection.ExecuteAsync(updateBooksCommand, new { id = borrowing.BookId });
            return new Response<string>(null, "Borrowing uptadet");
        }
    }

    public async Task<Response<int>> GetCountAllBorrowingAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = "Select count(*) from Borrowings";
            var result = await connection.ExecuteScalarAsync<int>(cmd);
            return new Response<int>(result, "Successfuly");
        }
    }

    public async Task<Response<decimal>> GetAvgFineAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();


            string cmd = "Select avg(Fine) from Borrowings";
            var result = await connection.ExecuteScalarAsync<decimal>(cmd);
            return new Response<decimal>(result, "Successfuly");
        }
    }

}
