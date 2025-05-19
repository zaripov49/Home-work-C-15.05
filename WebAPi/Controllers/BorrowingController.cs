using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingController(BorrowingService borrowingService)
{
    [HttpGet]
    public async Task<List<Borrowings>> GetAllBorrowingsAsync()
    {
        return await borrowingService.GetAllBorrowingsAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<Borrowings?> GetBorrowingByMemberId(int memberId)
    {
        return await borrowingService.GetBorrowingByMemberId(memberId);
    }

    [HttpPost]
    public async Task<string> CreateBorrowingAsync(Borrowings borrowings)
    {
        return await borrowingService.CreateBorrowingAsync(borrowings);
    }

    [HttpPut]
    public async Task<string> ReturnBookAsync(int borrowingId)
    {
        return await borrowingService.ReturnBookAsync(borrowingId);
    }

    [HttpGet]
    public async Task<int> GetCountAllBorrowingAsync()
    {
        return await borrowingService.GetCountAllBorrowingAsync();
    }

    [HttpGet]
    public async Task<decimal> GetAvgFineAsync()
    {
        return await borrowingService.GetAvgFineAsync();
    }
}
