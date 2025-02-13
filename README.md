# 📦 Inventory Management System (IMS)

Welcome to the **Inventory Management System (IMS)** project. IMS is a
web-based application designed to help businesses efficiently manage their
inventory, orders, and reporting using the power of **Azure**, **.NET 9 STS**,
and **SQL Database**.

## 🚀 Project Overview

IMS provides an intuitive interface for managing products, tracking orders, and
generating reports. Built with scalability and performance in mind, it
leverages modern cloud technologies to ensure high availability and
reliability.

- **Frontend:** ASP.NET Core Razor Pages (.NET 9 STS)
- **Backend:** C# with Entity Framework Core
- **Database:** Azure SQL Database (NOT Avaliable), MySQL (Current)
- **Version Control:** GitHub

## 🌐 Live Demo

Visit the live application here:

[Inventory Management
System](https://inventorymanagementsystem-cceeg6b6cbgsebhk.westus-01.azurewebsites.net)

## 📂 Project Structure

IMS/

├── Pages/               # Razor Pages
(Frontend UI)

│   ├── Index.cshtml     # Home Page

│   └── ...

├── wwwroot/             # Static files
(CSS, JS, images)

├── appsettings.json     # Configuration
settings

├── Program.cs           # Entry point

└── IMS.csproj           # Project file

## 🛠️ Getting Started

### Prerequisites

- **.NET 9 SDK**
- **Visual Studio Code** (with C# & Azure extensions)
- **Azure CLI** (for deployment)

### Installation

1. Clone the repository:

   git clone
   https://github.com/Delinquent-Pointer/IMS.git

   cd IMS
2. Run the application locally:

   dotnet run

   - Visit http://localhost:5000 in your
     browser.
3. Deploy to Azure:

   - Right-click the project in VS Code →
     Deploy to Web App → Select InventoryManagementSystem.

## 🗒️ Features

- ✅ User-friendly homepage with navigation links
- ✅ Azure SQL Database integration
- ✅ Cloud deployment via Azure App Services
- ✅ Role-based access control for team collaboration

Upcoming Features:

- 📦 Product management dashboard
- 📊 Real-time inventory tracking
- 🗃️ Advanced reporting and analytics

## 👥 Team Members

- **Project Lead:** [Your Name]
- **Contributors:** [Teammate 1], [Teammate 2], [Teammate 3], [Teammate 4]

## 🗂️ Branch Management

- **main**: Production-ready code
- **dev**: Active development branch

Pull requests should be submitted to the `dev` branch for code review before
merging into `main`.

## ⚡ Deployment Information

- **Azure Resource Group:** IMS_Resources
- **App Service Name:** InventoryManagementSystem
- **Database Name:** inventorymanagementsystem-database
- **Region:** West US

## 🗃️ License

This project is licensed under the [MIT License](LICENSE). (Not correct)

## 📬 Contact

For questions or support, please contact [Your Email] or open an issue on
GitHub.

> **Note:** This project is part of the CSCD 488 Senior Project at EWU.
