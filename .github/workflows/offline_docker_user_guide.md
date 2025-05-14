# Team Use Guide: Offline Docker Environment for IMS

This guide explains how to get a **fully self‑contained**, **offline** copy of the Inventory Management System (IMS) running on your local machine using Docker and Docker Compose.

---

## Prerequisites

- **Docker** (Engine & CLI) installed  
- **Docker Compose** installed  
- *(Optional)* **sqlpackage** CLI if you need to import a `.bacpac` snapshot  

---

## 1. Download Artifacts

From the **latest** GitHub Actions run on `main`, download these artifacts:

- **Web App image** (`offline-docker-image`)  
  ```
  ims-offline_<COMMIT_SHA>.tar.gz
  ```
- **SQL Server engine image** (`offline-sqlserver-image`)  
  ```
  sqlserver_2019.tar.gz
  ```
- **Database snapshot** (`offline-db-bacpac`) *(optional)*  
  ```
  IMS_Db.bacpac
  ```

Place them all into one folder, e.g.:
```
/path/to/ims-offline/
├── ims-offline_abcdef1234.tar.gz
├── sqlserver_2019.tar.gz
└── IMS_Db.bacpac          ← optional
```

---

## 2. Load Docker Images

### Bash

```bash
# Load the IMS web‑app image
docker load < ims-offline_abcdef1234.tar.gz

# Load the SQL Server engine image
docker load < sqlserver_2019.tar.gz
```

### PowerShell

```powershell
# Load the IMS web‑app image
docker load --input .\ims-offline_abcdef1234.tar.gz

# Load the SQL Server engine image
docker load --input .\sqlserver_2019.tar.gz
```

---

## 3. (Optional) Import Production Data

### Bash

```bash
# 1) Start a temporary SQL Server container
docker run -d --name sql-temp \
  -e ACCEPT_EULA=Y \
  -e SA_PASSWORD=YourStrong!Pass \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2019-latest

# 2) Import the .bacpac
sqlpackage \
  /Action:Import \
  /SourceFile:IMS_Db.bacpac \
  /TargetServerName:localhost \
  /TargetDatabaseName:IMS \
  /TargetUser:sa \
  /TargetPassword:YourStrong!Pass

# 3) Clean up the temp container
docker stop sql-temp
docker rm sql-temp
```

### PowerShell

```powershell
# 1) Start a temporary SQL Server container
docker run -d --name sql-temp `
  -e ACCEPT_EULA=Y `
  -e SA_PASSWORD='YourStrong!Pass' `
  -p 1433:1433 `
  mcr.microsoft.com/mssql/server:2019-latest

# 2) Import the .bacpac
sqlpackage `
  /Action:Import `
  /SourceFile:IMS_Db.bacpac `
  /TargetServerName:localhost `
  /TargetDatabaseName:IMS `
  /TargetUser:sa `
  /TargetPassword:YourStrong!Pass

# 3) Clean up the temp container
docker stop sql-temp
docker rm sql-temp
```

---

## 4. Create Docker‑Compose File

Create `docker-compose.offline.yml` in the same folder with the following content:

```yaml
version: '3.8'

services:
  sql:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong!Pass
    ports:
      - "1433:1433"
    volumes:
      - sql_data:/var/opt/mssql

  web:
    image: ims-offline:abcdef1234   # or ims-offline:latest
    depends_on:
      - sql
    ports:
      - "5000:80"
    environment:
      - ConnectionStrings__Default=Server=sql;Database=IMS;User Id=sa;Password=YourStrong!Pass

volumes:
  sql_data:
```

> **Note:**  
> Replace `abcdef1234` with the actual commit SHA tag, or use `ims-offline:latest`.

---

## 5. Spin Up the Stack

### Bash

```bash
docker-compose -f docker-compose.offline.yml up -d
```

### PowerShell

```powershell
docker-compose -f .\docker-compose.offline.yml up -d
```

---

## 6. Verify & Browse

Open your browser and go to:

```
http://localhost:5000
```

---

## 7. Tear Down

### Bash

```bash
docker-compose -f docker-compose.offline.yml down
docker volume rm <folder>_sql_data
```

### PowerShell

```powershell
docker-compose -f .\docker-compose.offline.yml down
docker volume rm <folder>_sql_data
```

---

## Troubleshooting

- **Port conflicts**: ensure nothing else is using ports 1433 or 5000.  
- **Password mismatches**: ensure `SA_PASSWORD` matches in both Compose and import commands.  
- **Missing images**: confirm you ran `docker load` successfully for both tarballs.  
