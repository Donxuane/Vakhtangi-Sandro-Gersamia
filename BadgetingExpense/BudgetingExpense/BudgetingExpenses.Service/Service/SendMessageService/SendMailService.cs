using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IEmailService;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpenses.Service.Service.SendMessageService
{
    public class SendMailService : IEmailService
    {
        public async Task SendEmail(EmailModel emailModel)
        {
            var Email = new MailMessage
            {
                From = new MailAddress("vakhtangichitashvili55@gmail.com","Admin"),
                Subject = emailModel.Subject,
                Body = emailModel.Message,
                IsBodyHtml = false

            };
            Email.To.Add(emailModel.Email);
            using var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("vakhtangichitashvili55@gmail.com", "cevowsvrortigggg");
            smtp.EnableSsl =true;

            await smtp.SendMailAsync(Email);

        }
    }
}
