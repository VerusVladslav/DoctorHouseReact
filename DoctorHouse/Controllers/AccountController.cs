using System.Threading.Tasks;
using DoctorHouse.DAL;
using DoctorHouse.DAL.Entities;
using DoctorHouse.Helper;
using DoctorHouse.Infrastructure.Interfaces;
using DoctorHouse.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoctorHouse.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtTokenService _JwtTokenService;

        public AccountController(UserManager<DbUser> userManager, SignInManager<DbUser> signInManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _JwtTokenService = jwtTokenService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = CustomValidator.GetErrorsByModel(ModelState);
                return BadRequest(errors);
            }


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new {
                    title = "Login failed",
                    invalidParams = new[] {
                        new {
                            name="email",
                            reason="Даний користувач не знайденний"
                        }
                    }
                });
            

            var result = _signInManager
                .PasswordSignInAsync(user, model.Password, false, false).Result;

            if (!result.Succeeded)
                return BadRequest(new {
                    title = "Validation errors",
                    invalidParams = new[] {
                        new {
                            name="password",
                            reason="Невірно введений пароль"
                        }
                    }
                });

            await _signInManager.SignInAsync(user, isPersistent: true);

            return Ok(
                new
                {
                    token = _JwtTokenService.CreateToken(user)
                });
            
        }
    }
}