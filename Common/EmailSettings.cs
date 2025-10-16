namespace ppe_detection_api.Common;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public List<string> Recipients { get; set; } = new List<string>();
}
