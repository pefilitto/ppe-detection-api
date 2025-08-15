using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using ppe_detection_api.Login.Dto;
using ppe_detection_api.Login.Service;

namespace ppe_detection_api.Login.Controller;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly LoginService _loginService;
    
    public LoginController(LoginService loginService)
    {
        _loginService = loginService;
    }
    
    [HttpPost]
    public IActionResult Login(LoginRequestDto loginRequestDto)
    {
        try
        {
            if (string.IsNullOrEmpty(loginRequestDto.UserName) || string.IsNullOrEmpty(loginRequestDto.Password))
            {
                return BadRequest(new { Message = "Informe o usuario e senha!" });
            }

            try
            {
                LoginResponseDto response = _loginService.Login(loginRequestDto);
                
                response.Token = new JwtBuilder()
                    .WithAlgorithm(new JWT.Algorithms.HMACSHA256Algorithm())
                    .WithSecret("your_secret_key")
                    .AddClaim("iss", "ppe_detection_api")
                    .AddClaim("sub", loginRequestDto.UserName)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                    .Encode();

                return Ok(new LoginResponseDto { Token = response.Token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error: " + ex.Message });
        }
    }
}