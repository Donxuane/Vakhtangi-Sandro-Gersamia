using BudgetingExpense.Domain.Contracts.IServiceContracts.IFinanceManageServices;
using BudgetingExpense.Domain.Contracts.IServiceContracts.IMessageService;
using BudgetingExpense.Domain.Contracts.IUnitOfWork;
using BudgetingExpense.Domain.Models.MainModels;

namespace BudgetingExpenses.Service.Service.ManageFinanceServices;

public class BudgetPlanningService : IBudgetPlanningService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    

    public BudgetPlanningService(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }
    public async Task SendMessageAsync(string UserId)
    {



    }

    public async Task AllExpenses(string UserId, int CategoryId)
    {
        var User = await _unitOfWork.BudgetPlanning.GetEmail(UserId);
        var AllExpenses = await _unitOfWork.BudgetPlanning.GetAll(UserId, CategoryId);
        var result = AllExpenses.Sum(x => x.ExpenseAmount);
        var Limit = AllExpenses.Select(x => x.limitAmount).FirstOrDefault();
        if (Limit < result)
        {
            _emailService.SendEmail(new EmailModel()
            { Email = User, Message = "limit exceeded", Subject = "limit exceeded" });
        }
        else
        {
            _emailService.SendEmail(new EmailModel()
            { Email = User, Message = "ndfkasdfnkasd", Subject = "dnkfskdnc" });
        }
    }
}
