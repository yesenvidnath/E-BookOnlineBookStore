document.getElementById('loginForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Prevent default form submission

    const formData = new FormData(this);
    const response = await fetch(this.action, {
        method: 'POST',
        body: formData,
    });

    const result = await response.json();

    // Handle response (success or error)
    const loginMessage = document.getElementById('loginMessage');
    if (response.ok) {
        loginMessage.innerHTML = `<div class="alert alert-success">${result.message}</div>`;
    } else {
        loginMessage.innerHTML = `<div class="alert alert-danger">${result.message}</div>`;
    }
});