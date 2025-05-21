CREATE DATABASE BankDB;
GO

USE BankDB;
GO
-- Creating Account table
CREATE TABLE Accounts (
    AccountNumber INT PRIMARY KEY,
    AccountHolderName NVARCHAR(255) NOT NULL,
    BankBalance DECIMAL(18, 2) NOT NULL,
    AccountType NVARCHAR(50) NOT NULL
);
GO






-- Creating stroed procedure for creating account
DROP PROCEDURE IF EXISTS CreateAccount;
GO  -- Start a new batch
CREATE PROCEDURE CreateAccount
    @AccountNumber INT,
    @AccountHolderName NVARCHAR(255),
    @BankBalance DECIMAL(18,2),
    @AccountType NVARCHAR(50)
AS
BEGIN

    INSERT INTO Accounts (AccountNumber, AccountHolderName, BankBalance, AccountType)
    VALUES (@AccountNumber, @AccountHolderName, @BankBalance, @AccountType);
END;
GO  -- End of batch



DROP PROCEDURE IF EXISTS GetAccountDetails;
GO
CREATE PROCEDURE GetAccountDetails
    @AccountNumber INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AccountNumber, AccountHolderName, BankBalance, AccountType
    FROM Accounts
    WHERE AccountNumber = @AccountNumber;
END;
GO

DROP PROCEDURE IF EXISTS GetAccountBalance;
GO
CREATE PROCEDURE GetAccountBalance
    @AccountNumber INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT BankBalance
    FROM Accounts
    WHERE AccountNumber = @AccountNumber;
END;
GO


DROP PROCEDURE IF EXISTS UpdateAccountBalance;
GO
CREATE PROCEDURE UpdateAccountBalance
    @AccountNumber INT,
    @NewBalance DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Accounts
    SET BankBalance = @NewBalance
    WHERE AccountNumber = @AccountNumber;
END;
GO

