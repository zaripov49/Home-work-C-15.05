using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class MemberService : IMemberService
{
    private readonly DataContext context = new DataContext();

    public async Task<string> CreatMemberAsync(Members members)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"INSERT INTO Members (fullName, phone, email, membershipDate)
                            VALUES (@fullName, @phone, @email, @membershipDate);";
            var result = await connection.ExecuteAsync(cmd, members);
            if (result > 0)
            {
                return "Created Member successfully";
            }
            return "Created Member not successfully";
        }
    }

    public async Task<List<Members>> GetAllMembersAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Members";
            var result = await connection.QueryAsync<Members>(cmd);
            return result.ToList();
        }
    }

    public async Task<Members?> GetMemberAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Members Where id = @id";
            var result = await connection.QueryFirstOrDefaultAsync<Members>(cmd, new {id = id});
            return result;
        }
    }

    public async Task<string> UpdateMemberAsync(Members members)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Update Members set 
                        fullName = @fullName, phone = @phone, email = @email, membershipDate = @membershipDate";
            var result = await connection.ExecuteAsync(cmd, members);
            if (result > 0)
            {
                return "Updated Member successfully";
            }
            return "Updated Member not successfully";
        }
    }

    public async Task<string> DeleteMemberAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Delete from Members Where id = @id";
            var result = await connection.ExecuteAsync(cmd, new {id = id});
            if (result > 0)
            {
                return "Deleted Member successfully";
            }
            return "Deleted Member not successfully";
        }
    }
}
