# 📘 Tài Liệu Đặc Tả RESTful API - Hệ Thống IT Helpdesk

---

## 1. Thông tin hệ thống

- **Base URL:** `http://localhost:5000/api`
- **Định dạng dữ liệu:** `application/json`
- **Xác thực:** JWT Bearer Token
- **Kiến trúc:** RESTful API

---

## 2. Cấu trúc phản hồi chuẩn

Tất cả API đều sử dụng response wrapper thống nhất:

```json
{
  "success": true,
  "message": "Thông báo kết quả",
  "data": {},
  "errors": []
}
```

---

## 3. AUTH API

### 3.1 Đăng ký tài khoản
- **Endpoint:** `POST /api/Auth/register`  
- **Mô tả:** Đăng ký tài khoản mới trên hệ thống.

### 3.2 Đăng nhập
- **Endpoint:** `POST /api/Auth/login`  
- **Mô tả:** Đăng nhập hệ thống để nhận JWT Access Token và Refresh Token.

### 3.3 Refresh token
- **Endpoint:** `POST /api/Auth/refresh`  
- **Mô tả:** Cấp lại Access Token mới khi token cũ hết hạn.

### 3.4 Gán vai trò (Admin)
- **Endpoint:** `POST /api/Auth/assign-role`  
- **Mô tả:** Quyền Admin thực hiện gán vai trò (Role) cho người dùng.

---

## 4. USERS API

### 4.1 Thông tin user hiện tại
- **Endpoint:** `GET /api/Users/me`  
- **Mô tả:** Lấy thông tin chi tiết của người dùng đang đăng nhập hiện tại.

---

## 5. TICKETS API (Core Entity)

### 5.1 Lấy danh sách ticket
- **Endpoint:** `GET /api/Tickets`  
- **Phân quyền hiển thị:**
  - User → Chỉ thấy ticket của chính mình tạo
  - Admin/Technician → Thấy toàn bộ ticket

### 5.2 Tạo ticket
- **Endpoint:** `POST /api/Tickets`  
- **Mô tả:** Tạo một yêu cầu hỗ trợ mới.

### 5.3 Xem chi tiết ticket
- **Endpoint:** `GET /api/Tickets/{id}`  
- **Mô tả:** Lấy thông tin chi tiết của một ticket cụ thể theo id.

### 5.4 Xóa ticket
- **Endpoint:** `DELETE /api/Tickets/{id}`  
- **Mô tả:** Xóa ticket khỏi hệ thống.

### 5.5 Gán người xử lý
- **Endpoint:** `PUT /api/Tickets/{id}/assign`  
- **Mô tả:** Gán kỹ thuật viên chịu trách nhiệm xử lý ticket.

### 5.6 Cập nhật trạng thái ticket

- **Endpoint:** `PUT /api/Tickets/{id}/status`

**Bảng trạng thái:**

| Code | Trạng thái   | Mô tả |
|------|-------------|------|
| 0    | Open        | Ticket mới tạo, chờ tiếp nhận |
| 1    | InProgress  | Đang trong quá trình xử lý |
| 2    | Resolved    | Đã xử lý xong, chờ xác nhận |
| 3    | Closed      | Đã đóng ticket |

### 5.7 Restore ticket
- **Endpoint:** `PUT /api/Tickets/{id}/restore`  
- **Mô tả:** Khôi phục lại ticket đã bị xóa trước đó.

### 5.8 Hide ticket
- **Endpoint:** `PUT /api/Tickets/{id}/hide`  
- **Mô tả:** Ẩn ticket khỏi danh sách hiển thị chung.

### 5.9 Unhide ticket
- **Endpoint:** `PUT /api/Tickets/{id}/unhide`  
- **Mô tả:** Hiển thị lại ticket đã bị ẩn.

---

## 6. TAGS API

### 6.1 Lấy danh sách tags
- **Endpoint:** `GET /api/Tags`  
- **Tối ưu:** Có Memory Cache giúp tăng tốc độ phản hồi.

### 6.2 Tạo tag
- **Endpoint:** `POST /api/Tags`  
- **Mô tả:** Tạo nhãn phân loại mới.

### 6.3 Cập nhật tag
- **Endpoint:** `PUT /api/Tags/{id}`  
- **Mô tả:** Cập nhật thông tin tag theo id.

### 6.4 Xóa tag
- **Endpoint:** `DELETE /api/Tags/{id}`  
- **Mô tả:** Xóa tag khỏi hệ thống.

---

## 7. AUDIT LOGS API

### 7.1 Lấy toàn bộ logs
- **Endpoint:** `GET /api/AuditLogs`  
- **Mô tả:** Dành cho Admin xem lịch sử hoạt động toàn hệ thống.

### 7.2 Lấy logs theo ticket
- **Endpoint:** `GET /api/AuditLogs/ticket/{ticketId}`  
- **Mô tả:** Lấy lịch sử thay đổi của ticket theo ticketId.

---

## 8. TỔNG KẾT

- RESTful API architecture: Thiết kế rõ ràng, nhất quán
- JWT Authentication: Bảo mật bằng token
- RBAC: Phân quyền theo vai trò
- Module hóa: Dễ mở rộng và bảo trì
- Cache Tags: Tối ưu hiệu năng
- Audit log: Theo dõi toàn bộ hành vi hệ thống
- Tickets là core entity: Trung tâm toàn bộ nghiệp vụ
```