// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Toggle Password Visibility
document.getElementById('togglePassword').addEventListener('click', function () {
    const passwordField = document.getElementById('loginPassword');
    const passwordFieldType = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordField.setAttribute('type', passwordFieldType);

    // Toggle icon class
    this.querySelector('i').classList.toggle('bi-eye');
    this.querySelector('i').classList.toggle('bi-eye-slash');
});


document.addEventListener("DOMContentLoaded", function () {
    const togglePasswordButton = document.getElementById('toggleAdminPassword');
    const passwordField = document.getElementById('adminPassword');

    // Add click event listener to the toggle button
    togglePasswordButton.addEventListener('click', function () {
        const passwordFieldType = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordField.setAttribute('type', passwordFieldType);

        // Toggle the eye icon
        this.querySelector('i').classList.toggle('bi-eye');
        this.querySelector('i').classList.toggle('bi-eye-slash');
    });
});


// The Registration script
document.addEventListener("DOMContentLoaded", function () {

    // Add event listener for password toggle buttons in the registration form
    document.querySelectorAll('.toggle-password').forEach(button => {
        button.addEventListener('click', function () {
            const targetId = this.getAttribute('data-target');
            const passwordField = document.getElementById(targetId);

            // Toggle password visibility
            const passwordFieldType = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordField.setAttribute('type', passwordFieldType);

            // Toggle the icon class
            const icon = this.querySelector('i');
            icon.classList.toggle('bi-eye');
            icon.classList.toggle('bi-eye-slash');
        });
    });

    const steps = document.querySelectorAll('.step');
    const nextButtons = document.querySelectorAll('.next-step');
    let currentStep = 0;

    // Show the current step and hide others
    function showStep(stepIndex) {
        steps.forEach((step, index) => {
            step.style.display = index === stepIndex ? 'block' : 'none';
        });
    }

    // Initialize the first step
    showStep(currentStep);

    // Next button event listeners
    nextButtons.forEach((button) => {
        button.addEventListener('click', () => {
            currentStep++;
            showStep(currentStep);
        });
    });
});

