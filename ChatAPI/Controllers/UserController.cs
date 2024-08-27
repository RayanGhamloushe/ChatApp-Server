using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ChatAPI.Interfaces;
using ChatAPI.Models;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] Users user)
        {
            try
            {
                if (await _userRepository.UserExistsAsync(user.Email))
                {
                    return BadRequest(new { success = false, message = "User already registered" });
                }

                await _userRepository.AddUserAsync(user);
                return Ok(new { success = true, message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                return StatusCode(500, new { success = false, message = "An error occurred while registering the user.", error = ex.Message });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] Users user)
        {
            try
            {
                // Log the received payload
                Console.WriteLine($"Received SignIn Request: Email={user.Email}, Password={user.Password}");

                var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    Console.WriteLine("User not registered");
                    return BadRequest(new { success = false, message = "User not registered" });
                }

                // Log fetched user details
                Console.WriteLine($"User found: Email={existingUser.Email}, Password={existingUser.Password}");

                if (user.Password != existingUser.Password)
                {
                    Console.WriteLine("Invalid password");
                    return Unauthorized(new { success = false, message = "Invalid password" });
                }

                Console.WriteLine("User signed in successfully");
                return Ok(new { success = true, message = "User signed in successfully", userId = existingUser.Id, userName = existingUser.Name });
            }
            catch (Exception ex)
            {
                // Log the exception (logging not shown here for brevity)
                return StatusCode(500, new { success = false, message = "An error occurred while signing in the user.", error = ex.Message });
            }
        }
    }
}
