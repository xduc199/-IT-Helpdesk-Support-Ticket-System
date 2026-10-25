@echo off
title SQL Docker Menu - Nhom14Db

:MENU
cls
echo ===============================
echo   SQL DOCKER MENU - NHOM14
echo ===============================
echo 1. Xem tất ca Users
echo 2. Xem tat ca Tickets
echo 3. Xem AuditLogs
echo 4. Xem Tags
echo 5. Xem so luong ban ghi
echo 6. Mo SQL Console (full control)
echo 0. Thoat
echo ===============================
set /p choice=Chon option: 

if "%choice%"=="1" goto USERS
if "%choice%"=="2" goto TICKETS
if "%choice%"=="3" goto AUDIT
if "%choice%"=="4" goto TAGS
if "%choice%"=="5" goto COUNT
if "%choice%"=="6" goto CONSOLE
if "%choice%"=="0" exit

goto MENU

:USERS
docker exec -it mssql_server_nhom14 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Password -C -d Nhom14Db -Q "SELECT * FROM Users;"
pause
goto MENU

:TICKETS
docker exec -it mssql_server_nhom14 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Password -C -d Nhom14Db -Q "SELECT * FROM Tickets;"
pause
goto MENU

:AUDIT
docker exec -it mssql_server_nhom14 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Password -C -d Nhom14Db -Q "SELECT TOP 50 * FROM AuditLogs ORDER BY CreatedAt DESC;"
pause
goto MENU

:TAGS
docker exec -it mssql_server_nhom14 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Password -C -d Nhom14Db -Q "SELECT * FROM Tags;"
pause
goto MENU

:COUNT
docker exec -it mssql_server_nhom14 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Password -C -d Nhom14Db -Q "SELECT 'Users' AS TableName, COUNT(*) AS Total FROM Users UNION ALL SELECT 'Tickets', COUNT(*) FROM Tickets UNION ALL SELECT 'AuditLogs', COUNT(*) FROM AuditLogs;"
pause
goto MENU

:CONSOLE
docker exec -it mssql_server_nhom14 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Password -C -d Nhom14Db
goto MENU