using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class BorrowingService(DataContext context) : IBorrowingService
{
    public async Task<List<Borrowings>> GetAllBorrowingsAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Borrowings";
            var result = await connection.QueryAsync<Borrowings>(cmd);
            return result.ToList();
        }
    }

    public async Task<Borrowings?> GetBorrowingByMemberId(int memberId)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Borrowings Where memberId = @memberId";
            var result = await connection.QueryFirstOrDefaultAsync<Borrowings>(cmd, new { memberId = memberId });
            return result;
        }
    }

    public async Task<string> CreateBorrowingAsync(Borrowings borrowings)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string bookCommand = $@"Select * from books Where id = @id";
            var book = await connection.QueryFirstOrDefaultAsync<Books>(bookCommand, new { id = borrowings.BookId });
            if (book == null)
            {
                return "Book not found";
            }

            if (book.AvailableCopies <= 0)
            {
                return "Kniga v daniy moment ne svobodnyi";
            }

            if (borrowings.BorrowDate >= borrowings.DueDate)
            {
                return "data выдачи книг не может быть позже даты обратно вырнуть";
            }

            string borrowingCommand = $@"Insert into Borrowings(bookId, memberId, borrowDate, dueDate) values (@bookId, @memberId, @borrowDate, @dueDate)";
            var result = await connection.ExecuteAsync(borrowingCommand, borrowings);

            if (result == 0)
            {
                return "Borrowing not created";
            }

            var updateBookCommand = "Update books set availableCopies = availableCopies - 1 where id = @id";
            await connection.ExecuteAsync(updateBookCommand, new { id = borrowings.BookId });

            return "Borrowing created";
        }
    }

    public async Task<string> ReturnBookAsync(int borrowingId)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string borrowingCommand = "Select * from borrowings Where id = @id";
            var borrowing = await connection.QueryFirstOrDefaultAsync<Borrowings>(borrowingCommand, new { id = borrowingId });
            if (borrowing == null)
            {
                return "Borrowing not found";
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
                return "Borrowing not updated";
            }

            string updateBooksCommand = "Update Book Set AvailableCopies = AvailableCopies + 1 where id = @id";
            await connection.ExecuteAsync(updateBooksCommand, new { id = borrowing.BookId });
            return "Borrowing uptadet";
        }
    }

    public async Task<int> GetCountAllBorrowingAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = "Select count(*) from Borrowings";
            var result = await connection.ExecuteScalarAsync<int>(cmd);
            return result;
        }
    }

    public async Task<decimal> GetAvgFineAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();


            string cmd = "Select avg(Fine) from Borrowings";
            var result = await connection.ExecuteScalarAsync<decimal>(cmd);
            return result;
        }
    }

}
