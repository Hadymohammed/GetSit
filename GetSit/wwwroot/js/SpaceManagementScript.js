document.addEventListener('DOMContentLoaded', function () {
    const navLinks = document.querySelectorAll('nav a');
    const sections = document.querySelectorAll('section');

    // Add click event listeners to navigation links
    navLinks.forEach(function (link) {
        link.addEventListener('click', function (event) {
            event.preventDefault();

            // Remove "active" class from all sections and navigation links
            sections.forEach(function (section) {
                section.classList.remove('active');
            });
            navLinks.forEach(function (link) {
                link.classList.remove('active');
            });

            // Add "active" class to the selected section and navigation link
            const targetId = this.getAttribute('href');
            const targetSection = document.querySelector(targetId);
            targetSection.classList.add('active');
            this.classList.add('active');
        });
    });
});
