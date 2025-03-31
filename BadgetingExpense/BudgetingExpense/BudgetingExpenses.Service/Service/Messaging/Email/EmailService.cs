using System.Net;
using System.Net.Mail;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Messaging.Email;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;
    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration; 
    }
    public async Task SendEmailAsync(EmailModel emailModel)
    {
        try
        {
            var settings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
            if (emailModel != null && settings!=null)
            {
                var Email = new MailMessage
                {
                    From = new MailAddress(settings.CompanyEmail, settings.CompanyName),
                    Subject = emailModel.Subject,
                    Body = emailModel.Message,
                    IsBodyHtml = true
                };
                Email.To.Add(emailModel.Email);
                using var smtp = new SmtpClient(settings.Host, settings.Port);
                smtp.Credentials = new NetworkCredential(settings.CompanyEmail, settings.AppPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(Email);
                _logger.LogInformation("Email Sent To {email}", emailModel.Email);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
            throw;
        }
    }
}
