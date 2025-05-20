using Domain.ApiResponse;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IMemberService
{
    public Task<Response<string>> CreatMemberAsync(Members members);
    public Task<Response<List<Members>>> GetAllMembersAsync();
    public Task<Response<Members?>> GetMemberAsync(int id);
    public Task<Response<string>> UpdateMemberAsync(Members members);
    public Task<Response<string>> DeleteMemberAsync(int id);
    public Task<Response<Members?>> GetMemberTakeMaxBookAsync();
    public Task<Response<int>> GetCountMemberOneBorrowingsAsync();
}
