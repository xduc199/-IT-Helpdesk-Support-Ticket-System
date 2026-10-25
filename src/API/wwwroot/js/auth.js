/**
 * Auth Module - Quản lý authentication và authorization
 */

class AuthManager {
    constructor() {
        this.token = localStorage.getItem('accessToken');
        this.username = localStorage.getItem('username');
        this.role = localStorage.getItem('role');
    }

    /**
     * Kiểm tra xem user có đăng nhập không
     */
    isLoggedIn() {
        return !!this.token;
    }

    /**
     * Kiểm tra xem user có role cần thiết không
     */
    hasRole(requiredRole) {
        if (Array.isArray(requiredRole)) {
            return requiredRole.includes(this.role);
        }
        return this.role === requiredRole;
    }

    /**
     * Lưu thông tin auth
     */
    saveAuthData(token, username, role) {
        localStorage.setItem('accessToken', token);
        localStorage.setItem('username', username);
        if (role) localStorage.setItem('role', role);
        
        this.token = token;
        this.username = username;
        this.role = role;
    }

    /**
     * Xóa thông tin auth
     */
    clearAuth() {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('username');
        localStorage.removeItem('role');
        
        this.token = null;
        this.username = null;
        this.role = null;
    }

    /**
     * Logout
     */
    logout() {
        this.clearAuth();
        window.location.href = '/login.html';
    }

    /**
     * Kiểm tra auth và redirect
     */
    requireLogin() {
        if (!this.isLoggedIn()) {
            window.location.href = '/login.html';
            return false;
        }
        return true;
    }

    /**
     * Kiểm tra auth và role, redirect nếu không hợp lệ
     */
    requireRole(requiredRole) {
        if (!this.requireLogin()) return false;
        
        if (!this.hasRole(requiredRole)) {
            window.location.href = '/index.html';
            return false;
        }
        
        return true;
    }

    /**
     * Lấy thông tin user
     */
    getUsername() {
        return this.username;
    }

    /**
     * Lấy role user
     */
    getRole() {
        return this.role;
    }

    /**
     * Lấy token
     */
    getToken() {
        return this.token;
    }

    /**
     * Kiểm tra các roles
     */
    isAdmin() {
        return this.role === 'Admin';
    }

    isStaff() {
        return this.role === 'IT_Support' || this.role === 'Staff';
    }

    isCustomer() {
        return this.role === 'Customer';
    }

    /**
     * Redirect dựa trên role
     */
    redirectByRole() {
        if (this.isAdmin()) {
            window.location.href = '/admin.html';
        } else if (this.isStaff()) {
            window.location.href = '/staff.html';
        } else {
            window.location.href = '/customer.html';
        }
    }
}

// Tạo instance duy nhất
const auth = new AuthManager();

// Preload auth info lên page
document.addEventListener('DOMContentLoaded', () => {
    // Có thể tự động load user info nếu cần
    // Ví dụ: fetch user details từ API
});

// Export
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { auth, AuthManager };
}
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>