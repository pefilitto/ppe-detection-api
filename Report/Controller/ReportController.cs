using Microsoft.AspNetCore.Mvc;
using ppe_detection_api.Report.Service;
using ppe_detection_api.S3;
using ppe_detection_api.Email;

namespace ppe_detection_api.Report.Controller;

[ApiController]
[Route("api/report")]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;
    private readonly S3Service _s3Service;
    private readonly EmailService _emailService;
    
    public ReportController(ReportService reportService, S3Service s3Service, EmailService emailService)
    {
        _reportService = reportService;
        _s3Service = s3Service;
        _emailService = emailService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterReport(Dto.Report report)
    {
        try
        {
            Guid id = _reportService.RegisterReport(report);
            string imageUrl = null;
            
            if (report.Image != null && report.Image.Length > 0)
            {
                string fileName = $"{id}.{Path.GetExtension(report.Image.FileName).TrimStart('.')}";
                imageUrl = await _s3Service.UploadImageAsync(report.Image, fileName);
                
                await _emailService.SendViolationReportAsync(imageUrl, id.ToString());
            }

            return Ok(new {
                Message = "Report registered successfully!",
                ReportId = id,
                ImageUrl = imageUrl
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error: " + ex.Message });
        }
    }
    
    [HttpGet]
    public IActionResult GetReports()
    {
        try
        {
            return Ok(new { Reports = _reportService.GetReports() });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error: " + ex.Message });
        }
    }
    
    [HttpGet("get-report-image/{reportId}")]
    public IActionResult GetReportImage(Guid reportId)
    {
        try
        {
            string fileName = $"{reportId}.jpg";
            string imageUrl = _s3Service.GetImageUrl($"reports/{fileName}");

            return Ok(new { ImageUrl = imageUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error: " + ex.Message });
        }
    }
}