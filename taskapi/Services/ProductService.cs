using Mapster;
using Microsoft.EntityFrameworkCore;
using taskapi.Data;
using taskapi.Entities;
using taskapi.Model;
using taskapi.Statics;

namespace taskapi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    public ProductService(AppDbContext context)
    {
        _context = context ;
    }

    public async ValueTask<Result<Product>> CreateAsync(Product model, string userId)
    {
        try
        {
            if(model is null) return new("Model is null");

             await _context.Products!.AddAsync(model);

            await _context.SaveChangesAsync();

            return new(true) {Data = model};

        }
        catch (System.Exception e)
        {
             throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<List<Product>>> GetAll()
    {
        try
        {
            var products =await  _context.Products!.ToListAsync();

            if(products.Count == 0)
                return new("Product not found");

            products.ForEach(product => {
              product.TotalPriceWithVat = Calculate.VatCalculating(product,Vat.Value);
              product.Price = product.TotalPriceWithVat;
            });

            return new(true){Data = products};
              
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<Product>> GetByIdAsync(long id)
    {
        try
        {
            var product = await _context.Products!.FindAsync(id);

            if(product is null)
                  return new("Product not found"); 

            product.Price = Calculate.VatCalculating(product,Vat.Value);
    
            return new(true) {Data = product}; 
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<Product>> Remove(long id, string userId)
    {
        try
        {
            var product =await  _context.Products!.FindAsync(id);

            if(product is null)
                return new("Product not found");

             _context.Products.Remove(product);

             _context.SaveChanges();

            return new(true){ };

        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<Product>> Update(long id, Product model, string userId)
    {
        try
        {
            var product = await _context.Products!.FindAsync(id);

            if(product is null)
                return new("Product not found");

            product.Title = model.Title;
            product.Quantity = model.Quantity;
            product.Price = model.Price;

            _context.SaveChanges();

            return new(true){Data = product}; 
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}