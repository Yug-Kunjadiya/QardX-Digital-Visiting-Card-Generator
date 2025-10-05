-- ===============================================
-- QardXDB Complete Database Creation Script
-- Creates fresh QardXDB database with all Phase 1 & Phase 2 features
-- ===============================================

-- Drop database if exists (CAUTION: This will delete all existing data!)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'QardXDB')
BEGIN
    ALTER DATABASE QardXDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QardXDB;
END
GO

-- Create new database
CREATE DATABASE QardXDB;
GO

USE QardXDB;
GO

-- Drop existing objects to avoid conflicts
IF OBJECT_ID('UserAnalyticsView', 'V') IS NOT NULL
    DROP VIEW UserAnalyticsView;
IF OBJECT_ID('CardAnalyticsView', 'V') IS NOT NULL
    DROP VIEW CardAnalyticsView;
IF OBJECT_ID('BatchJobs', 'U') IS NOT NULL
    DROP TABLE BatchJobs;
IF OBJECT_ID('CardViews', 'U') IS NOT NULL
    DROP TABLE CardViews;
IF OBJECT_ID('CustomTemplates', 'U') IS NOT NULL
    DROP TABLE CustomTemplates;
IF OBJECT_ID('EmailTemplates', 'U') IS NOT NULL
    DROP TABLE EmailTemplates;
IF OBJECT_ID('UserRoles', 'U') IS NOT NULL
    DROP TABLE UserRoles;
IF OBJECT_ID('ContactForms', 'U') IS NOT NULL
    DROP TABLE ContactForms;
IF OBJECT_ID('VisitingCards', 'U') IS NOT NULL
    DROP TABLE VisitingCards;
IF OBJECT_ID('Templates', 'U') IS NOT NULL
    DROP TABLE Templates;
IF OBJECT_ID('Users', 'U') IS NOT NULL
    DROP TABLE Users;

-- ===============================================
-- CREATE TABLES
-- ===============================================

-- 1. Users Table
CREATE TABLE [Users] (
    [UserId] INT IDENTITY(1,1) PRIMARY KEY,
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- 2. Templates Table
CREATE TABLE [Templates] (
    [TemplateId] INT IDENTITY(1,1) PRIMARY KEY,
    [TemplateName] NVARCHAR(100) NOT NULL,
    [FilePath] NVARCHAR(500) NOT NULL
);

-- 3. VisitingCards Table (Complete with all Phase 1 & Phase 2 fields)
CREATE TABLE [VisitingCards] (
    [CardId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [TemplateId] INT NOT NULL,
    
    -- Basic Information
    [FirstName] NVARCHAR(100) NULL,
    [LastName] NVARCHAR(100) NULL,
    [Company] NVARCHAR(200) NULL,
    [JobTitle] NVARCHAR(100) NULL,
    [Phone] NVARCHAR(20) NULL,
    [Address] NVARCHAR(500) NULL,
    
    -- Social Media Links
    [Instagram] NVARCHAR(200) NULL,
    [Twitter] NVARCHAR(200) NULL,
    [Facebook] NVARCHAR(200) NULL,
    [LinkedIn] NVARCHAR(200) NULL,
    [Website] NVARCHAR(200) NULL,
    
    -- Professional Information
    [Skills] NVARCHAR(500) NULL,
    [Languages] NVARCHAR(200) NULL,
    [AvailabilityStatus] NVARCHAR(50) NULL,
    
    -- Card Styling
    [PrimaryColor] NVARCHAR(7) NULL DEFAULT '#3456a3',
    [SecondaryColor] NVARCHAR(7) NULL DEFAULT '#5478c4',
    [FontFamily] NVARCHAR(50) NULL DEFAULT 'Arial',
    [CardOrientation] NVARCHAR(20) NULL DEFAULT 'Landscape',
    [LogoPath] NVARCHAR(500) NULL,
    
    -- Analytics
    [ViewCount] INT NOT NULL DEFAULT 0,
    [LastViewed] DATETIME2 NULL,
    
    -- Timestamps
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    -- Foreign Keys
    CONSTRAINT [FK_VisitingCards_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE,
    CONSTRAINT [FK_VisitingCards_Templates] FOREIGN KEY ([TemplateId]) REFERENCES [Templates]([TemplateId])
);

-- 4. ContactForms Table (Phase 2 Feature)
CREATE TABLE [ContactForms] (
    [ContactId] INT IDENTITY(1,1) PRIMARY KEY,
    [CardId] INT NOT NULL,
    [SenderName] NVARCHAR(100) NOT NULL,
    [SenderEmail] NVARCHAR(255) NOT NULL,
    [Subject] NVARCHAR(200) NOT NULL,
    [Message] NVARCHAR(1000) NOT NULL,
    [SubmittedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [IsRead] BIT NOT NULL DEFAULT 0,
    
    CONSTRAINT [FK_ContactForms_VisitingCards] FOREIGN KEY ([CardId]) REFERENCES [VisitingCards]([CardId]) ON DELETE CASCADE
);

-- 5. UserRoles Table (Phase 2 Feature - Role-based Access Control)
CREATE TABLE [UserRoles] (
    [UserRoleId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [Role] NVARCHAR(50) NOT NULL DEFAULT 'User', -- 'Admin', 'User', 'Premium'
    [AssignedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE
);

-- 6. EmailTemplates Table (Phase 2 Feature)
CREATE TABLE [EmailTemplates] (
    [TemplateId] INT IDENTITY(1,1) PRIMARY KEY,
    [TemplateName] NVARCHAR(100) NOT NULL,
    [Subject] NVARCHAR(200) NOT NULL,
    [Body] NVARCHAR(MAX) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- 7. CustomTemplates Table (Phase 2 Feature - Template Builder)
CREATE TABLE [CustomTemplates] (
    [CustomTemplateId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [TemplateName] NVARCHAR(100) NOT NULL,
    [HtmlContent] NVARCHAR(MAX) NOT NULL,
    [CssContent] NVARCHAR(MAX) NOT NULL,
    [IsPublic] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [FK_CustomTemplates_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE
);

-- 8. CardViews Table (For detailed analytics)
CREATE TABLE [CardViews] (
    [ViewId] INT IDENTITY(1,1) PRIMARY KEY,
    [CardId] INT NOT NULL,
    [ViewerIP] NVARCHAR(45) NULL,
    [UserAgent] NVARCHAR(500) NULL,
    [Country] NVARCHAR(100) NULL,
    [City] NVARCHAR(100) NULL,
    [DeviceType] NVARCHAR(50) NULL, -- 'Desktop', 'Mobile', 'Tablet'
    [ReferrerUrl] NVARCHAR(500) NULL,
    [ViewedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [FK_CardViews_VisitingCards] FOREIGN KEY ([CardId]) REFERENCES [VisitingCards]([CardId]) ON DELETE CASCADE
);

-- 9. BatchJobs Table (Phase 2 Feature - Batch QR Code Generation)
CREATE TABLE [BatchJobs] (
    [BatchJobId] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] INT NOT NULL,
    [JobName] NVARCHAR(200) NOT NULL,
    [CardIds] NVARCHAR(MAX) NOT NULL, -- JSON array of card IDs
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Processing', 'Completed', 'Failed'
    [FilePath] NVARCHAR(500) NULL, -- Path to generated ZIP file
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [CompletedAt] DATETIME2 NULL,
    [ErrorMessage] NVARCHAR(MAX) NULL,
    
    CONSTRAINT [FK_BatchJobs_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]) ON DELETE CASCADE
);

-- ===============================================
-- INSERT INITIAL DATA
-- ===============================================

-- Insert default templates
INSERT INTO [Templates] ([TemplateName], [FilePath]) VALUES
('Professional Blue', '/templates/professional-blue.html'),
('Modern Gradient', '/templates/modern-gradient.html'),
('Classic White', '/templates/classic-white.html'),
('Creative Dark', '/templates/creative-dark.html');

-- Insert default email templates
INSERT INTO [EmailTemplates] ([TemplateName], [Subject], [Body]) VALUES
('Welcome Email', 'Welcome to QardX!', 
'<h2>Welcome to QardX!</h2><p>Thank you for joining us. Start creating amazing digital visiting cards today!</p>'),
('Card Shared', 'Your QardX card was shared!', 
'<h2>Great News!</h2><p>Your digital visiting card has been shared. Check your analytics for more details.</p>'),
('Contact Form Submission', 'New contact form submission', 
'<h2>New Contact</h2><p>You have received a new message through your QardX card contact form.</p>');

-- Create indexes for better performance
CREATE INDEX [IX_VisitingCards_UserId] ON [VisitingCards] ([UserId]);
CREATE INDEX [IX_VisitingCards_CreatedAt] ON [VisitingCards] ([CreatedAt]);
CREATE INDEX [IX_ContactForms_CardId] ON [ContactForms] ([CardId]);
CREATE INDEX [IX_ContactForms_SubmittedAt] ON [ContactForms] ([SubmittedAt]);
CREATE INDEX [IX_UserRoles_UserId] ON [UserRoles] ([UserId]);
CREATE INDEX [IX_CustomTemplates_UserId] ON [CustomTemplates] ([UserId]);
CREATE INDEX [IX_CardViews_CardId] ON [CardViews] ([CardId]);
CREATE INDEX [IX_CardViews_ViewedAt] ON [CardViews] ([ViewedAt]);
CREATE INDEX [IX_BatchJobs_UserId] ON [BatchJobs] ([UserId]);
CREATE INDEX [IX_BatchJobs_Status] ON [BatchJobs] ([Status]);
GO

-- ===============================================
-- CREATE VIEWS FOR ANALYTICS
-- ===============================================

-- User Analytics View
CREATE VIEW [UserAnalyticsView] AS
SELECT 
    u.[UserId],
    u.[FullName] AS [UserName],
    u.[Email],
    COUNT(vc.[CardId]) AS [TotalCards],
    SUM(vc.[ViewCount]) AS [TotalViews],
    MAX(vc.[LastViewed]) AS [LastCardViewed],
    COUNT(cf.[ContactId]) AS [TotalContacts]
FROM [Users] u
LEFT JOIN [VisitingCards] vc ON u.[UserId] = vc.[UserId]
LEFT JOIN [ContactForms] cf ON vc.[CardId] = cf.[CardId]
GROUP BY u.[UserId], u.[FullName], u.[Email];
GO

-- Card Analytics View
CREATE VIEW [CardAnalyticsView] AS
SELECT 
    vc.[CardId],
    vc.[FirstName] + ' ' + ISNULL(vc.[LastName], '') AS [CardOwnerName],
    vc.[Company],
    vc.[JobTitle],
    vc.[ViewCount],
    vc.[LastViewed],
    vc.[CreatedAt],
    COUNT(cf.[ContactId]) AS [ContactCount],
    t.[TemplateName]
FROM [VisitingCards] vc
INNER JOIN [Templates] t ON vc.[TemplateId] = t.[TemplateId]
LEFT JOIN [ContactForms] cf ON vc.[CardId] = cf.[CardId]
GROUP BY vc.[CardId], vc.[FirstName], vc.[LastName], vc.[Company], vc.[JobTitle], 
         vc.[ViewCount], vc.[LastViewed], vc.[CreatedAt], t.[TemplateName];
GO

PRINT 'QardXDB database created successfully with all Phase 1 & Phase 2 features!';
PRINT 'Database includes:';
PRINT '- Users, Templates, VisitingCards (with FirstName/LastName)';
PRINT '- ContactForms, UserRoles, EmailTemplates';
PRINT '- Analytics Views and Performance Indexes';
PRINT '- Sample data for templates and email templates';
</attempt_completion>
