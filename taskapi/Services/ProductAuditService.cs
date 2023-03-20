using Microsoft.EntityFrameworkCore;
using taskapi.Data;
using taskapi.Dtos.ProductAudit;
using taskapi.Entities;

namespace taskapi.Services;

public class ProductAuditService : IProductAuditService
{
    private readonly AppDbContext _context;

    public ProductAuditService(AppDbContext context)
    {
        _context = context ;
    }
    public async Task AddAsync(ProductAudit productAudit)
    {
        await _context.ProductAudits!.AddAsync(productAudit);
        await _context.SaveChangesAsync();
    }

    public Task<List<ProductAuditDto>> FilterAsync(Filter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProductAudit>> GetAllAsync()
    {
        var historList =await _context.ProductAudits!.ToListAsync();
        return historList;
    }
}