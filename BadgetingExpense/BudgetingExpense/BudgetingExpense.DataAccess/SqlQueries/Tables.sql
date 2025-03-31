CREATE TABLE Categories
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    [Name] nvarchar(255) NOT NULL,
    [Type] INT,
    CONSTRAINT Check_TypeOfCategory CHECK([Type] IN (0,1)),
    CONSTRAINT Unique_CategoryName UNIQUE([Name])
);
CREATE TABLE Incomes
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Currency INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    CategoryId INT NULL,
    [Date] DATETIME NOT NULL DEFAULT GETDATE(),
    UserId NVARCHAR(450) NOT NULL,
	CONSTRAINT FK_IncomesCategoryId_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)ON DELETE SET NULL,
	CONSTRAINT FK_IncomesUserId_AspNetUsers FOREIGN KEY (userId) REFERENCES AspNetUsers(Id)ON DELETE CASCADE,
    CONSTRAINT Check_IncomeAmount CHECK(Amount > 0)
);
CREATE TABLE Expenses
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Currency INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    CategoryId INT NULL,
    [Date] DATETIME NOT NULL DEFAULT GETDATE(),
    UserId nvarchar(450) NOT NULL,
    CONSTRAINT Check_ExpenseAmount CHECK(Amount >= 0),
	CONSTRAINT FK_ExpensesCategoryId_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)ON DELETE SET NULL,
	CONSTRAINT FK_ExpensesUserId_AspNetUser  FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)ON DELETE CASCADE
);
CREATE TABLE Limits
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL,
    CategoryId INT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PeriodCategory INT NOT NULL,
    DateAdded DATETIME DEFAULT GETDATE(),
    Currency INT NULL,
	CONSTRAINT FK_LimitsUserId_AspNetUser FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)ON DELETE CASCADE,
	CONSTRAINT FK_LimitsCategoryId_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)ON DELETE SET NULL,
    CONSTRAINT Check_LimitsAmount CHECK(Amount > 0),
    CONSTRAINT Check_LimitsPeriod CHECK(PeriodCategory > 0)
);