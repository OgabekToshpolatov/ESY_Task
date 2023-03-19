using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace taskapi.Entities;

public class ProductAudit
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? UserId { get; set; }
    
    [Display(Name ="OldValue")]
    public virtual Product? OldValue { get; set; }

    [Display(Name ="NewValue")]
    public virtual Product? NewValue { get; set; }
    public EStatus Status { get; set; }

}