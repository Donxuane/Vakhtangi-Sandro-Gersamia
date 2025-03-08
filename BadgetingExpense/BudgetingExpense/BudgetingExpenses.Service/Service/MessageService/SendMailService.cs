using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IMessageService;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpenses.Service.Service.MessageService
{
    public class SendMailService : IEmailService
    {
        public async Task SendEmail(EmailModel emailModel)
        {
            var Email = new MailMessage
            {
                From = new MailAddress("budgetingandexpensetracker12@gmail.com", "Admin"),
                Subject = emailModel.Subject,
                Body = emailModel.Message,
                IsBodyHtml = false

            };
            Email.To.Add(emailModel.Email);
            using var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("budgetingandexpensetracker12@gmail.com", "mnyteecfzwylmqoo");
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(Email);

        }
    }
}
