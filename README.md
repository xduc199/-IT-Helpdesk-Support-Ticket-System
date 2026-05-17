# Đồ án: Hệ thống IT Helpdesk Support (.NET 10)

Đồ án xây dựng ứng dụng Web API quản lý và xử lý sự cố kỹ thuật nội bộ, phục vụ cho môn học phần **Lập trình Dev**.

---

## 👥 1. Thành viên thực hiện (Nhóm 14)

- **Lê Xuân Đức** (24ATT116)  
  
  - Thiết lập kiến trúc Clean Architecture  
  - Code logic xử lý (State Machine, JWT, Cache)  
  - Viết Unit Test  

- **Phan Văn Kiều**  
  
  - Thiết kế cơ sở dữ liệu  
  - Chạy Migration SQL Server  
  - Viết Repository và API Controller  

---

## 🏗️ 2. Cấu trúc thư mục code (Clean Architecture)

### `Core`
Chứa:
- Thực thể dữ liệu (`Users`, `Tickets`, `Tags`)
- Enum
- Logic kiểm soát trạng thái (State Machine)

### `Infrastructure`
Chứa:
- Kết nối SQL Server qua Entity Framework Core
- Xử lý truy vấn dữ liệu
- Migration Database

### `API`
Chứa:
- Cấu hình Web API
- API Controller
- Xác thực JWT
- Phân quyền người dùng
- Cấu hình Cache

### Hệ thống IT Helpdesk áp dụng nghiêm ngặt kiến trúc **Clean Architecture** tách biệt các tầng nghiệp vụ, tích hợp bộ kiểm soát trạng thái tự động **State Machine** để quản lý vòng đời phiếu hỗ trợ (Ticket). 

Dự án tối ưu hóa tối đa điểm số thông qua các công nghệ nâng cao:
*   Bảo mật stateless bằng **JWT Token** phân quyền 3 nhóm Roles.
*   Xử lý lỗi tập trung bằng **Global Exception Handling Middleware** bảo vệ an toàn hệ thống.
*   Tối ưu hóa truy vấn bằng cơ chế **Paging/Filtering** (Phân trang và lọc) sự cố.
*   Tăng tốc độ phản hồi bằng bộ nhớ đệm **In-Memory Cache**.
*   Đóng gói hoàn chỉnh bằng **Dockerfile** và kiểm soát chất lượng bằng bộ **Unit Test (xUnit)**.
---

## ⚙️ 3. Hướng dẫn cài đặt & chạy ứng dụng

### Bước 1: Sửa chuỗi kết nối Database

Mở file:

```json
src/API/appsettings.json
```

Sửa lại giá trị:

```json
"DefaultConnection"
```

cho đúng với SQL Server trên máy của bạn.

---

### Bước 2: Tạo Database và chạy Migration

```bash
cd src/Infrastructure
dotnet ef database update --startup-project ../API
```

---

### Bước 3: Chạy dự án Backend

```bash
dotnet clean
dotnet run --project src/API
```

Sau khi chạy thành công, giao diện test API sẽ mở tại:

```text
http://localhost:5050/scalar/v1
```

---

### Bước 4: Khởi chạy dự án bằng Docker (Nếu chấm bằng Container)
Tại thư mục gốc chứa file `Dockerfile`, thực thi lệnh để build và chạy ứng dụng:
```bash
# Build Image từ Dockerfile
docker build -t it-helpdesk-api .

# Khởi chạy Container kết nối cổng 5000
docker run -d -p 5000:80 --name helpdesk-app it-helpdesk-api



## 📁 4. Danh mục tài liệu đính kèm (`docs/`)

| File | Nội dung |
|---|---|
| `1_SRS.md` | Mô tả chức năng hệ thống và phân quyền người dùng |
| `2_Database.md` | Thiết kế cơ sở dữ liệu và cấu trúc bảng |
| `3_API_Specs.md` | Đặc tả API Request / Response |
| `4_Sprint_Logs.md` | Nhật ký Sprint và phân công công việc |

---

## ⚡ 5. Các file Script tự động hóa (`.bat`)

Các file script được đặt tại thư mục gốc của dự án để chạy nhanh bằng cách click đúp chuột.

---

### 5.1 `run.bat` — Chạy dự án

```bat
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
```

---

### 5.2 `test.bat` — Chạy Unit Test

```bat
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
```

---

### 5.3 `pull.bat` — Kéo code mới từ GitHub

```bat
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
```

---

### 5.4 `push.bat` — Đẩy code lên GitHub

```bat
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
```
