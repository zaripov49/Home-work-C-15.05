using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class BorrowingService : IBorrowingService
{
    private DataContext context = new DataContext();

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
            var result = await connection.QueryFirstOrDefaultAsync<Borrowings>(cmd, new {memberId = memberId});
            return result;
        }
    }

}
