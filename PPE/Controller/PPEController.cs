using Microsoft.AspNetCore.Mvc;
using ppe_detection_api.dto;
using ppe_detection_api.PPE.Service;

namespace ppe_detection_api.controller;

[ApiController]
[Route("api/ppe")]
public class PPEController : ControllerBase
{
    private readonly PPEService _ppeService;
    
    public PPEController(PPEService ppeService)
    {
        _ppeService = ppeService;
    }
    
    
    [HttpPost("register")]
    public IActionResult Register(dto.PPE ppe)
    {
        try
        {
            if (string.IsNullOrEmpty(ppe.Name))
            {
                return BadRequest(new { Message = "Nome do EPI é obrigatório" });
            }
            
            _ppeService.Register(ppe);
            
            return Ok(new { Message = "EPI registrado com sucesso", Name = ppe.Name });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor: " + ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var ppes = _ppeService.GetAll();
            return Ok(ppes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor: " + ex.Message });
        }
    }
    
    [HttpGet("ppesbyrole/{roleId}")]
    public IActionResult GetPPEsByRoleId(Guid roleId)
    {
        try
        {
            PPEsByRole ppe = _ppeService.GetPPEsByRoleId(roleId);
            
            if (ppe == new PPEsByRole())
            {
                return NotFound(new { Message = "Nenhum EPI encontrado para a profissão especificada." });
            }
            
            return Ok(ppe);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor: " + ex.Message });
        }
    }
}