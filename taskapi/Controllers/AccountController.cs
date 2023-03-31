using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

        System.Console.WriteLine("##############################################################################");
        
        if(await _userManager.Users.AnyAsync(u => u.Email == createUserDto.Email))
                return BadRequest(new { Message = "Email already exist! " });
        System.Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

        if(await _userManager.Users.AnyAsync(u => u.UserName == createUserDto.UserName))
                return BadRequest(new { Message = "Username already exist! " });

        var passMessage = CheckPasswordStrength(createUserDto.Password!);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });

        if(createUserDto.Password != createUserDto.ConfirmPassword)
                return BadRequest(new { Message = "Password doesn't equal to confirm password"});

        System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

        var user = createUserDto.Adapt<User>();

        user.Roles = new string[] {"user"};

        await _userManager.CreateAsync(user, createUserDto.Password);

        _logger.LogInformation("User saved to database with id {0}", user.Id);

        await _signManager.SignInAsync(user,isPersistent:true);

        _logger.LogInformation("The user has successfully registered");

        return Ok(new {Message = "User Registered "});
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(UserSignInDto userSignInDto)
    {
        if(!ModelState.IsValid)
                return BadRequest();

        if(!await _userManager.Users.AnyAsync(user => user.UserName == userSignInDto.UserName))
                return NotFound();

        var result = await _signManager.PasswordSignInAsync(userSignInDto.UserName, userSignInDto.Password,
                isPersistent:true,false);

        if(!result.Succeeded)
                return BadRequest();

        return Ok(userSignInDto);
    }

     private static string CheckPasswordStrength(string pass)
     {
        StringBuilder sb = new StringBuilder();
        if (pass.Length < 8)
            sb.Append("Minimum password length should be 8" + Environment.NewLine);
        if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
            sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
        if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
            sb.Append("Password should contain special charcter" + Environment.NewLine);
        return sb.ToString();
     }

}