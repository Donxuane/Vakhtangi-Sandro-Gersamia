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
    MAX(ex.Currency) AS Currency  
FROM Limits l
LEFT JOIN Expenses ex 
    ON l.CategoryId = ex.CategoryId 
    AND l.UserId = ex.UserId 
    AND ex.Date >= l.DateAdded  
    AND ex.Date < DATEADD(MONTH, l.PeriodCategory, l.DateAdded)  
GROUP BY 
   l.Id, l.UserId, l.Amount, l.PeriodCategory, l.DateAdded, l.CategoryId;
