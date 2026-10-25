@echo off
cd /d %~dp0

:MENU
cls
echo ==============================
echo      EF CORE TOOL MENU
echo ==============================
echo 1. Tao Migration moi
echo 2. Update Database
echo 3. Tao Migration + Update DB
echo 4. Remove Migration cuoi
echo 5. Exit
echo ==============================
set /p choice=Chon chuc nang: 

if "%choice%"=="1" goto ADD
if "%choice%"=="2" goto UPDATE
if "%choice%"=="3" goto ADD_UPDATE
if "%choice%"=="4" goto REMOVE
if "%choice%"=="5" exit

goto MENU

:ADD
echo.
set /p name=Nhap ten migration: 
dotnet ef migrations add %name% -p src/Infrastructure -s src/API
pause
goto MENU

:UPDATE
echo.
echo Dang update database...
dotnet ef database update -p src/Infrastructure -s src/API
pause
goto MENU

:ADD_UPDATE
echo.
set /p name=Nhap ten migration: 
dotnet ef migrations add %name% -p src/Infrastructure -s src/API

echo.
echo Dang update database...
dotnet ef database update -p src/Infrastructure -s src/API
pause
goto MENU

:REMOVE
echo.
echo Dang xoa migration cuoi...
dotnet ef migrations remove -p src/Infrastructure -s src/API
pause
goto MENU