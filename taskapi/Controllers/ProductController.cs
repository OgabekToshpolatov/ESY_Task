using System;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taskapi.Entities;
using taskapi.Services;

namespace taskapi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController:ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;
    private readonly UserManager<User> _userManager;
    private readonly IProductAuditService _productAuditService;

    public ProductController(
        ILogger<ProductController> logger,
        IProductService productService,
        UserManager<User> userManager,
        IProductAuditService productAuditService)
    {
        _logger = logger ;
        _productService = productService ;
        _userManager = userManager ;
        _productAuditService = productAuditService ;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostProduct(Dtos.Product.Product product)
    {
        System.Console.WriteLine(product.Price);

        if(!ModelState.IsValid) 
            return BadRequest(new { Message = "Modelstate is not  Valid"});

        var userIdentityName = User.Identity!.Name;

        var user = _userManager.Users.FirstOrDefault(x => x.UserName == userIdentityName);
      
         if(user is null)
        {
          return BadRequest(new {Message ="User No"});
        }

        var entity =  product.Adapt<Entities.Product>();  
                                                        //user.Id
        await _productService.CreateAsync(entity,user.Id);

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        System.Console.WriteLine("################################################################################");
        Console.WriteLine("________________________________________________________________________________________");
        var products = await _productService.GetAll();

        System.Console.WriteLine("Salom gulim qandaysan ");

        if(products.Data!.Count == 0)
            return Ok(products.Data);
        
        var productView = products.Data!
            .Select(product => product.Adapt<Dtos.Product.ProductView>());

        return Ok(products.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(long id)
    {
        var product =await _productService.GetByIdAsync(id);

        if(product.Data is null)
            return NotFound("Product not found");

        var productView = product.Data.Adapt<Dtos.Product.ProductView>();

        return Ok(productView);      
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id)
    { 
        var userIdentityName = User.Identity!.Name;

        var user = _userManager.Users.FirstOrDefault(x => x.UserName == userIdentityName);

        if(user is null)
            return BadRequest();
        
        var product =await _productService.Remove(id,user.Id);

        if(!product.IsSuccess)
                return BadRequest("Product not found"); 
 
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(long id, Dtos.Product.Product updateproduct)
    {
        var userIdentityName = User.Identity!.Name;

        var user = _userManager.Users.FirstOrDefault(x => x.UserName == userIdentityName);

        if(user is null)
            return BadRequest("User is null");

        if(!ModelState.IsValid)
            return BadRequest("ModelState is Valid");

        var entity = updateproduct.Adapt<Entities.Product>();    
        
        var product = await _productService.Update(id,entity,user.Id);

        if(!product.IsSuccess)
            return BadRequest("Product not found");
           
        return Ok(product.Data!.Adapt<Dtos.Product.Product>());
    }

}