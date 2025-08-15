using Microsoft.AspNetCore.Mvc;
using ppe_detection_api.dto;

namespace ppe_detection_api.controller;

[ApiController]
[Route("api/ppe")]
public class PPEController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(PPE ppe)
    {
        try
        {
            if (string.IsNullOrEmpty(ppe.Name))
            {
                return BadRequest(new { Message = "Nome do EPI é obrigatório" });
            }

            return Ok(new { Message = "EPI registrado com sucesso", Name = ppe.Name });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor: " + ex.Message });
        }
    }
    
    [HttpPut("update")]
    public IActionResult Update(PPE ppe)
    {
        try
        {
            if (string.IsNullOrEmpty(ppe.Name))
            {
                return BadRequest(new { Message = "Nome do EPI é obrigatório" });
            }
            
            return Ok(new { Message = "EPI atualizado com sucesso", Name = ppe.Name });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor: " + ex.Message });
        }
    }
}