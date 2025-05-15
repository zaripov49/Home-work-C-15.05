using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBorrowingService
{
    public Task<List<Borrowings>> GetAllBorrowingsAsync();
    public Task<Borrowings?> GetBorrowingByMemberId(int memberId);
}
