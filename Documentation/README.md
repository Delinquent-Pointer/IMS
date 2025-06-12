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
- [3. Uninstallation](#3-uninstallation)
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
  - [5.8 IT Dashboard](#58-it-dashboard)
  - [5.9 Sales Page](#59-sales-page)
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

- **Powershell 7 (Recommended Terminal)**  
  For running scripts.
  [Download Powershell 7](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.5)

- **GitHub Access**  
  - **Developer Repository**: [Delinquent-Pointer/IMS](https://github.com/Delinquent-Pointer/IMS)  
  - **Public Docker Showcase**: [IMS-Docker](https://github.com/Delinquent-Pointer/IMS-Docker)

- **Access Credentials (Admin/Database) [Only if building Azure cloud service]**  
  Some functionality requires access credentials (e.g., `admin password, database connection string`). These can be obtained from your Azure portal or deployment lead.  
  [Access Azure Portal](https://portal.azure.com/)

# 2. Installation

<table>
  <tr>
    <td>
      <b>Step 1: Download the IMS Docker Bundle</b><br><br>
      - Visit the latest release on GitHub: <a href="https://github.com/Delinquent-Pointer/IMS-Docker/releases">IMS Docker Releases</a><br>
      - Download the <code>.zip</code> file containing Docker images and setup scripts.
    </td>
    <td>
      <img src="install_1_1.png" width="500"/>
      <img src="install_1_2.png" width="700"/>
    </td>
  </tr>

  <tr>
    <td>
      <b>Step 2: Extract the ZIP File</b><br><br>
      - Right-click the downloaded archive and choose <b>Extract All</b>.<br>
      - Extract to a preferred location (e.g., `Documents\IMS`).
    </td>
    <td></td>
  </tr>

  <tr>
    <td>
      <b>Step 3: Open PowerShell in the Project Directory</b><br><br>
      - Navigate into the extracted folder<br>
      - Locate the folder containing <code>install.ps1</code><br>
      - Right-click and choose <b>Open in Terminal</b>
    </td>
    <td>
      <img src="install_3.png" width="700"/>
    </td>
  </tr>

  <tr>
    <td>
      <b>Step 4: Ensure Docker Desktop Is Running in Linux Mode</b><br><br>
      - Launch Docker Desktop<br>
      - Right-click the Docker tray icon and select <b>Switch to Linux containers</b> if needed
    </td>
    <td>
      <img src="install_4.png" width="200"/>
    </td>
  </tr>

  <tr>
    <td>
      <b>Step 5: Run the Installation Script</b><br><br>
      In PowerShell, enter:<br>
      <code>./install.ps1</code><br><br>
      This will load Docker images and start the IMS containers.
    </td>
    <td>
      <img src="install_5.png" width="700"/>
    </td>
  </tr>
</table>

---

# 3. Uninstallation

<table>
  <tr>
    <td>
      <b>Step 1: Locate <code>uninstall.ps1</code></b><br><br>
      - Navigate to the same folder where <code>install.ps1</code> was run.<br>
      - You should see a file named <code>uninstall.ps1</code>.
    </td>
    <td>
      <img src="uninstall_1.png" width="700"/>
    </td>
  </tr>

  <tr>
    <td>
      <b>Step 2: Run the Script in PowerShell</b><br><br>
      - Open PowerShell in that directory<br>
      - Enter:<br>
      <code>./uninstall.ps1</code><br><br>
      This will stop containers and remove related Docker images.
    </td>
    <td></td>
  </tr>

  <tr>
    <td>
      <b>Step 3: Manually Verify in Docker Desktop</b><br><br>
      - Open Docker Desktop<br>
      - Confirm IMS-related containers and images are removed<br>
      - Delete anything left over
    </td>
    <td>
      <img src="uninstall_2_1.png" width="700"/>
      <img src="uninstall_2_2.png" width="700"/>
    </td>
  </tr>

  <tr>
    <td colspan="2"><b>Done:</b> IMS has been fully removed. You may now delete the project folder.</td>
  </tr>
</table>


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
      <img src="image-1.png" alt="Login screenshot" width="700"/>
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
      <img src="image.png" alt="Create Account Screenshot" width="800"/>
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
        <img src="index.png" alt="Product Management Screenshot" width="900"/>
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
        <img src="advsearch.png" alt="Advanced Search Screenshot" width="700"/>
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
        <img src="Popup.png" alt="Product Popup Screenshot" width="700"/>
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

  </tr>
</table>


## 5.6 Inventory Alerts

<table>
  <tr>
    <td>
      <b>The Inventory Alerts system notifies users when item quantities fall below set thresholds.</b><br><br>
      Users can configure alerts by editing a product and specifying a "notify me when below" value.<br>
      These alerts are visible on both the dashboard and the calendar view for the associated product.
    </td>
    <td>
      <img src="image_5_7_but.png" alt="Alert Setting Modal" width="400"/><br>
    </td>
  </tr>
</table>

---

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
            <li>Scan items based off of their SKU using a printed barcode.</li>
            <li>Currently there is no integrated barcode generator.
              <ul>
                <li>Use: <a href="https://barcode.tec-it.com/en">Barcode.tec-it</a></li>
                <li>Recommended type: <b>Code-128</b></li>
              </ul>
            </li>
            <li>After scanning, click <b>'Complete Transaction'</b> to:
              <ul>
                <li>Deduct scanned item quantities from inventory.</li>
                <li>Generate a receipt on the Sales Page.</li>
              </ul>
            </li>
          </ul>
        </li>
      </ul>
    </td>
    <td>
      <img src="image-4.png" alt="Dashboard Scanner Screenshot" width="400"/>
    </td>
  </tr>

  <tr>
    <td>
      <b>Chart Selection:</b><br>
      Visualize current inventory using customizable chart types such as pie and bar charts. Use the dropdown to change views.
    </td>
    <td>
      <img src="image_5_7_1.png" alt="Chart Selection Dropdown" width="400"/>
    </td>
  </tr>

  <tr>
    <td>
      <b>Quick Product Search:</b><br>
      Use the quick search bar for instant filtering of products based on name, SKU, or category. Designed for fast barcode-driven use.
    </td>
    <td>
      <img src="image_5_7_qps.png" alt="Quick Product Search" width="400"/>
    </td>
  </tr>

  <tr>
    <td>
      <b>Interactive Notes:</b><br>
      Notes added to items show up on the dashboard calendar with timestamps. Useful for tracking reminders, stock actions, or alerts.
    </td>
    <td>
      <img src="image_5_7_notes.png" alt="Dashboard Notes on Calendar" width="400"/>
    </td>
  </tr>
</table>

## 5.8 IT Dashboard
<table>
  <tr>
    <td>
      <b>The IT Dashboard allows IT users or Admins the ability to verify and delete accounts.</b><br><br>
      <ul>
        <li>This page is only visible to users with Admin privileges.</li>
        <li>To verify a new account, press the Verify button.</li>
        <li>To delete any account, press the Delete button.</li>
        <li>This page lists the following:
          <ul>
            <li>Username</li>
            <li>ID</li>
            <li>Account Type (General User or IT User)</li>
            <li>Verified Status</li>
            <li>Actions</li>
          </ul>
        </li>
      </ul>
    </td>
    <td>
      <img src="image-6.png" alt="IT Dashboard Screenshot" width="700"/>
    </td>
  </tr>
</table>

## 5.9 Sales Page
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
        <li>SKUs are defined by multiple 'categories' separated by dashes.<br> e.g: `AAAA-BBBB` </li>
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