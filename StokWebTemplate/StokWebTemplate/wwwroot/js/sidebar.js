// Modern Sidebar JavaScript
class ModernSidebar {
    constructor() {
        this.sidebar = document.getElementById('sidebar');
        this.overlay = null;
        this.isOpen = false;
        this.init();
    }
    
    init() {
        this.createOverlay();
        this.bindEvents();
        this.setActiveMenuItem();
        this.addSmoothAnimations();
    }
    
    createOverlay() {
        this.overlay = document.createElement('div');
        this.overlay.className = 'sidebar-overlay';
        this.overlay.addEventListener('click', () => this.toggle());
        document.body.appendChild(this.overlay);
    }
    
    bindEvents() {
        // Mobile toggle button
        const mobileToggle = document.querySelector('.mobile-toggle');
        if (mobileToggle) {
            mobileToggle.addEventListener('click', () => this.toggle());
        }
        
        // ESC key to close sidebar
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isOpen) {
                this.close();
            }
        });
        
        // Resize handler
        window.addEventListener('resize', () => {
            if (window.innerWidth > 768 && this.isOpen) {
                this.close();
            }
        });
        
        // Sidebar menu items hover effects
        const menuItems = this.sidebar.querySelectorAll('a');
        menuItems.forEach(item => {
            item.addEventListener('mouseenter', (e) => {
                this.addHoverEffect(e.target);
            });
            
            item.addEventListener('mouseleave', (e) => {
                this.removeHoverEffect(e.target);
            });
        });
    }
    
    setActiveMenuItem() {
        const currentPath = window.location.pathname;
        const menuItems = this.sidebar.querySelectorAll('a');
        
        menuItems.forEach(item => {
            if (item.getAttribute('href') === currentPath) {
                item.classList.add('active');
            }
        });
    }
    
    addSmoothAnimations() {
        // Add staggered animation to menu items
        const menuItems = this.sidebar.querySelectorAll('li');
        menuItems.forEach((item, index) => {
            item.style.animationDelay = `${index * 0.1}s`;
        });
    }
    
    toggle() {
        if (this.isOpen) {
            this.close();
        } else {
            this.open();
        }
    }
    
    open() {
        this.sidebar.classList.add('show');
        this.overlay.classList.add('show');
        this.isOpen = true;
        
        // Add body scroll lock
        document.body.style.overflow = 'hidden';
        
        // Animate menu items
        this.animateMenuItems();
    }
    
    close() {
        this.sidebar.classList.remove('show');
        this.overlay.classList.remove('show');
        this.isOpen = false;
        
        // Remove body scroll lock
        document.body.style.overflow = '';
    }
    
    animateMenuItems() {
        const menuItems = this.sidebar.querySelectorAll('li');
        menuItems.forEach((item, index) => {
            setTimeout(() => {
                item.style.opacity = '1';
                item.style.transform = 'translateX(0)';
            }, index * 100);
        });
    }
    
    addHoverEffect(element) {
        // Add ripple effect
        const ripple = document.createElement('span');
        ripple.className = 'ripple';
        ripple.style.cssText = `
            position: absolute;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.3);
            transform: scale(0);
            animation: ripple 0.6s linear;
            pointer-events: none;
        `;
        
        const rect = element.getBoundingClientRect();
        const size = Math.max(rect.width, rect.height);
        const x = event.clientX - rect.left - size / 2;
        const y = event.clientY - rect.top - size / 2;
        
        ripple.style.width = ripple.style.height = size + 'px';
        ripple.style.left = x + 'px';
        ripple.style.top = y + 'px';
        
        element.appendChild(ripple);
        
        setTimeout(() => {
            ripple.remove();
        }, 600);
    }
    
    removeHoverEffect(element) {
        const ripples = element.querySelectorAll('.ripple');
        ripples.forEach(ripple => ripple.remove());
    }
}

// Theme management
class ThemeManager {
    constructor() {
        this.currentTheme = localStorage.getItem('theme') || 'light';
        this.init();
    }
    
    init() {
        this.applyTheme();
        this.bindEvents();
    }
    
    bindEvents() {
        const themeToggle = document.querySelector('.theme-toggle');
        if (themeToggle) {
            themeToggle.addEventListener('click', () => this.toggleTheme());
        }
    }
    
    applyTheme() {
        document.body.classList.toggle('dark-theme', this.currentTheme === 'dark');
        
        // Update theme toggle button
        const themeToggle = document.querySelector('.theme-toggle');
        if (themeToggle) {
            const icon = themeToggle.querySelector('i');
            const text = themeToggle.querySelector('span') || document.createTextNode(' Tema');
            
            if (this.currentTheme === 'dark') {
                icon.className = 'fas fa-sun';
                themeToggle.innerHTML = '<i class="fas fa-sun"></i> Açık Tema';
            } else {
                icon.className = 'fas fa-moon';
                themeToggle.innerHTML = '<i class="fas fa-moon"></i> Koyu Tema';
            }
        }
    }
    
    toggleTheme() {
        this.currentTheme = this.currentTheme === 'light' ? 'dark' : 'light';
        localStorage.setItem('theme', this.currentTheme);
        this.applyTheme();
        
        // Send theme change to server
        this.saveThemeToServer();
    }
    
    async saveThemeToServer() {
        try {
            await fetch('/Theme/Toggle', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ theme: this.currentTheme })
            });
        } catch (error) {
            console.error('Theme save error:', error);
        }
    }
}

// User info enhancements
class UserInfoManager {
    constructor() {
        this.init();
    }
    
    init() {
        this.addUserAvatar();
        this.addDropdownMenu();
    }
    
    addUserAvatar() {
        const userInfo = document.querySelector('.user-info');
        if (userInfo) {
            const username = userInfo.querySelector('span')?.textContent || 'Kullanıcı';
            const initials = this.getInitials(username);
            
            const avatar = document.createElement('div');
            avatar.className = 'user-avatar';
            avatar.style.cssText = `
                width: 40px;
                height: 40px;
                border-radius: 50%;
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                display: flex;
                align-items: center;
                justify-content: center;
                color: white;
                font-weight: bold;
                font-size: 1.1rem;
                margin-right: 0.75rem;
            `;
            avatar.textContent = initials;
            
            userInfo.insertBefore(avatar, userInfo.firstChild);
        }
    }
    
    getInitials(name) {
        return name
            .split(' ')
            .map(word => word.charAt(0))
            .join('')
            .toUpperCase()
            .slice(0, 2);
    }
    
    addDropdownMenu() {
        const userInfo = document.querySelector('.user-info');
        if (userInfo) {
            userInfo.style.cursor = 'pointer';
            userInfo.addEventListener('click', () => this.toggleDropdown());
            
            const dropdown = document.createElement('div');
            dropdown.className = 'user-dropdown';
            dropdown.style.cssText = `
                position: absolute;
                top: 100%;
                right: 0;
                background: white;
                border-radius: 12px;
                box-shadow: 0 10px 30px rgba(0,0,0,0.2);
                padding: 1rem;
                min-width: 200px;
                opacity: 0;
                visibility: hidden;
                transform: translateY(-10px);
                transition: all 0.3s ease;
                z-index: 1000;
            `;
            
            dropdown.innerHTML = `
                <div class="dropdown-header">
                    <div class="user-avatar" style="width: 50px; height: 50px; margin-bottom: 0.5rem;"></div>
                    <div class="user-details">
                        <strong>${userInfo.querySelector('span')?.textContent || 'Kullanıcı'}</strong>
                        <small class="text-muted">Aktif</small>
                    </div>
                </div>
                <hr>
                <div class="dropdown-menu">
                    <a href="#" class="dropdown-item">
                        <i class="fas fa-user me-2"></i>Profil
                    </a>
                    <a href="#" class="dropdown-item">
                        <i class="fas fa-cog me-2"></i>Ayarlar
                    </a>
                    <a href="/Giris" class="dropdown-item text-danger">
                        <i class="fas fa-sign-out-alt me-2"></i>Çıkış
                    </a>
                </div>
            `;
            
            userInfo.style.position = 'relative';
            userInfo.appendChild(dropdown);
        }
    }
    
    toggleDropdown() {
        const dropdown = document.querySelector('.user-dropdown');
        if (dropdown) {
            const isVisible = dropdown.style.visibility === 'visible';
            
            if (isVisible) {
                dropdown.style.opacity = '0';
                dropdown.style.visibility = 'hidden';
                dropdown.style.transform = 'translateY(-10px)';
            } else {
                dropdown.style.opacity = '1';
                dropdown.style.visibility = 'visible';
                dropdown.style.transform = 'translateY(0)';
            }
        }
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    // Initialize sidebar
    if (document.getElementById('sidebar')) {
        new ModernSidebar();
    }
    
    // Initialize theme manager
    new ThemeManager();
    
    // Initialize user info manager
    new UserInfoManager();
    
    // Add ripple effect CSS
    const style = document.createElement('style');
    style.textContent = `
        @keyframes ripple {
            to {
                transform: scale(4);
                opacity: 0;
            }
        }
        
        .dropdown-item {
            display: block;
            padding: 0.5rem 1rem;
            color: #333;
            text-decoration: none;
            border-radius: 8px;
            transition: all 0.3s ease;
        }
        
        .dropdown-item:hover {
            background: #f8f9fa;
            transform: translateX(5px);
        }
        
        .dropdown-header {
            text-align: center;
            padding: 1rem;
        }
        
        .user-details {
            text-align: center;
        }
        
        .user-details strong {
            display: block;
            margin-bottom: 0.25rem;
        }
    `;
    document.head.appendChild(style);
}); 