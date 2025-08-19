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
        try
        {
            _roleRepository.BeginTransaction();
               
            Guid roleId = _roleRepository.Register(role);
            
            foreach(var ppe in role.RequiredPPEs)
            {
                _roleRepository.RegisterRequiredPPEs(roleId, ppe.PPE.Id);
            }
            
            _roleRepository.Commit();
        }
        catch (Exception ex)
        {
            _roleRepository.RollBack();
            throw ex;
        }
    }
}