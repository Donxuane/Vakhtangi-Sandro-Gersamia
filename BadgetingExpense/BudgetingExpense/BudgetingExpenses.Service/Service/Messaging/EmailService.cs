using System.Net;
using System.Net.Mail;
using BudgetingExpense.Domain.Contracts.IServices.IMessaging;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpenses.Service.Service.Messaging;

public class EmailService : IEmailService
{
    public async Task SendEmail(EmailModel emailModel)
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
    }
}
