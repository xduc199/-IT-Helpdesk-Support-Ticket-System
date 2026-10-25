# Software Requirements Specification (SRS) - IT Helpdesk Support System

---

# 1. Giới thiệu

## 1.1 Mục đích

Tài liệu này quy định chi tiết các yêu cầu chức năng và phi chức năng cho hệ thống Backend Web API của ứng dụng IT Helpdesk Support.

Hệ thống được xây dựng nhằm mục đích số hóa toàn bộ quy trình tiếp nhận, quản lý, điều phối và xử lý các sự cố kỹ thuật công nghệ thông tin (như lỗi phần mềm, hỏng hóc thiết bị phần cứng, sự cố đường truyền mạng) trong nội bộ trường học/doanh nghiệp.

Mục tiêu hướng tới là thay thế hoàn toàn các phương thức báo lỗi thủ công, rời rạc qua ứng dụng chat, bảo đảm mọi sự cố đều được lưu vết, phân loại và xử lý đúng tiến độ.

---

## 1.2 Phạm vi dự án

### Hệ thống thực hiện (In-Scope)

- Cung cấp các API xác thực, phân quyền người dùng dựa trên cơ chế Stateless JWT Token cho 3 nhóm đối tượng:
  - Admin
  - Technician
  - Employee

- Quản lý vòng đời của phiếu yêu cầu hỗ trợ (Ticket) từ lúc khởi tạo cho đến khi đóng hoàn toàn thông qua bộ kiểm soát trạng thái tự động (State Machine) được cấu hình chặt chẽ tại tầng nghiệp vụ.

- Lưu trữ dữ liệu tập trung trên SQL Server, tối ưu hóa các truy vấn danh mục tĩnh bằng giải pháp bộ nhớ đệm (Memory Cache).

### Nằm ngoài phạm vi (Out-of-Scope)

- Dự án này chỉ tập trung xây dựng giải pháp Backend Web API, không bao gồm việc phát triển giao diện người dùng (Frontend) trên nền tảng Web hay ứng dụng di động.

- Hệ thống không tích hợp các dịch vụ bên thứ ba như:
  - Cổng thanh toán
  - Email thông báo
  - SMS thông báo

---

# 2. Mô tả tổng quan

## 2.1 Các nhóm người dùng (User Classes)

Hệ thống phân chia quyền hạn truy cập API nghiêm ngặt dựa trên 3 vai trò (Roles) chính sau:

### Employee (Người dùng phổ thông)

Là sinh viên, giảng viên hoặc cán bộ nhân viên trong trường/doanh nghiệp.

#### Quyền hạn:
- Gửi yêu cầu báo lỗi kỹ thuật lên hệ thống
- Xem tiến độ xử lý Ticket
- Xem lịch sử các Ticket do chính mình tạo

#### Giới hạn:
- Không được chỉnh sửa Ticket của người khác
- Không được thay đổi trạng thái xử lý Ticket

---

### Technician (Kỹ thuật viên)

Là nhân viên thuộc phòng IT/Kỹ thuật.

#### Quyền hạn:
- Xem danh sách toàn bộ Ticket đang mở
- Nhận xử lý Ticket
- Cập nhật trạng thái xử lý
- Gắn nhãn phân loại lỗi (Tags)

---

### Admin (Quản trị viên)

Là người quản trị cao nhất của hệ thống.

#### Quyền hạn:
- CRUD phòng ban (Departments)
- CRUD người dùng (Users)
- Phân quyền tài khoản
- Giám sát toàn bộ hệ thống Ticket
- Điều phối xử lý sự cố

---

## 2.2 Môi trường vận hành

### Môi trường triển khai

- Ứng dụng Backend chạy trên nền tảng `.NET 10 Runtime`
- Hỗ trợ triển khai trên:
  - Windows Server
  - Linux (Ubuntu/CentOS)
  - Docker Container

### Hệ quản trị cơ sở dữ liệu

- Microsoft SQL Server 2019 trở lên
- Kết nối thông qua `Connection String` bảo mật

---

# 3. Yêu cầu chức năng (Functional Requirements)

Hệ thống bắt buộc phải hiện thực hóa đầy đủ các chức năng lõi được đặc tả dưới đây.

| ID | Tính năng | Mô tả chi tiết | Ưu tiên |
|---|---|---|---|
| FR-01 | Đăng nhập & Xác thực | Người dùng gửi Email và Password lên API `/api/auth/login`. Hệ thống xác thực và trả về JWT Token hợp lệ để sử dụng cho các request tiếp theo. | Cao |
| FR-02 | Khởi tạo Ticket | Employee tạo Ticket mới qua API `POST /api/tickets` với thông tin: Tiêu đề, Mô tả lỗi, Mức độ ưu tiên. Hệ thống tự sinh GUID, CreatedAt, RequesterId và trạng thái mặc định `Open`. | Cao |
| FR-03 | Truy vấn danh sách vé | API `GET /api/tickets` tự động lọc dữ liệu theo quyền: Employee chỉ xem Ticket của chính mình; Admin/Technician xem toàn bộ Ticket. | Cao |
| FR-04 | Cập nhật tiến độ xử lý | Technician cập nhật trạng thái Ticket qua API `PUT /api/tickets/{id}/status`: `Open → InProgress → Resolved → Closed`. | Cao |
| FR-05 | Chặn lỗi logic trạng thái (State Machine) | Hệ thống kiểm tra tính hợp lệ của luồng chuyển trạng thái Ticket. Nếu trạng thái không hợp lệ sẽ trả về `400 Bad Request`. | Cao |
| FR-06 | Gán nhãn phân loại lỗi | Technician có thể gắn nhiều Tag cho Ticket nhằm phục vụ phân loại và báo cáo thống kê lỗi. | Trung bình |

---

# 4. Yêu cầu phi chức năng (Non-functional Requirements)

## 4.1 Kiến trúc phần mềm (Architectural Standard)

Toàn bộ mã nguồn bắt buộc triển khai theo mô hình:

# Clean Architecture

Bao gồm 3 tầng độc lập:

## Core Layer
Chứa:
- Entities
- Interfaces
- Business Logic
- State Machine

## Infrastructure Layer
Chứa:
- Entity Framework Core
- SQL Server Configuration
- Repository
- Database Migration

## API Layer
Chứa:
- Controllers
- Middleware
- JWT Authentication
- Dependency Injection
- Memory Cache

### Ràng buộc kiến trúc

- Không được phép phụ thuộc vòng giữa các tầng
- Business Logic phải độc lập với Framework
- Dễ bảo trì và mở rộng

---

## 4.2 Bảo mật hệ thống (Security Requirements)

### Mật khẩu

- Không được lưu mật khẩu dạng Plain-text
- Mật khẩu phải được Hash một chiều trước khi lưu Database

### JWT Authentication

- Tất cả API bảo vệ phải yêu cầu Bearer Token hợp lệ
- Phân quyền dựa trên Role

### Git Workflow

Quy trình làm việc Git nghiêm cấm:
```bash
git push --force
```

Mục tiêu:
- Tránh mất dữ liệu source code
- Tránh xung đột branch
- Bảo vệ tiến độ làm việc nhóm

---

## 4.3 Hiệu năng & Tối ưu hóa (Performance & Optimization)

### Memory Cache

Đối với dữ liệu tĩnh như:
- Tags
- Departments
- Danh mục hệ thống

Ứng dụng phải sử dụng:
```csharp
IMemoryCache
```

### Mục tiêu hiệu năng

- API phản hồi dưới `500ms`
- Giảm truy vấn SQL không cần thiết
- Tối ưu I/O Database

### Cơ chế hoạt động

Khi Client gọi API lấy danh sách Tags:
1. Hệ thống kiểm tra dữ liệu trong RAM
2. Nếu tồn tại Cache → trả về ngay
3. Nếu chưa có → truy vấn SQL Server và lưu lại vào Cache

---

# 5. Công nghệ sử dụng đề xuất

| Thành phần | Công nghệ |
|---|---|
| Backend Framework | ASP.NET Core .NET 10 |
| Database | SQL Server 2019+ |
| ORM | Entity Framework Core |
| Authentication | JWT Bearer Token |
| Cache | IMemoryCache |
| API Testing | Swagger / Scalar |
| Version Control | Git + GitHub |
| Containerization | Docker |

---

# 6. Quy trình xử lý Ticket (Ticket Lifecycle)

```text
Open
  ↓
InProgress
  ↓
Resolved
  ↓
Closed
```

## Luồng hợp lệ

- Open → InProgress
- InProgress → Resolved
- Resolved → Closed

## Luồng không hợp lệ

- Open → Closed
- Open → Resolved
- Closed → Open

Hệ thống phải tự động từ chối các luồng sai logic.

---

# 7. Kết luận

Hệ thống IT Helpdesk Support hướng đến việc xây dựng một nền tảng quản lý sự cố kỹ thuật nội bộ hiện đại, bảo mật và dễ mở rộng.

Việc áp dụng:
- Clean Architecture
- JWT Authentication
- State Machine
- Memory Cache

giúp hệ thống:
- Dễ bảo trì
- Tối ưu hiệu năng
- Bảo đảm tính toàn vẹn nghiệp vụ
- Phù hợp triển khai thực tế trong doanh nghiệp hoặc trường học.
