using Microsoft.AspNetCore.Mvc;
using ppe_detection_api.Role.Service;

namespace ppe_detection_api.Role.Controller;

[ApiController]
[Route("api/role")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;
    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("register")]
    public IActionResult Register(Dto.Role role)
    {
        try
        {
            if (string.IsNullOrEmpty(role.Name))
            {
                return BadRequest(new { Message = "Nome da profissão é obrigatório" });
            }
            
            if(role.RequiredPPEs.Count == 0)
            {
                return BadRequest(new { Message = "Pelo menos um EPI é necessário para a profissão" });
            }

            _roleService.Register(role);
            
            return Ok(new { Message = "Profissão registrada com sucesso", Name = role.Name });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor: " + ex.Message });
        }
    }
}