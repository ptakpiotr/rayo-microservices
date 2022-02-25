using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RayoAuth.RabbitMQ;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RayoAuth.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _sender;
        private readonly IConfiguration _config;

        public AccountController(ILogger<AccountController> logger,UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,IMapper mapper,IEmailSender sender,IConfiguration config)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _sender = sender;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = _mapper.Map<IdentityUser>(model);

                IdentityResult res = await _userManager.CreateAsync(user, model.Password);

                if (res.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        IdentityRole adminRole = new() { Name = "Admin" };
                        IdentityRole userRole = new() { Name = "User" };

                        await _roleManager.CreateAsync(adminRole);
                        await _roleManager.CreateAsync(userRole);
                    }

                    await _userManager.AddToRoleAsync(user,"User");

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string? redirUrl = Url.Action("ConfirmEmail", "Account", new { email = user.Email, token }, Request.Scheme);

                    await _sender.SendEmailAsync(user.Email, "RayoFanSite Account confirmation",redirUrl);

                    return Ok(new ResponseModel
                    {
                        Message = "Account Succesfully created, confirm it"
                    });
                }
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(model.Email);

                if(user is null)
                {
                    return NotFound();
                }

                bool passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

                if (passwordCorrect && user.EmailConfirmed)
                {
                    List<Claim> claims = new();
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();

                    foreach(string r in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, r));
                    }

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
                    var tokenDescriptor = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
                        expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

                    return Ok(new ResponseModel { Message = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor) });
                }
            }

            return BadRequest();
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string email,string token)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            IdentityUser user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                return NotFound();
            }

            var res = await _userManager.ConfirmEmailAsync(user, token);

            if (res.Succeeded)
            {
                return Ok(new ResponseModel
                {
                    Message="Account confirmed succesfully"
                });
            }

            return BadRequest();
        }

        [HttpPost("passwordforget")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.Email);

                if (user is null)
                {
                    return NotFound();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                string? redirUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                await _sender.SendEmailAsync(model.Email,"RayoFanSite Password reset",redirUrl);

                return Ok(new ResponseModel
                {
                    Message="Visit your email for following instructions"
                });
            }

            return BadRequest();
        }

        [HttpPost("passwordreset")]
        public async Task<IActionResult> ResetPassword(string email,string token,ResetPasswordModel model)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token) || !ModelState.IsValid)
            {
                return Unauthorized();
            }

            IdentityUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            var res = await _userManager.ResetPasswordAsync(user, token,model.NewPassword);

            if (res.Succeeded)
            {
                return Ok(new ResponseModel
                {
                    Message = "Password changed succesfully"
                });
            }

            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(DeleteUserModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(model.Email);

                if(user is null)
                {
                    return NotFound();
                }

                await _userManager.DeleteAsync(user);

                return NoContent();
            }

            return BadRequest();
        }
    }
}
