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

This project is licensed under the MIT License.  
Feel free to use and modify it as per your needs.

---

⭐ If you like this project, don't forget to star this repository on GitHub!

---
---

✅ This README:
- Uses perfect Markdown structure  
- Includes sections for setup, features, screenshots, and contribution  
- Works beautifully in GitHub preview  

Would you like me to also generate a **small “About” section + badges (like .NET, SQL, License, etc.)**  at the top for a more professional GitHub look?

The following references were attached as context:

File path: README.md
Source URL: https://github.com/Yug-Kunjadiya/QardX-Digital-Visiting-Card-Generator/blob/66f3df687ccee9d3db36a2c694b082b9ae89c843/README.md
Repo: Yug-Kunjadiya/QardX-Digital-Visiting-Card-Generator
CommitOID: 66f3df687ccee9d3db36a2c694b082b9ae89c843
BlobSha: 
```markdown
# QardX-Digital-Visiting-Card-Generator
QardX is a full-featured ASP.NET Core MVC app for creating, managing, and sharing digital visiting cards with QR codes, templates, analytics, and PDF export. Includes authentication, email sharing, an[...]

```
