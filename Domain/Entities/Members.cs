namespace Domain.Entities;

public class Members
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTimeKind MembershipDate { get; set; }
}
