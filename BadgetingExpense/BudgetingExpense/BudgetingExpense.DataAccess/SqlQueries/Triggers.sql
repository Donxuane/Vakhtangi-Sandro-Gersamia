CREATE TRIGGER ValidateIncomeCategoryType
ON Incomes
AFTER INSERT
AS
BEGIN
DECLARE @CategoryType INT;
DECLARE @CategoryId INT;
SELECT @CategoryId = CategoryId FROM INSERTED;
SELECT @CategoryType = [Type] FROM Categories WHERE Id = @CategoryId;
IF @CategoryType!=1
BEGIN
ROLLBACK TRANSACTION;
END;
END;
Go
CREATE TRIGGER ValidateExpenseCategoryType
ON Expenses
AFTER INSERT
AS
BEGIN
DECLARE @CategoryType INT;
DECLARE @CategoryId INT;
SELECT @CategoryId = CategoryId FROM INSERTED;
SELECT @CategoryType = [Type] FROM Categories WHERE Id = @CategoryId;
IF @CategoryType!=0
BEGIN
ROLLBACK TRANSACTION;
END;
END;
