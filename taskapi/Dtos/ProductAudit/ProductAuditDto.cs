namespace taskapi.Dtos.ProductAudit;

public class ProductAuditDto
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public OldValue? OldValue { get; set; }
    public NewValue? NewValue { get; set; }
    public EStatus Status { get; set; }
    public DateTime ChangeData { get; set; }
}