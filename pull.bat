@echo off
title Đồng Bộ Code Team - Nhóm 14
cls
chcp 65001 > nul

echo ======================================================================
echo                TIẾN TRÌNH ĐỒNG BỘ CODE AN TOÀN TỪ TEAM
echo ======================================================================
echo.

:: Nhận diện nhánh hiện tại
for /f "tokens=*" %%i in ('git branch --show-current 2^nul') do set CURRENT_BRANCH=%%i

if "%CURRENT_BRANCH%"=="" (
    echo [LỖI] Không thể nhận diện nhánh. Hãy chắc chắn thư mục có chứa .git!
    pause
    exit /b 1
)

echo Bạn đang đứng tại nhánh: [%CURRENT_BRANCH%]
echo ----------------------------------------------------------------------

echo [1/3] Đang lưu tạm thời các thay đổi chưa commit (git stash)...
git stash > nul

echo [2/3] Đang tải thông tin code mới nhất từ GitHub (git fetch)...
git fetch origin

if "%CURRENT_BRANCH%"=="main" (
    echo [3/3] Đang cập nhật trực tiếp nhánh main...
    git pull origin main
) else (
    echo [3/3] Đang gộp code mới nhất từ main vào nhánh [%CURRENT_BRANCH%]...
    git merge origin/main
)

if %ERRORLEVEL% neq 0 (
    echo.
    echo [CẢNH BÁO XUNG ĐỘT] Có xung đột code (Conflict) giữa file của bạn và team!
    echo Vui lòng mở VS Code/Visual Studio để Resolve Conflict nhé.
    pause
    exit /b %ERRORLEVEL%
)

echo ----------------------------------------------------------------------
echo [4/4] Đang khôi phục lại các file đang code dở của bạn...
git stash pop > nul 2>&1

echo ======================================================================
echo [OK] Code của bạn đã được đồng bộ và cập nhật mới nhất!
echo ======================================================================
pause