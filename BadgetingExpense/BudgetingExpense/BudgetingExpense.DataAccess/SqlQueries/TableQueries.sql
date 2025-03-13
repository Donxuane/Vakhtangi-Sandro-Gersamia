CREATE TABLE Categories
(
    Id int primary key identity(1,1),
    [Name] nvarchar(max),
    [Type] int
)

CREATE TABLE Incomes
(
    Id int Primary Key Identity(1,1),
    Currency int,
    Amount decimal(18,2),
    CategoryId int,
    [Date] datetime Default GetDate(),
    UserId nvarchar(450),
	Constraint FK_IncomesCategoryId_Categories foreign key (CategoryId) references Categories(Id)On Delete Cascade,
	Constraint FK_IncomesUserId_AspNetUsers foreign key (userId) references AspNetUsers(Id)On Delete Cascade
)

CREATE TABLE Expenses
(
    Id int Primary Key Identity(1,1),
    Currency int,
    Amount decimal(18,2),
    CategoryId int Null,
    [Date] datetime Default GetDate(),
    UserId nvarchar(450),
	Constraint FK_ExpensesCategoryId_Categories foreign key (CategoryId) references Categories(Id)On Delete Cascade,
	Constraint FK_ExpensesUserId_AspNetUser  foreign key (userId) references AspNetUsers(Id)On Delete Cascade
)

Create table Limits
(
    Id int Primary Key Identity(1,1),
    UserId nvarchar(450),
    CategoryId int NULL,
    Amount decimal(18,2),
    PeriodCategory int,
    DateAdded datetime DEFAULT GetDate(),
	Constraint FK_LimitsUserId_AspNetUser foreign key (userId) references AspNetUsers(Id)On Delete Cascade,
	Constraint FK_LimitsCategoryId_Categories foreign key (CategoryId) references Categories(Id)On Delete Cascade
)
Go
Create View IncomeCategories As
Select 
i.UserId As UserId,
i.Date As IncomeDate,
i.Amount, 
i.Currency,
Coalesce(NullIf(c.Name, ''),'General') As CategoryName
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
    COALESCE(SUM(ex.Amount), 0) AS TotalExpenses, 
    COUNT(ex.Id) AS ExpenseCount,  
    MAX(ex.Currency) AS Currency  
FROM Limits l
LEFT JOIN Expenses ex 
    ON l.CategoryId = ex.CategoryId 
    AND l.UserId = ex.UserId 
    AND ex.Date >= l.DateAdded  
    AND ex.Date < DATEADD(MONTH, l.PeriodCategory, l.DateAdded)  
GROUP BY 
    l.UserId, l.Amount, l.PeriodCategory, l.DateAdded, l.CategoryId;
Go