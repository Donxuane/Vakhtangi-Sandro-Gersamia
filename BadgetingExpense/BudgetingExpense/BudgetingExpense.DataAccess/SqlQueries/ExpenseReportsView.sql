Create View ExpenseCategories As
Select
ex.UserId,
ex.Amount,
ex.Currency,
ex.Date,
Coalesce(NullIf(c.Name,''),'General') As CategoryName
From Expenses ex
Left Join Categories c on ex.CategoryId = c.Id where c.Type = 0;

