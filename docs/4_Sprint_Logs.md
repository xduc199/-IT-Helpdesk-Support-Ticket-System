# Project Sprint Logs - Hệ thống Hỗ trợ Kỹ thuật IT Helpdesk

## Sprint 01: Thiết lập Kiến trúc, Cơ sở dữ liệu & API cốt lõi
- **Thời gian:** 04/05/2026 - 15/05/2026
- **Mục tiêu (Sprint Goal):** Khởi tạo hệ thống theo Clean Architecture, hoàn thiện Database SQL Server, xây dựng API nền tảng, triển khai State Machine cho Ticket và đảm bảo toàn bộ logic vượt qua Unit Test (TDD)

### 1. Danh sách công việc (Backlog)

| ID | Tên Task / User Story | Người phụ trách | Trạng thái | Ghi chú |
|:---|:---|:---|:---|:---|
| T-01 | Thiết lập cấu trúc Clean Architecture (.NET 10) | Lê Xuân Đức | [x] Done | Sửa lỗi `.csproj` bị rỗng, build thành công |
| T-02 | Xây dựng Entity (Nguoidung, Ticket, Tag, Department) | Phan Văn Kiều | [x] Done | Chuẩn hóa tên không dấu |
| T-03 | Định nghĩa Interface Repository | Lê Xuân Đức | [x] Done | Hoàn thiện contract cho tầng Infrastructure |
| T-04 | Cài đặt State Machine cho Ticket | Lê Xuân Đức | [x] Done | Áp dụng luồng Open → InProgress → Resolved → Closed |
| T-05 | Viết Unit Test (TDD) cho State Machine | Lê Xuân Đức | [x] Done | 5/5 test case pass |
| T-06 | Triển khai ApplicationDbContext & Fluent API | Phan Văn Kiều | [x] Done | Fix lỗi namespace |
| T-07 | Migration Database SQL Server | Phan Văn Kiều | [x] Done | Mapping enum Status → INT |
| T-08 | Cài đặt Repository Layer (EF Core) | Phan Văn Kiều | [x] Done | Sử dụng `.Include()` load dữ liệu liên quan |
| T-09 | Xây dựng API Authentication + JWT | Lê Xuân Đức | [x] Done | Phân quyền Role-based |
| T-10 | Triển khai Memory Cache cho Tag API | Lê Xuân Đức | [x] Done | Tối ưu response < 100ms |
| T-11 | Hoàn thiện tài liệu hệ thống (SRS, DB, API) | Cả nhóm | [x] Done | Đồng bộ 100% với code |

*Ghi chú trạng thái: [ ] To Do, [/] In Progress, [x] Done*

---

### 2. Tổng kết Sprint (Sprint Retrospective)

- **Thành công:**
  - Hoàn thiện kiến trúc Clean Architecture chuẩn, tách biệt rõ các tầng.
  - Build hệ thống thành công, xử lý triệt để 33 lỗi namespace và assembly.
  - State Machine hoạt động chính xác, kiểm soát chặt vòng đời Ticket.
  - Đạt 100% Unit Test (5/5 test case).
  - Tích hợp Swagger/Scalar giúp test API trực quan.
  - Tối ưu hiệu năng với Memory Cache cho dữ liệu danh mục.

- **Khó khăn:**
  - Lỗi file bị khóa khi build:
    ```
    Unable to delete file... file is locked
    ```
  - Nguyên nhân: tiến trình API cũ vẫn chạy ngầm trên Windows.
  - Một số lỗi namespace và dependency injection gây crash khi build ban đầu.

- **Hành động cải thiện (Action Items):**
  - Tạo script `.bat` để kill tiến trình trước khi build:
    ```bat
    taskkill /f /im API.exe
    ```
  - Chuẩn hóa quy tắc:
    - Không dùng force push Git bừa bãi
    - Kiểm tra tiến trình chạy ngầm trước khi build
  - Chuẩn hóa DI và namespace ngay từ đầu để tránh lỗi dây chuyền

---

## Sprint 02: Hoàn thiện chức năng Ticket & Phân quyền hệ thống
- **Thời gian:** 16/05/2026 - 28/05/2026
- **Mục tiêu (Sprint Goal):** Hoàn thiện quy trình xử lý Ticket thực tế, phân quyền người dùng, tối ưu API và đảm bảo hệ thống hoạt động ổn định ở mức production cơ bản

### 1. Danh sách công việc (Backlog)

| ID | Tên Task / User Story | Người phụ trách | Trạng thái | Ghi chú |
|:---|:---|:---|:---|:---|
| T-12 | API tạo Ticket kèm Tag | Phan Văn Kiều | [x] Done | Validate dữ liệu đầu vào |
| T-13 | Gán Technician cho Ticket | Lê Xuân Đức | [x] Done | Chỉ Admin thực hiện |
| T-14 | API cập nhật trạng thái Ticket | Phan Văn Kiều | [x] Done | Kiểm soát bằng State Machine |
| T-15 | Hoàn thiện Authorization (JWT Role) | Lê Xuân Đức | [x] Done | Áp dụng `[Authorize]` |
| T-16 | Middleware xử lý Exception | Lê Xuân Đức | [x] Done | Trả JSON chuẩn |
| T-17 | Validation DTO (FluentValidation) | Phan Văn Kiều | [x] Done | Chặn dữ liệu sai |
| T-18 | Tối ưu EF Core (`AsNoTracking`) | Phan Văn Kiều | [x] Done | Giảm memory usage |
| T-19 | Tích hợp Logging (Serilog) | Lê Xuân Đức | [x] Done | Log file hệ thống |
| T-20 | API tìm kiếm & lọc Ticket | Phan Văn Kiều | [x] Done | Filter theo status, tag |
| T-21 | Integration Test API | Lê Xuân Đức | [x] Done | Test auth + ticket flow |
| T-22 | Cập nhật tài liệu API | Cả nhóm | [x] Done | Đồng bộ với hệ thống |

*Ghi chú trạng thái: [ ] To Do, [/] In Progress, [x] Done*

---

### 2. Tổng kết Sprint (Sprint Retrospective)

- **Thành công:**
  - Hoàn thiện đầy đủ CRUD Ticket và luồng xử lý thực tế.
  - Phân quyền Role-based hoạt động chính xác.
  - Middleware xử lý lỗi giúp debug dễ dàng.
  - Logging hệ thống hỗ trợ theo dõi lỗi runtime.
  - Tối ưu hiệu năng truy vấn với EF Core.

- **Khó khăn:**
  - Lỗi vòng lặp JSON:
    ```
    Self referencing loop detected
    ```
  - Lỗi `401 Unauthorized` do JWT hết hạn khi test.

- **Hành động cải thiện (Action Items):**
  - Cấu hình xử lý vòng lặp JSON:
    ```csharp
    ReferenceHandler.IgnoreCycles
    ```
  - Xây dựng cơ chế Refresh Token ở Sprint sau.
  - Chuẩn hóa:
    - Không trả Entity trực tiếp
    - Luôn sử dụng DTO trong API response

```