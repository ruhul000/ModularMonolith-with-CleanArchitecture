PRINT '--------START--------'

USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'ControlHubDB')
BEGIN
	PRINT 'DB: Create ControlHubDB database'
	CREATE DATABASE ControlHubDB;
END
GO

USE ControlHubDB;
GO
-- Users
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
BEGIN
	PRINT 'TABLE: Drop Users table'
    DROP TABLE dbo.Users;
END

Print 'TABLE: Create Users table'
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    UserName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(MAX) NOT NULL,
    Salt NVARCHAR(MAX) NOT NULL,
    VerificationToken NVARCHAR(MAX),
    PasswordResetToken NVARCHAR(MAX),
    PasswordResetTokenExpiry DATETIME2(3),
    IsVerified BIT DEFAULT 0,
    IsActive BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0,
    CreatedBy INT,
    CreatedAt DATETIME2(3),
    UpdatedBy INT,
    UpdatedAt DATETIME2(3),
    DeletedBy INT,
    DeletedAt DATETIME2(3),
    CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (UserId ASC)
);

-- RefreshToken
IF OBJECT_ID('dbo.RefreshToken', 'U') IS NOT NULL
BEGIN
	PRINT 'TABLE: Drop RefreshToken table'
    DROP TABLE dbo.RefreshToken;
END

PRINT 'TABLE: Create RefreshToken table'
CREATE TABLE RefreshToken (
    UserName NVARCHAR(100) NOT NULL,
    TokenID NVARCHAR(MAX) NOT NULL,
    RefreshToken NVARCHAR(MAX) NOT NULL,
    IsActive BIT NOT NULL
);

PRINT  '--------END--------'