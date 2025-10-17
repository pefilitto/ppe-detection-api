using MySql.Data.MySqlClient;
using ppe_detection_api.Context;

namespace ppe_detection_api.Report.Repository;

public class ReportRepository
{
    private readonly DbContext _context;
    
    public ReportRepository(DbContext context)
    {
        _context = context;
    }
    
    public Guid RegisterReport(Dto.Report report)
    {
        if (report.Id == Guid.Empty)
            report.Id = Guid.NewGuid();

        var connection = _context.CreateConnection();
        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO report (id, date_begin, date_end, description) VALUES (@id, @date_begin, @date_end, @description)";
            command.Parameters.Add(new MySqlParameter("@id", report.Id));
            command.Parameters.Add(new MySqlParameter("@date_begin", report.DateBegin));
            command.Parameters.Add(new MySqlParameter("@date_end", report.DateEnd));
            command.Parameters.Add(new MySqlParameter("@description", report.Description));
            command.ExecuteNonQuery();
        }
        connection.Close();
        
        return report.Id;
    }
    
    public List<Dto.Report> GetReports()
    {
        
        var connection = _context.CreateConnection();
        var reports = new List<Dto.Report>();
        
        connection.Open();
        
        using(var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT id, date_begin, date_end, description FROM report";
            
            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    var report = new Dto.Report
                    {
                        Id = reader.GetGuid(0),
                        DateBegin = reader.GetDateTime(1),
                        DateEnd = reader.GetDateTime(2),
                        Description = reader.GetString(3)
                    };
                    
                    reports.Add(report);
                }
            }
        }
        
        connection.Close();
        
        return reports;
    }
}