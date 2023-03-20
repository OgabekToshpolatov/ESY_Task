using taskapi.Dtos.ProductAudit;
using taskapi.Entities;

namespace taskapi.Services;

public interface IProductAuditService
{
    Task<List<ProductAudit>> GetAllAsync();
    Task AddAsync(ProductAudit productAudit);
    Task<List<ProductAuditDto>> FilterAsync(Filter filter);
}