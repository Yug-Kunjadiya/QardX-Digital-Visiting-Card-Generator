
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
| Dashboard       | <img width="1919" height="1076" alt="image" src="https://github.com/user-attachments/assets/13056300-58a7-4f66-9aa9-89d63f14a487" /> |
| Card Creator    | <img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/b87b8a89-1002-4a41-aa76-048f174120cf" /> |
| My Card | <img width="1919" height="681" alt="image" src="https://github.com/user-attachments/assets/e51e9888-ce9a-4829-9e52-a3566f79f3b6" /> |
| Analytics | <img width="1919" height="867" alt="image" src="https://github.com/user-attachments/assets/2063f25b-b9a3-4b6a-a017-e9f26f17af9d" /> |
| Admin Panel     | <img width="1919" height="867" alt="image" src="https://github.com/user-attachments/assets/27793cc7-4136-4204-8c0f-35f857240d4f" /> |

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

---
