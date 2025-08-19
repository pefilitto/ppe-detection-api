using Microsoft.AspNetCore.Mvc;
using ppe_detection_api.dto;
using ppe_detection_api.User.Service;

namespace ppe_detection_api.controller;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public IActionResult Register(dto.User user)
    {
        try
        {
            if(string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new { Message = "Invalid user data" });
            }
            
            _userService.Register(user);
            
            return Ok(new { Message = "User registered successfully", UserName = user.UserName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error: " + ex.Message });
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(Guid id)
    {
        try
        {
            dto.User user = _userService.GetUserById(id);

            if (user == new dto.User())
            {
                return NotFound(new { Message = "User not found" });
            }
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error: " + ex.Message });
        }
    }
}