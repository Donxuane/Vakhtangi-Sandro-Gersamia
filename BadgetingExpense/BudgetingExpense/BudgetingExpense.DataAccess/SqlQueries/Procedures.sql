CREATE PROCEDURE UpdateProcedure
    @TableName NVARCHAR(MAX),
    @Currency INT = NULL,
    @Amount DECIMAL(18,2) = NULL,
    @CategoryId INT = NULL,
    @Date DATETIME = NULL,
    @PeriodCategory INT = NULL,
    @Id INT,
	@UserId NVARCHAR(450)
AS
BEGIN
    DECLARE @query NVARCHAR(MAX);

    IF @TableName = 'Incomes' OR @TableName = 'Expenses'
    BEGIN
        SET @query = 'UPDATE ' + QUOTENAME(@TableName) + ' SET Currency = COALESCE(@Currency, Currency), Amount = COALESCE(@Amount, Amount),
		CategoryId = COALESCE(@CategoryId, CategoryId), Date = COALESCE(@Date, Date) WHERE Id = @Id AND UserId = @UserId';
    END;
	IF @TableName = 'Limits'
	BEGIN
	    SET @query ='UPDATE ' + QUOTENAME(@TableName) + ' SET CategoryId = COALESCE(@CategoryId, CategoryId), Amount = COALESCE(@Amount,Amount),
		PeriodCategory = COALESCE(@PeriodCategory,PeriodCategory), DateAdded = COALESCE(@Date,DateAdded) WHERE Id = @Id AND UserId = @UserId';
    END;
	EXEC sp_executesql @query, 
             N'@Currency INT, @Amount DECIMAL(18,2), @CategoryId INT, @Date DATETIME, @PeriodCategory INT, @Id INT,@UserId NVARCHAR(450)',
             @Currency, @Amount, @CategoryId, @Date, @PeriodCategory, @Id, @UserId;
END;