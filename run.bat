@echo off
chcp 65001 > nul
title PIPELINE TRIỂN KHAI TỰ ĐỘNG - NHÓM 14
cls

echo ======================================================================
echo           BẮT ĐẦU QUY TRÌNH KHỞI CHẠY HỆ THỐNG DỰ ÁN DOCKER
echo ======================================================================
echo.

echo [1/9] Đang quét sạch thư mục cache rác (bin/obj) trên máy local...
for /d /r %%x in (bin obj) do @if exist "%%x" rd /s /q "%%x" 2>nul

echo [2/9] Đang thực thi: dotnet clean...
dotnet clean
if %errorlevel% neq 0 goto Lỗi

echo [3/9] Đang thực thi: dotnet restore...
dotnet restore
if %errorlevel% neq 0 goto Lỗi

echo [4/9] Đang thực thi: dotnet build...
dotnet build --no-restore
if %errorlevel% neq 0 goto Lỗi

echo [OK] Biên dịch mã nguồn local thành công. Bắt đầu triển khai Docker...
echo ----------------------------------------------------------------------

echo [5/9] Đang thực thi: docker compose down -v (Dọn dẹp tài nguyên cũ)...
docker compose down -v
if %errorlevel% neq 0 (
    echo [CẢNH BÁO] Không thể dừng container sạch sẽ. Tiến hành cưỡng chế...
)

echo [6/9] Đang khởi dựng hệ thống container mới (docker compose up)...
docker compose up -d --build
if %errorlevel% neq 0 goto Lỗi

echo [7/9] Đang chờ SQL Server hoàn tất cấu hình nội bộ...
timeout /t 12 /nobreak

echo [8/9] Đang di chuyển vào phân tầng dữ liệu...
cd src

echo [9/9] Đang thực thi Migration đổ cấu trúc bảng vào Database Docker...
dotnet ef database update --project Infrastructure --startup-project API
if %errorlevel% neq 0 goto Lỗi-Migration

cd ..
echo ======================================================================
echo ✔ [THÀNH CÔNG] Hệ thống đã khởi chạy hoàn tất trên Docker!
echo ✔ Truy cập Scalar API Test tại: http://localhost:5050/scalar/v1
echo ======================================================================
pause
exit /b 0

:Lỗi
echo ======================================================================
echo ❌ [LỖI CHÍ MẠNG] Quy trình bị dừng lại. Kiểm tra log hệ thống phía trên.
echo ======================================================================
pause
exit /b 1

:Lỗi-Migration
cd ..
echo ======================================================================
echo ❌ [LỖI MIGRATION] Container chạy ngon nhưng đổ DB thất bại. 
echo Hãy kiểm tra xem có file Migration nào trong thư mục bị lặp tên cột không.
echo ======================================================================
pause
exit /b 1