Create Table UserIncome(
Id int Primary Key Identity (1,1),
Currency nvarchar(10) Not Null,
IncomeType nvarchar(20) Not Null,
UserId nvarchar(450) Not Null 
);

Create Table IncomeDetails(
Id int Primary Key Identity(1,1),
IncomeAmount decimal(18,2) Not Null,
IncomeDate date Not Null,
UserIncomeId int Not Null
Constraint Fk_IncomeDetails_UserIncomeId Foreign Key (UserIncomeId) References UserIncome(Id) On Delete Cascade
);			

Create Table UserExpense(
Id int Primary key Identity(1,1),
Currency nvarchar(10) Not Null,
ExpenseType nvarchar(50) Not Null,
UserId nvarchar(450) Not Null
);

Create Table ExpenseDetails(
Id int Primary Key Identity(1,1),
ExpenseAmount decimal(18,2) Not Null,
ExpenseDate date Not Null,
UserExpenseId int Not Null
Constraint Fk_ExpenseDetails_UserExpenseId Foreign Key (UserExpenseId) References UserExpense(Id) On Delete Cascade
);

Create Table ExpenseLimits(
Id int Primary Key Identity(1,1),
UserId nvarchar(450) Not Null,
LimitPeriod nvarchar(10) Not Null,
LimitExpenseType nvarchar(50),
LimitAmount decimal(18,2) Not Null,
);

Alter Table UserExpense
Add Constraint Fk_UserExpenses_UserMainId Foreign Key (UserId) References AspNetUsers(Id) on Delete Cascade;

Alter Table UserIncome
Add Constraint Fk_UserIncome_UserMainId Foreign Key (UserId) References AspNetUsers(Id) on Delete Cascade;

Alter Table ExpenseLimits
Add Constraint FK_ExpenseLimits_UserId Foreign Key (UserId) References AspNetUsers(Id) on Delete Cascade;
