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
Go
CREATE PROCEDURE SavingsAnalyticsProcedure
    @UserId NVARCHAR(450),
    @Period INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @AdjustedPeriod INT = ISNULL(@Period, 1);
    WITH IncomeData AS (
        SELECT 
            Currency, 
            AVG(Amount) AS IncomeAvg
        FROM IncomeCategories 
        WHERE UserId = @UserId 
            AND IncomeDate >= DATEADD(Month, -@AdjustedPeriod, GETDATE())
        GROUP BY Currency
    ),
    ExpenseData AS (
        SELECT 
            Currency, 
            AVG(Amount) AS ExpenseAvg
        FROM ExpenseCategories 
        WHERE UserId = @UserId 
            AND [Date] >= DATEADD(Month, -@AdjustedPeriod, GETDATE())
        GROUP BY Currency
    )
    SELECT 
        COALESCE(i.Currency, e.Currency) AS Currency,
        ISNULL(i.IncomeAvg, 0) AS AverageIncome,
        ISNULL(e.ExpenseAvg, 0) AS AverageExpense,
        ISNULL(i.IncomeAvg, 0) - ISNULL(e.ExpenseAvg, 0) AS Savings,
		CASE
		    WHEN i.IncomeAvg IS NULL OR i.IncomeAvg = 0 THEN NULL
		    ELSE (ISNULL(i.IncomeAvg, 0) - ISNULL(e.ExpenseAvg, 0))*100/ISNULL(i.IncomeAvg, 1) 
		END As [Percentage]
    FROM IncomeData i
    FULL JOIN ExpenseData e ON i.Currency = e.Currency;
END;
 
