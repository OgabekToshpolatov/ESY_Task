namespace taskapi.Dtos.Product;

public class ProductViewList
{
    public int Number { get; set; } = 1;
    public string? Title { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double TotalPriceWithVat { get; set; }
}