# 🎫 Ticket Support System - Frontend Documentation

Hệ thống quản lý ticket hỗ trợ khách hàng hiện đại với giao diện đẹp mắt, chức năng đầy đủ.

## 📋 Nội Dung

- [Cấu Trúc Dự Án](#cấu-trúc-dự-án)
- [Cài Đặt & Chạy](#cài-đặt--chạy)
- [Trang Web](#trang-web)
- [Hướng Dẫn Sử Dụng](#hướng-dẫn-sử-dụng)
- [API Integration](#api-integration)
- [Tùy Chỉnh & Mở Rộng](#tùy-chỉnh--mở-rộng)

---

## 📁 Cấu Trúc Dự Án

```
frontend/
├── index.html              # 🏠 Trang chủ (Landing page)
├── login.html              # 🔐 Đăng nhập
├── register.html           # 📝 Đăng ký
├── customer.html           # 👤 Trang khách hàng
├── staff.html              # 👨‍🔧 Trang nhân viên
├── admin.html              # ⚙️ Trang quản trị
├── js/
│   ├── api.js              # API client với interceptors
│   ├── auth.js             # Authentication manager
│   ├── helper.js           # Utility functions
│   └── notification.js     # Toast & Alert notifications
└── README.md               # Tài liệu này
```

---

## 🚀 Cài Đặt & Chạy

### Yêu Cầu
- Node.js 14+ (cho development server)
- Modern browser (Chrome, Firefox, Safari, Edge)
- Backend API đang chạy (localhost:5050)

### Chạy Dự Án

**Option 1: Sử dụng Live Server (Visual Studio Code)**
```bash
# Cài Live Server extension
# Chuột phải vào thư mục → "Open with Live Server"
# Truy cập http://localhost:5500
```

**Option 2: Sử dụng Python**
```bash
# Python 3
python -m http.server 5000

# Python 2
python -m SimpleHTTPServer 5000
```

**Option 3: Sử dụng Node.js**
```bash
npm install -g http-server
http-server -p 5000
```

**Option 4: Sử dụng Nginx/Apache**
Copy tất cả files vào webroot và cấu hình server.

---

## 📱 Trang Web

### 1. **index.html** 🏠
- **Mô tả**: Trang chủ - Giới thiệu hệ thống
- **Tính năng**:
  - Hero section với call-to-action
  - Features showcase
  - Modern UI với gradients & animations
- **Người dùng**: Chưa đăng nhập
- **Navigation**: Login → Trang tương ứng

### 2. **login.html** 🔐
- **Mô tả**: Trang đăng nhập
- **Tính năng**:
  - Login form
  - Demo accounts (test nhanh)
  - Remember me checkbox
  - Remember password link
  - Redirect tự động theo role
- **API Used**: `POST /Auth/login`, `GET /Users/me`
- **Validation**: Email format, password strength

### 3. **register.html** 📝
- **Mô tả**: Trang đăng ký tài khoản
- **Tính năng**:
  - Registration form
  - Password strength indicator
  - Confirm password
  - Terms & conditions
  - Form validation
- **API Used**: `POST /Auth/register`
- **Fields**: Fullname, Username, Email, Password

### 4. **customer.html** 👤
- **Mô tả**: Trang khách hàng - Gửi & quản lý ticket
- **Tính năng**:
  - ✅ Tạo ticket mới
  - 🔍 Search & Filter tickets
  - 📋 Xem danh sách tickets
  - 👁️ Xem chi tiết ticket
  - 💬 Thêm comments/notes
  - 🗑️ Xóa ticket (chỉ khi New)
  - 📊 Status badges
- **API Used**:
  - `GET /Tags`
  - `GET /Tickets`
  - `POST /Tickets`
  - `DELETE /Tickets/{id}`
  - `POST /Tickets/{id}/comments`
- **Role**: Customer

### 5. **staff.html** 👨‍🔧
- **Mô tả**: Trang nhân viên - Quản lý & xử lý ticket
- **Tính năng**:
  - 📊 Dashboard stats (New, In Progress, Resolved)
  - 🔍 Search tickets
  - 📋 Filter by status
  - ✅ Nhận ticket (Claim)
  - 📝 Cập nhật trạng thái
  - 👁️ Xem chi tiết
  - 🔄 Phân trang
  - 📥 Pagination
- **API Used**:
  - `GET /Tickets`
  - `GET /Users/me`
  - `PUT /Tickets/{id}/assign`
  - `PUT /Tickets/{id}/status`
- **Role**: IT_Support, Staff

### 6. **admin.html** ⚙️
- **Mô tả**: Trang quản trị - Toàn bộ hệ thống
- **Tính năng**:
  - 📊 Dashboard với biểu đồ (Chart.js)
  - 📈 Statistics (Total, Pending, Done)
  - 🏷️ Quản lý Tags (Add, Delete)
  - 👥 Assign Role (Admin, Staff, Customer)
  - 📋 Audit Logs viewer
  - 📥 Export CSV/PDF/JSON
  - 🎯 Tabbed interface
- **API Used**:
  - `GET /Tickets`
  - `GET /Tags`
  - `POST /Tags`
  - `DELETE /Tags/{id}`
  - `POST /Auth/assign-role`
  - `GET /AuditLogs`
- **Role**: Admin

---

## 📚 Hướng Dẫn Sử Dụng

### Tài Khoản Demo

```
👨‍💼 Admin
Username: admin
Password: password

👨‍🔧 Staff/IT Support
Username: staff
Password: password

👤 Customer
Username: customer
Password: password
```

### Dòng Công Việc Ticket

**1. Khách Hàng (Customer)**
- Đăng nhập → Tạo ticket mới
- Nhập tiêu đề, mô tả, chọn tags
- Gửi ticket
- Xem status: New → In Progress → Resolved → Closed
- Có thể thêm comments để hỏi/trả lời
- Chỉ có thể xóa ticket khi status = "New"

**2. Nhân Viên (Staff/IT Support)**
- Đăng nhập → Xem tất cả tickets
- Filter theo status, tìm kiếm
- Nhận ticket (Claim) → Status tự động = "In Progress"
- Xem chi tiết, hiểu vấn đề
- Cập nhật status: In Progress → Resolved → Closed
- Thêm comments để communicate với khách hàng

**3. Quản Trị Viên (Admin)**
- Đăng nhập → Xem Dashboard
- Quản lý Tags (thêm/xóa)
- Phân quyền cho users (Customer/Staff/Admin)
- Xem Audit Logs (lịch sử hoạt động)
- Export báo cáo (CSV, PDF, JSON)
- Monitor toàn bộ hệ thống

---

## 🔌 API Integration

### Cấu Hình API

```javascript
// Mặc định
const BASE_URL = '/api';

// Custom (nếu khác)
// const BASE_URL = 'http://localhost:5050/api';
```

### Sử Dụng API Module

```javascript
// Import (nếu dùng modules)
import { TicketAPI, TagAPI, AuthAPI } from './js/api.js';

// Hoặc direct reference nếu include script
// <script src="js/api.js"></script>

// Login
const data = await AuthAPI.login('username', 'password');

// Get all tickets
const tickets = await TicketAPI.getAll();

// Create ticket
const ticket = await TicketAPI.create('Title', 'Description', ['tagId1', 'tagId2']);

// Update ticket status
await TicketAPI.updateStatus('ticketId', 'Resolved');

// Get all tags
const tags = await TagAPI.getAll();

// Add tag
await TagAPI.create('New Tag Name');

// Add comment
await TicketAPI.addComment('ticketId', 'Comment content');
```

### API Endpoints

```
Auth:
  POST   /Auth/login
  POST   /Auth/register
  POST   /Auth/refresh
  POST   /Auth/assign-role

Users:
  GET    /Users/me

Tickets:
  GET    /Tickets?pageNumber=1&pageSize=100
  GET    /Tickets/{id}
  POST   /Tickets
  PUT    /Tickets/{id}
  PUT    /Tickets/{id}/assign
  PUT    /Tickets/{id}/status
  PUT    /Tickets/{id}/restore
  PUT    /Tickets/{id}/hide
  PUT    /Tickets/{id}/unhide
  DELETE /Tickets/{id}

Tags:
  GET    /Tags
  POST   /Tags
  PUT    /Tags/{id}
  DELETE /Tags/{id}

AuditLogs:
  GET    /AuditLogs
  GET    /AuditLogs/ticket/{ticketId}
```

---

## 🛠️ Utility Modules

### 1. **api.js** - API Client

```javascript
// GET request
const data = await api.get('/Tickets');

// POST request
const result = await api.post('/Tickets', { title, description, tagIds });

// PUT request
const updated = await api.put(`/Tickets/${id}`, { status: 'Resolved' });

// DELETE request
await api.delete(`/Tickets/${id}`);

// Handles authentication, errors, token refresh automatically
```

### 2. **auth.js** - Auth Manager

```javascript
// Check if logged in
if (auth.isLoggedIn()) { ... }

// Get user info
const username = auth.getUsername();
const role = auth.getRole();

// Check role
if (auth.hasRole('Admin')) { ... }
if (auth.isAdmin()) { ... }
if (auth.isStaff()) { ... }

// Require authentication
auth.requireLogin(); // redirects if not logged in

// Logout
auth.logout();
```

### 3. **helper.js** - Utilities

```javascript
// String utilities
StringUtils.truncate(str, 50);
StringUtils.capitalize(str);
StringUtils.isValidEmail(email);
StringUtils.isValidUsername(username);

// Date utilities
DateUtils.formatDate(date, 'DD/MM/YYYY HH:MM');
DateUtils.getRelativeTime(date); // "2 giờ trước"

// Array utilities
ArrayUtils.unique([1,2,2,3]);
ArrayUtils.groupBy(tickets, 'status');
ArrayUtils.sortBy(tickets, 'createdAt', 'desc');

// Ticket utilities
TicketUtils.getStatusColor('In Progress');
TicketUtils.canEdit(status);
TicketUtils.canDelete(status);
TicketUtils.calculateStats(tickets);

// DOM utilities
DOMUtils.query('.selector');
DOMUtils.show(element);
DOMUtils.hide(element);
DOMUtils.addClass(element, 'className');

// Validation
ValidationUtils.required(value);
ValidationUtils.minLength(value, 5);
ValidationUtils.validatePassword(pwd); // {strength, score}

// Storage
StorageUtils.setLocal('key', value);
StorageUtils.getLocal('key');
StorageUtils.removeLocal('key');
```

### 4. **notification.js** - Alerts & Toasts

```javascript
// Success
notify.success('Thành Công', 'Dữ liệu đã được lưu');

// Error
notify.error('Lỗi', 'Không thể thực hiện hành động');

// Warning
notify.warning('Cảnh Báo', 'Bạn có chắc chắn không?');

// Confirm
const result = await notify.confirm('Xóa', 'Bạn có chắc chắn muốn xóa?');
if (result.isConfirmed) { ... }

// Toast (nhỏ, tự động đóng)
notify.toast('Thành Công', 'Ticket đã được tạo', 'success');

// Input
const response = await notify.input('Tên Tag', 'Nhập tên tag mới');
const inputValue = response.value;

// Textarea
const response = await notify.textarea('Ghi Chú', 'Thêm ghi chú...');

// Loading
notify.loading('Đang xử lý...');
// ... do something
notify.close();

// Close current notification
notify.close();
```

---

## 🎨 Tùy Chỉnh & Mở Rộng

### Thay Đổi Màu Sắc

Tất cả trang đều sử dụng Tailwind CSS + Custom CSS variables.

**Trong file HTML `<style>` section:**

```css
/* Gradient primary */
.gradient-primary {
    background: linear-gradient(135deg, #YOUR_COLOR_1 0%, #YOUR_COLOR_2 100%);
}

/* Status colors */
.status-new { background: #YOUR_COLOR; }
.status-in-progress { background: #YOUR_COLOR; }
.status-resolved { background: #YOUR_COLOR; }
```

**Hoặc inline Tailwind:**
```html
<div class="bg-gradient-to-r from-blue-500 to-cyan-500">
```

### Thêm Tính Năng Mới

**Ví dụ: Thêm tính năng favorite ticket**

1. **Thêm API call** (`js/api.js`):
```javascript
const TicketAPI = {
    // ...existing code...
    async favorite(ticketId) {
        return api.post(`/Tickets/${ticketId}/favorite`, {});
    }
};
```

2. **Sử dụng trong HTML**:
```javascript
<button onclick="favoriteTicket('${t.id}')">⭐ Yêu Thích</button>

async function favoriteTicket(ticketId) {
    try {
        await TicketAPI.favorite(ticketId);
        notify.success('Thêm yêu thích', 'Ticket đã được thêm vào yêu thích');
    } catch (error) {
        notify.error('Lỗi', error.message);
    }
}
```

### Cải Thiện Hiệu Suất

1. **Lazy Load Images**
```html
<img src="..." loading="lazy" alt="...">
```

2. **Debounce Search**
```javascript
function debounce(func, delay) {
    let timeout;
    return function() {
        clearTimeout(timeout);
        timeout = setTimeout(func, delay);
    };
}

const handleSearch = debounce(() => {
    // Perform search
}, 300);
```

3. **Cache API Results**
```javascript
const cache = new Map();

async function getCachedData(key, fetchFn) {
    if (cache.has(key)) return cache.get(key);
    const data = await fetchFn();
    cache.set(key, data);
    return data;
}
```

### Thêm Authentication Guards

```javascript
// Protect pages
if (!auth.requireLogin()) return;

// Protect by role
if (!auth.requireRole('Admin')) return;

// Or in form submissions
if (!auth.isLoggedIn()) {
    notify.warning('Cần Đăng Nhập', 'Vui lòng đăng nhập trước');
    window.location.href = '/login.html';
    return;
}
```

---

## 🐛 Troubleshooting

### CORS Error
**Vấn đề**: `No 'Access-Control-Allow-Origin' header`

**Giải Pháp**:
1. Chắc chắn backend cho phép CORS
2. Hoặc deploy frontend & backend cùng domain
3. Hoặc sử dụng proxy

### Token Hết Hạn
**Vấn đề**: Lỗi 401 Unauthorized sau vài giờ

**Giải Pháp**:
- API sẽ tự động logout & redirect login
- Hoặc implement token refresh tự động trong `api.js`

### Comments Endpoint Not Found
**Vấn đề**: 404 khi gửi comment

**Giải Pháp**:
- Kiểm tra backend có implement `/Tickets/{id}/comments`
- Nếu không, comment lại bình luận trong UI mà không gọi API

### Chart Không Hiện
**Vấn đề**: Chart.js không load

**Giải Pháp**:
- Kiểm tra CDN link: `https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.9.1/chart.min.js`
- Hoặc install locally: `npm install chart.js`

---

## 📞 Support & Questions

- Kiểm tra console (F12) để xem lỗi
- Xem Network tab để debug API calls
- Kiểm tra backend logs
- Test API endpoints bằng Postman

---

## 📝 Changelog

### v1.0.0 (Current)
✅ Landing page đẹp mắt
✅ Authentication (Login/Register)
✅ Customer: Create, view, delete tickets
✅ Staff: List, claim, update status
✅ Admin: Dashboard, tags, roles, audit logs
✅ Utilities (API, Auth, Helpers, Notifications)
✅ Modern UI with Tailwind CSS
✅ Full responsive design
✅ SweetAlert2 notifications
✅ Chart.js statistics
✅ Export CSV/PDF/JSON

---

**Happy Coding! 🚀**