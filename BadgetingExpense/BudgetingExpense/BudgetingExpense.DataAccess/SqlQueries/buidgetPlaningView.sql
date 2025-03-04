
      CREATE VIEW BudgetPlaning 
  AS
  SELECT 
  l.UserId,l.Amount as limitAmount,l.PeriodCategory as LimitPeriod,l.DateAdded,ex.Amount as ExpeneseAmount,ex.Currency,ex.Date,l.CategoryId
  FROM Limits l
  LEFT JOIN Expenses ex ON l.CategoryId = ex.CategoryId 
WHERE l.UserId = ex.UserId;