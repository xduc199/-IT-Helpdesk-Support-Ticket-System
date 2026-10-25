/**
 * Notification Module - Quản lý toast notifications và alerts
 * Sử dụng SweetAlert2 library
 */

class NotificationManager {
    constructor() {
        // SweetAlert2 đã được load từ CDN
        this.defaultPosition = 'top-end';
    }

    /**
     * Success notification
     */
    success(title, message = '', options = {}) {
        return Swal.fire({
            icon: 'success',
            title: title,
            text: message,
            confirmButtonColor: '#10b981',
            ...options
        });
    }

    /**
     * Error notification
     */
    error(title, message = '', options = {}) {
        return Swal.fire({
            icon: 'error',
            title: title,
            text: message,
            confirmButtonColor: '#ef4444',
            ...options
        });
    }

    /**
     * Warning notification
     */
    warning(title, message = '', options = {}) {
        return Swal.fire({
            icon: 'warning',
            title: title,
            text: message,
            confirmButtonColor: '#f59e0b',
            ...options
        });
    }

    /**
     * Info notification
     */
    info(title, message = '', options = {}) {
        return Swal.fire({
            icon: 'info',
            title: title,
            text: message,
            confirmButtonColor: '#3b82f6',
            ...options
        });
    }

    /**
     * Confirm dialog
     */
    confirm(title, message = '', options = {}) {
        return Swal.fire({
            icon: 'question',
            title: title,
            text: message,
            showCancelButton: true,
            confirmButtonColor: '#10b981',
            cancelButtonColor: '#6b7280',
            confirmButtonText: 'Xác Nhận',
            cancelButtonText: 'Hủy',
            ...options
        });
    }

    /**
     * Toast notification (nhỏ, tự động đóng)
     */
    toast(title, message = '', type = 'success', position = 'top-end') {
        const icons = {
            success: 'success',
            error: 'error',
            warning: 'warning',
            info: 'info'
        };

        const colors = {
            success: '#10b981',
            error: '#ef4444',
            warning: '#f59e0b',
            info: '#3b82f6'
        };

        Swal.fire({
            icon: icons[type] || icons.success,
            title: title,
            toast: true,
            position: position,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            background: '#1f2937',
            titleText: title,
            html: message,
            color: '#e5e7eb',
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer);
                toast.addEventListener('mouseleave', Swal.resumeTimer);
            }
        });
    }

    /**
     * Loading notification
     */
    loading(title = 'Đang xử lý...', message = '') {
        Swal.fire({
            title: title,
            html: message,
            allowOutsideClick: false,
            allowEscapeKey: false,
            didOpen: async () => {
                await Swal.showLoading();
            }
        });
    }

    /**
     * Đóng notification hiện tại
     */
    close() {
        Swal.close();
    }

    /**
     * Input dialog
     */
    input(title, message = '', options = {}) {
        return Swal.fire({
            title: title,
            text: message,
            input: 'text',
            inputPlaceholder: 'Nhập dữ liệu...',
            showCancelButton: true,
            confirmButtonColor: '#10b981',
            cancelButtonColor: '#6b7280',
            confirmButtonText: 'Xác Nhận',
            cancelButtonText: 'Hủy',
            ...options
        });
    }

    /**
     * Textarea dialog
     */
    textarea(title, message = '', options = {}) {
        return Swal.fire({
            title: title,
            text: message,
            input: 'textarea',
            inputPlaceholder: 'Nhập nội dung...',
            inputAttributes: {
                'aria-label': 'Nhập nội dung của bạn ở đây',
                rows: 8
            },
            showCancelButton: true,
            confirmButtonColor: '#10b981',
            cancelButtonColor: '#6b7280',
            confirmButtonText: 'Xác Nhận',
            cancelButtonText: 'Hủy',
            ...options
        });
    }

    /**
     * Select dialog
     */
    select(title, options = {}, selectOptions = []) {
        const optionsHtml = selectOptions
            .map(opt => `<option value="${opt.value}">${opt.label}</option>`)
            .join('');

        return Swal.fire({
            title: title,
            html: `<select id="select-input" class="swal2-input" style="height: 40px; padding: 10px;">${optionsHtml}</select>`,
            showCancelButton: true,
            confirmButtonColor: '#10b981',
            cancelButtonColor: '#6b7280',
            confirmButtonText: 'Xác Nhận',
            cancelButtonText: 'Hủy',
            preConfirm: () => {
                const value = document.getElementById('select-input')?.value;
                if (!value) {
                    Swal.showValidationMessage('Vui lòng chọn một tùy chọn');
                }
                return value;
            },
            ...options
        });
    }

    /**
     * HTML dialog
     */
    html(title, htmlContent, options = {}) {
        return Swal.fire({
            title: title,
            html: htmlContent,
            showCancelButton: true,
            confirmButtonColor: '#10b981',
            cancelButtonColor: '#6b7280',
            confirmButtonText: 'Xác Nhận',
            cancelButtonText: 'Hủy',
            ...options
        });
    }
}

// Tạo instance duy nhất
const notify = new NotificationManager();

// Shortcuts cho dễ dùng
const Notify = {
    success: (title, message) => notify.success(title, message),
    error: (title, message) => notify.error(title, message),
    warning: (title, message) => notify.warning(title, message),
    info: (title, message) => notify.info(title, message),
    confirm: (title, message) => notify.confirm(title, message),
    toast: (title, message, type) => notify.toast(title, message, type),
    loading: (title) => notify.loading(title),
    close: () => notify.close(),
    input: (title, message) => notify.input(title, message),
    textarea: (title, message) => notify.textarea(title, message),
    select: (title, options) => notify.select(title, {}, options)
};

// Export
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { notify, Notify, NotificationManager };
}
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.0/signalr.min.js"></script>