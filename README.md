# QardX - Digital Visiting Card Generator

QardX is a comprehensive ASP.NET Core MVC web application that allows users to create, manage, and share professional digital visiting cards with QR codes, advanced analytics, and multiple export options.

## 🚀 Features

### Core Features
- **User Authentication**: Secure registration and login with BCrypt password hashing
- **Digital Card Creation**: Create personalized visiting cards with complete contact information
- **Multiple Templates**: Choose from Classic Blue, Minimal White, and Modern Dark designs
- **QR Code Integration**: Automatic QR code generation with vCard data for easy contact sharing
- **Export Options**: Download cards as high-quality PDF or PNG files
- **Responsive Design**: Modern Bootstrap UI that works on all devices
- **Live Preview**: Real-time card preview while editing

### Advanced Features
- **Analytics Dashboard**: Track card views, engagement metrics, and visitor insights
- **Social Media Integration**: Support for LinkedIn, Instagram, Twitter, Facebook profiles
- **Batch Operations**: Bulk export and management of multiple cards
- **Email Sharing**: Send cards directly to contacts via email
- **Public Card URLs**: Shareable links for your digital cards
- **Custom Templates**: Template builder for creating custom card designs
- **Contact Forms**: Embedded contact forms on public card pages
- **Admin Panel**: Comprehensive admin dashboard for user and content management

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: Microsoft SQL Server with Entity Framework Core
- **Authentication**: Cookie-based authentication with BCrypt.Net
- **QR Codes**: QRCoder library for high-quality QR code generation
- **PDF Export**: QuestPDF for professional PDF generation
- **Image Processing**: System.Drawing.Common for image manipulation
- **UI Framework**: Bootstrap 5.3 + Font Awesome icons
- **Email Service**: SMTP integration for email functionality
- **Analytics**: Custom analytics engine with detailed reporting

## 📋 Prerequisites

- .NET 8.0 SDK or later
- SQL Server (SQL Server Express LocalDB or full SQL Server)
- Visual Studio 2022 or VS Code with C# extension
- Internet connection for CDN resources (Bootstrap, Font Awesome)

## 🔧 Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/Yug-Kunjadiya/QardX-Digital-Visiting-Card-Generator.git
cd QardX-Digital-Visiting-Card-Generator
```

### 2. Database Setup

#### Option A: Using the provided SQL script
1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Open and execute the `CreateDatabase_QardXDB_Complete.sql` file to create the database and tables

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
=======
# 💳 QardX - Digital Visiting Card Generator

QardX is a **Digital Visiting Card Generator** built using **ASP.NET Core MVC** and **SQL Server**.  
It allows users to **create, customize, manage, and share** professional visiting cards online.  
Each card comes with a **unique QR code**, **PDF download**, and **smart sharing options**.

---

## 🚀 Features

### 👤 User Features
- Create your own digital visiting card in minutes.
- Customize fields like name, title, company, contact info, and social links.
- Upload profile photo and company logo.
- Generate and download a **QR code** for your card.
- Export card as **PDF**.
- Share card link via **email or social media**.
- Real-time **card preview** before saving.

### 🛠️ Admin Features
- Manage all user accounts and cards.
- Approve or reject card requests.
- View total cards created and user statistics.
- Generate analytics reports (downloads, shares, etc.).
- Manage templates and categories.

### 📱 Additional Functionalities
- Multiple card templates with responsive design.
- Integrated **authentication** (login/register/reset password).
- Email notifications (optional via SMTP).
- Data validation and error handling.
- Built-in **search and filter** for easy card management.

---

## 🏗️ Tech Stack

| Layer | Technology |
|-------|-------------|
| **Frontend** | ASP.NET Core MVC with Razor Pages, Bootstrap 5 |
| **Backend** | ASP.NET Core Web API (C#) |
| **Database** | Microsoft SQL Server |
| **ORM** | Entity Framework Core |
| **Authentication** | ASP.NET Identity + BCrypt.Net |
| **Other Tools** | QR Code Generator API, PDF Export (iTextSharp), SMTP for Emails |

---

## ⚙️ Project Setup

### 🧩 Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio / VS Code](https://visualstudio.microsoft.com/)
- [Git](https://git-scm.com/)

---

### 🪜 Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/YOUR-USERNAME/QardX-Digital-Visiting-Card-Generator.git
   cd QardX-Digital-Visiting-Card-Generator
   ```
2. **Setup the Database**

   Open SQL Server Management Studio.

   Create a new database named: QardXDB.

   Run the following script:

   ```sql
   CREATE DATABASE QardXDB;
   USE QardXDB;

   CREATE TABLE Users (
       UserID INT PRIMARY KEY IDENTITY,
       FullName NVARCHAR(100),
       Email NVARCHAR(100) UNIQUE,
       PasswordHash NVARCHAR(255),
       CreatedAt DATETIME DEFAULT GETDATE()
   );

   CREATE TABLE VisitingCards (
       CardID INT PRIMARY KEY IDENTITY,
       UserID INT FOREIGN KEY REFERENCES Users(UserID),
       FullName NVARCHAR(100),
       JobTitle NVARCHAR(100),
       CompanyName NVARCHAR(100),
       Phone NVARCHAR(20),
       Email NVARCHAR(100),
       Website NVARCHAR(100),
       Address NVARCHAR(255),
       QRCode NVARCHAR(MAX),
       CreatedAt DATETIME DEFAULT GETDATE()
   );
   ```

3. **Update the Connection String**
   
   In `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_server_name ;Database=QardXDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. **Run the Application**

   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

5. **Access the Web App**

   Open your browser and go to:  
   👉 https://localhost:5001 or http://localhost:5000

---

## 🧭 Folder Structure

```pgsql
QardX-Digital-Visiting-Card-Generator/
│
├── Controllers/
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── CardController.cs
│   └── AdminController.cs
│
├── Models/
│   ├── User.cs
│   ├── VisitingCard.cs
│   ├── Template.cs
│
├── Views/
│   ├── Home/
│   ├── Account/
│   ├── Cards/
│   └── Admin/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── images/
│
├── appsettings.json
├── Program.cs
├── Startup.cs
└── QardX-Digital-Visiting-Card-Generator.csproj
```

---

## 🖼️ Screenshots

| Feature         | Screenshot   |
|-----------------|-------------|
| Dashboard       | ![Dashboard Screenshot](#) |
| Card Creator    | ![Card Creator Screenshot](#) |
| Admin Panel     | ![Admin Panel Screenshot](#) |

---

## 🧠 Future Enhancements

- Add AI-based smart card design recommendations.
- Integrate payment gateway for premium templates.
- Add NFC (Tap-to-Share) support.
- Allow card embedding in LinkedIn or portfolio websites.

---

## 🤝 Contributing

Contributions are always welcome!

To contribute:

1. Fork the repo
2. Create a new branch: `git checkout -b feature-name`
3. Commit changes: `git commit -m "Added new feature"`
4. Push: `git push origin feature-name`
5. Open a Pull Request

---

## 🧑‍💻 Author

👋 Developed by: Yug Kunjadiya  
📧 Email: yugkunjadiy007@gmail.com  
🌐 GitHub: github.com/Yug-Kunjadiya

---

## 📄 License

## 📄 License

This project is licensed under the MIT License.  
Feel free to use and modify it as per your needs.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📧 Contact

**Yug Kunjadiya** - [GitHub Profile](https://github.com/Yug-Kunjadiya)

Project Link: [https://github.com/Yug-Kunjadiya/QardX-Digital-Visiting-Card-Generator](https://github.com/Yug-Kunjadiya/QardX-Digital-Visiting-Card-Generator)

---

⭐ If you like this project, don't forget to star this repository on GitHub!
