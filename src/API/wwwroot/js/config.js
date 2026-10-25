/**
 * Configuration File
 * Centralized configuration cho toàn bộ ứng dụng
 */

const CONFIG = {
    // ===== API Configuration =====
    API: {
        
        BASE_URL: 'http://localhost:5050/api', // Uncomment nếu cần
        TIMEOUT: 10000, // milliseconds
        RETRY_ATTEMPTS: 3
    },

    // ===== Application Settings =====
    APP: {
        NAME: 'Ticket Support System',
        VERSION: '1.0.0',
        LANGUAGE: 'vi-VN',
        
        // Pagination
        PAGINATION: {
            DEFAULT_PAGE_SIZE: 10,
            MAX_PAGE_SIZE: 100
        },

        // Search
        SEARCH: {
            DEBOUNCE_DELAY: 300, // milliseconds
            MIN_SEARCH_LENGTH: 2
        },

        // Upload
        UPLOAD: {
            MAX_FILE_SIZE: 5 * 1024 * 1024, // 5MB
            ALLOWED_TYPES: ['image/jpeg', 'image/png', 'application/pdf']
        }
    },

    // ===== UI/UX Settings =====
    UI: {
        // Toast notifications
        TOAST: {
            DURATION: 3000, // milliseconds
            POSITION: 'top-end' // 'top-start', 'top', 'top-end', 'center-start', 'center', 'center-end', 'bottom-start', 'bottom', 'bottom-end'
        },

        // Animation
        ANIMATION: {
            ENABLED: true,
            DURATION: 300 // milliseconds
        },

        // Theme
        THEME: {
            PRIMARY_COLOR: '#10b981', // Emerald
            SECONDARY_COLOR: '#3b82f6', // Blue
            DANGER_COLOR: '#ef4444', // Red
            WARNING_COLOR: '#f59e0b', // Amber
            SUCCESS_COLOR: '#10b981', // Green
            INFO_COLOR: '#3b82f6' // Blue
        }
    },

    // ===== Validation Rules =====
    VALIDATION: {
        USERNAME: {
            MIN_LENGTH: 3,
            MAX_LENGTH: 30,
            PATTERN: /^[a-zA-Z0-9_]{3,}$/ // Chữ, số, dấu gạch
        },
        PASSWORD: {
            MIN_LENGTH: 6,
            MAX_LENGTH: 50,
            REQUIRE_UPPERCASE: false,
            REQUIRE_NUMBER: false,
            REQUIRE_SPECIAL: false
        },
        EMAIL: {
            PATTERN: /^[^\s@]+@[^\s@]+\.[^\s@]+$/
        },
        TICKET: {
            TITLE: {
                MIN_LENGTH: 5,
                MAX_LENGTH: 200
            },
            DESCRIPTION: {
                MIN_LENGTH: 10,
                MAX_LENGTH: 5000
            }
        }
    },

    // ===== Feature Flags =====
    FEATURES: {
        COMMENTS: true, // Cho phép comment trên ticket
        ATTACHMENTS: false, // File attachments (cần implement)
        NOTIFICATIONS: true, // Real-time notifications
        AUDIT_LOGS: true, // Lịch sử hoạt động
        EXPORT: true, // Export data
        ADVANCED_SEARCH: true, // Advanced filters
        TWO_FACTOR_AUTH: false, // 2FA (cần implement)
        DARK_MODE: false // Dark mode toggle (tính năng)
    },

    // ===== Status Configuration =====
    TICKET_STATUS: {
        NEW: 'New',
        IN_PROGRESS: 'In Progress',
        RESOLVED: 'Resolved',
        CLOSED: 'Closed',
        
        // Display info
        DISPLAY: {
            'New': { icon: '🆕', color: 'blue', label: 'Mới' },
            'In Progress': { icon: '⏳', color: 'amber', label: 'Đang Xử Lý' },
            'Resolved': { icon: '✅', color: 'green', label: 'Đã Giải Quyết' },
            'Closed': { icon: '🔒', color: 'gray', label: 'Đóng' }
        }
    },

    // ===== Role Configuration =====
    USER_ROLES: {
        ADMIN: 'Admin',
        STAFF: 'IT_Support',
        CUSTOMER: 'Customer',

        // Permissions
        PERMISSIONS: {
            'Admin': ['create_tag', 'delete_tag', 'view_logs', 'assign_role', 'export_data', 'view_all_tickets'],
            'IT_Support': ['assign_ticket', 'update_status', 'view_all_tickets', 'add_comment'],
            'Customer': ['create_ticket', 'delete_ticket', 'view_own_tickets', 'add_comment']
        }
    },

    // ===== Cache Configuration =====
    CACHE: {
        ENABLED: true,
        DURATION: 5 * 60 * 1000, // 5 minutes in milliseconds

        // Keys to cache
        KEYS: {
            TAGS: 'cache_tags',
            USERS: 'cache_users',
            TICKETS: 'cache_tickets'
        }
    },

    // ===== Logging Configuration =====
    LOGGING: {
        ENABLED: true,
        LEVEL: 'info', // 'debug', 'info', 'warn', 'error'
        CONSOLE: true,
        STORAGE: false // Log to localStorage (cần implement)
    },

    // ===== Security =====
    SECURITY: {
        // Token
        TOKEN: {
            STORAGE_KEY: 'accessToken',
            REFRESH_BEFORE_EXPIRY: 5 * 60 * 1000 // 5 minutes
        },

        // CSRF (nếu cần)
        CSRF_ENABLED: false,
        CSRF_HEADER: 'X-CSRF-Token'
    },

    // ===== Messages =====
    MESSAGES: {
        // Success
        SUCCESS: {
            TICKET_CREATED: 'Ticket đã được tạo thành công',
            TICKET_UPDATED: 'Ticket đã được cập nhật',
            TICKET_DELETED: 'Ticket đã được xóa',
            TAG_CREATED: 'Tag đã được tạo',
            TAG_DELETED: 'Tag đã được xóa',
            LOGIN_SUCCESS: 'Đăng nhập thành công',
            REGISTER_SUCCESS: 'Đăng ký thành công'
        },

        // Error
        ERROR: {
            NETWORK_ERROR: 'Lỗi kết nối. Vui lòng kiểm tra internet connection',
            SERVER_ERROR: 'Lỗi máy chủ. Vui lòng thử lại sau',
            UNAUTHORIZED: 'Phiên làm việc hết hạn. Vui lòng đăng nhập lại',
            FORBIDDEN: 'Bạn không có quyền thực hiện hành động này',
            NOT_FOUND: 'Dữ liệu không tìm thấy',
            VALIDATION_ERROR: 'Dữ liệu không hợp lệ',
            UNKNOWN_ERROR: 'Đã xảy ra lỗi không xác định'
        },

        // Warning
        WARNING: {
            UNSAVED_CHANGES: 'Bạn có những thay đổi chưa lưu',
            DELETE_CONFIRMATION: 'Bạn có chắc chắn muốn xóa?',
            SESSION_EXPIRING: 'Phiên làm việc sắp hết hạn'
        },

        // Info
        INFO: {
            LOADING: 'Đang tải dữ liệu...',
            NO_DATA: 'Chưa có dữ liệu'
        }
    },

    // ===== Demo Accounts (chỉ dùng cho development) =====
    DEMO_ACCOUNTS: [
        {
            username: 'admin',
            password: 'password',
            role: 'Admin',
            label: '👨‍💼 Admin'
        },
        {
            username: 'staff',
            password: 'password',
            role: 'IT_Support',
            label: '👨‍🔧 Staff'
        },
        {
            username: 'customer',
            password: 'password',
            role: 'Customer',
            label: '👤 Customer'
        }
    ]
};

// ===== Helper Functions =====

/**
 * Lấy configuration value
 */
function getConfig(path) {
    const keys = path.split('.');
    let value = CONFIG;
    
    for (const key of keys) {
        value = value[key];
        if (value === undefined) return null;
    }
    
    return value;
}

/**
 * Set configuration value (runtime)
 */
function setConfig(path, value) {
    const keys = path.split('.');
    const lastKey = keys.pop();
    let obj = CONFIG;
    
    for (const key of keys) {
        if (!(key in obj)) obj[key] = {};
        obj = obj[key];
    }
    
    obj[lastKey] = value;
}

/**
 * Reset configuration to default
 */
function resetConfig() {
    // Re-initialize CONFIG object
    // Hoặc reload page
    location.reload();
}

// ===== Export =====
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { CONFIG, getConfig, setConfig, resetConfig };
}
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>