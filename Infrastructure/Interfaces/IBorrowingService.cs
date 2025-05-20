using Domain.ApiResponse;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IBorrowingService
{
    public Task<Response<List<Borrowings>>> GetAllBorrowingsAsync();
    public Task<Response<Borrowings?>> GetBorrowingByMemberId(int memberId);
    public Task<Response<string>> CreateBorrowingAsync(Borrowings borrowings);
    public Task<Response<string>> ReturnBookAsync(int borrowingId);
    public Task<Response<int>> GetCountAllBorrowingAsync();
    public Task<Response<decimal>> GetAvgFineAsync();
}
