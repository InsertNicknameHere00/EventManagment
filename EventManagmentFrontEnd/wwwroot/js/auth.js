// Common authentication utilities
function isAuthenticated() {
    const token = localStorage.getItem('token');
    return !!token;
}

function getUserEmail() {
    return localStorage.getItem('userEmail');
}

function getAuthToken() {
    return localStorage.getItem('token');
}

function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userEmail');
    localStorage.removeItem('userData');
    window.location.href = '/';
}

function initializeAuth() {
    const token = localStorage.getItem('token');
    const isLoggedIn = !!token;
    
    // Show/hide protected navigation items
    document.querySelectorAll('.protected-nav').forEach(el => {
        el.style.display = isLoggedIn ? 'block' : 'none';
    });
    
    // Show/hide authentication navigation items
    document.querySelectorAll('.auth-nav').forEach(el => {
        el.style.display = isLoggedIn ? 'none' : 'block';
    });
    
    // Setup logout button
    const logoutBtn = document.getElementById('logoutBtn');
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function(e) {
            e.preventDefault();
            logout();
        });
    }
}

function redirectIfNotAuthenticated(redirectUrl = '/Login') {
    if (!isAuthenticated()) {
        window.location.href = redirectUrl;
        return true;
    }
    return false;
}

function redirectIfAuthenticated(redirectUrl = '/Events') {
    if (isAuthenticated()) {
        window.location.href = redirectUrl;
        return true;
    }
    return false;
}

// Common toast function
function showToast(message, type = 'info') {
    const toast = document.getElementById('toastMessage');
    if (!toast) return;
    
    const toastBody = toast.querySelector('.toast-body');
    const toastIcon = toast.querySelector('.toast-header i');
    
    toastBody.textContent = message;
    
    // Update icon based on type
    if (toastIcon) {
        toastIcon.className = 'me-2';
        if (type === 'success') {
            toastIcon.classList.add('fas', 'fa-check-circle', 'text-success');
        } else if (type === 'error') {
            toastIcon.classList.add('fas', 'fa-exclamation-triangle', 'text-danger');
        } else {
            toastIcon.classList.add('fas', 'fa-info-circle', 'text-primary');
        }
    }
    
    // Add color based on type
    toast.classList.remove('bg-success', 'bg-danger', 'bg-info');
    if (type === 'success') toast.classList.add('bg-success', 'text-white');
    if (type === 'error') toast.classList.add('bg-danger', 'text-white');
    if (type === 'info') toast.classList.add('bg-info', 'text-white');

    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();
}

// Initialize auth on page load
document.addEventListener('DOMContentLoaded', function() {
    initializeAuth();
});