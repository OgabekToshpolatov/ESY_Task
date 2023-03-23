using System.Net;
using System.Security.Cryptography;
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
            User = productAudit.User == null ? null : ConvertToUserModel(productAudit.User),
            Status = ConvertToEStatusModel(productAudit.Status),
            ChangeData = productAudit.ChangeData
        };
    }

    private Dtos.AppUser.UserDto ConvertToUserModel(Entities.User user)
    {
        return new Dtos.AppUser.UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }

    private Dtos.ProductAudit.EStatus ConvertToEStatusModel(Entities.EStatus status)
    => status switch
    {
        Entities.EStatus.Create => Dtos.ProductAudit.EStatus.Create,
        Entities.EStatus.Delete => Dtos.ProductAudit.EStatus.Delete,
        Entities.EStatus.Update => Dtos.ProductAudit.EStatus.Update,
                              _ => Dtos.ProductAudit.EStatus.Create
    };


}