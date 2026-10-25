/**
 * API Module - Quản lý tất cả các request API
 * Bao gồm token management, error handling, interceptors
 */

const API_CONFIG = {
    BASE_URL: 'http://localhost:5050/api',
    TIMEOUT: 10000,
    RETRY_ATTEMPTS: 3
};

class APIClient {
    constructor(config = {}) {
        this.config = { ...API_CONFIG, ...config };
        this.token = localStorage.getItem('accessToken');
    }

    /**
     * Tạo header mặc định với token
     */
    getHeaders() {
        const headers = {
            'Content-Type': 'application/json'
        };

        if (this.token) {
            headers['Authorization'] = `Bearer ${this.token}`;
        }

        return headers;
    }

    /**
     * Xử lý lỗi từ response
     */
    async handleError(response) {
        if (response.status === 401) {
            // Token hết hạn
            localStorage.clear();
            window.location.href = '/login.html';
            throw new Error('Phiên làm việc hết hạn. Vui lòng đăng nhập lại.');
        }

        if (response.status === 403) {
            throw new Error('Bạn không có quyền thực hiện hành động này.');
        }

        if (response.status === 404) {
            throw new Error('Dữ liệu không tìm thấy.');
        }

        if (response.status >= 500) {
            throw new Error('Lỗi máy chủ. Vui lòng thử lại sau.');
        }

        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || `Lỗi ${response.status}`);
    }

    /**
     * GET request
     */
    async get(endpoint, options = {}) {
        try {
            const url = `${this.config.BASE_URL}${endpoint}`;
            const response = await fetch(url, {
                method: 'GET',
                headers: this.getHeaders(),
                ...options
            });

            if (!response.ok) {
                await this.handleError(response);
            }

            return await response.json();
        } catch (error) {
            throw error;
        }
    }

    /**
     * POST request
     */
    async post(endpoint, data = {}, options = {}) {
        try {
            const url = `${this.config.BASE_URL}${endpoint}`;
            const response = await fetch(url, {
                method: 'POST',
                headers: this.getHeaders(),
                body: JSON.stringify(data),
                ...options
            });

            if (!response.ok) {
                await this.handleError(response);
            }

            return await response.json();
        } catch (error) {
            throw error;
        }
    }

    /**
     * PUT request
     */
    async put(endpoint, data = {}, options = {}) {
        try {
            const url = `${this.config.BASE_URL}${endpoint}`;
            const response = await fetch(url, {
                method: 'PUT',
                headers: this.getHeaders(),
                body: JSON.stringify(data),
                ...options
            });

            if (!response.ok) {
                await this.handleError(response);
            }

            return await response.json();
        } catch (error) {
            throw error;
        }
    }

    /**
     * DELETE request
     */
    async delete(endpoint, options = {}) {
        try {
            const url = `${this.config.BASE_URL}${endpoint}`;
            const response = await fetch(url, {
                method: 'DELETE',
                headers: this.getHeaders(),
                ...options
            });

            if (!response.ok) {
                await this.handleError(response);
            }

            return await response.json().catch(() => ({}));
        } catch (error) {
            throw error;
        }
    }
}

// Tạo instance duy nhất
const api = new APIClient();

// ===== AUTH API =====
const AuthAPI = {
    async login(username, password) {
        const data = await api.post('/Auth/login', { username, password });
        localStorage.setItem('accessToken', data.token);
        localStorage.setItem('username', data.username);
        api.token = data.token;
        return data;
    },

    async register(fullname, username, email, password) {
        return api.post('/Auth/register', { fullname, username, email, password });
    },

    async refreshToken() {
        const data = await api.post('/Auth/refresh', {});
        localStorage.setItem('accessToken', data.token);
        api.token = data.token;
        return data;
    },

    async assignRole(username, role) {
        return api.post('/Auth/assign-role', { username, role });
    },

    logout() {
        localStorage.clear();
        window.location.href = '/login.html';
    }
};

// ===== USER API =====
const UserAPI = {
    async getMe() {
        return api.get('/Users/me');
    },

    async getProfile() {
        return api.get('/Users/me');
    }
};

// ===== TICKET API =====
const TicketAPI = {
    async getAll(pageNumber = 1, pageSize = 100) {
        return api.get(`/Tickets?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    },

    async getById(ticketId) {
        return api.get(`/Tickets/${ticketId}`);
    },

    async create(title, description, tagIds = []) {
        return api.post('/Tickets', { title, description, tagIds });
    },

    async update(ticketId, data) {
        return api.put(`/Tickets/${ticketId}`, data);
    },

    async delete(ticketId) {
        return api.delete(`/Tickets/${ticketId}`);
    },

    async assign(ticketId, assignedToId) {
        return api.put(`/Tickets/${ticketId}/assign`, { assignedToId });
    },

    async updateStatus(ticketId, status) {
        return api.put(`/Tickets/${ticketId}/status`, { status });
    },

    async restore(ticketId) {
        return api.put(`/Tickets/${ticketId}/restore`, {});
    },

    async hide(ticketId) {
        return api.put(`/Tickets/${ticketId}/hide`, {});
    },

    async unhide(ticketId) {
        return api.put(`/Tickets/${ticketId}/unhide`, {});
    },

    async addComment(ticketId, content) {
        return api.post(`/Tickets/${ticketId}/comments`, { content });
    }
};

// ===== TAG API =====
const TagAPI = {
    async getAll() {
        return api.get('/Tags');
    },

    async create(name) {
        return api.post('/Tags', { name });
    },

    async update(tagId, name) {
        return api.put(`/Tags/${tagId}`, { name });
    },

    async delete(tagId) {
        return api.delete(`/Tags/${tagId}`);
    }
};

// ===== AUDIT LOG API =====
const AuditLogAPI = {
    async getAll() {
        return api.get('/AuditLogs');
    },

    async getByTicket(ticketId) {
        return api.get(`/AuditLogs/ticket/${ticketId}`);
    }
};

// Export
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { api, AuthAPI, UserAPI, TicketAPI, TagAPI, AuditLogAPI };
}
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>