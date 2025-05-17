using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBorrowingService
{
    public Task<List<Borrowings>> GetAllBorrowingsAsync();
    public Task<Borrowings?> GetBorrowingByMemberId(int memberId);
    public Task<string> CreateBorrowingAsync(Borrowings borrowings);
    public Task<string> ReturnBookAsync(int borrowingId);
    public Task<int> GetCountAllBorrowingAsync();
    public Task<decimal> GetAvgFineAsync();
}
