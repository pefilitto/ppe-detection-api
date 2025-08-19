using ppe_detection_api.dto;
using ppe_detection_api.PPE.Repository;
using ppe_detection_api.Role.Repository;

namespace ppe_detection_api.PPE.Service;

public class PPEService
{
    private readonly PPERepository _repository;
    private readonly RoleRepository _roleRepository;
    
    public PPEService(PPERepository repository, RoleRepository roleRepository)
    {
        _repository = repository;
        _roleRepository = roleRepository;
    }
    
    public void Register(dto.PPE ppe)
    {
        try
        {
            _repository.Register(ppe);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public List<dto.PPE> GetAll()
    {
        try
        {
            return _repository.GetAll();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public PPEsByRole GetPPEsByRoleId(Guid roleId)
    {
        PPEsByRole result = new PPEsByRole();
        
        try
        {
            Role.Dto.Role role = _roleRepository.GetRoleById(roleId);

            if (role != new Role.Dto.Role())
            {
                result.Role = role;
                result.RequiredPPEs = _repository.GetPPEsByRoleId(roleId);   
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return result;
    }
}