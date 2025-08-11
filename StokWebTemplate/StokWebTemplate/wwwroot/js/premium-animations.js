/* Optimized Mobile-First Animations */

document.addEventListener('DOMContentLoaded', function () {
    // Check if reduced motion is preferred
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const isMobile = window.innerWidth <= 768;

    if (!prefersReducedMotion) {
        initOptimizedAnimations();
        if (!isMobile) {
            initDesktopAnimations();
        }
        initScrollAnimations();
    }

    // Always init essential interactions
    initEssentialInteractions();
});

// Essential interactions (always load)
function initEssentialInteractions() {
    // Mobile menu toggle
    const menuToggle = document.querySelector('.navbar-toggler');
    const sidebar = document.querySelector('.pcoded-navbar');

    if (menuToggle && sidebar) {
        menuToggle.addEventListener('click', function (e) {
            e.preventDefault();
            sidebar.classList.toggle('show-sidebar');
        });

        // Close on outside click (mobile only)
        if (window.innerWidth <= 768) {
            document.addEventListener('click', function (e) {
                if (!sidebar.contains(e.target) && !menuToggle.contains(e.target)) {
                    sidebar.classList.remove('show-sidebar');
                }
            });
        }
    }

    // Form enhancements
    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.classList.add('focused');
        });

        input.addEventListener('blur', function () {
            this.parentElement.classList.remove('focused');
        });
    });
}

// Lightweight loading overlay
function showLightLoadingOverlay() {
    const overlay = document.createElement('div');
    overlay.className = 'light-loading-overlay';
    overlay.innerHTML = `
        <div class="simple-spinner"></div>
    `;

    document.body.appendChild(overlay);

    // Remove after 2 seconds
    setTimeout(() => {
        if (overlay.parentNode) {
            overlay.remove();
        }
    }, 2000);
}

// Optimized animations for mobile performance
function initOptimizedAnimations() {
    const isMobile = window.innerWidth <= 768;

    // Lighter card animations for mobile
    const cards = document.querySelectorAll('.card');
    cards.forEach((card, index) => {
        if (!isMobile) {
            card.style.animationDelay = `${index * 0.05}s`; // Reduced delay
            card.classList.add('animate-card-entrance');
        }
    });

    // Counter animations (only on desktop or WiFi)
    if (!isMobile || navigator.connection?.effectiveType !== 'slow-2g') {
        initCounterAnimations();
    }
}

// Desktop-only animations
function initDesktopAnimations() {
    // Button ripple effect (desktop only)
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        button.addEventListener('click', createRipple);
    });

    // Premium loading animations
    const pageLinks = document.querySelectorAll('a[asp-controller], a[href^="/"]');
    pageLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            if (!this.getAttribute('href').startsWith('#')) {
                showLightLoadingOverlay();
            }
        });
    });
}

// Ripple effect for buttons
function createRipple(e) {
    const button = e.currentTarget;
    const rect = button.getBoundingClientRect();
    const size = Math.max(rect.width, rect.height);
    const x = e.clientX - rect.left - size / 2;
    const y = e.clientY - rect.top - size / 2;

    const ripple = document.createElement('span');
    ripple.className = 'ripple-effect';
    ripple.style.cssText = `
        width: ${size}px;
        height: ${size}px;
        left: ${x}px;
        top: ${y}px;
    `;

    button.appendChild(ripple);

    setTimeout(() => {
        ripple.remove();
    }, 600);
}

// Form field focus animations
function animateFieldFocus(e) {
    const field = e.target;
    const parent = field.parentElement;

    // Add glow effect
    field.classList.add('field-focused');

    // Animate label if exists
    const label = parent.querySelector('.form-label');
    if (label) {
        label.classList.add('label-focused');
    }
}

function animateFieldBlur(e) {
    const field = e.target;
    const parent = field.parentElement;

    field.classList.remove('field-focused');

    const label = parent.querySelector('.form-label');
    if (label) {
        label.classList.remove('label-focused');
    }
}

// Optimized scroll animations
function initScrollAnimations() {
    // Only on desktop or good connections
    if (window.innerWidth <= 768 && navigator.connection?.effectiveType === 'slow-2g') {
        return;
    }

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-fade-in-up');
                observer.unobserve(entry.target);
            }
        });
    }, {
        threshold: 0.2, // Higher threshold for better performance
        rootMargin: '0px 0px -30px 0px'
    });

    // Only observe cards on desktop
    if (window.innerWidth > 768) {
        const elementsToAnimate = document.querySelectorAll('.premium-stat-card');
        elementsToAnimate.forEach(el => {
            observer.observe(el);
        });
    }
}

// Optimized counter animations
function initCounterAnimations() {
    const counters = document.querySelectorAll('.premium-stat-number[data-target]');

    // Use requestAnimationFrame for smoother animation
    counters.forEach(counter => {
        const target = parseInt(counter.getAttribute('data-target'));
        if (target && target > 0) {
            animateCounterOptimized(counter, 0, target, 1000); // Reduced duration
        }
    });
}

// Optimized counter animation using requestAnimationFrame
function animateCounterOptimized(element, start, end, duration) {
    const startTime = performance.now();

    function updateCounter(currentTime) {
        const elapsed = currentTime - startTime;
        const progress = Math.min(elapsed / duration, 1);

        // Easing function for smooth animation
        const easeOutQuart = 1 - Math.pow(1 - progress, 4);
        const current = Math.floor(start + (end - start) * easeOutQuart);

        element.textContent = current.toLocaleString('tr-TR');

        if (progress < 1) {
            requestAnimationFrame(updateCounter);
        }
    }

    requestAnimationFrame(updateCounter);
}

// Smooth page transitions
function initPageTransitions() {
    // Add page transition effects
    document.body.classList.add('page-loaded');

    // Handle back/forward buttons
    window.addEventListener('popstate', function () {
        document.body.style.opacity = '0';
        setTimeout(() => {
            location.reload();
        }, 150);
    });
}

// Premium notification system
window.showPremiumNotification = function (message, type = 'info', duration = 4000) {
    const notification = document.createElement('div');
    notification.className = `premium-notification notification-${type}`;

    notification.innerHTML = `
        <div class="notification-content">
            <div class="notification-icon">
                <i data-feather="${getNotificationIcon(type)}"></i>
            </div>
            <div class="notification-message">${message}</div>
            <button class="notification-close">
                <i data-feather="x"></i>
            </button>
        </div>
        <div class="notification-progress"></div>
    `;

    document.body.appendChild(notification);

    // Initialize feather icons
    if (window.feather) {
        feather.replace();
    }

    // Auto remove
    setTimeout(() => {
        notification.classList.add('notification-exit');
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 300);
    }, duration);

    // Close button
    notification.querySelector('.notification-close').addEventListener('click', () => {
        notification.classList.add('notification-exit');
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 300);
    });

    // Start progress animation
    const progress = notification.querySelector('.notification-progress');
    progress.style.animationDuration = `${duration}ms`;
};

function getNotificationIcon(type) {
    const icons = {
        success: 'check-circle',
        error: 'x-circle',
        warning: 'alert-triangle',
        info: 'info'
    };
    return icons[type] || 'info';
}

// Initialize everything
initPageTransitions();