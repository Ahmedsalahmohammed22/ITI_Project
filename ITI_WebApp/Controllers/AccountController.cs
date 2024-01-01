using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestRestApi.Data.Models;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<AppUser> userManager , IConfiguration configuration) 
        {
            _userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser(DtoNewUser user)
        {
            if(ModelState.IsValid)
            {
                AppUser newUser = new()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(newUser , user.Password);
                if(result.Succeeded)
                {
                    return Ok("Success");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError(" ", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);

        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(DtoUser login)
        {
            if(ModelState.IsValid)
            {
                AppUser? appUser = await _userManager.FindByNameAsync(login.Name);
                if(appUser != null)
                {
                    if(await _userManager.CheckPasswordAsync(appUser, login.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, appUser.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(appUser);
                        foreach(var role in roles) 
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                                claims: claims,
                                issuer: configuration["JWT:Issuer"],
                                audience: configuration["JWT:Audience"],
                                expires: DateTime.Now.AddHours(1),
                                signingCredentials: sc
                            );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                else
                {
                    ModelState.AddModelError("", "User Name is invalid");
                }


                return Ok();
            }
            return BadRequest();
        }
    }
}
