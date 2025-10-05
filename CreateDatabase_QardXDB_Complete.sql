-- ===============================================
-- QardX - Complete Database Setup Script
-- Version: 2.0 (Phase 1 & Phase 2 Complete)
-- Date: October 2025
-- Author: QardX Development Team
-- 
-- COMPLETE DATABASE CREATION SCRIPT
-- This script creates the entire QardX database from scratch
-- Run this script in SQL Server Management Studio (SSMS)
-- ===============================================

-- Set NOCOUNT ON to prevent extra result sets from interfering
SET NOCOUNT ON;
GO

-- Use master database to create new database
USE master;
GO

-- Drop database if it exists (Clean Start)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'QardXDB')
BEGIN
    ALTER DATABASE QardXDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QardXDB;
    PRINT 'Existing QardXDB database dropped successfully.';
END
GO

-- Create fresh QardX database with proper configuration
CREATE DATABASE QardXDB
ON 
(
    NAME = 'QardXDB',
    FILENAME = 'QardXDB.mdf',
    SIZE = 100MB,
    MAXSIZE = 1GB,
    FILEGROWTH = 10MB
)
LOG ON 
(
    NAME = 'QardXDB_Log',
    FILENAME = 'QardXDB_Log.ldf',
    SIZE = 10MB,
    MAXSIZE = 100MB,
    FILEGROWTH = 5MB
);
GO

PRINT 'QardXDB database created successfully.';

-- Switch to the new database
USE QardXDB;
GO

-- ===============================================
-- TABLE CREATION (Phase 1 & Phase 2)
-- ===============================================

-- 1. Users Table (Authentication & User Management)
CREATE TABLE [dbo].[Users] (
    [UserId] INT IDENTITY(1,1) NOT NULL,
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL DEFAULT 'User',
    [IsActive] BIT NOT NULL DEFAULT 1,
    [ProfilePicture] NVARCHAR(500) NULL,
    [PhoneNumber] NVARCHAR(20) NULL,
    [CompanyName] NVARCHAR(200) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [LastLoginAt] DATETIME2 NULL,
    
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [UK_Users_Email] UNIQUE ([Email])
);
GO

PRINT 'Users table created successfully.';

-- 2. Templates Table (Card Templates)
CREATE TABLE [dbo].[Templates] (
    [TemplateId] INT IDENTITY(1,1) NOT NULL,
    [TemplateName] NVARCHAR(100) NOT NULL,
    [TemplateDescription] NVARCHAR(500) NULL,
    [FilePath] NVARCHAR(500) NOT NULL,
    [PreviewImagePath] NVARCHAR(500) NULL,
    [Category] NVARCHAR(50) NULL DEFAULT 'Business',
    [IsActive] BIT NOT NULL DEFAULT 1,
    [IsPremium] BIT NOT NULL DEFAULT 0,
    [SortOrder] INT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED ([TemplateId] ASC)
);
GO

PRINT 'Templates table created successfully.';

-- 3. VisitingCards Table (Complete Card Information)
CREATE TABLE [dbo].[VisitingCards] (
    [CardId] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [TemplateId] INT NOT NULL,
    
    -- Personal Information
    [FirstName] NVARCHAR(100) NULL,
    [LastName] NVARCHAR(100) NULL,
    [Email] NVARCHAR(255) NULL,
    [Phone] NVARCHAR(20) NULL,
    [AlternatePhone] NVARCHAR(20) NULL,
    [Address] NVARCHAR(500) NULL,
    [City] NVARCHAR(100) NULL,
    [State] NVARCHAR(100) NULL,
    [Country] NVARCHAR(100) NULL,
    [PostalCode] NVARCHAR(20) NULL,
    
    -- Professional Information  
    [JobTitle] NVARCHAR(100) NULL,
    [Company] NVARCHAR(200) NULL,
    [Department] NVARCHAR(100) NULL,
    [Skills] NVARCHAR(500) NULL,
    [Languages] NVARCHAR(200) NULL,
    [AvailabilityStatus] NVARCHAR(50) NULL,
    [Bio] NVARCHAR(1000) NULL,
    [Experience] NVARCHAR(500) NULL,
    [Education] NVARCHAR(500) NULL,
    
    -- Social Media & Web Links
    [Website] NVARCHAR(300) NULL,
    [LinkedIn] NVARCHAR(300) NULL,
    [Twitter] NVARCHAR(300) NULL,
    [Instagram] NVARCHAR(300) NULL,
    [Facebook] NVARCHAR(300) NULL,
    [YouTube] NVARCHAR(300) NULL,
    [GitHub] NVARCHAR(300) NULL,
    [Portfolio] NVARCHAR(300) NULL,
    [WhatsApp] NVARCHAR(300) NULL,
    [Telegram] NVARCHAR(300) NULL,
    
    -- Card Customization
    [PrimaryColor] NVARCHAR(7) NULL DEFAULT '#3456a3',
    [SecondaryColor] NVARCHAR(7) NULL DEFAULT '#5478c4',
    [AccentColor] NVARCHAR(7) NULL DEFAULT '#ffffff',
    [FontFamily] NVARCHAR(50) NULL DEFAULT 'Arial',
    [FontSize] NVARCHAR(20) NULL DEFAULT 'Medium',
    [CardOrientation] NVARCHAR(20) NULL DEFAULT 'Landscape',
    [LogoPath] NVARCHAR(500) NULL,
    [BackgroundImagePath] NVARCHAR(500) NULL,
    [ProfileImagePath] NVARCHAR(500) NULL,
    
    -- SEO & Metadata
    [CardTitle] NVARCHAR(200) NULL,
    [CardDescription] NVARCHAR(500) NULL,
    [Keywords] NVARCHAR(300) NULL,
    [QRCodePath] NVARCHAR(500) NULL,
    [CardUrl] NVARCHAR(500) NULL,
    [IsPublic] BIT NOT NULL DEFAULT 1,
    [IsActive] BIT NOT NULL DEFAULT 1,
    
    -- Analytics & Tracking
    [ViewCount] INT NOT NULL DEFAULT 0,
    [UniqueViewCount] INT NOT NULL DEFAULT 0,
    [LastViewed] DATETIME2 NULL,
    [ShareCount] INT NOT NULL DEFAULT 0,
    [DownloadCount] INT NOT NULL DEFAULT 0,
    
    -- Timestamps
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [PublishedAt] DATETIME2 NULL,
    
    -- Foreign Keys
    CONSTRAINT [PK_VisitingCards] PRIMARY KEY CLUSTERED ([CardId] ASC),
    CONSTRAINT [FK_VisitingCards_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId]) ON DELETE CASCADE,
    CONSTRAINT [FK_VisitingCards_Templates] FOREIGN KEY ([TemplateId]) REFERENCES [dbo].[Templates]([TemplateId])
);
GO

PRINT 'VisitingCards table created successfully.';

-- 4. ContactForms Table (Contact Form Submissions)
CREATE TABLE [dbo].[ContactForms] (
    [ContactId] INT IDENTITY(1,1) NOT NULL,
    [CardId] INT NOT NULL,
    [SenderName] NVARCHAR(100) NOT NULL,
    [SenderEmail] NVARCHAR(255) NOT NULL,
    [SenderPhone] NVARCHAR(20) NULL,
    [SenderCompany] NVARCHAR(200) NULL,
    [Subject] NVARCHAR(200) NOT NULL,
    [Message] NVARCHAR(1000) NOT NULL,
    [Priority] NVARCHAR(20) NULL DEFAULT 'Normal',
    [Status] NVARCHAR(50) NULL DEFAULT 'New',
    [IsRead] BIT NOT NULL DEFAULT 0,
    [IsReplied] BIT NOT NULL DEFAULT 0,
    [IPAddress] NVARCHAR(45) NULL,
    [UserAgent] NVARCHAR(500) NULL,
    [ReferrerUrl] NVARCHAR(500) NULL,
    [SubmittedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [ReadAt] DATETIME2 NULL,
    [RepliedAt] DATETIME2 NULL,
    
    CONSTRAINT [PK_ContactForms] PRIMARY KEY CLUSTERED ([ContactId] ASC),
    CONSTRAINT [FK_ContactForms_VisitingCards] FOREIGN KEY ([CardId]) REFERENCES [dbo].[VisitingCards]([CardId]) ON DELETE CASCADE
);
GO

PRINT 'ContactForms table created successfully.';

-- 5. CustomTemplates Table (Template Builder)
CREATE TABLE [dbo].[CustomTemplates] (
    [CustomTemplateId] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [TemplateName] NVARCHAR(100) NOT NULL,
    [TemplateDescription] NVARCHAR(500) NULL,
    [HtmlContent] NVARCHAR(MAX) NOT NULL,
    [CssContent] NVARCHAR(MAX) NOT NULL,
    [JavaScriptContent] NVARCHAR(MAX) NULL,
    [PreviewImagePath] NVARCHAR(500) NULL,
    [Category] NVARCHAR(50) NULL DEFAULT 'Custom',
    [IsPublic] BIT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [UsageCount] INT NOT NULL DEFAULT 0,
    [Rating] DECIMAL(3,2) NULL DEFAULT 0,
    [Tags] NVARCHAR(300) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [PublishedAt] DATETIME2 NULL,
    
    CONSTRAINT [PK_CustomTemplates] PRIMARY KEY CLUSTERED ([CustomTemplateId] ASC),
    CONSTRAINT [FK_CustomTemplates_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId]) ON DELETE CASCADE
);
GO

PRINT 'CustomTemplates table created successfully.';

-- 6. CardViews Table (Detailed Analytics)
CREATE TABLE [dbo].[CardViews] (
    [ViewId] INT IDENTITY(1,1) NOT NULL,
    [CardId] INT NOT NULL,
    [ViewerIP] NVARCHAR(45) NULL,
    [UserAgent] NVARCHAR(500) NULL,
    [Country] NVARCHAR(100) NULL,
    [City] NVARCHAR(100) NULL,
    [Region] NVARCHAR(100) NULL,
    [DeviceType] NVARCHAR(50) NULL,
    [Browser] NVARCHAR(100) NULL,
    [OperatingSystem] NVARCHAR(100) NULL,
    [ReferrerUrl] NVARCHAR(500) NULL,
    [ReferrerDomain] NVARCHAR(200) NULL,
    [SessionId] NVARCHAR(100) NULL,
    [IsUniqueView] BIT NOT NULL DEFAULT 1,
    [ViewDuration] INT NULL,
    [ViewedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [PK_CardViews] PRIMARY KEY CLUSTERED ([ViewId] ASC),
    CONSTRAINT [FK_CardViews_VisitingCards] FOREIGN KEY ([CardId]) REFERENCES [dbo].[VisitingCards]([CardId]) ON DELETE CASCADE
);
GO

PRINT 'CardViews table created successfully.';

-- 7. BatchJobs Table (Batch Operations)
CREATE TABLE [dbo].[BatchJobs] (
    [BatchJobId] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [JobName] NVARCHAR(200) NOT NULL,
    [JobType] NVARCHAR(50) NOT NULL DEFAULT 'QRGeneration',
    [CardIds] NVARCHAR(MAX) NOT NULL,
    [Configuration] NVARCHAR(MAX) NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    [Progress] INT NOT NULL DEFAULT 0,
    [TotalItems] INT NOT NULL DEFAULT 0,
    [ProcessedItems] INT NOT NULL DEFAULT 0,
    [FailedItems] INT NOT NULL DEFAULT 0,
    [FilePath] NVARCHAR(500) NULL,
    [FileSize] BIGINT NULL,
    [ErrorMessage] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [StartedAt] DATETIME2 NULL,
    [CompletedAt] DATETIME2 NULL,
    [ExpiresAt] DATETIME2 NULL,
    
    CONSTRAINT [PK_BatchJobs] PRIMARY KEY CLUSTERED ([BatchJobId] ASC),
    CONSTRAINT [FK_BatchJobs_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId]) ON DELETE CASCADE
);
GO

PRINT 'BatchJobs table created successfully.';

-- 8. CardShares Table (Sharing Analytics)
CREATE TABLE [dbo].[CardShares] (
    [ShareId] INT IDENTITY(1,1) NOT NULL,
    [CardId] INT NOT NULL,
    [ShareMethod] NVARCHAR(50) NOT NULL,
    [Platform] NVARCHAR(100) NULL,
    [SharerIP] NVARCHAR(45) NULL,
    [UserAgent] NVARCHAR(500) NULL,
    [SharedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [PK_CardShares] PRIMARY KEY CLUSTERED ([ShareId] ASC),
    CONSTRAINT [FK_CardShares_VisitingCards] FOREIGN KEY ([CardId]) REFERENCES [dbo].[VisitingCards]([CardId]) ON DELETE CASCADE
);
GO

PRINT 'CardShares table created successfully.';

-- 9. EmailTemplates Table (Email Management)
CREATE TABLE [dbo].[EmailTemplates] (
    [TemplateId] INT IDENTITY(1,1) NOT NULL,
    [TemplateName] NVARCHAR(100) NOT NULL,
    [TemplateCode] NVARCHAR(50) NOT NULL,
    [Subject] NVARCHAR(200) NOT NULL,
    [Body] NVARCHAR(MAX) NOT NULL,
    [PlainTextBody] NVARCHAR(MAX) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED ([TemplateId] ASC),
    CONSTRAINT [UK_EmailTemplates_Code] UNIQUE ([TemplateCode])
);
GO

PRINT 'EmailTemplates table created successfully.';

-- 10. SystemSettings Table (Application Configuration)
CREATE TABLE [dbo].[SystemSettings] (
    [SettingId] INT IDENTITY(1,1) NOT NULL,
    [SettingKey] NVARCHAR(100) NOT NULL,
    [SettingValue] NVARCHAR(MAX) NOT NULL,
    [SettingDescription] NVARCHAR(500) NULL,
    [DataType] NVARCHAR(50) NOT NULL DEFAULT 'String',
    [Category] NVARCHAR(100) NULL DEFAULT 'General',
    [IsEncrypted] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED ([SettingId] ASC),
    CONSTRAINT [UK_SystemSettings_Key] UNIQUE ([SettingKey])
);
GO

PRINT 'SystemSettings table created successfully.';

-- ===============================================
-- CREATE INDEXES FOR PERFORMANCE
-- ===============================================

-- Users table indexes
CREATE NONCLUSTERED INDEX [IX_Users_Email] ON [dbo].[Users] ([Email]);
CREATE NONCLUSTERED INDEX [IX_Users_CreatedAt] ON [dbo].[Users] ([CreatedAt]);
CREATE NONCLUSTERED INDEX [IX_Users_Role] ON [dbo].[Users] ([Role]);

-- VisitingCards table indexes
CREATE NONCLUSTERED INDEX [IX_VisitingCards_UserId] ON [dbo].[VisitingCards] ([UserId]);
CREATE NONCLUSTERED INDEX [IX_VisitingCards_TemplateId] ON [dbo].[VisitingCards] ([TemplateId]);
CREATE NONCLUSTERED INDEX [IX_VisitingCards_CreatedAt] ON [dbo].[VisitingCards] ([CreatedAt]);
CREATE NONCLUSTERED INDEX [IX_VisitingCards_IsPublic] ON [dbo].[VisitingCards] ([IsPublic]);
CREATE NONCLUSTERED INDEX [IX_VisitingCards_ViewCount] ON [dbo].[VisitingCards] ([ViewCount]);

-- ContactForms table indexes
CREATE NONCLUSTERED INDEX [IX_ContactForms_CardId] ON [dbo].[ContactForms] ([CardId]);
CREATE NONCLUSTERED INDEX [IX_ContactForms_SubmittedAt] ON [dbo].[ContactForms] ([SubmittedAt]);
CREATE NONCLUSTERED INDEX [IX_ContactForms_IsRead] ON [dbo].[ContactForms] ([IsRead]);

-- CustomTemplates table indexes
CREATE NONCLUSTERED INDEX [IX_CustomTemplates_UserId] ON [dbo].[CustomTemplates] ([UserId]);
CREATE NONCLUSTERED INDEX [IX_CustomTemplates_IsPublic] ON [dbo].[CustomTemplates] ([IsPublic]);
CREATE NONCLUSTERED INDEX [IX_CustomTemplates_Category] ON [dbo].[CustomTemplates] ([Category]);

-- CardViews table indexes
CREATE NONCLUSTERED INDEX [IX_CardViews_CardId] ON [dbo].[CardViews] ([CardId]);
CREATE NONCLUSTERED INDEX [IX_CardViews_ViewedAt] ON [dbo].[CardViews] ([ViewedAt]);
CREATE NONCLUSTERED INDEX [IX_CardViews_IsUniqueView] ON [dbo].[CardViews] ([IsUniqueView]);

-- BatchJobs table indexes
CREATE NONCLUSTERED INDEX [IX_BatchJobs_UserId] ON [dbo].[BatchJobs] ([UserId]);
CREATE NONCLUSTERED INDEX [IX_BatchJobs_Status] ON [dbo].[BatchJobs] ([Status]);
CREATE NONCLUSTERED INDEX [IX_BatchJobs_CreatedAt] ON [dbo].[BatchJobs] ([CreatedAt]);

-- CardShares table indexes
CREATE NONCLUSTERED INDEX [IX_CardShares_CardId] ON [dbo].[CardShares] ([CardId]);
CREATE NONCLUSTERED INDEX [IX_CardShares_SharedAt] ON [dbo].[CardShares] ([SharedAt]);

GO

PRINT 'Performance indexes created successfully.';

-- ===============================================
-- INSERT INITIAL DATA
-- ===============================================

-- Insert default templates
INSERT INTO [dbo].[Templates] ([TemplateName], [TemplateDescription], [FilePath], [PreviewImagePath], [Category], [SortOrder]) VALUES
('Professional Blue', 'Clean and professional blue gradient design perfect for business cards', '/templates/professional-blue.html', '/images/templates/professional-blue-preview.jpg', 'Business', 1),
('Modern Gradient', 'Contemporary gradient design with modern typography', '/templates/modern-gradient.html', '/images/templates/modern-gradient-preview.jpg', 'Modern', 2),
('Classic White', 'Timeless white background with elegant black text', '/templates/classic-white.html', '/images/templates/classic-white-preview.jpg', 'Classic', 3),
('Creative Dark', 'Bold dark theme with vibrant accent colors', '/templates/creative-dark.html', '/images/templates/creative-dark-preview.jpg', 'Creative', 4),
('Minimalist', 'Clean minimalist design with maximum white space', '/templates/minimalist.html', '/images/templates/minimalist-preview.jpg', 'Minimalist', 5),
('Corporate Gold', 'Luxury gold accents for premium business cards', '/templates/corporate-gold.html', '/images/templates/corporate-gold-preview.jpg', 'Corporate', 6);

-- Insert default email templates
INSERT INTO [dbo].[EmailTemplates] ([TemplateName], [TemplateCode], [Subject], [Body], [PlainTextBody]) VALUES
('Welcome Email', 'WELCOME', 'Welcome to QardX - Your Digital Business Card Platform!', 
'<!DOCTYPE html><html><head><style>body{font-family:Arial,sans-serif;line-height:1.6;color:#333;}.container{max-width:600px;margin:0 auto;padding:20px;}.header{background:#3456a3;color:white;padding:20px;text-align:center;}.content{padding:20px;}.footer{background:#f8f9fa;padding:15px;text-align:center;font-size:14px;}</style></head><body><div class="container"><div class="header"><h1>Welcome to QardX!</h1></div><div class="content"><h2>Hello {{UserName}},</h2><p>Thank you for joining QardX, the premier platform for creating stunning digital business cards!</p><p>Here''s what you can do:</p><ul><li>Create unlimited digital business cards</li><li>Choose from professional templates</li><li>Share instantly via QR codes</li><li>Track analytics and engagement</li><li>Build custom templates</li></ul><p><a href="{{LoginUrl}}" style="background:#3456a3;color:white;padding:12px 30px;text-decoration:none;border-radius:5px;display:inline-block;">Get Started Now</a></p></div><div class="footer"><p>Best regards,<br>The QardX Team</p></div></div></body></html>',
'Welcome to QardX! Thank you for joining us. Start creating amazing digital visiting cards today! Visit: {{LoginUrl}}'),

('Contact Form Notification', 'CONTACT_FORM', 'New Contact Form Submission - QardX', 
'<!DOCTYPE html><html><head><style>body{font-family:Arial,sans-serif;line-height:1.6;color:#333;}.container{max-width:600px;margin:0 auto;padding:20px;}.header{background:#28a745;color:white;padding:20px;text-align:center;}.content{padding:20px;}.info-box{background:#f8f9fa;padding:15px;border-left:4px solid #28a745;margin:15px 0;}.footer{background:#f8f9fa;padding:15px;text-align:center;font-size:14px;}</style></head><body><div class="container"><div class="header"><h1>New Contact Form Submission</h1></div><div class="content"><p>You have received a new message through your QardX business card!</p><div class="info-box"><strong>From:</strong> {{SenderName}} ({{SenderEmail}})<br><strong>Subject:</strong> {{Subject}}<br><strong>Date:</strong> {{SubmissionDate}}</div><div class="info-box"><strong>Message:</strong><br>{{Message}}</div><p><a href="{{DashboardUrl}}" style="background:#28a745;color:white;padding:12px 30px;text-decoration:none;border-radius:5px;display:inline-block;">View in Dashboard</a></p></div><div class="footer"><p>QardX - Digital Business Cards</p></div></div></body></html>',
'New contact form submission from {{SenderName}} ({{SenderEmail}}). Subject: {{Subject}}. Message: {{Message}}'),

('Card View Notification', 'CARD_VIEWED', 'Your QardX Card Was Viewed!', 
'<!DOCTYPE html><html><head><style>body{font-family:Arial,sans-serif;line-height:1.6;color:#333;}.container{max-width:600px;margin:0 auto;padding:20px;}.header{background:#17a2b8;color:white;padding:20px;text-align:center;}.content{padding:20px;}.stats-box{background:#e7f3ff;padding:15px;border-radius:5px;margin:15px 0;}.footer{background:#f8f9fa;padding:15px;text-align:center;font-size:14px;}</style></head><body><div class="container"><div class="header"><h1>Card Activity Update</h1></div><div class="content"><p>Great news! Your QardX business card has been viewed.</p><div class="stats-box"><strong>Card:</strong> {{CardName}}<br><strong>Total Views:</strong> {{TotalViews}}<br><strong>This Month:</strong> {{MonthlyViews}}<br><strong>Location:</strong> {{ViewerLocation}}</div><p><a href="{{AnalyticsUrl}}" style="background:#17a2b8;color:white;padding:12px 30px;text-decoration:none;border-radius:5px;display:inline-block;">View Analytics</a></p></div><div class="footer"><p>QardX - Digital Business Cards</p></div></div></body></html>',
'Your QardX card "{{CardName}}" was viewed! Total views: {{TotalViews}}. View analytics: {{AnalyticsUrl}}');

-- Insert system settings
INSERT INTO [dbo].[SystemSettings] ([SettingKey], [SettingValue], [SettingDescription], [Category]) VALUES
('AppName', 'QardX', 'Application name displayed throughout the system', 'General'),
('AppVersion', '2.0', 'Current application version', 'General'),
('MaxCardsPerUser', '50', 'Maximum number of cards a user can create', 'Limits'),
('MaxTemplatesPerUser', '10', 'Maximum number of custom templates per user', 'Limits'),
('EnableAnalytics', 'true', 'Enable detailed analytics tracking', 'Features'),
('EnableContactForms', 'true', 'Enable contact form functionality', 'Features'),
('EnableBatchOperations', 'true', 'Enable batch QR code generation', 'Features'),
('DefaultCardExpiry', '365', 'Default card expiry in days (0 for no expiry)', 'Cards'),
('QRCodeSize', '300', 'Default QR code size in pixels', 'QR Codes'),
('MaxFileUploadSize', '5242880', 'Maximum file upload size in bytes (5MB)', 'Files'),
('EmailNotifications', 'true', 'Enable email notifications', 'Email'),
('SMTPServer', '', 'SMTP server for sending emails', 'Email'),
('SMTPPort', '587', 'SMTP server port', 'Email'),
('SMTPUsername', '', 'SMTP username', 'Email'),
('SMTPPassword', '', 'SMTP password (encrypted)', 'Email'),
('ContactEmail', 'support@qardx.com', 'Support contact email', 'Contact'),
('MaintenanceMode', 'false', 'Enable maintenance mode', 'System'),
('CacheExpiry', '3600', 'Cache expiry time in seconds', 'Performance');

GO

PRINT 'Initial data inserted successfully.';

-- ===============================================
-- CREATE VIEWS FOR ANALYTICS AND REPORTING
-- ===============================================

-- User Analytics View
CREATE VIEW [dbo].[UserAnalyticsView] AS
SELECT 
    u.[UserId],
    u.[FullName] AS [UserName],
    u.[Email],
    u.[Role],
    u.[CreatedAt] AS [UserCreatedAt],
    COUNT(DISTINCT vc.[CardId]) AS [TotalCards],
    ISNULL(SUM(vc.[ViewCount]), 0) AS [TotalViews],
    ISNULL(SUM(vc.[UniqueViewCount]), 0) AS [TotalUniqueViews],
    ISNULL(SUM(vc.[ShareCount]), 0) AS [TotalShares],
    MAX(vc.[LastViewed]) AS [LastCardViewed],
    COUNT(DISTINCT cf.[ContactId]) AS [TotalContacts],
    COUNT(DISTINCT ct.[CustomTemplateId]) AS [CustomTemplates],
    COUNT(DISTINCT bj.[BatchJobId]) AS [BatchJobs]
FROM [dbo].[Users] u
LEFT JOIN [dbo].[VisitingCards] vc ON u.[UserId] = vc.[UserId]
LEFT JOIN [dbo].[ContactForms] cf ON vc.[CardId] = cf.[CardId]
LEFT JOIN [dbo].[CustomTemplates] ct ON u.[UserId] = ct.[UserId]
LEFT JOIN [dbo].[BatchJobs] bj ON u.[UserId] = bj.[UserId]
GROUP BY u.[UserId], u.[FullName], u.[Email], u.[Role], u.[CreatedAt];
GO

-- Card Analytics View
CREATE VIEW [dbo].[CardAnalyticsView] AS
SELECT 
    vc.[CardId],
    vc.[UserId],
    u.[FullName] AS [UserName],
    ISNULL(vc.[FirstName] + ' ' + vc.[LastName], 'Unnamed Card') AS [CardOwnerName],
    vc.[Company],
    vc.[JobTitle],
    vc.[Email] AS [CardEmail],
    vc.[Phone] AS [CardPhone],
    vc.[ViewCount],
    vc.[UniqueViewCount],
    vc.[ShareCount],
    vc.[DownloadCount],
    vc.[LastViewed],
    vc.[CreatedAt] AS [CardCreatedAt],
    vc.[UpdatedAt] AS [CardUpdatedAt],
    vc.[IsPublic],
    vc.[IsActive],
    COUNT(DISTINCT cf.[ContactId]) AS [ContactCount],
    COUNT(DISTINCT cv.[ViewId]) AS [DetailedViewCount],
    COUNT(DISTINCT cs.[ShareId]) AS [DetailedShareCount],
    t.[TemplateName],
    t.[Category] AS [TemplateCategory]
FROM [dbo].[VisitingCards] vc
INNER JOIN [dbo].[Users] u ON vc.[UserId] = u.[UserId]
INNER JOIN [dbo].[Templates] t ON vc.[TemplateId] = t.[TemplateId]
LEFT JOIN [dbo].[ContactForms] cf ON vc.[CardId] = cf.[CardId]
LEFT JOIN [dbo].[CardViews] cv ON vc.[CardId] = cv.[CardId]
LEFT JOIN [dbo].[CardShares] cs ON vc.[CardId] = cs.[CardId]
GROUP BY vc.[CardId], vc.[UserId], u.[FullName], vc.[FirstName], vc.[LastName], 
         vc.[Company], vc.[JobTitle], vc.[Email], vc.[Phone], vc.[ViewCount], 
         vc.[UniqueViewCount], vc.[ShareCount], vc.[DownloadCount], vc.[LastViewed], 
         vc.[CreatedAt], vc.[UpdatedAt], vc.[IsPublic], vc.[IsActive], 
         t.[TemplateName], t.[Category];
GO

-- Daily Analytics View
CREATE VIEW [dbo].[DailyAnalyticsView] AS
SELECT 
    CAST([ViewedAt] AS DATE) AS [ViewDate],
    COUNT(*) AS [TotalViews],
    COUNT(DISTINCT [CardId]) AS [UniqueCards],
    COUNT(DISTINCT [ViewerIP]) AS [UniqueVisitors],
    COUNT(CASE WHEN [IsUniqueView] = 1 THEN 1 END) AS [NewViews],
    COUNT(CASE WHEN [DeviceType] = 'Mobile' THEN 1 END) AS [MobileViews],
    COUNT(CASE WHEN [DeviceType] = 'Desktop' THEN 1 END) AS [DesktopViews],
    COUNT(CASE WHEN [DeviceType] = 'Tablet' THEN 1 END) AS [TabletViews]
FROM [dbo].[CardViews]
WHERE [ViewedAt] >= DATEADD(day, -30, GETDATE())
GROUP BY CAST([ViewedAt] AS DATE);
GO

-- Template Usage View
CREATE VIEW [dbo].[TemplateUsageView] AS
SELECT 
    t.[TemplateId],
    t.[TemplateName],
    t.[Category],
    t.[IsActive],
    t.[IsPremium],
    COUNT(vc.[CardId]) AS [UsageCount],
    ISNULL(SUM(vc.[ViewCount]), 0) AS [TotalViews],
    ISNULL(AVG(CAST(vc.[ViewCount] AS FLOAT)), 0) AS [AverageViews],
    MAX(vc.[CreatedAt]) AS [LastUsed]
FROM [dbo].[Templates] t
LEFT JOIN [dbo].[VisitingCards] vc ON t.[TemplateId] = vc.[TemplateId]
GROUP BY t.[TemplateId], t.[TemplateName], t.[Category], t.[IsActive], t.[IsPremium];
GO

PRINT 'Analytics views created successfully.';

-- ===============================================
-- CREATE STORED PROCEDURES
-- ===============================================

-- Procedure to get user dashboard statistics
CREATE PROCEDURE [dbo].[GetUserDashboardStats]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        (SELECT COUNT(*) FROM [dbo].[VisitingCards] WHERE [UserId] = @UserId AND [IsActive] = 1) AS [TotalCards],
        (SELECT ISNULL(SUM([ViewCount]), 0) FROM [dbo].[VisitingCards] WHERE [UserId] = @UserId) AS [TotalViews],
        (SELECT COUNT(*) FROM [dbo].[ContactForms] cf INNER JOIN [dbo].[VisitingCards] vc ON cf.[CardId] = vc.[CardId] WHERE vc.[UserId] = @UserId) AS [TotalContacts],
        (SELECT COUNT(*) FROM [dbo].[CustomTemplates] WHERE [UserId] = @UserId AND [IsActive] = 1) AS [CustomTemplates],
        (SELECT COUNT(*) FROM [dbo].[ContactForms] cf INNER JOIN [dbo].[VisitingCards] vc ON cf.[CardId] = vc.[CardId] WHERE vc.[UserId] = @UserId AND cf.[IsRead] = 0) AS [UnreadContacts],
        (SELECT COUNT(*) FROM [dbo].[CardViews] cv INNER JOIN [dbo].[VisitingCards] vc ON cv.[CardId] = vc.[CardId] WHERE vc.[UserId] = @UserId AND cv.[ViewedAt] >= DATEADD(day, -7, GETDATE())) AS [ViewsThisWeek];
END;
GO

-- Procedure to update card view count
CREATE PROCEDURE [dbo].[UpdateCardViewCount]
    @CardId INT,
    @ViewerIP NVARCHAR(45) = NULL,
    @UserAgent NVARCHAR(500) = NULL,
    @Country NVARCHAR(100) = NULL,
    @City NVARCHAR(100) = NULL,
    @DeviceType NVARCHAR(50) = NULL,
    @ReferrerUrl NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @IsUniqueView BIT = 1;
    
    -- Check if this IP has viewed this card in the last 24 hours
    IF EXISTS (
        SELECT 1 FROM [dbo].[CardViews] 
        WHERE [CardId] = @CardId 
        AND [ViewerIP] = @ViewerIP 
        AND [ViewedAt] >= DATEADD(hour, -24, GETDATE())
    )
    BEGIN
        SET @IsUniqueView = 0;
    END
    
    -- Insert view record
    INSERT INTO [dbo].[CardViews] (
        [CardId], [ViewerIP], [UserAgent], [Country], [City], 
        [DeviceType], [ReferrerUrl], [IsUniqueView]
    ) VALUES (
        @CardId, @ViewerIP, @UserAgent, @Country, @City, 
        @DeviceType, @ReferrerUrl, @IsUniqueView
    );
    
    -- Update card view counts
    UPDATE [dbo].[VisitingCards] 
    SET [ViewCount] = [ViewCount] + 1,
        [UniqueViewCount] = CASE WHEN @IsUniqueView = 1 THEN [UniqueViewCount] + 1 ELSE [UniqueViewCount] END,
        [LastViewed] = GETDATE()
    WHERE [CardId] = @CardId;
END;
GO

-- Procedure to clean up expired data
CREATE PROCEDURE [dbo].[CleanupExpiredData]
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CleanupDate DATETIME2 = DATEADD(month, -6, GETDATE());
    
    -- Clean up old card views (keep only last 6 months)
    DELETE FROM [dbo].[CardViews] WHERE [ViewedAt] < @CleanupDate;
    
    -- Clean up old batch jobs (keep only last 3 months)
    DELETE FROM [dbo].[BatchJobs] 
    WHERE [Status] IN ('Completed', 'Failed') 
    AND [CompletedAt] < DATEADD(month, -3, GETDATE());
    
    -- Clean up old contact forms (keep only last 12 months for read messages)
    DELETE FROM [dbo].[ContactForms] 
    WHERE [IsRead] = 1 
    AND [ReadAt] < DATEADD(month, -12, GETDATE());
    
    PRINT 'Expired data cleanup completed.';
END;
GO

PRINT 'Stored procedures created successfully.';

-- ===============================================
-- CREATE TRIGGERS FOR AUDIT TRAIL
-- ===============================================

-- Trigger to update UpdatedAt timestamp on VisitingCards
CREATE TRIGGER [dbo].[TR_VisitingCards_UpdatedAt]
ON [dbo].[VisitingCards]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[VisitingCards]
    SET [UpdatedAt] = GETDATE()
    FROM [dbo].[VisitingCards] vc
    INNER JOIN inserted i ON vc.[CardId] = i.[CardId];
END;
GO

-- Trigger to update UpdatedAt timestamp on CustomTemplates
CREATE TRIGGER [dbo].[TR_CustomTemplates_UpdatedAt]
ON [dbo].[CustomTemplates]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[CustomTemplates]
    SET [UpdatedAt] = GETDATE()
    FROM [dbo].[CustomTemplates] ct
    INNER JOIN inserted i ON ct.[CustomTemplateId] = i.[CustomTemplateId];
END;
GO

-- Trigger to update UpdatedAt timestamp on Users
CREATE TRIGGER [dbo].[TR_Users_UpdatedAt]
ON [dbo].[Users]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[Users]
    SET [UpdatedAt] = GETDATE()
    FROM [dbo].[Users] u
    INNER JOIN inserted i ON u.[UserId] = i.[UserId];
END;
GO

PRINT 'Database triggers created successfully.';

-- ===============================================
-- FINAL VERIFICATION AND SUMMARY
-- ===============================================

-- Verify all tables exist
DECLARE @TableCount INT;
SELECT @TableCount = COUNT(*) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'dbo';

PRINT '===============================================';
PRINT 'QardX DATABASE SETUP COMPLETED SUCCESSFULLY!';
PRINT '===============================================';
PRINT 'Database: QardXDB';
PRINT 'Tables Created: ' + CAST(@TableCount AS NVARCHAR(10));
PRINT 'Views Created: 4';
PRINT 'Stored Procedures: 3';
PRINT 'Triggers: 3';
PRINT 'Indexes: Multiple performance indexes';
PRINT 'Initial Data: Templates, Email Templates, System Settings';
PRINT '';
PRINT 'FEATURES INCLUDED:';
PRINT '✓ User Management & Authentication';
PRINT '✓ Digital Business Cards with Full Customization';
PRINT '✓ Template System with Custom Template Builder';
PRINT '✓ Contact Forms & Lead Management';
PRINT '✓ Advanced Analytics & Reporting';
PRINT '✓ Batch Operations (QR Code Generation)';
PRINT '✓ Email Template System';
PRINT '✓ Social Media Integration';
PRINT '✓ SEO & Metadata Support';
PRINT '✓ Performance Optimization';
PRINT '✓ Data Cleanup Procedures';
PRINT '';
PRINT 'The database is ready for use with your QardX application!';
PRINT 'Connection String: Server=.;Database=QardXDB;Trusted_Connection=true;';
PRINT '===============================================';
</attempt_completion>
