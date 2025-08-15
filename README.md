# DrivePal - Driving Instructor Booking Platform (University Integrated Project 3)


<div align ="center">
 <img src ="wwwroot/img/logo.png" alt="DrivePal Logo" width="200">
</div>


A comprehensive web application connecting learners with driving instructors, featuring real-time communication, interactive mapping, and seamless booking management.

## 🚗 Features

- **Role-Based Authentication**: Separate portals for learners and driving instructors
- **Interactive Maps**: Google Maps integration showing instructor locations and availability
- **Real-Time Messaging**: SignalR-powered chat system for learner-instructor communication
- **Smart Booking System**: FullCalendar integration for scheduling lessons
- **Payment Processing**: Stripe integration for secure payment handling
- **Instant Notifications**: SMS and email confirmations via Twilio and MailKit
- **Responsive Design**: Bootstrap-powered UI optimised for desktop and mobile.

##  Tech Stack

### Backend
- **Framework**: ASP.NET Core MVC
- **Language**: C#
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core (Code-First)
- **Authentication**: ASP.NET Core Identity
- **Query Language**: LINQ

### Frontend
- **Markup**: Razor HTML
- **Styling**: CSS, Bootstrap
- **Scripting**: JavaScript
- **Real-time Communication**: SignalR

### APIs & External Services
- **Google Maps API**: Location services and mapping
- **Stripe API**: Payment processing
- **Twilio API**: SMS notifications
- **MailKit**: Email notifications
- **FullCalendar**: Interactive scheduling

## 🏗 Architecture

The application follows the **Model-View-Controller (MVC)** pattern with:
- **Models**: Entity Framework entities representing data structure
- **Views**: Razor pages with Bootstrap styling
- **Controllers**: Business logic and API endpoints

## 📋 Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)
- [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Docker](https://www.docker.com/) (optional, for containerised database)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/drivepal.git
cd drivepal
```

### 2. Configure Database Connection
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DrivePalDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Set Up API Keys
Create an `appsettings.json` file with your API keys:
```json
{
  "GoogleMaps": {
    "ApiKey": "your-google-maps-api-key"
  },
  "Stripe": {
    "PublishableKey": "your-stripe-publishable-key",
    "SecretKey": "your-stripe-secret-key"
  },
  "Twilio": {
    "AccountSid": "your-twilio-account-sid",
    "AuthToken": "your-twilio-auth-token",
    "PhoneNumber": "your-twilio-phone-number"
  }
}
```

### 4. Install Dependencies
```bash
dotnet restore
```

### 5. Run Database Migrations
```bash
dotnet ef database update
```

### 6. Seed Initial Data (Optional)
The application includes a database initialiser that seeds prototype data for testing purposes.

### 7. Run the Application
```bash
dotnet run
```

Navigate to `https://localhost:5001` or `http://localhost:5000`

## 🐳 Docker Setup (Alternative)

If using Docker for SQL Server:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:latest
```

## 📱 Usage

### For Learners:
1. Register as a learner
2. Browse available instructors on the interactive map
3. View instructor profiles and availability
4. Book lessons through the calendar interface
5. Make payments securely via Stripe
6. Communicate with instructors through real-time chat
7. Receive SMS and email confirmations

### For Instructors:
1. Register as an instructor
2. Set up profile with location and availability
3. Manage bookings through the calendar system
4. Communicate with learners via integrated messaging
5. Receive notifications for new bookings


