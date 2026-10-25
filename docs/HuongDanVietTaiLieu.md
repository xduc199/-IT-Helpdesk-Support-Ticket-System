# Hướng Dẫn & Biểu Mẫu Viết Tài Liệu Dự Án Phần Mềm

Tài liệu này cung cấp hướng dẫn và bộ khung chuẩn (template) cho 4 tài liệu cốt lõi trong quy trình phát triển phần mềm. Để sử dụng, bạn chỉ cần tạo các file có tên tương ứng (.md) và sao chép phần template vào để bắt đầu điền nội dung.

---

## 1. Tài liệu `1_SRS.md` (Đặc tả Yêu cầu Phần mềm)

**Mục đích:** Mô tả chi tiết phần mềm sẽ làm gì, hoạt động như thế nào và các ràng buộc đi kèm. Đây là tài liệu nền tảng cho cả đội ngũ phát triển và khách hàng.

**Template:**
```markdown
# Software Requirements Specification (SRS) - [Tên Dự Án]

## 1. Giới thiệu
### 1.1 Mục đích
[Mô tả mục đích của dự án phần mềm hướng tới]
### 1.2 Phạm vi dự án
[Liệt kê những gì hệ thống sẽ thực hiện và những gì nằm ngoài phạm vi]

## 2. Mô tả tổng quan
### 2.1 Các nhóm người dùng (User Classes)
- **Admin:** [Mô tả quyền và vai trò]
- **User:** [Mô tả quyền và vai trò]
### 2.2 Môi trường vận hành
[Hệ điều hành, trình duyệt, thiết bị hỗ trợ]

## 3. Yêu cầu chức năng (Functional Requirements)
| ID | Tính năng | Mô tả (User Story) | Ưu tiên |
|:---|:---|:---|:---|
| FR-01 | Đăng nhập | Là người dùng, tôi muốn đăng nhập để truy cập dữ liệu. | Cao |
| FR-02 | Tìm kiếm | Là người dùng, tôi muốn tìm sản phẩm theo tên. | Trung bình |

## 4. Yêu cầu phi chức năng (Non-functional Requirements)
- **Bảo mật:** [Ví dụ: Mật khẩu phải được mã hóa bcrypt]
- **Hiệu năng:** [Ví dụ: Thời gian tải trang < 2 giây]
- **Khả dụng:** [Ví dụ: Hệ thống hoạt động 99.9% uptime]
```

---

## 2. Tài liệu `2_Database.md` (Thiết kế Cơ sở Dữ liệu)

**Mục đích:** Đặc tả cách lưu trữ, tổ chức dữ liệu và các mối quan hệ giữa các thực thể trong hệ thống.

**Template:**
```markdown
# Database Design Documentation - [Tên Dự Án]

## 1. Sơ đồ thực thể liên kết (ERD)
![Sơ đồ ERD](link_anh_erd_cua_ban_o_day)

## 2. Chi tiết các bảng (Table Schema)

### 2.1 Bảng `users` (Người dùng)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|:---|:---|:---|:---|
| `id` | INT | PK, Auto Increment | ID duy nhất |
| `username` | VARCHAR(50) | Not Null, Unique | Tên đăng nhập |
| `password` | VARCHAR(255) | Not Null | Mật khẩu đã mã hóa |
| `created_at` | DATETIME | Default Current_Timestamp | Thời gian tạo |

### 2.2 Bảng `products` (Sản phẩm)
| Cột | Kiểu dữ liệu | Ràng buộc | Mô tả |
|:---|:---|:---|:---|
| `id` | INT | PK | ID sản phẩm |
| `name` | VARCHAR(100) | Not Null | Tên sản phẩm |
| `price` | DECIMAL(10,2)| Not Null | Giá bán |

## 3. Quy ước đặt tên
- Tên bảng: `snake_case`, số nhiều (VD: `users`, `orders`).
- Khóa chính: `id`.
- Khóa ngoại: `[tên_bảng_số_ít]_id` (VD: `user_id`).
```

---

## 3. Tài liệu `3_API_Specs.md` (Đặc tả API)

**Mục đích:** Làm "hợp đồng" giao tiếp giữa Front-end và Back-end, liệt kê chi tiết các endpoint, dữ liệu đầu vào và kết quả trả về.

**Template:**
```markdown
# API Specification - [Tên Dự Án]

## 1. Thông tin chung
- **Base URL:** `https://api.yourdomain.com/v1`
- **Định dạng:** `application/json`
- **Xác thực:** Bearer Token (JWT)

## 2. Chi tiết Endpoints

### 2.1 Lấy danh sách sản phẩm
- **Endpoint:** `/products`
- **Method:** `GET`
- **Query Params:**
  - `limit` (int, optional): Số lượng item trên 1 trang.
  - `page` (int, optional): Số trang.
- **Response (200 OK):**
  {
    "status": "success",
    "data": [
      { "id": 1, "name": "Sản phẩm A", "price": 100000 }
    ]
  }
  

### 2.2 Thêm sản phẩm mới
- **Endpoint:** `/products`
- **Method:** `POST`
- **Body:**
  {
    "name": "Sản phẩm mới",
    "price": 250000,
    "category_id": 5
  }
  
- **Responses:**
    - `201 Created`: Thêm mới thành công.
    - `400 Bad Request`: Dữ liệu đầu vào không hợp lệ.
    - `401 Unauthorized`: Chưa xác thực (thiếu token).

```
---

## 4. Tài liệu `4_Sprint_Logs.md` (Nhật ký Tiến độ / Sprint)

**Mục đích:** Theo dõi tiến độ công việc, phân công nhiệm vụ và tổng kết rút kinh nghiệm sau mỗi chu kỳ (Sprint).

**Template:**
```markdown
# Project Sprint Logs - [Tên Dự Án]

## Sprint 01: [Tên/Chủ đề của Sprint]
- **Thời gian:** [Ngày bắt đầu] - [Ngày kết thúc]
- **Mục tiêu (Sprint Goal):** [Ghi rõ mục tiêu lớn nhất cần đạt được trong Sprint này]

### 1. Danh sách công việc (Backlog)
| ID | Tên Task / User Story | Người phụ trách | Trạng thái | Ghi chú |
|:---|:---|:---|:---|:---|
| T-01 | Thiết lập Server & Database | Nguyễn Văn A | [x] Done | Đã deploy lên staging |
| T-02 | Thiết kế UI trang Đăng nhập | Trần Thị B | [/] In Progress | Chờ chốt màu sắc |
| T-03 | Code API Đăng nhập | Lê Văn C | [ ] To Do | Chờ T-01 xong |

*Ghi chú trạng thái: [ ] To Do, [/] In Progress, [x] Done*

### 2. Tổng kết Sprint (Sprint Retrospective)
- **Thành công:** [Những việc team làm tốt, đúng tiến độ]
- **Khó khăn:** [Những bottleneck, bug hoặc lý do trễ tiến độ]
- **Hành động cải thiện (Action Items):** [Các giải pháp cho Sprint sau]
```
