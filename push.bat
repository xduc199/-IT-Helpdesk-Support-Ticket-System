@echo off
title Hệ Thống Đẩy Code Toàn Diện - Nhóm 14 (.NET 10)
cls

:: Cấu hình bảng mã UTF-8 để hiển thị tiếng Việt có dấu chuẩn chỉnh
chcp 65001 > nul

echo ======================================================================
echo          TIẾN TRÌNH KIỂM TRA VÀ ĐẨY CODE AN TOÀN - NHÓM 14
echo ======================================================================
echo.

:: 1. Tự động quét và lấy tên nhánh hiện tại đang làm việc
for /f "tokens=*" %%i in ('git branch --show-current') do set CURRENT_BRANCH=%%i

echo Nhánh hiện tại trên máy cục bộ của sếp là: [%CURRENT_BRANCH%]
echo ----------------------------------------------------------------------
echo CHỌN PHƯƠNG THỨC SẾP MUỐN ĐẨY CODE LÊN GITHUB:
echo [1] Đẩy thẳng lên nhánh hiện tại [%CURRENT_BRANCH%]
echo [2] Tự nhập tên một nhánh Feature khác (Nhánh phụ)
echo [3] ĐẨY THẲNG LÊN NHÁNH CHÍNH [main] (Yêu cầu xác nhận nghiêm ngặt)
echo [4] Hủy bỏ tiến trình
echo ----------------------------------------------------------------------
set /p menu_choice="Nhập lựa chọn của sếp (1/2/3/4): "

if "%menu_choice%"=="4" goto CANCEL
if "%menu_choice%"=="3" goto CHOOSE_MAIN
if "%menu_choice%"=="2" goto CUSTOM_BRANCH
if "%menu_choice%"=="1" (
    set TARGET_BRANCH=%CURRENT_BRANCH%
    goto CONFIG_CONFIRM
)
echo.
echo [LỖI] Lựa chọn không hợp lệ, vui lòng chạy lại script!
pause
exit /b 1

:CUSTOM_BRANCH
echo.
set /p TARGET_BRANCH="Nhập tên nhánh feature sếp muốn đẩy lên: "
if "%TARGET_BRANCH%"=="" (
    echo [LỖI] Tên nhánh mục tiêu không được để trống!
    pause
    exit /b 1
)
goto CONFIG_CONFIRM

:CHOOSE_MAIN
set TARGET_BRANCH=main
goto CONFIG_CONFIRM

:CONFIG_CONFIRM
echo.
echo ======================================================================
echo               BƯỚC 1: XÁC NHẬN CẤU HÌNH TRƯỚC KHI COMMIT
echo ======================================================================
echo Nhánh đích sẽ đẩy lên GitHub: [%TARGET_BRANCH%]
echo.
set /p confirm_step1="Sếp có chắc chắn muốn tiếp tục với cấu hình này không? (Y/N): "
if /i "%confirm_step1%" neq "Y" goto CANCEL

:: 2. Gom tất cả các file thay đổi vào Staging Area
echo ----------------------------------------------------------------------
echo Đang quét và gom tất cả các file thay đổi (git add .)...
git add .

:: 3. Nhập thông điệp Commit định danh tính năng
echo.
set /p commit_msg="Nhập nội dung commit (Ví dụ: code xong api ticket): "
if "%commit_msg%"=="" set commit_msg="Update code tinh nang cho nhanh %TARGET_BRANCH%"

:: 4. Thực thi Commit cục bộ
git commit -m "%commit_msg%"

:: 5. Phân luồng xử lý: Nếu là nhánh main thì kích hoạt bộ xác thực tối cao
if "%TARGET_BRANCH%"=="main" goto CONFIRM_MAIN
goto CONFIRM_FEATURE

:CONFIRM_FEATURE
echo.
echo ======================================================================
echo               BƯỚC 2: XÁC NHẬN ĐẨY CODE LÊN NHÁNH FEATURE
echo ======================================================================
echo Hệ thống chuẩn bị đẩy code lên nhánh phụ [%TARGET_BRANCH%].
echo Hành động này sẽ không làm ảnh hưởng trực tiếp đến mã nguồn chung của nhóm.
echo.
set /p push_feat_confirm="Sếp có chắc chắn muốn PUSH lên [%TARGET_BRANCH%] không? (Y/N): "
if /i "%push_feat_confirm%" neq "Y" goto CANCEL
goto EXECUTE_PUSH

:CONFIRM_MAIN
echo.
echo ======================================================================
echo    [CẢNH BÁO NGUY HIỂM] XÁC THỰC ĐẨY MÃ NGUỒN TRỰC TIẾP VÀO NHÁNH MAIN
echo ======================================================================
echo Nhánh 'main' chứa mã nguồn chạy ổn định của toàn bộ Team Nhóm 14!
echo Việc đẩy code trực tiếp không qua Pull Request (PR) có thể gây xung đột hỏng code.
echo.
set /p push_main_confirm1="Sếp đã tự chạy thử nghiệm chạy ổn định và sửa hết lỗi chưa? (Y/N): "
if /i "%push_main_confirm1%" neq "Y" goto CANCEL

echo.
set /p push_main_confirm2="XÁC NHẬN LẦN CUỐI: Sếp có chắc chắn muốn push thẳng vào [main]? (Y/N): "
if /i "%push_main_confirm2%" neq "Y" goto CANCEL
goto EXECUTE_PUSH

:EXECUTE_PUSH
echo ----------------------------------------------------------------------
echo Đang thực thi lệnh đẩy code lên [%TARGET_BRANCH%] (AN TOÀN - KHÔNG FORCE)...
git push origin %TARGET_BRANCH%

:: Kiểm tra mã lỗi trả về của lệnh git push
if %ERRORLEVEL% neq 0 (
    echo.
    echo ----------------------------------------------------------------------
    echo [LỖI BIẾN CỐ] Tiến trình đẩy mã nguồn thất bại!
    echo Nguyên nhân: Có thể mã nguồn trên GitHub có phiên bản mới hơn máy cục bộ.
    echo Giải pháp: Sếp hãy chạy file .\pull.bat trước để gộp code mới, xử lý xung đột rồi thử lại.
    echo ----------------------------------------------------------------------
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo ======================================================================
echo [THÀNH CÔNG] Đã đẩy toàn bộ mã nguồn lên [%TARGET_BRANCH%] an toàn!
if "%TARGET_BRANCH%"=="main" (
    echo Mã nguồn nhánh chính đã được cập nhật trực tiếp thành công.
) else (
    echo Giờ sếp có thể lên giao diện GitHub tạo Pull Request để nhóm duyệt vào main.
)
echo ======================================================================
pause
exit /b 0

:CANCEL
echo.
echo ======================================================================
echo [ĐÃ HỦY] Tiến trình đã dừng lại. Không có thay đổi nào được đẩy lên GitHub.
echo ======================================================================
pause
exit /b 0