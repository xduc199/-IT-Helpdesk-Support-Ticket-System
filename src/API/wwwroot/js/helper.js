/**
 * Helper Module - Các hàm tiện ích và utilities
 */

// ===== STRING UTILITIES =====
const StringUtils = {
    /**
     * Truncate string với ellipsis
     */
    truncate(str, length = 50) {
        if (!str) return '';
        return str.length > length ? str.substring(0, length) + '...' : str;
    },

    /**
     * Capitalize từ đầu tiên
     */
    capitalize(str) {
        if (!str) return '';
        return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
    },

    /**
     * Xóa khoảng trắng
     */
    trim(str) {
        return (str || '').trim();
    },

    /**
     * Kiểm tra email hợp lệ
     */
    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    },

    /**
     * Kiểm tra username hợp lệ (chữ, số, dấu gạch)
     */
    isValidUsername(username) {
        const usernameRegex = /^[a-zA-Z0-9_]{3,}$/;
        return usernameRegex.test(username);
    }
};

// ===== DATE UTILITIES =====
const DateUtils = {
    /**
     * Format date theo locale Việt Nam
     */
    formatDate(date, format = 'DD/MM/YYYY') {
        if (!date) return '';
        
        const d = new Date(date);
        const day = String(d.getDate()).padStart(2, '0');
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const year = d.getFullYear();
        const hours = String(d.getHours()).padStart(2, '0');
        const minutes = String(d.getMinutes()).padStart(2, '0');

        const formats = {
            'DD/MM/YYYY': `${day}/${month}/${year}`,
            'YYYY-MM-DD': `${year}-${month}-${day}`,
            'DD/MM/YYYY HH:MM': `${day}/${month}/${year} ${hours}:${minutes}`,
            'HH:MM': `${hours}:${minutes}`
        };

        return formats[format] || d.toLocaleDateString('vi-VN');
    },

    /**
     * Lấy thời gian dạng "2 giờ trước"
     */
    getRelativeTime(date) {
        if (!date) return '';

        const now = new Date();
        const d = new Date(date);
        const diffMs = now - d;
        const diffSecs = Math.floor(diffMs / 1000);
        const diffMins = Math.floor(diffSecs / 60);
        const diffHours = Math.floor(diffMins / 60);
        const diffDays = Math.floor(diffHours / 24);

        if (diffSecs < 60) return 'Vừa xong';
        if (diffMins < 60) return `${diffMins} phút trước`;
        if (diffHours < 24) return `${diffHours} giờ trước`;
        if (diffDays < 7) return `${diffDays} ngày trước`;
        
        return this.formatDate(date, 'DD/MM/YYYY');
    },

    /**
     * Kiểm tra xem ngày đã qua chưa
     */
    isPastDate(date) {
        return new Date(date) < new Date();
    }
};

// ===== ARRAY UTILITIES =====
const ArrayUtils = {
    /**
     * Làm phẳng nested array
     */
    flatten(arr) {
        return arr.reduce((flat, toFlatten) => {
            return flat.concat(Array.isArray(toFlatten) ? this.flatten(toFlatten) : toFlatten);
        }, []);
    },

    /**
     * Loại bỏ duplicates
     */
    unique(arr) {
        return [...new Set(arr)];
    },

    /**
     * Group array by property
     */
    groupBy(arr, prop) {
        return arr.reduce((groups, item) => {
            const key = item[prop];
            if (!groups[key]) groups[key] = [];
            groups[key].push(item);
            return groups;
        }, {});
    },

    /**
     * Sắp xếp array
     */
    sortBy(arr, prop, order = 'asc') {
        return [...arr].sort((a, b) => {
            if (order === 'desc') {
                return a[prop] > b[prop] ? -1 : 1;
            }
            return a[prop] < b[prop] ? -1 : 1;
        });
    }
};

// ===== OBJECT UTILITIES =====
const ObjectUtils = {
    /**
     * Kiểm tra object trống
     */
    isEmpty(obj) {
        return Object.keys(obj).length === 0;
    },

    /**
     * Clone object
     */
    clone(obj) {
        return JSON.parse(JSON.stringify(obj));
    },

    /**
     * Merge objects
     */
    merge(target, source) {
        return { ...target, ...source };
    },

    /**
     * Lấy giá trị nested
     */
    getNestedValue(obj, path) {
        return path.split('.').reduce((current, prop) => current?.[prop], obj);
    }
};

// ===== TICKET UTILITIES =====
const TicketUtils = {
    /**
     * Lấy màu status badge
     */
    getStatusColor(status) {
        const colors = {
            'New': { bg: 'bg-blue-100', text: 'text-blue-700', icon: '🆕' },
            'In Progress': { bg: 'bg-amber-100', text: 'text-amber-700', icon: '⏳' },
            'Resolved': { bg: 'bg-green-100', text: 'text-green-700', icon: '✅' },
            'Closed': { bg: 'bg-gray-100', text: 'text-gray-700', icon: '🔒' }
        };
        return colors[status] || colors['New'];
    },

    /**
     * Kiểm tra ticket có thể edit không
     */
    canEdit(status) {
        return status === 'New';
    },

    /**
     * Kiểm tra ticket có thể delete không
     */
    canDelete(status) {
        return status === 'New';
    },

    /**
     * Kiểm tra ticket có thể assign không
     */
    canAssign(status) {
        return status === 'New';
    },

    /**
     * Extract data từ response
     */
    parseResponse(response) {
        if (!response) return [];
        if (Array.isArray(response)) return response;
        if (Array.isArray(response.data)) return response.data;
        if (Array.isArray(response.Data)) return response.Data;
        return [];
    },

    /**
     * Calculate ticket statistics
     */
    calculateStats(tickets) {
        return {
            total: tickets.length,
            new: tickets.filter(t => t.status === 'New').length,
            inProgress: tickets.filter(t => t.status === 'In Progress').length,
            resolved: tickets.filter(t => t.status === 'Resolved').length,
            closed: tickets.filter(t => t.status === 'Closed').length
        };
    }
};

// ===== DOM UTILITIES =====
const DOMUtils = {
    /**
     * Query selector
     */
    query(selector) {
        return document.querySelector(selector);
    },

    /**
     * Query all selectors
     */
    queryAll(selector) {
        return document.querySelectorAll(selector);
    },

    /**
     * Tạo element
     */
    createElement(tag, className = '', innerHTML = '') {
        const el = document.createElement(tag);
        if (className) el.className = className;
        if (innerHTML) el.innerHTML = innerHTML;
        return el;
    },

    /**
     * Show/Hide element
     */
    show(el) {
        if (el) el.classList.remove('hidden');
    },

    hide(el) {
        if (el) el.classList.add('hidden');
    },

    toggle(el) {
        if (el) el.classList.toggle('hidden');
    },

    /**
     * Add/Remove class
     */
    addClass(el, className) {
        if (el) el.classList.add(className);
    },

    removeClass(el, className) {
        if (el) el.classList.remove(className);
    },

    /**
     * Set attributes
     */
    attr(el, name, value) {
        if (el) {
            if (value === undefined) {
                return el.getAttribute(name);
            }
            el.setAttribute(name, value);
        }
    },

    /**
     * Clear element
     */
    clear(el) {
        if (el) el.innerHTML = '';
    }
};

// ===== VALIDATION UTILITIES =====
const ValidationUtils = {
    /**
     * Validate required field
     */
    required(value) {
        return value && value.trim() !== '';
    },

    /**
     * Validate min length
     */
    minLength(value, min) {
        return value && value.length >= min;
    },

    /**
     * Validate max length
     */
    maxLength(value, max) {
        return value && value.length <= max;
    },

    /**
     * Validate password strength
     */
    validatePassword(password) {
        const score = {
            length: password.length >= 8 ? 1 : 0,
            lowercase: /[a-z]/.test(password) ? 1 : 0,
            uppercase: /[A-Z]/.test(password) ? 1 : 0,
            number: /\d/.test(password) ? 1 : 0,
            special: /[!@#$%^&*]/.test(password) ? 1 : 0
        };
        
        const total = Object.values(score).reduce((a, b) => a + b, 0);
        
        if (total <= 2) return { strength: 'weak', score: 20 };
        if (total <= 3) return { strength: 'fair', score: 40 };
        if (total <= 4) return { strength: 'good', score: 60 };
        return { strength: 'strong', score: 100 };
    }
};

// ===== STORAGE UTILITIES =====
const StorageUtils = {
    /**
     * Set localStorage
     */
    setLocal(key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },

    /**
     * Get localStorage
     */
    getLocal(key) {
        const value = localStorage.getItem(key);
        try {
            return JSON.parse(value);
        } catch {
            return value;
        }
    },

    /**
     * Remove localStorage
     */
    removeLocal(key) {
        localStorage.removeItem(key);
    },

    /**
     * Clear localStorage
     */
    clearLocal() {
        localStorage.clear();
    },

    /**
     * Set sessionStorage
     */
    setSession(key, value) {
        sessionStorage.setItem(key, JSON.stringify(value));
    },

    /**
     * Get sessionStorage
     */
    getSession(key) {
        const value = sessionStorage.getItem(key);
        try {
            return JSON.parse(value);
        } catch {
            return value;
        }
    }
};

// Export
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        StringUtils,
        DateUtils,
        ArrayUtils,
        ObjectUtils,
        TicketUtils,
        DOMUtils,
        ValidationUtils,
        StorageUtils
    };
}
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>