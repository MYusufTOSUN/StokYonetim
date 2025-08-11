// Modern Pagination JavaScript
class ModernPagination {
    constructor(container, options = {}) {
        this.container = container;
        this.options = {
            currentPage: 1,
            totalPages: 1,
            maxVisiblePages: 7,
            onPageChange: null,
            ...options
        };
        
        this.init();
    }
    
    init() {
        this.render();
        this.bindEvents();
    }
    
    render() {
        const { currentPage, totalPages, maxVisiblePages } = this.options;
        
        if (totalPages <= 1) {
            this.container.innerHTML = '';
            return;
        }
        
        const pages = this.generatePageNumbers(currentPage, totalPages, maxVisiblePages);
        
        this.container.innerHTML = `
            <div class="pagination-container">
                <nav>
                    <ul class="pagination">
                        ${this.renderPageItem(currentPage - 1, '‹', currentPage > 1, 'prev')}
                        ${pages.map(page => this.renderPageItem(page, page, true, page === currentPage ? 'active' : '')).join('')}
                        ${this.renderPageItem(currentPage + 1, '›', currentPage < totalPages, 'next')}
                    </ul>
                </nav>
                <div class="pagination-info">
                    <small class="text-muted">📄 Sayfa ${currentPage} / ${totalPages}</small>
                </div>
            </div>
        `;
    }
    
    generatePageNumbers(current, total, maxVisible) {
        const pages = [];
        
        if (total <= maxVisible) {
            // Tüm sayfaları göster
            for (let i = 1; i <= total; i++) {
                pages.push(i);
            }
        } else {
            // Akıllı sayfa numaralandırma
            const halfVisible = Math.floor(maxVisible / 2);
            
            // İlk sayfalar
            for (let i = 1; i <= Math.min(halfVisible, current - 1); i++) {
                pages.push(i);
            }
            
            // Orta sayfalar
            const start = Math.max(1, current - halfVisible);
            const end = Math.min(total, current + halfVisible);
            
            for (let i = start; i <= end; i++) {
                if (!pages.includes(i)) {
                    pages.push(i);
                }
            }
            
            // Son sayfalar
            for (let i = Math.max(end + 1, total - halfVisible + 1); i <= total; i++) {
                if (!pages.includes(i)) {
                    pages.push(i);
                }
            }
        }
        
        return pages;
    }
    
    renderPageItem(page, text, enabled, className = '') {
        const disabled = !enabled ? 'disabled' : '';
        const active = className === 'active' ? 'active' : '';
        
        return `
            <li class="page-item ${disabled} ${active}">
                <a class="page-link ${className}" href="#" data-page="${page}" ${disabled ? 'tabindex="-1"' : ''}>
                    ${text}
                </a>
            </li>
        `;
    }
    
    bindEvents() {
        this.container.addEventListener('click', (e) => {
            if (e.target.classList.contains('page-link')) {
                e.preventDefault();
                
                const page = parseInt(e.target.dataset.page);
                if (page && page >= 1 && page <= this.options.totalPages) {
                    this.goToPage(page);
                }
            }
        });
    }
    
    goToPage(page) {
        if (this.options.onPageChange) {
            this.options.onPageChange(page);
        } else {
            // URL parametrelerini güncelle
            const url = new URL(window.location);
            url.searchParams.set('sayfa', page);
            window.location.href = url.toString();
        }
    }
    
    update(options) {
        this.options = { ...this.options, ...options };
        this.render();
    }
}

// Global pagination helper functions
window.PaginationHelper = {
    // Sayfa numaralarını akıllı şekilde göster
    createSmartPagination: function(container, currentPage, totalPages, onPageChange) {
        return new ModernPagination(container, {
            currentPage: currentPage,
            totalPages: totalPages,
            onPageChange: onPageChange
        });
    },
    
    // URL'den sayfa parametresini al
    getCurrentPage: function() {
        const urlParams = new URLSearchParams(window.location.search);
        return parseInt(urlParams.get('sayfa')) || 1;
    },
    
    // Sayfa değişikliği için URL güncelle
    updatePageInUrl: function(page) {
        const url = new URL(window.location);
        url.searchParams.set('sayfa', page);
        window.history.pushState({}, '', url);
    },
    
    // Pagination container'ı oluştur
    createContainer: function() {
        const container = document.createElement('div');
        container.className = 'modern-pagination';
        return container;
    }
};

// Sayfa yüklendiğinde otomatik pagination başlat
document.addEventListener('DOMContentLoaded', function() {
    const paginationContainers = document.querySelectorAll('.pagination-container');
    
    paginationContainers.forEach(container => {
        const currentPage = PaginationHelper.getCurrentPage();
        const totalPages = parseInt(container.dataset.totalPages) || 1;
        
        if (totalPages > 1) {
            PaginationHelper.createSmartPagination(container, currentPage, totalPages, (page) => {
                PaginationHelper.updatePageInUrl(page);
                window.location.reload();
            });
        }
    });
}); 