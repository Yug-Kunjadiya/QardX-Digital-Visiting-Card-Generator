// Site-wide JavaScript for QardX

$(document).ready(function() {
    // Add fade-in animation to cards
    $('.card').addClass('fade-in');
    
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    
    // Auto-hide alerts after 5 seconds
    $('.alert').delay(5000).fadeOut('slow');
    
    // Smooth scrolling for anchor links
    $('a[href*="#"]').not('[href="#"]').not('[href="#0"]').click(function(event) {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                event.preventDefault();
                $('html, body').animate({
                    scrollTop: target.offset().top - 80
                }, 800);
            }
        }
    });
    
    // Enhanced feature card interactions
    $('.feature-card').hover(
        function() {
            $(this).find('.feature-icon').addClass('animate__pulse');
        },
        function() {
            $(this).find('.feature-icon').removeClass('animate__pulse');
        }
    );
    
    // Template preview interactions
    $('.template-preview').click(function() {
        $(this).addClass('selected');
        $('.template-preview').not(this).removeClass('selected');
    });
    
    // Form enhancements
    $('.form-control').on('focus', function() {
        $(this).parent().addClass('focused');
    }).on('blur', function() {
        if (!$(this).val()) {
            $(this).parent().removeClass('focused');
        }
    });
    
    // Loading states for buttons
    $('.btn-cta').click(function() {
        var btn = $(this);
        var originalText = btn.html();
        btn.html('<i class="fas fa-spinner fa-spin me-2"></i>Creating...');
        btn.prop('disabled', true);
        
        setTimeout(function() {
            btn.html(originalText);
            btn.prop('disabled', false);
        }, 2000);
    });
    
    // Intersection Observer for animations
    if ('IntersectionObserver' in window) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('animate-in');
                }
            });
        }, { threshold: 0.1 });
        
        document.querySelectorAll('.feature-card, .template-preview, .progress-step').forEach(el => {
            observer.observe(el);
        });
    }
    
    // Counter animation for stats
    function animateCounter(element, target) {
        let current = 0;
        const increment = target / 100;
        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                current = target;
                clearInterval(timer);
            }
            element.textContent = Math.floor(current) + '+';
        }, 20);
    }
    
    // Trigger counter animation when hero stats come into view
    const statsObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const counters = entry.target.querySelectorAll('h4');
                counters.forEach(counter => {
                    const target = parseInt(counter.textContent);
                    animateCounter(counter, target);
                });
                statsObserver.unobserve(entry.target);
            }
        });
    });
    
    const heroStats = document.querySelector('.hero-stats');
    if (heroStats) {
        statsObserver.observe(heroStats);
    }
});

// Utility functions
function showAlert(message, type = 'info') {
    const alertDiv = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;
    $('.container main').prepend(alertDiv);
}

function formatPhoneNumber(phone) {
    // Simple phone number formatting
    const cleaned = phone.replace(/\D/g, '');
    const match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/);
    if (match) {
        return '(' + match[1] + ') ' + match[2] + '-' + match[3];
    }
    return phone;
}