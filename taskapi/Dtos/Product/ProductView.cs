namespace taskapi.Dtos.Product;

public class ProductView
{
    public string? Title { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double TotalPriceWithVat { get; set; }
}