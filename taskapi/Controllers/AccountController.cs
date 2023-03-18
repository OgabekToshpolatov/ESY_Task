using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskapi.Dtos.AppUser;
using taskapi.Entities;

namespace taskapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController:ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signManager;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger)
    {
        _logger = logger ;
        _userManager = userManager;
        _signManager = signInManager;
    }

    [HttpPost("signup")]
    public  async Task<IActionResult> SignUp(UserSignUpDto createUserDto)
    {
        if(!ModelState.IsValid)
                return BadRequest();
        
        if(await _userManager.Users.AnyAsync(u => u.Email == createUserDto.Email))
                return BadRequest("This email is registered");

        if(await _userManager.Users.AnyAsync(u => u.UserName == createUserDto.UserName))
                return BadRequest("There is a user with this username");

        var user = createUserDto.Adapt<User>();

        await _userManager.CreateAsync(user, createUserDto.Password);

        _logger.LogInformation("User saved to database with id {0}", user.Id);

        await _signManager.SignInAsync(user,isPersistent:true);

        _logger.LogInformation("The user has successfully registered");

        return Ok();
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(UserSignInDto userSignInDto)
    {
        if(!ModelState.IsValid)
                return BadRequest();

        // if(!await _userManager.Users.AnyAsync(user => user.UserName == userSignInDto.UserName))
        //         return NotFound();

        var user = _userManager.FindByNameAsync(userSignInDto.UserName);

        if(user is null)
                return NotFound();

        var result = await _signManager.PasswordSignInAsync(userSignInDto.UserName, userSignInDto.Password,
                isPersistent:true,false);

        if(!result.Succeeded)
                return BadRequest();

       return Ok();
    }
}