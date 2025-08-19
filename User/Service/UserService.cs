using ppe_detection_api.dto;
using ppe_detection_api.User.Repository;

namespace ppe_detection_api.User.Service;

public class UserService
{
    private readonly UserRepository _userRepository;
    
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public void Register(dto.User user)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Password = hashedPassword;
        _userRepository.Register(user);    
    }
    
    public dto.User GetUserById(Guid userId)
    {
        return _userRepository.GetUserById(userId);
    }
}