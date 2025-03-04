using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetingExpense.Domain.Contracts.IServiceContracts.IEmailService
{
    public interface IEmailService
    {
        public Task SendEmail(string email,);

    }
}
