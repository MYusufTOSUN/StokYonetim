/* DattaAble Template Core JavaScript */

(function () {
    'use strict';

    // DOM Content Loaded
    document.addEventListener('DOMContentLoaded', function () {
        initializeSidebar();
        initializeTooltips();
        initializeDropdowns();
        initializeMobileMenu();
    });

    // Sidebar Management
    function initializeSidebar() {
        const sidebar = document.querySelector('.pcoded-navbar');
        const mainContainer = document.querySelector('.pcoded-main-container');
        const header = document.querySelector('.pcoded-header');

        // Set active menu item
        const currentPath = window.location.pathname;
        const menuLinks = document.querySelectorAll('.pcoded-inner-navbar a');

        menuLinks.forEach(link => {
            if (link.getAttribute('href') === currentPath) {
                link.classList.add('active');
            }
        });

        // Mobile menu toggle
        const menuToggle = document.createElement('button');
        menuToggle.className = 'navbar-toggler d-lg-none';
        menuToggle.innerHTML = '<i class="feather icon-menu"></i>';
        menuToggle.style.cssText = 'position: fixed; top: 15px; left: 15px; z-index: 1001; background: var(--bs-primary); color: white; border: none; padding: 10px; border-radius: 6px;';

        document.body.appendChild(menuToggle);

        menuToggle.addEventListener('click', function () {
            sidebar.classList.toggle('show-sidebar');
        });
    }

    // Initialize Tooltips
    function initializeTooltips() {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // Initialize Dropdowns
    function initializeDropdowns() {
        const dropdownElementList = [].slice.call(document.querySelectorAll('.dropdown-toggle'));
        dropdownElementList.map(function (dropdownToggleEl) {
            return new bootstrap.Dropdown(dropdownToggleEl);
        });
    }

    // Mobile Menu Management
    function initializeMobileMenu() {
        const sidebar = document.querySelector('.pcoded-navbar');
        const menuToggle = document.getElementById('mobile-menu-toggle') || document.querySelector('.navbar-toggler');

        // Mobile menu toggle functionality
        if (menuToggle) {
            menuToggle.addEventListener('click', function (e) {
                e.preventDefault();
                sidebar.classList.toggle('show-sidebar');
            });
        }

        // Close sidebar when clicking outside on mobile
        document.addEventListener('click', function (e) {
            if (window.innerWidth <= 991) {
                if (!sidebar.contains(e.target) && !menuToggle.contains(e.target)) {
                    sidebar.classList.remove('show-sidebar');
                }
            }
        });

        // Handle window resize
        window.addEventListener('resize', function () {
            if (window.innerWidth > 991) {
                sidebar.classList.remove('show-sidebar');
            }
        });
    }

    // Utility Functions
    window.DattaAble = {
        // Show notification
        notify: function (message, type = 'info') {
            const notification = document.createElement('div');
            notification.className = `alert alert-${type} alert-dismissible fade show`;
            notification.style.cssText = 'position: fixed; top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
            notification.innerHTML = `
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            `;

            document.body.appendChild(notification);

            setTimeout(() => {
                notification.remove();
            }, 5000);
        },

        // Loading state
        showLoading: function (element) {
            if (typeof element === 'string') {
                element = document.querySelector(element);
            }
            element.innerHTML = '<div class="spinner-border spinner-border-sm" role="status"><span class="visually-hidden">Yükleniyor...</span></div>';
        },

        // Hide loading
        hideLoading: function (element, originalContent) {
            if (typeof element === 'string') {
                element = document.querySelector(element);
            }
            element.innerHTML = originalContent;
        },

        // Confirm dialog
        confirm: function (message, callback) {
            if (window.confirm(message)) {
                callback();
            }
        },

        // Format currency
        formatCurrency: function (amount) {
            return new Intl.NumberFormat('tr-TR', {
                style: 'currency',
                currency: 'TRY'
            }).format(amount);
        }
    };

})();

// Mobile sidebar styles
const mobileStyles = `
@media (max-width: 991px) {
    .pcoded-navbar {
        transform: translateX(-100%);
        transition: transform 0.3s ease;
    }
    
    .pcoded-navbar.show-sidebar {
        transform: translateX(0);
    }
    
    .pcoded-navbar::before {
        content: '';
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        z-index: -1;
        opacity: 0;
        transition: opacity 0.3s ease;
    }
    
    .pcoded-navbar.show-sidebar::before {
        opacity: 1;
    }
}
`;

// Add mobile styles to head
const styleSheet = document.createElement('style');
styleSheet.textContent = mobileStyles;
document.head.appendChild(styleSheet);