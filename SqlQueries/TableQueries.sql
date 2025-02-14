Create Table [User](
Id int Primary Key Identity(1,1),
UserName nvarchar(50) Not Null,
UserSurname nvarchar(50) Not Null,
Email nvarchar(255) Not Null,
[Password] nvarchar(255) Not Null,
RegisterDate date Not Null,
Notifications bit null
);

Create Table UserIncome(
Id int Primary Key,
Currency nvarchar(10) Not Null,
IncomeType nvarchar(20) Not Null,
Constraint Fk_UserIncome_UserMainId Foreign Key (Id) References [User](Id) On Delete Cascade
);

Create Table IncomeDetails(
Id int Primary Key,
IncomeAmount decimal(18,2) Not Null,
IncomeDate date Not Null,
Constraint Fk_IncomeDetails_UserIncomeId Foreign Key (Id) References UserIncome(Id) On Delete Cascade
);			

Create Table UserExpenses(
Id int Primary key,
Currency nvarchar(10) Not Null,
ExpenseType nvarchar(50) Not Null,
Constraint Fk_UserExpenses_UserMainId Foreign Key (Id) References [User](Id) On Delete Cascade
);

Create Table ExpenseDetails(
Id int Primary Key,
ExpenseAmount decimal(18,2) Not Null,
ExpenseDate date Not Null,
Constraint Fk_ExpenseDetails_UserExpenseId Foreign Key (Id) References UserExpenses(Id) On Delete Cascade
);

Create Table ExpenseLimits(
Id int Primary Key,
UserId int Not Null,
LimitPeriod nvarchar(10) Not Null,
LimitExpenseType nvarchar(50),
LimitAmount decimal(18,2) Not Null,
Constraint FK_ExpenseLimits_UserId Foreign Key (UserId) References [User](Id) On Delete Cascade
);

Create Unique Index IDX_UserMain_Email On [User](Email);
Create Index IDX_UserIncome_UserId On UserIncome(Id);