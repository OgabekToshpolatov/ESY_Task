using taskapi.Entities;
using taskapi.Model;

namespace taskapi.Services;

public interface IProductService
{
    ValueTask<Result<Product>> CreateAsync(Product model,string userId);
    ValueTask<Result<Product>> Update(long id, Product model,string userId);
    ValueTask<Result<Product>> Remove(long id,string userId);
    ValueTask<Result<List<Product>>> GetAll();
    ValueTask<Result<Product>> GetByIdAsync(long id);
}