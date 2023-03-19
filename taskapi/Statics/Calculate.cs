using taskapi.Entities;

namespace taskapi.Statics;

public static class Calculate
{
    public static double VatCalculating(this Product product, double vat)
    {
        try
        {
            product.TotalPriceWithVat = (product.Quantity * product.Price) * (1 + vat);

            return product.TotalPriceWithVat;
        }
        catch (System.Exception e)
        {
          throw new Exception(e.Message);
        }
    }
}