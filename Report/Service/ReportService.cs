using ppe_detection_api.Report.Repository;

namespace ppe_detection_api.Report.Service;

public class ReportService
{
    private readonly ReportRepository _reportRepository;

    public ReportService(ReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }
    
    public Guid RegisterReport(Dto.Report report)
    {
        return _reportRepository.RegisterReport(report);
    }
    
    public List<Dto.Report> GetReports()
    {
        return _reportRepository.GetReports();
    }
}