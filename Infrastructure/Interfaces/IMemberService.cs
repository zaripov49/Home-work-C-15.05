using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IMemberService
{
    public Task<string> CreatMemberAsync(Members members);
    public Task<List<Members>> GetAllMembersAsync();
    public Task<Members?> GetMemberAsync(int id);
    public Task<string> UpdateMemberAsync(Members members);
    public Task<string> DeleteMemberAsync(int id);
    public Task<Members?> GetMemberTakeMaxBookAsync();
    public Task<int> GetCountMemberOneBorrowingsAsync();
}
