% Inventory Management System (IMS)  Official User Guide  
% Team CodeBaddies  
% May 2025  

### Project Contributors
- [@Delinquent-Pointer](https://github.com/Delinquent-Pointer)  
- [@Jared-Schimpf](https://github.com/Jared-Schimpf)  
- [@Pbrown34](https://github.com/Pbrown34)  

# Table of Contents
- [Table of Contents](#table-of-contents)
- [1. Prerequisites](#1-prerequisites)
- [2. Installation](#2-installation)
    - [Step 1: Download the IMS Docker Bundle](#step-1-download-the-ims-docker-bundle)
  - [](#)
    - [Step 2: Extract the ZIP File](#step-2-extract-the-zip-file)
    - [Step 3: Open PowerShell in the Project Directory](#step-3-open-powershell-in-the-project-directory)
    - [Step 4: Ensure Docker Desktop Is Running in Linux Mode](#step-4-ensure-docker-desktop-is-running-in-linux-mode)
    - [Step 5: Run the Installation Script](#step-5-run-the-installation-script)
- [3. Uninstallation](#3-uninstallation)
    - [Step 1: Locate `uninstall.ps1`](#step-1-locate-uninstallps1)
    - [Step 2: Run the Script in PowerShell](#step-2-run-the-script-in-powershell)
    - [Step 3: Manually Verify in Docker Desktop](#step-3-manually-verify-in-docker-desktop)
  - [](#-1)
    - [Done](#done)
- [4. Quick Start](#4-quick-start)
- [5. Feature Walkthrough](#5-feature-walkthrough)
  - [5.1 Login](#51-login)
  - [5.2 Create Account](#52-create-account)
  - [5.3 Account Settings](#53-account-settings)
  - [5.4 Product Creation](#54-product-creation)
  - [5.5 Product Management](#55-product-management)
    - [5.5.1 Product Searching](#551-product-searching)
    - [5.5.2 Product Popup](#552-product-popup)
    - [5.5.2 CSV Upload \& Download](#552-csv-upload--download)
  - [5.6 Inventory Alerts](#56-inventory-alerts)
  - [5.7 Main Dashboard](#57-main-dashboard)
  - [5.8 Sales Page](#58-sales-page)
- [6. Additional Information](#6-additional-information)
  - [6.1 Password Requirements](#61-password-requirements)
  - [6.2 SKU Requirements](#62-sku-requirements)
- [7. Troubleshooting](#7-troubleshooting)
  - [Docker Issues](#docker-issues)
  - [PowerShell Issues](#powershell-issues)
  - [Login / UI Issues](#login--ui-issues)
  - [Database Issues](#database-issues)
  - [General Tips](#general-tips)
- [8. Contact](#8-contact)
- [9. License \& Academic Use](#9-license--academic-use)
    - [Usage Notice](#usage-notice)
    - [Educational Scope](#educational-scope)
    - [Contact for Permissions](#contact-for-permissions)

# 1. Prerequisites

Before installing or running the Inventory Management System (IMS), ensure the following tools and access credentials are available:

- **Docker Desktop**  
  Ensure Docker is installed and running on your system.  
  [Download Docker Desktop](https://www.docker.com/products/docker-desktop/)

- **.NET SDK 9.0.301**  
  Required to build and run the IMS web application locally.  
  [Download .NET 9.0.301](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

- **Git**  
  Git is required to clone the project repository.  
  [Download Git](https://git-scm.com/downloads)

- **Code Editor (Recommended: Visual Studio Code)**  
  For editing, running, and debugging the project.  
  [Download VS Code](https://code.visualstudio.com/)

- **GitHub Access**  
  - **Developer Repository**: [Delinquent-Pointer/IMS](https://github.com/Delinquent-Pointer/IMS)  
  - **Public Docker Showcase**: [IMS-Docker](https://github.com/Delinquent-Pointer/IMS-Docker)

- **Access Credentials (Admin/Database) [Only if building Azure cloud service]**  
  Some functionality requires access credentials (e.g., admin password, database connection string). These can be obtained from your Azure portal or deployment lead.  
  [Access Azure Portal](https://portal.azure.com/)

# 2. Installation

Follow these steps to install and launch the Inventory Management System (IMS) using the latest Docker bundle.

---

###  Step 1: Download the IMS Docker Bundle

- Go to the latest release on GitHub:  
  [IMS Docker Releases](https://github.com/Delinquent-Pointer/IMS-Docker/releases)
- Download the `.zip` file containing the Docker images and setup scripts.

![Step 1.1 - Download ZIP](install_1_1.png)
![Step 1.2 - Download ZIP](install_1_2.png)
---

###  Step 2: Extract the ZIP File

- Right-click the downloaded archive and select **Extract All**
- Choose a destination (e.g., your Desktop or `Documents\IMS`)
---

###  Step 3: Open PowerShell in the Project Directory

- Navigate to the extracted folder that contains `install.ps1` ...IMS_Docker_Offline -> offline-images
- Right-click in the folder and choose **Open in Terminal** or **Open PowerShell window here**

![Step 3 - Open PowerShell](install_3.png)

---

###  Step 4: Ensure Docker Desktop Is Running in Linux Mode

- Launch Docker Desktop from your Start menu or tray icon
- Ensure its set to **Linux containers**
  - Right-click the Docker icon in the taskbar  "Switch to Linux containers" (if needed)

![Step 4 - Docker Linux Mode](install_4.png)

---

###  Step 5: Run the Installation Script

In PowerShell, type the following and press `Enter`:

```powershell
./install.ps1
```
![Step 5 - Docker Linux Mode](install_5.png)

---

# 3. Uninstallation

To remove the Inventory Management System (IMS) from your local environment, follow the steps below:

---

###  Step 1: Locate `uninstall.ps1`

- Navigate to the same directory where you originally ran the installation script (`install.ps1`).
- You should find a file named `uninstall.ps1` in that folder.

> This script is responsible for stopping IMS Docker containers and images.

![Step 1 - Locate uninstall.ps1](uninstall_1.png)

---

###  Step 2: Run the Script in PowerShell

- Open PowerShell in the same folder where `uninstall.ps1` is located.
- Execute the following command:

```powershell
./uninstall.ps1
```

This will:
- Stop and remove Docker containers used by IMS
- Remove loaded Docker images for IMS and SQL Server

---
###  Step 3: Manually Verify in Docker Desktop 

You can verify the cleanup or remove any remaining containers/images manually:

1. Open **Docker Desktop**
2. Navigate to the **Containers** tab and confirm that IMS-related containers (e.g., `offline-images`) are removed
3. Switch to the **Images** tab and remove any leftover IMS-related images

![Step 3 - Locate uninstall.ps1](uninstall_2_1.png)
![Step 3 - Docker Manual Cleanup](uninstall_2_2.png)
---

###  Done

Your system is now clean of IMS and ready for a fresh install or full removal. You may delete the extracted folder if no longer needed.

# 4. Quick Start

> **Note:** This section is only applicable if the application is hosted on Azure. If you are running IMS locally or offline, please refer to [Installation](#2-installation).

If the IMS is already hosted on Azure, you can skip local setup and access it directly via the live deployment link below:

 **Live App**: [https://inventorymanagementsystem-cceeg6b6cbgsebhk.westus-01.azurewebsites.net](https://inventorymanagementsystem-cceeg6b6cbgsebhk.westus-01.azurewebsites.net)

Once the page loads, begin by logging in. Refer to [Section 5.1  Login](#51-login) of this guide to proceed.

---

If you're not using the Azure deployment, follow the installation steps to run the system locally using Docker:

Access at http://localhost:5000

# 5. Feature Walkthrough
This section walks through the login process for your first visit of the webpage, as well as an overview of the functionality of each webpage. 
## 5.1 Login
<table>
  <tr>
    <td>
    <b>The Login page is used to validate access the inventory system via a verified account.</b></br></br>
    This is the first page displayed wehn accessing the website, it can also be accessed via the "Login" option located on the navigation bar while signed out.<br><br>
    First Login:
      <ol>
        <li>Navigate to the login page.</li>
        <li>Use the provided demo account or a previously registered account.
      <br><b>The demo account's login is:</b>
      <ul>
        <li><b>Username: Admin</b> </li>
        <li><b>Password: Password1!</b> </li>
      </ul>
      </li>
      <li>We recommend you visit the <a href= "#53-account-settings">Account settings</a> page and change this account's information after your first login, as this information is default to all new IMS instances and may present a vulnerability.</li>
        <li>Use this account as a starting point to create more accounts with or without IT permissions.</li>
      </ol>
    </td>
    <td>
      <img src="image-1.png" alt="Login screenshot" width="500"/>
    </td>
  </tr>
</table>

## 5.2 Create Account

<table>
  <tr>
    <td>
    <b>The Create Account page is used to create new User and I.T. User accounts which are required to access the inventory system.</b><br><br>
    To access this page, press the "Create Account" button located on the Login page or the "Create Account" option located on the navigation bar while signed out.<br><br>
    Account creation:
    <ul>
      <li>To create an account, enter a unique username and a password. Password creation follows the requirements outlined in <a href= "#61-password-requirements">6.1 Password Requirements</a>.</li>
      <li>Created accounts must first be verified by an Administrator for Login capability.</li>
    </ul>
    IT Account Creation:
    <ul>
    <li>To create an I.T. Account, the Admin Key of an existing Administrator is required. It is recommended for the sake of security that Administrators create accounts for new Administrators and provide them the account details rather than distribute their own Admin Key.</li>
      <li>A fresh Admin Key will be generated for each IT account. this key can be changed by visiting the Account Settings page as an IT user.</li></ul>
    </td>
    <td>
      <img src="image.png" alt="Create Account Screenshot" width="500"/>
    </td>
  </tr>
</table>

## 5.3 Account Settings
<table>
  <tr>
    <td>
    <b>The Account Settings page provides functionality to modify existing account information for the currently logged in account.</b><br><br>
    To access this page, press the "Manage Account" option on the navigation bar.<br><br>
    Editing Account Information:
      <ul>
      <li>New usernames must be unique.
      <li>New passwords must meet the requirements outlined in <a href= "#61-password-requirements">6.1 Password Requirements.</a></li>
      <li>Administrators will see an option to edit their admin key. An updated key can be any permutation of 8 or more alphanumeric characters.
      <li>Changes made to the current account information must be confirmed with the user's current password.
      </ul>
    </td>
    <td>
      <div style="display: flex; flex-direction: column; gap: 10px;">
        <img src="accsettings.png" alt="Account Settings Screenshot" width="500"/>
        <img src="pwconfirm.png" alt="Password confirmation Screenshot" width="500"/>
      </div>
    </td>
  </tr>
</table>

## 5.4 Product Creation
<table>
  <tr>
    <td>
    <b>The Create Product page is used to create new product information which is entered into the inventory database.</b><br><br>
    To access this page, press the "Create New Product" option on the navigation bar.<br><br>
    Creating a Product:
    <ul>
      <li>To create a product, enter the relevant information into the fields.
      <li>The only required field to create a product is name. There is no uniqueness requirement for product names.
      <li> The formatting requirements for a product's SKU are outlined in <a href= "#62-sku-requirements">6.2 SKU Requirements</a>.</li>
    </ul>
    </td>
    <td>
      <img src="createprod.png" alt="Create Product Screenshot" width="400"/>
    </td>
  </tr>
</table>


## 5.5 Product Management
<table>
  <tr>
    <td>
    <b>The Inventory page provides a range of functionality for viewing, searching, and updating the inventory database.</b><br><br>
    To access this page, press the "Inventory" option on the navigation bar.<br><br>
    Included Features:
      <ul>
        <li>Inventory searching by category
        <li>Advanced Searching by multiple categories
        <li>Product information popups with editing and deletion.</li>
        <li>Direct upload of CSV tables to the inventory
        <li>Downloading the current search result to a CSV file
        </li>
      </ul>
    </td>
    <td>
        <img src="index.png" alt="Product Management Screenshot" width="800"/>
    </td>
  </tr>
</table>

### 5.5.1 Product Searching
<table>
  <tr>
    <td>
<b>Located at the top of the page is a search bar which can search the inventory list for products based off of certain categories.</b><br><br>
Search Categories:
  <ul>
    <li>Name</li>
    <li>Description</li>
    <li>Price</li>
    <li>Quantity</li>
    <li>SKU</li>
    <li>Category</li>
    <li>Location</li>
    <li>Advanced Query
  </ul>
Advanced Query:
  <ul>
    <li>The advanced Query option allows for multiple different catagories to be searched at once.</li>
    <li>Advanced queries can be typed manually or can be build using the query builder, which is accessible by pressing the "Build Query" button when the Advanced Query category is selected.
  </ul>
    <td>
        <img src="advsearch.png" alt="Advanced Search Screenshot" width="500"/>
    </td>
  </tr>
</table>

### 5.5.2 Product Popup
<table>
  <tr>
    <td>
    <b>Selecting a product from the inventory table will display a popup which lists the full product information. This popup also provides functionality to edit the individual product fields or delete the product from the database.</b><br><br>
Popup Options:
  <ul>
    <li>Pressing the "Edit" button will allow you to update all of the selected product's fields, pressing "Save Changes" will confirm these changes.</li>
    <li>Pressing the "Delete" button will prompt a secondary confirmation popup to delete the item from the database. Pressing "Confirm" will fully remove this item from the database.
  </ul>
    <td>
        <img src="popup.png" alt="Product Popup Screenshot" width="500"/>
    </td>
  </tr>
</table>

### 5.5.2 CSV Upload & Download
<table>
  <tr>
    <td>
    <b>CSV files can be directly uploaded and Downloaded form the inventory page. Uploaded CSVs will populate their table information directly into the inventory database. Downloaded CSVs are populated with the current search results.</b><br><br>
CSV Upload:
  <ul>
    <li>The Inventory page supports the ability to directly upload CSV table information to the inventory.
    <li>Uploaded CSV's are required to have a header and must be comma-separated.
    <li>The only required field for a CSV is Name, though all included fields must have a header.
    <li>Pressing the "Choose File" button will allow you to choose the file to upload.</li>
    <li>Pressing the "Upload" button will attempt to upload the file contents to the database.
    <li>A error popup will appear if the uploaded file is incorrectly formatted or contains bad data.
</ul>
CSV Download:
<ul>
  <li>The Inventory page supports the ability to directly download the current search result as a comma-separated CSV file.
  <li>Pressing the "Download" buttom wild initiate a download of the current search result as a CSV file.
  <li>Attempting to download an empty search result will instead give you a CSV of the full inventory.
    <td>
    <img src="csv.png" alt="CSV Example Screenshot" width="800"/>
    </td>
  </tr>
</table>

## 5.6 Inventory Alerts
- Set thresholds via item modal.
- View alerts in the dashboard and calendar.

## 5.7 Main Dashboard
<table>
  <tr>
    <td>
      <ul>
        <li>Monitor pie chart, alerts, and scanning activity.</li>
        <li>Use filters to query data.</li>
        <li>To use the Scanner, you must have a Camera on the device you are using.
          <ul>
            <li>It will auto-detect your Camera and ask for permission to use it.</li>
            <li>Scan Items based off of their SKU inside of the database by creating a barcode.</li>
            <li>Currently there is no integrated barcode generator.
              <ul>
                <li>The website used to create barcodes for testing purposes is: <a href="https://barcode.tec-it.com/en">Barcode.tec-it</a></li>
                <li>Any type of barcode is valid, use version 'Code-128' for best results.</li>
              </ul>
            </li>
            <li>Once finished scanning items, click the 'Complete Transaction' button.
              <ul>
                <li>This will remove the items scanned from the database, based off their quantity.</li>
                <li>It will also add a receipt to the Sales Page.</li>
              </ul>
            </li>
          </ul>
        </li>
      </ul>
    </td>
    <td>
      <img src="image-4.png" alt="Dashboard Scanner Screenshot" width="350"/>
    </td>
  </tr>
</table>

## 5.8 Sales Page
<table>
  <tr>
    <td>
      <ul>
        <li>The Sales page, found by clicking on Sales on the navigation bar, is basic.</li>
        <li>It is a page dedicated to housing Receipts made by the Scanner on the Dashboard page.</li>
        <li>It displays:
          <ul>
            <li>Product</li>
            <li>Quantity</li>
            <li>SKU</li>
            <li>Price of each item</li>
            <li>The total of each item</li>
            <li>The total of the entire purchase</li>
            <li>Transaction ID</li>
            <li>Date/Time</li>
          </ul>
        </li>
      </ul>
    </td>
    <td>
      <img src="image-5.png" alt="Sales Page Screenshot" width="700"/>
    </td>
  </tr>
</table>

# 6. Additional Information
Notable information referenced by one or more pages.
## 6.1 Password Requirements
<table>
    <td>
      The creation and modification of any account passwords must comply with the following enforced requirements.<br><br>
      <b>Password Requirements:</b>
      <ul>
        <li>At least one uppercase letter</li>
        <li>At least one lowercase letter</li>
        <li>At least one special character</li>
        <li>At least one digit</li>
        <li>No whitespace characters</li>
        <li>At least 8 Characters</li>
      </ul>
  </tr>
</table>

## 6.2 SKU Requirements
<table>
    <td>
      The creation and modification of product SKUs must follow the following enforced format requirements.<br><br>
      <b>SKU Formatting:</b>
      <ul>
        <li>SKUs are defined by multiple 'categories' separated by dashes.<br> e.g: AAAA-BBBB </li>
        <li>A category must only contain capital letters and digits</li>
        <li>The initial category of an SKU must be 1-5 characters long</li>
        <li>Any following categories are 'subcategories', and can be 1-5 characters long</li>
        <li>An SKU can have at most 4 categories,<br> i.e: a single main category and three following subcategories.</li>
      </ul>
  </tr>
</table>

# 7. Troubleshooting

This section lists common problems encountered during installation or use of the Inventory Management System (IMS), along with suggested fixes.

---

## Docker Issues

- **Docker Not Running**
  - Ensure Docker Desktop is open and running in the background.
  - If unsure, check for the Docker whale icon in the system tray.

- **Docker Not in Linux Mode**
  - IMS requires Linux containers. If Docker is set to Windows containers:
    1. Right-click the Docker icon in the system tray.
    2. Select **"Switch to Linux containers..."**.
    3. Wait for Docker to restart.

- **Containers Don't Start After Running `install.ps1`**
  - Ensure no previous IMS containers are running by opening Docker Desktop and checking under **Containers**.
  - Try running `docker-compose down` before reinstalling.

- **Ports Already in Use**
  - If `localhost:5000` or other required ports are occupied, change the port in `docker-compose.yml` or stop other running apps.

---

## PowerShell Issues

- **Scripts Not Executing (`install.ps1` or `uninstall.ps1`)**
  - Make sure you are running PowerShell **as Administrator**.
  - If blocked, run this command to allow script execution temporarily:
    ```powershell
    Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
    ```

---

## Login / UI Issues

- **Cannot Log In with Demo Credentials**
  - Double-check credentials (case-sensitive):  
    `Username: Admin`  
    `Password: Password1!`
  - Make sure the backend is up and running (visit `http://localhost:5000`).
  - Check browser console or network tab for backend errors.

- **UI Not Loading or Blank Page**
  - Ensure the frontend service container (`ims-web`) is running.
  - Check browser dev tools (F12) for any JavaScript or network errors.
  - Refresh with hard cache clear: `Ctrl + Shift + R`.

---

## Database Issues

- **Database Connection Failed**
  - Confirm the `ims-sql` container is running in Docker.
  - Ensure the connection string in your config matches the SQL container name and credentials.

- **No Data After Startup**
  - The database starts empty. Use the demo login and begin adding data, or import a CSV file.

---

## General Tips

- **Reset Everything**
  - If things get stuck, try this sequence:
    ```bash
    docker-compose down
    docker system prune -af
    ./install.ps1
    ```

- **File Permission Errors (on Unix/macOS)**
  - Run `chmod +x install.sh` or `chmod +x uninstall.sh` if shell scripts fail to run.

- **Still Stuck?**
  - Delete all containers and images via Docker Desktop, then reinstall.
  - Or, contact your project team (see [Contact](#8-contact) section).

# 8. Contact

For questions, bug reports, or feature requests, please reach out to the project maintainers:

- GitHub Repository: [CodeBaddies IMS Project](https://github.com/Delinquent-Pointer/IMS)
- Team Members:
  - [@Delinquent-Pointer](https://github.com/Delinquent-Pointer)
  - [@Jared-Schimpf](https://github.com/Jared-Schimpf)
  - [@Pbrown34](https://github.com/Pbrown34)
- Email (for deployment or credential issues): *School-email* 

# 9. License & Academic Use

This project was developed as part of a university capstone course and is the intellectual property of the contributing team members.

### Usage Notice
- This software and its documentation are provided **strictly for academic purposes**.
- **Copying, redistributing, or using this project (or any part of it) without prior written permission is not authorized.**

### Educational Scope
- The Inventory Management System (IMS) was created to demonstrate understanding of full-stack development using .NET, Docker, and SQL Server.
- Any use beyond academic review or instruction requires explicit consent from all project contributors.

### Contact for Permissions
Please contact the project team for any inquiries about reuse or demonstration:
- [@Delinquent-Pointer](https://github.com/Delinquent-Pointer)
- [@Jared-Schimpf](https://github.com/Jared-Schimpf)
- [@Pbrown34](https://github.com/Pbrown34)
