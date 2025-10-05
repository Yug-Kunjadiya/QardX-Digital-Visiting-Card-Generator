# QardX - Digital Visiting Card Generator

QardX is a modern ASP.NET Core MVC web application that allows users to create professional digital visiting cards with QR codes and export them as PDF or PNG files.

## Features

- **User Authentication**: Secure registration and login with BCrypt password hashing
- **Digital Card Creation**: Create personalized visiting cards with contact information
- **Multiple Templates**: Choose from Classic Blue, Minimal White, and Modern Dark designs
- **QR Code Integration**: Automatic QR code generation with vCard data
- **Export Options**: Download cards as high-quality PDF or PNG files
- **Responsive Design**: Modern Bootstrap UI that works on all devices
- **Live Preview**: Real-time card preview while editing

## Technology Stack

- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: Microsoft SQL Server with Entity Framework Core
- **Authentication**: Cookie-based authentication with BCrypt.Net
- **QR Codes**: QRCoder library
- **PDF Export**: QuestPDF
- **Image Processing**: System.Drawing.Common
- **UI Framework**: Bootstrap 5.3 + Font Awesome icons

## Prerequisites

- .NET 8.0 SDK
- SQL Server (SQL Server Express LocalDB or full SQL Server)
- Visual Studio 2022 or VS Code with C# extension

## Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd QardX
```

### 2. Database Setup

#### Option A: Using the provided SQL script
1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance (`YUG-KUNJADIYA\SQLEXPRESS`)
3. Open and execute the `QardXDB.sql` file to create the database and tables

#### Option B: Using Entity Framework Migrations
```bash
dotnet ef database update
```

### 3. Update Connection String
Update the connection string in `appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YUG-KUNJADIYA\\SQLEXPRESS;Database=QardXDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### 4. Restore NuGet Packages
```bash
dotnet restore
```

### 5. Build and Run
```bash
dotnet build
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
QardX/
├── Controllers/           # MVC Controllers
│   ├── AccountController.cs    # User authentication
│   ├── CardController.cs       # Visiting card management
│   ├── ExportController.cs     # PDF/PNG export
│   ├── HomeController.cs       # Home page
│   └── AdminController.cs      # Admin dashboard
├── Models/               # Data models and view models
├── Views/                # Razor views
│   ├── Account/          # Login/Register views
│   ├── Card/             # Card management views
│   ├── Home/             # Home page views
│   └── Shared/           # Layout and shared views
├── Services/             # Business logic services
│   ├── QRCodeService.cs  # QR code generation
│   └── ExportService.cs  # PDF/PNG export
├── Data/                 # Entity Framework context
├── wwwroot/              # Static files (CSS, JS, images)
└── QardXDB.sql          # Database creation script
```

## Usage

### 1. User Registration
- Navigate to the registration page
- Create an account with your full name, email, and password
- Passwords are securely hashed using BCrypt

### 2. Create a Visiting Card
- Log in to your account
- Click "Create New Card"
- Fill in your professional details:
  - Phone number
  - Company name
  - LinkedIn profile
  - Address
- Select a template (Classic Blue, Minimal White, or Modern Dark)
- Preview your card in real-time

### 3. Export Your Card
- After creating a card, use the preview feature
- Export as PDF for printing or high-quality sharing
- Export as PNG for web use or social media

### 4. Manage Your Cards
- View all your created cards in the dashboard
- Edit existing cards
- Delete cards you no longer need

## Database Schema

### Users Table
- `UserId` (Primary Key)
- `FullName`
- `Email` (Unique)
- `PasswordHash`
- `CreatedAt`

### Templates Table
- `TemplateId` (Primary Key)
- `TemplateName`
- `FilePath`

### VisitingCards Table
- `CardId` (Primary Key)
- `UserId` (Foreign Key)
- `Phone`
- `LinkedIn`
- `Company`
- `Address`
- `TemplateId` (Foreign Key)
- `CreatedAt`

## Configuration

### Authentication Settings
The application uses cookie-based authentication with the following settings:
- Login path: `/Account/Login`
- Logout path: `/Account/Logout`
- Session duration: 7 days (sliding expiration)

### QR Code Features
- Generates vCard format data
- Includes name, email, phone, company, and LinkedIn
- High-quality PNG output
- Configurable error correction level

### Export Options
- **PDF Export**: Professional layout with QuestPDF
- **PNG Export**: High-resolution image with System.Drawing
- **Templates**: Styled according to selected template

## Templates

### Classic Blue
- Blue gradient background
- White text
- Professional appearance
- Suitable for corporate use

### Minimal White
- Clean white background
- Dark text with blue accents
- Modern minimalist design
- Versatile for any industry

### Modern Dark
- Dark gradient background
- Yellow/gold accents
- Contemporary design
- Great for creative professionals

## Security Features

- Password hashing with BCrypt (cost factor: 12)
- SQL injection prevention through Entity Framework
- CSRF protection on forms
- Secure cookie settings
- Input validation and sanitization

## Performance Optimizations

- Memory caching for frequently accessed data
- Optimized database queries with Entity Framework
- Compressed static assets
- Responsive image loading

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
- Mobile browsers (iOS Safari, Chrome Mobile)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support and questions, please contact:
- Email: support@qardx.com
- Issues: GitHub Issues page

## Changelog

### Version 1.0.0
- Initial release
- User authentication system
- Visiting card creation and management
- QR code generation
- PDF and PNG export
- Three professional templates
- Responsive design
- Admin dashboard

---

**QardX - Digital Visiting Card Generator**  
*Create professional digital visiting cards with QR codes in minutes*