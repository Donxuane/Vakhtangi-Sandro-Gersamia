Create View IncomeCategories As
Select 
i.UserId As UserId,
i.Date As IncomeDate,
i.Amount, 
i.Currency,
Coalesce(NullIf(c.Name,''),'General') As CategoryName
From Incomes i
Left Join Categories c On i.CategoryId = c.Id where c.Type = 1;
Go
Create View ExpenseCategories As
Select
ex.UserId,
ex.Amount,
ex.Currency,
ex.Date,
Coalesce(NullIf(c.Name,''),'General') As CategoryName
From Expenses ex
Left Join Categories c on ex.CategoryId = c.Id where c.Type = 0;
Go
CREATE VIEW BudgetPlaning AS
 SELECT 
    l.Id,
    l.UserId,
    l.Amount AS limitAmount,
    l.PeriodCategory AS LimitPeriod,
    l.DateAdded,
    l.CategoryId,
    COALESCE(SUM(ex.Amount),0) AS TotalExpenses, 
    COUNT(ex.Id) AS ExpenseCount,  
    l.Currency AS Currency  
FROM Limits l
LEFT JOIN Expenses ex 
    ON l.CategoryId = ex.CategoryId 
    AND l.UserId = ex.UserId 
    AND ex.Date >= l.DateAdded  
    AND ex.Date < DATEADD(MONTH, l.PeriodCategory, l.DateAdded)  
	AND ex.Currency = l.Currency
GROUP BY 
   l.Id, l.UserId, l.Amount, l.PeriodCategory, l.DateAdded, l.CategoryId,l.Currency;
