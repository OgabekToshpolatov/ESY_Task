using System.ComponentModel.DataAnnotations;

namespace taskapi.Dtos.Product;

public class Product
{
    [Required]
    public string? Title { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public double Price { get; set; }

}