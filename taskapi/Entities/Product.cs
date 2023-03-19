using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taskapi.Entities;

public class Product
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    [Display(Name = "Total Price with VAT")]
    [NotMapped]
    public double TotalPriceWithVat { get; set; }
}