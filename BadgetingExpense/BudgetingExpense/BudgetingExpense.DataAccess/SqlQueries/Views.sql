CREATE VIEW IncomeCategories AS
SELECT 
i.UserId AS UserId,
i.Date AS IncomeDate,
i.Amount, 
i.Currency,
Coalesce(NullIf(c.Name,''),'General') AS CategoryName
FROM Incomes i
LEFT JOIN Categories c ON i.CategoryId = c.Id WHERE c.Type = 1;
Go
CREATE VIEW ExpenseCategories AS
SELECT
ex.UserId,
ex.Amount,
ex.Currency,
ex.Date,
Coalesce(NullIf(c.Name,''),'General') AS CategoryName
FROM Expenses ex
LEFT JOIN Categories c ON ex.CategoryId = c.Id WHERE c.Type = 0;
Go
CREATE VIEW BudgetPlaning AS
 SELECT 
    l.Id,
    l.UserId,
    l.Amount AS LimitAmount,
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
