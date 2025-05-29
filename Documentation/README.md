\
% Inventory Management System - User Guide
% CodeBaddies
% May 2025

# Table of Contents
- [1. Prerequisites](#1-prerequisites)
- [2. Installation](#2-installation)
- [3. Uninstallation](#3-uninstallation)
- [4. Quick Start](#4-quick-start)
- [5. Feature Walkthrough](#5-feature-walkthrough)
  - [5.1 Login](#51-login)
  - [5.2 Product Management](#52-product-management)
  - [5.3 Inventory Alerts](#53-inventory-alerts)
  - [5.4 Export/Import](#54-exportimport)
  - [5.5 Dashboard Usage](#55-dashboard-usage)
- [6. Troubleshooting](#6-troubleshooting)
- [7. Contact](#7-contact)

# 1. Prerequisites
- Docker Desktop installed and running
- .NET 9 SDK
- Git
- VS Code or other editor
- Access credentials (DB password, etc.)

# 2. Installation
```bash
git clone https://github.com/YourTeam/ims.git
cd ims
.\install.ps1  # On Windows PowerShell
# or
./install.sh   # On Linux/macOS
```

# 3. Uninstallation
```bash
.\uninstall.ps1
# or
./uninstall.sh
```

# 4. Quick Start
```bash
docker-compose up
# Access at http://localhost:PORT
```

# 5. Feature Walkthrough

## 5.1 Login
1. Navigate to the login page.
2. Use a demo account or register.

## 5.2 Product Management
- Add/edit/delete products via the Products page.
- Real-time quantity updates after scans.

## 5.3 Inventory Alerts
- Set thresholds via item modal.
- View alerts in the dashboard and calendar.

## 5.4 Export/Import
- Go to Export page.
- Choose chart type or full data.
- Click download to export as CSV or PNG.

## 5.5 Dashboard Usage
- Monitor pie chart, alerts, and scanning activity.
- Use filters to query data.

# 6. Troubleshooting
- **Docker not running**: Make sure Docker Desktop is started.
- **Login issues**: Reset password from login screen.

# 7. Contact
- Maintained by: CodeBaddies
- Email: support@ims.example.com
