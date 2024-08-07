using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using UserManagement.API.Models;
using UserManagement.Domain;
using UserManagement.Repository;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Policy = "Admins")]
        public async Task<IActionResult> GetAllUsers(int CompanyId) 
        {
            try
            {
                var users = await _userRepository.GetAllUsers(CompanyId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Source, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "Users")]
        public async Task<IActionResult> GetAllNonAdminUsers(int CompanyId)
        {
            try
            {
                var users = await _userRepository.GetAllNonAdminUsers(CompanyId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Source, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admins")]
        public async Task<IActionResult> AddNewUser([FromBody] NewUserModel userModel)
        {
            try
            {
                var checkResult = await _userRepository.IsUserExists(userModel.Username);
                if (checkResult) 
                {
                    return BadRequest("User already exists");
                }

                var user = await _userRepository.CreateNewUser(userModel);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Source, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Policy = "Admins")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel userModel)
        {
            try
            {
                var result = await _userRepository.UpdateUser(userModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Source, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{UserId}")]
        [Authorize(Policy = "Admins")]
        public async Task<IActionResult> DeleteUser(long UserId)
        {
            try
            {
                var result = await _userRepository.DeleteUser(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.Source, ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
