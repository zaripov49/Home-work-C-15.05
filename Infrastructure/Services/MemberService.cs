using Dapper;
using Domain.ApiResponse;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class MemberService(DataContext context) : IMemberService
{
    public async Task<Response<string>> CreatMemberAsync(Members members)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"INSERT INTO Members (fullName, phone, email, membershipDate)
                            VALUES (@fullName, @phone, @email, @membershipDate);";
            var result = await connection.ExecuteAsync(cmd, members);
            if (result == 0)
            {
                return new Response<string>("Some thing went wrong", System.Net.HttpStatusCode.InternalServerError);
            }
            return new Response<string>(null, "Book successfully created");
        }
    }

    public async Task<Response<List<Members>>> GetAllMembersAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Members";
            var result = await connection.QueryAsync<Members>(cmd);
            return new Response<List<Members>>(result.ToList(), "Successfuly");
        }
    }

    public async Task<Response<Members?>> GetMemberAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Select * from Members Where id = @id";
            var result = await connection.QueryFirstOrDefaultAsync<Members>(cmd, new { id = id });
            return new Response<Members?>(result, "Successfully");
        }
    }

    public async Task<Response<string>> UpdateMemberAsync(Members members)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Update Members set 
                        fullName = @fullName, phone = @phone, email = @email, membershipDate = @membershipDate";
            var result = await connection.ExecuteAsync(cmd, members);
            if (result > 0)
            {
                return new Response<string>("Updated Member successfully", System.Net.HttpStatusCode.NotFound);
            }
            return new Response<string>(null, "Updated Member not successfully");
        }
    }

    public async Task<Response<string>> DeleteMemberAsync(int id)
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = $@"Delete from Members Where id = @id";
            var result = await connection.ExecuteAsync(cmd, new { id = id });
            if (result > 0)
            {
                return new Response<string>("Deleted Member successfully", System.Net.HttpStatusCode.NotFound);
            }
            return new Response<string>(null, "Deleted Member not successfully");
        }
    }

    public async Task<Response<Members?>> GetMemberTakeMaxBookAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = @"Select m.Id, m.FullName, count(b.Id) as TotalBorrowing 
                        from Members as m
                        JOIN Borrowings as b ON m.Id = b.MemberId
						Group by m.Id, m.FullName
                        Order by TotalBorrowing desc Limit 1";
            var result = await connection.QuerySingleOrDefaultAsync<Members>(cmd);
            return new Response<Members?>(result, "Successfuly");
        }
    }

    public async Task<Response<int>> GetCountMemberOneBorrowingsAsync()
    {
        using (var connection = await context.GetConnectionAsync())
        {
            connection.Open();

            string cmd = @"Select count(*) from Members 
                        where id = (
                        Select memberId from Borrowings)";
            var result = await connection.ExecuteScalarAsync<int>(cmd);
            return new Response<int>(result, "Successfully");
        }
    }
}
