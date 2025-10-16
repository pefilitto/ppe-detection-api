using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using ppe_detection_api.Common;

namespace ppe_detection_api.Email;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendViolationReportAsync(string imageUrl, string reportId, string description = null)
    {
        try
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = $"Infração de EPI Detectada - Relatório {reportId}",
                Body = CreateEmailBody(reportId, imageUrl, description),
                IsBodyHtml = true
            };

            foreach (var recipient in _emailSettings.Recipients)
            {
                mailMessage.To.Add(recipient);
            }

            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao enviar email: {ex.Message}", ex);
        }
    }

    private string CreateEmailBody(string reportId, string imageUrl, string description)
    {
        var defaultDescription = description ?? "Foi detectada uma infração de uso de Equipamento de Proteção Individual (EPI).";
        
        return $@"
        <html>
        <body style='font-family: Arial, sans-serif;'>
            <h2 style='color: #d32f2f;'>⚠️ Infração de EPI Detectada</h2>
            <p><strong>ID do Relatório:</strong> {reportId}</p>
            <p><strong>Data/Hora:</strong> {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
            <p><strong>Descrição:</strong> {defaultDescription}</p>
            
            <h3>Imagem da Infração:</h3>
            <img src='{imageUrl}' alt='Imagem da infração' style='max-width: 600px; border: 1px solid #ccc; border-radius: 5px;' />
            
            <hr>
            <p style='color: #666; font-size: 12px;'>
                Este é um email automático do Sistema de Detecção de EPI. 
                Por favor, não responda este email.
            </p>
        </body>
        </html>";
    }
}
