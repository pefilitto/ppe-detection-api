using ppe_detection_api.Role.Repository;

namespace ppe_detection_api.Role.Service;

public class RoleService
{
    private readonly RoleRepository _roleRepository;
    
    public RoleService(RoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public void Register(Dto.Role role)
    {
        _roleRepository.Register(role);
    }
}