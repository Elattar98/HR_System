# HR System - Employee Management Platform
## Overview
The HR System is a web-based employee management system built with ASP.NET MVC. It streamlines HR processes like employee onboarding, leave management, payroll, and performance reviews. The system includes role-based authentication and a responsive UI for efficient management.

---

## Features
- 🧑‍💼 Employee Management: Add, edit, and remove employees' records.
- 🗓️ Leave Management: Request, approve, and track leave requests.
- 💰 Payroll System: Manage salary details and generate pay slips.
- 📊 Performance Reviews: Conduct and review employee appraisals.
- 🔐 Role-Based Access: Admin, HR, and employee roles with different permissions.

---

## Technologies Used
### Core Stack
- Framework: ASP.NET MVC (.NET Core)
- Languages: C#, HTML, CSS, JavaScript
- Database: SQL Server (Entity Framework Core)
- Authentication: Identity Framework
### UI & Front-End Libraries
- Bootstrap (Responsive UI)
- jQuery (Dynamic interactions)
- Razor Views (Server-side rendering)

---

## Installation and Setup
### Clone the Repository:

```bash
git clone https://github.com/Elattar98/HR-System.git
cd HR-System
```
### Set Up the Database:

- Open SQL Server Management Studio (SSMS).
- Create a new database (e.g., HRSystemDb).
- Update the connection string in appsettings.json.

### Build the Project:

- Open the solution in Visual Studio 2022 or newer.
- Restore NuGet packages:
```bash
dotnet restore
```
### Run Database Migrations:

```bash
dotnet ef database update
```
### Run the Application:

- Press F5 or use the terminal:
```bash
dotnet run
```
### Access the Application:
- Open http://localhost:5000 in your web browser.

---

## Project Structure
```bash
HR-System/
  ├── Controllers/         # Application Controllers
  ├── Models/              # Data Models (Entity Framework)
  ├── Views/               # Razor Views for UI
  ├── Migrations/          # Database Migrations
  ├── wwwroot/             # Static Files (CSS, JS, Images)
  ├── Data/                # Database Context
  ├── appsettings.json     # Application Configuration
  ├── Program.cs           # Application Entry Point
  └── README.md            # Project Overview
```

## Contact
### Author: Abd Elrahman Elattar
- 📧 Email: abdelrahmanelattar79@gmail.com
- 🔗 GitHub Profile | LinkedIn Profile

