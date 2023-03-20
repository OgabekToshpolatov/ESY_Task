using taskapi.Dtos.AppUser;

namespace taskapi.Dtos.ProductAudit;

public class ProductAuditDto
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public UserDto? User { get; set; }
    public EStatus Status { get; set; }
    public DateTime ChangeData { get; set; }
}