using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskapi.Dtos.AppUser;
using taskapi.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace taskapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController:ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signManager;
    private readonly IConfiguration _configuration;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AccountController> logger,
        IConfiguration configuration)
    {
        _logger = logger ;
        _userManager = userManager;
        _signManager = signInManager;
        _configuration = configuration;

    }

    [HttpPost("signup")]
    public  async Task<IActionResult> SignUp(UserSignUpDto createUserDto)
    {
        if(!ModelState.IsValid)
                return BadRequest();

        
        if(await _userManager.Users.AnyAsync(u => u.Email == createUserDto.Email))
                return BadRequest(new { Message = "Email already exist! " });

        if(await _userManager.Users.AnyAsync(u => u.UserName == createUserDto.UserName))
                return BadRequest(new { Message = "Username already exist! " });

        var passMessage = CheckPasswordStrength(createUserDto.Password!);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });

        if(createUserDto.Password != createUserDto.ConfirmPassword)
                return BadRequest(new { Message = "Password doesn't equal to confirm password"});


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
        System.Console.WriteLine("##################################################################");
        var user = await _userManager.FindByNameAsync(userSignInDto.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, userSignInDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
             System.Console.WriteLine("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

            System.Console.WriteLine("LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
                var tokenHandlar = GetToken(authClaims);
            System.Console.WriteLine("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");

            

                return Ok(new
                {
                    token = tokenHandlar    
                });
            }
            return Unauthorized();
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

     private string GetToken(List<Claim> authClaims)
        { 
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            
            var identity = new ClaimsIdentity(authClaims);

            // var token = new JwtSecurityToken(
            //     issuer: _configuration["JWT:ValidIssuer"],
            //     audience: _configuration["JWT:ValidAudience"],
            //     expires: DateTime.Now.AddHours(1),
            //     claims: identity,
            //     signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //     );
            
            // var tokenCookie = new JwtSecurityTokenHandler().WriteToken(token);

            // HttpContext.Response.Cookies.Append("AuthTokenBirnam",tokenCookie);
                            // new Microsoft.AspNetCore.Http.CookieOptions
                            //              { Expires = DateTime.Now.AddHours(3)} 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

}