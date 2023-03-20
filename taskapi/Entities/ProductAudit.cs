using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace taskapi.Entities;

public class ProductAudit
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    public EStatus Status { get; set; }
    public DateTime ChangeData { get; set; }

}