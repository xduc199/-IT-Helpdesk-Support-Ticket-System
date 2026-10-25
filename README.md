# Helpdesk IT – Nhóm 14

## Giới thiệu dự án

Helpdesk IT là ứng dụng web hỗ trợ tiếp nhận và theo dõi các yêu cầu xử lý sự cố công nghệ thông tin trong một trường học hoặc doanh nghiệp. Dự án hướng tới giải quyết tình huống thường gặp: nhân viên báo máy tính hỏng qua chat hoặc nói trực tiếp, yêu cầu dễ bị bỏ sót, khó biết ai đang xử lý và không có nơi tra lại tiến độ.

Thay vì trao đổi rời rạc, người dùng đăng nhập và tạo **ticket** có tiêu đề, mô tả và tag. Ticket được lưu tập trung để đội ngũ hỗ trợ xem danh sách, nhận hoặc được phân công xử lý, rồi cập nhật tiến độ. Ví dụ: nhân viên báo không kết nối được Wi-Fi; bộ phận IT tiếp nhận ticket, gắn tag mạng, cập nhật từ `Open` sang `In Progress`, rồi `Resolved` khi đã khắc phục và `Closed` khi kết thúc. Những chuyển trạng thái sai thứ tự bị từ chối ở tầng nghiệp vụ.

### Ai sử dụng

- **Người dùng (User):** đăng ký/đăng nhập và gửi yêu cầu hỗ trợ.
- **Nhân viên hỗ trợ (Technician):** theo dõi ticket, nhận/phân công và cập nhật trạng thái.
- **Quản trị viên (Admin):** quản lý tag và vai trò, xem audit log; có thể khôi phục ticket đã xóa.

Ứng dụng gồm ASP.NET Core Web API và các trang HTML/JavaScript đi kèm cho người dùng, nhân viên và quản trị viên. SQL Server lưu tài khoản, ticket và tag; SignalR phát thông báo khi ticket được phân công, đổi trạng thái, ẩn hoặc xóa. Mục tiêu là giúp yêu cầu có đầu mối, tiến độ và lịch sử rõ ràng hơn. Đây là đồ án/prototype phục vụ minh họa một quy trình helpdesk cơ bản, chưa thay thế một hệ thống ITSM đầy đủ.

## 1. Công cụ cần cài

- **Git** để tải mã nguồn.
- **.NET 10 SDK** để build và chạy API.
- **SQL Server 2022** hoặc **Docker Desktop** để chạy SQL Server bằng container.
- Trình duyệt hiện đại (Chrome, Edge, Firefox).
- **Visual Studio 2022** (workload ASP.NET and web development) hoặc **VS Code** là tùy chọn để sửa mã.
- **EF Core CLI** để tạo database từ migration. Cài một lần:

```powershell
dotnet tool install --global dotnet-ef
```

Nếu đã cài, cập nhật công cụ bằng `dotnet tool update --global dotnet-ef`.

## 2. Tải mã nguồn

Clone repository của nhóm, rồi vào thư mục dự án:

```powershell
git clone https://github.com/xduc199/-IT-Helpdesk-Support-Ticket-System.git
cd nhom-14
```

Repository: [github.com/xduc199/nhom-14](https://github.com/xduc199/-IT-Helpdesk-Support-Ticket-System).

## 3. Cấu hình và khởi tạo database

### Cách A: dùng Docker cho SQL Server

Mở terminal tại thư mục dự án và chạy:

```powershell
docker compose up -d sqlserver
```

Compose mở SQL Server tại `localhost:1433`; tài khoản cấu hình sẵn trong `docker-compose.yml` là `sa` với mật khẩu phát triển `YourStrong@Password`.

### Cách B: dùng SQL Server cài trực tiếp

Cài và khởi động SQL Server 2022, rồi cập nhật connection string trong `src/API/appsettings.json` cho đúng máy, tài khoản và mật khẩu của bạn. Giá trị mặc định của dự án dùng `localhost,1433`, database `Nhom14Db`.

### Tạo/cập nhật schema

Từ thư mục gốc, chạy migration:

```powershell
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

Nếu cần thay cấu hình cho lần chạy hiện tại, có thể đặt biến môi trường PowerShell trước khi chạy lệnh/API:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=Nhom14Db;User Id=sa;Password=<mat-khau>;TrustServerCertificate=True;"
$env:Jwt__Key = "<chuoi-bi-mat-it-nhat-32-ky-tu>"
```

Không dùng mật khẩu mẫu hoặc commit secret thật khi triển khai dùng chung.

## 4. Chạy ứng dụng

```powershell
dotnet restore
dotnet run --project src/API
```

Profile phát triển hiện cấu hình API và giao diện tại **http://localhost:5050**. Trang chủ ở `/`; Scalar API ở `/scalar/v1` (chỉ trong Development). Đăng ký tài khoản tại giao diện để bắt đầu sử dụng.

Giao diện hiện gọi API tại `http://localhost:5050/api` trong `src/API/wwwroot/js/api.js`. Nếu đổi cổng hoặc host, cập nhật `BASE_URL` ở file này. Giao diện có các trang `login.html`, `register.html`, `customer.html`, `staff.html` và `admin.html`.

## 5. Chạy bằng Docker

`docker compose up --build` khởi động SQL Server và API, với cổng API host `7070`. Tuy nhiên, giao diện hiện cấu hình gọi API qua cổng `5050`; vì vậy khi chạy bằng Compose, cần đổi `BASE_URL` trong `src/API/wwwroot/js/api.js` thành `http://localhost:7070/api` rồi build lại image. Migration chưa được chạy tự động khi container khởi động, cần tạo schema trước khi dùng API.

Để dừng container: `docker compose down`. `docker compose down -v` xóa volume database và toàn bộ dữ liệu cục bộ.

## Tính năng và mức độ hoàn thiện

- JWT login/register, mật khẩu BCrypt và refresh token.
- Ticket có phân trang, gán người xử lý, luồng trạng thái `Open → In Progress → Resolved → Closed`, ẩn/hiện, xóa mềm/khôi phục.
- Quản lý tag, audit log, thông báo real-time qua SignalR; giao diện tĩnh cho người dùng, nhân viên và quản trị viên.
- Backend dùng .NET 10, EF Core, SQL Server; mã nguồn chia thành `Core`, `Infrastructure`, `API`. Có unit/integration test trong `tests/StudentTests`.

Đây là prototype/đồ án đã có các luồng chức năng chính, chưa nên xem là sản phẩm production-ready. Đặc tả trong `docs/` có một số mục chưa khớp với code; cần rà soát phân quyền endpoint và quyền truy cập ticket, cấu hình bảo mật, và đồng bộ frontend/API. Chưa xác nhận kết quả chạy test trong tài liệu này.

Tài liệu yêu cầu, thiết kế database, API và sprint nằm trong [`docs/`](docs/).
