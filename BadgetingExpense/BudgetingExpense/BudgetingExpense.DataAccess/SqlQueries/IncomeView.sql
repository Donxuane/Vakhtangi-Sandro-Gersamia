
Create View IncomeCategories As
Select 
i.UserId As UserId,
i.Date As IncomeDate,
i.Amount, 
i.Currency,
Coalesce(NullIf(c.Name,''),'General') As CategoryName
From Incomes i
Left Join Categories c On i.CategoryId = c.Id where c.Type = 1;