using Microsoft.AspNetCore.Mvc;
using UserManagement.Domain;
using UserManagement.Repository;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using UserManagement.API.Models;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController] 
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            IActionResult response = Unauthorized();
            var user = await _userRepository.ValidateUserLogin(loginModel);
 
            if (user != null && user.Id != 0) 
            {
                var tokenStr = GenerateJwt(user);
                response = Ok(new{ bearer = tokenStr });
            }

            return response;

        }

        private string GenerateJwt(UserModel userModel)
        {
            var sec = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cred = new SigningCredentials(sec, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("RoleId", userModel.RoleId.ToString()),
                new Claim("CompanyId", userModel.CompanyID.ToString())
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
