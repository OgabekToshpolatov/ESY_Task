using Microsoft.AspNetCore.Mvc;
using taskapi.Services;

namespace taskapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoryController:ControllerBase
{
    private readonly IProductAuditService _productAuditService;

    public HistoryController(IProductAuditService productAuditService)
    {
        _productAuditService = productAuditService ;
    }

    [HttpGet]
    public async Task<IActionResult> Audit()
    {
        var histories =await _productAuditService.GetAllAsync();

        var historiesView = histories.Select(history => ConvertToRoomModel(history)).ToList();

        return Ok(historiesView);
    }
    private Dtos.ProductAudit.ProductAuditDto ConvertToRoomModel(Entities.ProductAudit productAudit)
    {
        return new Dtos.ProductAudit.ProductAuditDto()
        {
            Id = productAudit.Id,
            UserId = productAudit.UserId,
            OldValue = null,
            NewValue = productAudit.NewValue == null ? null:ConvertToProductModel(productAudit.NewValue),
            ChangeData = productAudit.ChangeData
        };
    }

    private Dtos.ProductAudit.NewValue ConvertToProductModel(Entities.Product product)
    {
        return new Dtos.ProductAudit.NewValue
        {
            Id = product.Id,
            Title = product.Title,
            Quantity = product.Quantity,
            Price = product.Price
        };
    }


}