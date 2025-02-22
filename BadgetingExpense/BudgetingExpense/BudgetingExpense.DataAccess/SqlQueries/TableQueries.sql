CREATE TABLE Categories
(
    Id int primary key identity,
    [Name] nvarchar(max),
    [Type] int
)

CREATE TABLE Incomes
(
    Id int Primary Key Identity(1,1),
    Currency int,
    Amount decimal(18,2),
    CategoryId int foreign key (CategoryId) references Categories(Id)On Delete Cascade,
    [Date] datetime,
    UserId nvarchar(450) foreign key (userId) references AspNetUsers(Id)On Delete Cascade
)

CREATE TABLE Expenses
(
    Id int Primary Key Identity(1,1),
    Currency int,
    Amount decimal(18,2),
    CategoryId int foreign key (CategoryId) references Categories(Id)On Delete Cascade,
    [Date] datetime2,
    UserId nvarchar(450) foreign key (userId) references AspNetUsers(Id)On Delete Cascade
)

Create table Limits
(
    Id int Primary Key Identity(1,1),
    UserId nvarchar(450) foreign key (userId) references AspNetUsers(Id)On Delete Cascade,
    CategoryId int NULL foreign key (CategoryId) references Categories(Id)On Delete Cascade,
    Amount decimal(18,2),
    PeriodCategory int,
    DateAdded datetime DEFAULT GETDATE()
)