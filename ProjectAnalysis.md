# QardX - Digital Visiting Card Generator: Full Project Analysis

## Overview
QardX is a comprehensive ASP.NET Core MVC web application designed for creating, managing, and sharing professional digital visiting cards with integrated QR codes. The application supports user authentication, multiple templates, export options, analytics, and advanced features like batch operations, email integration, and custom template building.

## Technology Stack
- **Backend Framework**: ASP.NET Core 8.0 MVC
- **Database**: Microsoft SQL Server with Entity Framework Core
- **Authentication**: Cookie-based authentication with BCrypt.Net
- **QR Code Generation**: QRCoder library
- **PDF Export**: QuestPDF
- **Image Processing**: System.Drawing.Common
- **UI Framework**: Bootstrap 5.3 + Font Awesome icons
- **Email**: SMTP integration for notifications and sharing

## Core Features

### 1. User Authentication System
- **Registration**: Secure user account creation with email validation
- **Login/Logout**: Cookie-based authentication with 7-day session
- **Password Security**: BCrypt hashing with cost factor 12
- **Role-Based Access**: Admin and User roles with authorization controls

### 2. Digital Visiting Card Management
- **Create Cards**: Comprehensive form with personal and professional information
- **Edit Cards**: Full editing capabilities with real-time preview
- **Delete Cards**: Secure deletion with user ownership verification
- **Card Templates**: Multiple professional templates (Classic Blue, Minimal White, Modern Dark)

### 3. Advanced Card Features
- **Social Media Integration**: LinkedIn, Instagram, Twitter, Facebook, Website links
- **Logo Upload**: File upload service for company logos/photos
- **Professional Details**: Job title, skills, languages, availability status
- **Customization**: Primary/secondary colors, font family, card orientation
- **Analytics**: View count tracking and last viewed timestamps

### 4. QR Code Functionality
- **vCard Generation**: Enhanced vCard format with complete contact information
- **QR Code Images**: High-quality PNG generation with error correction
- **Public Sharing**: Shareable URLs for public card viewing
- **Batch QR Generation**: Bulk QR code creation with multiple formats (PNG, SVG, JPEG)

### 5. Export Options
- **PDF Export**: Professional layout using QuestPDF
- **PNG Export**: High-resolution images for web use
- **SVG Export**: Vector format for scalability
- **JPEG Export**: Compressed format with quality controls
- **Batch Export**: ZIP file downloads for multiple cards

### 6. Analytics Dashboard
- **Card Views**: Track view counts and viewing patterns
- **Detailed Analytics**: Per-card analytics with viewer information
- **Admin Dashboard**: System-wide statistics and user management

### 7. Email Integration
- **SMTP Configuration**: Professional email sending capabilities
- **Card Sharing**: Email cards with attachments
- **Contact Forms**: Lead capture through embeddable forms
- **Notifications**: Email alerts for contact submissions

### 8. Custom Template Builder
- **Drag-and-Drop Interface**: Visual template creation
- **Live Preview**: Real-time HTML/CSS preview
- **Template Management**: Save, load, and edit custom templates
- **HTML/CSS Generation**: Dynamic template code generation

### 9. Contact Form System
- **Embeddable Forms**: JavaScript embed code for websites
- **Lead Management**: Admin interface for managing submissions
- **Professional Styling**: Branded contact forms
- **Email Notifications**: Automatic notifications to card owners

### 10. Admin Features
- **User Management**: Role assignment and user administration
- **Contact Submissions**: Manage and respond to contact forms
- **System Statistics**: Dashboard with key metrics
- **Template Oversight**: Manage custom templates

## Database Schema

### Users Table
- `UserId` (Primary Key)
- `FullName`
- `Email` (Unique)
- `PasswordHash`
- `Role` (Admin/User)
- `CreatedAt`

### VisitingCards Table
- `CardId` (Primary Key)
- `UserId` (Foreign Key)
- `FirstName`, `LastName`
- `Email`, `Phone`
- `Company`, `JobTitle`, `Address`
- `LinkedIn`, `Instagram`, `Twitter`, `Facebook`, `Website`
- `Skills`, `Languages`, `AvailabilityStatus`
- `PrimaryColor`, `SecondaryColor`, `FontFamily`, `CardOrientation`
- `LogoPath`, `TemplateId`
- `ViewCount`, `LastViewed`, `CreatedAt`

### Templates Table
- `TemplateId` (Primary Key)
- `TemplateName`, `FilePath`

### CustomTemplates Table
- `CustomTemplateId` (Primary Key)
- `UserId`, `TemplateName`
- `HtmlContent`, `CssContent`
- `IsPublic`, `CreatedAt`

### ContactForms Table
- `ContactFormId` (Primary Key)
- `CardId`, `Name`, `Email`, `Phone`, `Company`, `Message`
- `SubmittedAt`, `IsRead`

### CardAnalytics Table
- `AnalyticsId` (Primary Key)
- `CardId`, `ViewDate`
- `ViewerIP`, `UserAgent`

## Application Architecture

### Controllers
- **AccountController**: User authentication (Login, Register, Logout)
- **CardController**: Card CRUD operations and QR code generation
- **HomeController**: Landing page and basic navigation
- **ExportController**: PDF/PNG export functionality
- **AdminController**: Admin dashboard and user management
- **AnalyticsController**: Analytics viewing and reporting
- **BatchController**: Bulk QR code operations
- **ContactController**: Contact form management
- **TemplateBuilderController**: Custom template creation
- **PublicViewController**: Public card viewing without authentication

### Services
- **QRCodeService**: QR code and vCard generation
- **ExportService**: Multi-format export capabilities
- **EmailService**: SMTP email sending and templates
- **FileUploadService**: Secure file upload handling
- **AnalyticsService**: View tracking and reporting
- **BatchQRService**: Bulk QR code processing

### Models
- **User**: User account information
- **VisitingCard**: Complete card data with all fields
- **Template**: Template definitions
- **VisitingCardViewModel**: View model for card operations
- **LoginViewModel/RegisterViewModel**: Authentication models
- **AdminDashboardViewModel**: Admin statistics
- **BatchModels**: Batch operation models
- **CardAnalytics**: Analytics data

### Views
- **Account/**: Login and registration forms
- **Card/**: Card management (Index, Create, Edit, Preview)
- **Home/**: Landing page and privacy policy
- **Admin/**: Admin dashboard and contact submissions
- **Analytics/**: Analytics dashboard and details
- **Batch/**: Batch QR code interface
- **Contact/**: Contact form views
- **TemplateBuilder/**: Template creation interface
- **PublicView/**: Public card display
- **Shared/**: Layout, error pages, partials

## Security Features
- **Authentication**: Cookie-based with secure settings
- **Authorization**: Role-based access control
- **CSRF Protection**: Anti-forgery tokens on forms
- **Input Validation**: Model validation and sanitization
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **Password Hashing**: BCrypt with high cost factor
- **File Upload Security**: Type and size validation

## Performance Optimizations
- **Memory Caching**: Frequently accessed data caching
- **Database Optimization**: Efficient EF Core queries
- **Static Asset Compression**: Optimized CSS/JS delivery
- **Responsive Images**: Adaptive image loading
- **Async Operations**: Non-blocking I/O operations

## Configuration
- **appsettings.json**: Database connection, SMTP settings
- **Program.cs**: Service registration and middleware configuration
- **EF Core**: Database context and migrations

## Deployment Requirements
- .NET 8.0 SDK
- SQL Server (Express or full)
- SMTP server for email functionality
- File system permissions for uploads

## User Workflow
1. **Registration**: Create account with email/password
2. **Login**: Authenticate and access dashboard
3. **Create Card**: Fill comprehensive form with professional details
4. **Customize**: Choose template and upload logo
5. **Preview**: View card with generated QR code
6. **Export**: Download PDF/PNG or share via email
7. **Analytics**: Monitor card views and engagement
8. **Manage**: Edit, delete, or create additional cards

## Admin Workflow
1. **Login**: Access with admin role
2. **Dashboard**: View system statistics
3. **User Management**: Assign roles and manage users
4. **Contact Forms**: Review and respond to submissions
5. **Template Oversight**: Manage custom templates
6. **System Monitoring**: Track overall usage

## API Endpoints (Controller Actions)
- `GET /Account/Login` - Login page
- `POST /Account/Login` - Authenticate user
- `GET /Account/Register` - Registration page
- `POST /Account/Register` - Create account
- `GET /Card/Index` - List user's cards
- `GET /Card/Create` - Create card form
- `POST /Card/Create` - Save new card
- `GET /Card/Edit/{id}` - Edit card form
- `POST /Card/Edit` - Update card
- `GET /Card/Preview/{id}` - Preview card
- `GET /Card/GetQRCode/{id}` - Download QR code
- `DELETE /Card/Delete/{id}` - Delete card
- `GET /Export/Pdf/{id}` - Export as PDF
- `GET /Export/Png/{id}` - Export as PNG
- `GET /Admin/Dashboard` - Admin dashboard
- `GET /Analytics/Dashboard` - Analytics overview
- `GET /Batch/Index` - Batch operations
- `GET /Contact/Embed/{id}` - Contact form embed
- `GET /TemplateBuilder/Builder` - Template builder
- `GET /PublicView/ViewCard/{id}` - Public card view

## File Structure
```
QardX/
├── Controllers/          # MVC Controllers (10 controllers)
├── Models/              # Data Models (15+ models)
├── Views/               # Razor Views (50+ views)
├── Services/            # Business Logic (8 services)
├── Data/                # EF Core Context
├── wwwroot/             # Static Assets (CSS, JS, Images)
├── QardXDB.sql         # Database Schema
├── appsettings.json     # Configuration
├── Program.cs           # Application Entry Point
└── QardX.csproj        # Project File
```

## Key Technical Implementations

### QR Code Generation
- Uses QRCoder library for high-quality PNG output
- Generates vCard 3.0 format with complete contact data
- Includes social media URLs and professional information
- Error correction level Q for reliability

### PDF Export
- QuestPDF for professional document generation
- Responsive layouts matching web templates
- High-resolution output suitable for printing

### Email System
- SMTP configuration with SSL support
- HTML email templates with branding
- Attachment support for card sharing
- Professional formatting and error handling

### File Upload
- Secure file handling with type validation
- Organized storage in wwwroot/uploads/
- Logo deletion and replacement support
- Size limits and security checks

### Analytics Tracking
- Automatic view count increment on public access
- IP and user agent logging
- Date-based analytics aggregation
- Admin reporting interface

## Conclusion
QardX is a feature-rich, enterprise-ready digital visiting card platform that combines modern web technologies with professional business requirements. The application successfully implements user management, card creation, QR code integration, multiple export formats, analytics, email functionality, and advanced features like batch operations and custom templates. With its comprehensive feature set and robust architecture, QardX serves as a complete solution for digital business card management in the modern professional landscape.
