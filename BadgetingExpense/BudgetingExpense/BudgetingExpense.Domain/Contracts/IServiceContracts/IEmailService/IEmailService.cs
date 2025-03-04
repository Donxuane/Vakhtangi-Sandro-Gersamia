using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IEmailService
{
    public interface IEmailService
    {
        public Task SendEmail(EmailModel emailModel);

    }
}
