using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController(MemberService memberService)
{
    [HttpPost]
    public async Task<string> CreateMemberAsync(Members members)
    {
        var result = await memberService.CreatMemberAsync(members);
        return result;
    }

    [HttpGet]
    public async Task<List<Members>> GetAllMembersAsync()
    {
        var result = await memberService.GetAllMembersAsync();
        return result;
    }

    [HttpGet("{id:int}")]
    public async Task<Members?> GetMemberAsync(int id)
    {
        var result = await memberService.GetMemberAsync(id);
        return result;
    }

    [HttpPut]
    public async Task<string> UpdateMemberAsync(Members members)
    {
        var result = await memberService.UpdateMemberAsync(members);
        return result;
    }

    [HttpDelete("{id:int}")]
    public async Task<string> DeleteMemberAsync(int id)
    {
        var result = await memberService.DeleteMemberAsync(id);
        return result;
    }

    [HttpGet("GetMemberTakeMaxBook")]
    public async Task<Members?> GetMemberTakeMaxBookAsync()
    {
        return await memberService.GetMemberTakeMaxBookAsync();
    }

    [HttpGet("GetCountMemberOneBorrowings")]
    public async Task<int> GetCountMemberOneBorrowingsAsync()
    {
        return await memberService.GetCountMemberOneBorrowingsAsync();
    }
}
