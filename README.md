# 🎓 Student Portal

> A full-stack ASP.NET Core MVC web application for managing student information, marks, and academic results with role-based access.

---

## 📖 Description

The **Student Portal** is a comprehensive web application designed to manage student data efficiently. It allows administrators to handle student records, marks, and results, while students can securely access and view their performance.

This project demonstrates real-world implementation of:

- CRUD operations  
- Role-based authentication (Admin & Student)  
- Dynamic UI interactions  
- Database integration using ADO.NET  
- Clean architecture using Repository Pattern  

---

## 🎯 Objective

- Build a real-time student management system  
- Implement ASP.NET Core MVC architecture  
- Practice database operations with SQL Server  
- Develop a structured and scalable application  

---

## 🚀 Features

### 👨‍💼 Admin Module
- Secure Admin Login  
- Add Student Details  
- Enter and Manage Marks  
- Edit Student Information  
- View Student List  
- Search & Filter Students  
- Dashboard with analytics  

### 👨‍🎓 Student Module
- Login using Ack No & Date of Birth  
- Search Result using Ack No & Mobile  
- View Marks & Result  
- Print Result  

### 📊 Dashboard
- Total Students  
- Passed Students  
- Failed Students  
- Average Performance  

---

## ⚙️ Advanced Functionalities

- Auto-generated Acknowledgement Number  
- Dynamic Location Selection (District → Taluk → Village → Pincode)  
- Automatic Result Calculation  
- Pass/Fail Evaluation Logic  
- Real-time Search Filtering  
- Session-based Authentication  
- Anti-forgery Token Validation  

---

## 🛠️ Technologies Used

### Frontend
- HTML  
- CSS  
- JavaScript  
- Bootstrap  

### Backend
- ASP.NET Core MVC  
- C#  
- Razor Views  

### Database
- SQL Server  
- ADO.NET  

---

## 🏗️ Architecture

View (UI)  
↓  
Controller (Logic)  
↓  
Repository (Data Access)  
↓  
SQL Server Database  

---

## 📂 Project Structure

Student-Portal/
│
├── Controllers/
├── Models/
│   ├── Repository/
├── Views/
├── wwwroot/
├── appsettings.json
├── Program.cs
└── README.md

---

## 📸 Screenshots

Add screenshots in `/screenshots` folder:

- login.png  
- dashboard.png  
- student-entry.png  
- student-list.png  
- result.png  

---

## 🎥 Demo Video

Add your demo video link here:

https://your-video-link.com

---

## ⚙️ Setup Instructions

### 1. Clone Repository
git clone https://github.com/your-username/student-portal.git  

### 2. Configure Database
Update connection string in `appsettings.json`

### 3. Run Project
dotnet run  

---

## 🔑 Login Details (Sample)

### Admin
- Username: admin  
- Password: admin123  

### Student
- Ack No + Date of Birth  

---

## 📊 Database Tables

- Students  
- StudentMarks  
- StudentResults  
- Subjects  
- Users  

---

## 🧮 Result Logic

- Total = Sum of all subject marks  
- Percentage = (Total / 500) × 100  
- Pass if all subjects ≥ 35  

---

## 🛡️ Security

- Session-based authentication  
- SQL parameterized queries  
- Anti-forgery tokens  
- Input validation  

---

## 📌 Future Enhancements

- JWT Authentication  
- Cloud Deployment (Azure)  
- Responsive UI  
- Charts & Analytics  
- Export to PDF/Excel  

---

## 👨‍💻 Author

**Parthasarathi**  
Aspiring .NET Full Stack Developer  

---

## ⭐ Support

If you like this project, give it a ⭐ on GitHub!
