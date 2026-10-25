@echo off
chcp 65001 > nul
title HỆ THỐNG KIỂM THỬ TỰ ĐỘNG - STUDENT TESTS (NHÓM 14)
cls

echo ======================================================================
echo           BẮT ĐẦU QUY TRÌNH CHẠY UNIT TEST / INTEGRATION TEST
echo ======================================================================
echo.

echo [1/4] Đang dọn dẹp cache build cũ để tránh sai lệch kết quả test...
for /d /r %%x in (bin obj) do @if exist "%%x" rd /s /q "%%x" 2>nul

echo [2/4] Đang thực thi: dotnet clean...
dotnet clean
if %errorlevel% neq 0 goto Lỗi

echo [3/4] Đang cập nhật gói thư viện kiểm thử (dotnet restore)...
dotnet restore
if %errorlevel% neq 0 goto Lỗi

echo [4/4] Đang kích hoạt bộ kiểm thử StudentTests...
echo ----------------------------------------------------------------------
:: Chỉ định chạy chính xác project StudentTests và hiển thị log chi tiết ra console
dotnet test tests\StudentTests\StudentTests.csproj --configuration Release --logger "console;verbosity=detailed"

if %errorlevel% neq 0 goto Lỗi-Test

echo.
echo ======================================================================
echo ✅ [HOÀN THÀNH] Tất cả các bài kiểm thử trong StudentTests đã VƯỢT QUA!
echo ======================================================================
pause
exit /b 0

:Lỗi
echo.
echo ======================================================================
echo ❌ [LỖI HỆ THỐNG] Không thể biên dịch mã nguồn để chạy Test.
echo ======================================================================
pause
exit /b 1

:Lỗi-Test
echo.
echo ======================================================================
echo ❌ [TEST FAILED] Có một hoặc nhiều bài kiểm thử bị đánh trượt (Failed)!
echo Hãy kiểm tra lại log chi tiết phía trên để sửa logic code.
echo ======================================================================
pause
exit /b 1