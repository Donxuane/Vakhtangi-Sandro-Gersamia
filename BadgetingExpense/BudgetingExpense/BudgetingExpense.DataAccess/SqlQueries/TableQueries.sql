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