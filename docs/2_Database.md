# 📘 Tài Liệu Thiết Kế Cơ Sở Dữ Liệu - Hệ Thống IT Helpdesk

---

## 1. Sơ đồ mối quan hệ giữa các thực thể (ERD)

- Một **Role** có thể được gán cho nhiều **User**.
- Một **Department** có thể có nhiều **User**.
- Một **User** có thể:
  - Tạo nhiều **Ticket**
  - Viết nhiều **Comment**
  - Sinh nhiều **AuditLog**
- Một **Department** có thể quản lý nhiều **Ticket**.
- Một **Ticket** có thể:
  - Có nhiều **Comment**
  - Có nhiều **Attachment**
  - Có nhiều **AuditLog**
  - Được tạo bởi một User (CreatedBy)
  - Được xử lý bởi một User (AssignedTo)

---

## 2. Chi tiết cấu trúc các bảng dữ liệu

---

### 2.1 Bảng `Roles`

| Tên trường | Kiểu dữ liệu | Khóa | Ràng buộc | Ghi chú |
| :--- | :--- | :--- | :--- | :--- |
| Id | INT | PK | IDENTITY(1,1) | Mã vai trò |
| Name | NVARCHAR(50) | | NOT NULL | Admin / Technician / Staff |

---

### 2.2 Bảng `Departments`

| Tên trường | Kiểu dữ liệu | Khóa | Ràng buộc | Ghi chú |
| :--- | :--- | :--- | :--- | :--- |
| Id | INT | PK | IDENTITY(1,1) | Mã phòng ban |
| Name | NVARCHAR(100) | | NOT NULL | Tên phòng ban |
| Description | NVARCHAR(255) | | NULL | Mô tả |

---

### 2.3 Bảng `Users`

| Tên trường | Kiểu dữ liệu | Khóa | Ràng buộc | Ghi chú |
| :--- | :--- | :--- | :--- | :--- |
| Id | INT | PK | IDENTITY(1,1) | Mã người dùng |
| FullName | NVARCHAR(100) | | NOT NULL | Họ và tên |
| Email | VARCHAR(100) | | NOT NULL, UNIQUE | Email đăng nhập |
| PasswordHash | VARCHAR(255) | | NOT NULL | Mật khẩu mã hóa |
| RoleId | INT | FK → Roles | NOT NULL | Vai trò |
| DepartmentId | INT | FK → Departments | NULL | Phòng ban |
| CreatedAt | DATETIME2 | | NOT NULL | Ngày tạo |
| IsActive | BIT | | NOT NULL | Trạng thái |

---

### 2.4 Bảng `Tickets`

| Tên trường | Kiểu dữ liệu | Khóa | Ràng buộc | Ghi chú |
| :--- | :--- | :--- | :--- | :--- |
| Id | INT | PK | IDENTITY(1,1) | Mã ticket |
| Title | NVARCHAR(200) | | NOT NULL | Tiêu đề |
| Description | NVARCHAR(MAX) | | NOT NULL | Nội dung |
| Status | INT | | NOT NULL | 0-Open, 1-InProgress, 2-Resolved, 3-Closed |
| Priority | INT | | NOT NULL | 0-Low, 1-Medium, 2-High, 3-Critical |
| CreatedBy | INT | FK → Users | NOT NULL | Người tạo |
| AssignedTo | INT | FK → Users | NULL | Người xử lý |
| DepartmentId | INT | FK → Departments | NOT NULL | Phòng ban xử lý |
| CreatedAt | DATETIME2 | | NOT NULL | Ngày tạo |
| UpdatedAt | DATETIME2 | | NULL | Ngày cập nhật |

---

### 2.5 Bảng `Comments`

| Tên trường | Kiểu dữ liệu | Khóa | Ràng buộc | Ghi chú |
| :--- | :--- | :--- | :--- | :--- |
| Id | INT | PK | IDENTITY(1,1) | Mã comment |
| TicketId | INT | FK → Tickets | NOT NULL | Ticket |
| UserId | INT | FK → Users | NOT NULL | Người bình luận |
| Content | NVARCHAR(MAX) | | NOT NULL | Nội dung |
| CreatedAt | DATETIME2 | | NOT NULL | Ngày tạo |

---

### 2.6 Bảng `Attachments`

| Tên trường | Kiểu dữ liệu | Khóa | Ràng buộc | Ghi chú |
| :--- | :--- | :--- | :--- | :--- |
| Id | INT | PK | IDENTITY(1,1) | Mã file |
| TicketId | INT | FK → Tickets | NOT NULL | Ticket |
| FileName | NVARCHAR(255) | | NOT NULL | Tên file |
| FileUrl | NVARCHAR(500) | | NOT NULL | Đường dẫn |
| UploadedAt | DATETIME2 | | NOT NULL | Ngày upload |

---

### 2.7 Bảng `AuditLogs`

| Tên trường | Kiểu dữ liệu | Khóa | Ràng buộc | Ghi chú |
| :--- | :--- | :--- | :--- | :--- |
| Id | INT | PK | IDENTITY(1,1) | Mã log |
| TicketId | INT | FK → Tickets | NOT NULL | Ticket |
| UserId | INT | FK → Users | NOT NULL | Người thực hiện |
| Action | NVARCHAR(100) | | NOT NULL | Hành động |
| OldValue | NVARCHAR(MAX) | | NULL | Giá trị cũ |
| NewValue | NVARCHAR(MAX) | | NULL | Giá trị mới |
| CreatedAt | DATETIME2 | | NOT NULL | Thời gian |

---

## 3. Quy ước thiết kế

- PK sử dụng `INT IDENTITY(1,1)`
- FK luôn có index để tối ưu truy vấn
- Enum (`Status`, `Priority`) lưu dưới dạng `INT`
- Thiết kế theo chuẩn **3NF**
- Ticket là thực thể trung tâm của hệ thống

---

## 4. Ghi chú kiến trúc

- Không sử dụng bảng N-N trong phiên bản hiện tại
- AuditLogs dùng để tracking toàn bộ thay đổi hệ thống
- Hệ thống tối ưu cho SQL Server + EF Core