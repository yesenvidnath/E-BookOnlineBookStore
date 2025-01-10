
# E-Book Management System

The E-Book Management System is a web-based application designed for customers to browse, purchase, and review books. It also allows administrators to manage books, customers, and orders efficiently.

---

## Table of Contents
1. [Introduction](#introduction)
2. [System Requirements](#system-requirements)
3. [Installation Guide](#installation-guide)
   - [Setting Up the Development Environment](#setting-up-the-development-environment)
   - [Configuring the Database](#configuring-the-database)
   - [Setting Up the Web Application](#setting-up-the-web-application)
4. [Application Configuration](#application-configuration)
   - [Database Connection](#database-connection)
   - [Stripe Payment Gateway Integration](#stripe-payment-gateway-integration)
   - [Mail Configuration](#mail-configuration)
5. [Running the Application](#running-the-application)
   - [Localhost Setup](#localhost-setup)
   - [Deployment on a Server](#deployment-on-a-server)
6. [Troubleshooting](#troubleshooting)
7. [Support](#support)

---

## Introduction

The E-Book Management System simplifies the process of buying and selling books for customers and administrators. Built using ASP.NET Core MVC and MSSQL, the system provides a seamless and secure experience for managing books, orders, and customer relationships.

---

## System Requirements

### Hardware Requirements
- **Processor**: Dual Core or higher
- **RAM**: 4 GB minimum (8 GB recommended)
- **Disk Space**: 10 GB minimum

### Software Requirements
- **Operating System**: Windows 10 or higher
- **Web Server**: IIS (Internet Information Services)
- **Framework**: ASP.NET Core 8
- **Database**: Microsoft SQL Server
- **IDE**: Visual Studio 2017 or later
- **Browser**: Google Chrome, Mozilla Firefox, or Microsoft Edge

---

## Installation Guide

### Setting Up the Development Environment
1. Install **Visual Studio 2017 or later** with the following workloads:
   - ASP.NET and Web Development
   - .NET Core Cross-Platform Development
2. Install **Microsoft SQL Server** and **SQL Server Management Studio (SSMS)** for database management.
3. Install **Node.js** if required for managing front-end dependencies.

### Configuring the Database
1. Open **SQL Server Management Studio**.
2. Create a new database named `EBookStore`.
3. Execute the provided SQL script to generate the necessary tables and relationships.

### Setting Up the Web Application
1. Open the project solution in **Visual Studio**.
2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
3. Build the project:
   ```bash
   dotnet build
   ```

---

## Application Configuration

### Database Connection
1. Navigate to the `appsettings.json` file in the root directory of the project.
2. Update the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=EBookStore;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

### Stripe Payment Gateway Integration
1. Obtain Stripe API keys (publishable and secret keys).
2. Update the `appsettings.json` file:
   ```json
   "Stripe": {
     "PublishableKey": "YOUR_PUBLISHABLE_KEY",
     "SecretKey": "YOUR_SECRET_KEY"
   }
   ```

### Mail Configuration
1. Configure SMTP settings for email notifications.
2. Update the `appsettings.json` file:
   ```json
   "EmailSettings": {
     "SMTPServer": "smtp.gmail.com",
     "Port": 587,
     "SenderEmail": "your-email@gmail.com",
     "SenderPassword": "your-email-password"
   }
   ```

---

## Running the Application

### Localhost Setup
1. Open the project in **Visual Studio**.
2. Set the project as the startup project.
3. Run the application:
   ```bash
   dotnet run
   ```
4. Access the application at `http://localhost:5000`.

### Deployment on a Server
1. Publish the application:
   - Right-click the project in Visual Studio and select **Publish**.
2. Configure IIS:
   - Add a new site in IIS and point it to the published folder.
   - Bind the site to a domain or localhost.
3. Start the site and access it via a browser.

---

## Troubleshooting

### Common Issues
1. **Database Connection Errors**:
   - Verify the database server is running and the connection string is correct.
2. **NuGet Package Issues**:
   - Restore packages:
     ```bash
     dotnet restore
     ```
3. **Application Not Starting**:
   - Ensure all dependencies are installed, and the project builds without errors.
4. **Payment Gateway Errors**:
   - Verify Stripe API keys and account status.
5. **Email Configuration Issues**:
   - Ensure SMTP settings are correct and the email account allows less secure app access (if using Gmail).

---

## Support

For further assistance, contact the development team:  
- **Email**: support@ebookstore.com  
- **Phone**: +1-800-BOOKS-HELP  

---

This README provides a comprehensive guide to setting up, configuring, and running the E-Book Management System.
