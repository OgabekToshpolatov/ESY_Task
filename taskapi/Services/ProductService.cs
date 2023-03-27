using System.Collections.Concurrent;
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
    private readonly IProductAuditService _productAuditService;

    public ProductService(
        AppDbContext context,
        IProductAuditService productAuditService
        )
    {
        _context = context ;
        _productAuditService = productAuditService ;
    }

    public async ValueTask<Result<Product>> CreateAsync(Product model, string userId)
    {
        try
        {
            if(model is null) return new("Model is null");


            await _context.Products!.AddAsync(model);

            await _context.SaveChangesAsync();

            var newProductAudit = new ProductAudit()
            {
               UserId = userId,
               ProductId = model.Id,
               Status = EStatus.Create,
               ChangeData = DateTime.Now
            };

            await _productAuditService.AddAsync(newProductAudit);

            return new(true) {Data = model};

        }
        catch (System.Exception e)
        {
            System.Console.WriteLine("1111111111111111111111111111111111111111111111111111111111111");
             throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<List<Product>>> GetAll()
    {
        try
        {
            var products =await  _context.Products!.ToListAsync();

            if(products.Count != 0)
            {
                products.ForEach(product => {

                product.TotalPriceWithVat = Calculate.VatCalculating(product,Vat.Value);

                });
            }
            
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

            product.TotalPriceWithVat = Calculate.VatCalculating(product,Vat.Value);
    
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

            var newProductAudit = new ProductAudit()
           {
               UserId = userId,
               ProductId = id,
               Status = EStatus.Delete,
               ChangeData = DateTime.Now
           };

            await _productAuditService.AddAsync(newProductAudit);

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

            var newProductAudit = new ProductAudit()
           {
               UserId = userId,
               ProductId = model.Id,
               Status = EStatus.Update,
               ChangeData = DateTime.Now
           };

            await _productAuditService.AddAsync(newProductAudit);

            return new(true){Data = product}; 
        }
        catch (System.Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}