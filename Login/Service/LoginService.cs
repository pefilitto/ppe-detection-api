using Microsoft.AspNetCore.Identity.Data;
using ppe_detection_api.Login.Dto;
using ppe_detection_api.Login.Repository;

namespace ppe_detection_api.Login.Service;

public class LoginService
{
    private readonly LoginRepository _loginRepository;
    
    public LoginService(LoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }
    
    public LoginResponseDto Login(LoginRequestDto request)
    {
        var response = _loginRepository.Login(request);

        string hashedPassword = response.Password;
        
        if (string.IsNullOrEmpty(response.UserName) || !BCrypt.Net.BCrypt.Verify(request.Password, hashedPassword))
        {
            throw new Exception("Invalid username or password.");
        }    

        return new LoginResponseDto();
    }
}