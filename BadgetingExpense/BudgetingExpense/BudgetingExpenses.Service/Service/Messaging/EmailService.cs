using System.Net;
using System.Net.Mail;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Models.MainModels;
using Microsoft.Extensions.Logging;

namespace BudgetingExpenses.Service.Service.Messaging;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }
    public async Task SendEmailAsync(EmailModel emailModel)
    {
        try
        {
            var Email = new MailMessage
            {
                From = new MailAddress("budgetingandexpensetracker12@gmail.com", "Bank"),
                Subject = emailModel.Subject,
                Body = emailModel.Message,
                IsBodyHtml = true
            };
            Email.To.Add(emailModel.Email);
            using var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("budgetingandexpensetracker12@gmail.com", "mnyteecfzwylmqoo");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(Email);
            _logger.LogInformation("Email Sent To {email}", emailModel.Email);
        }
        catch(Exception ex)
        {
            _logger.LogError("Exception ex:{ex}", ex.Message);
        }
    }
}
