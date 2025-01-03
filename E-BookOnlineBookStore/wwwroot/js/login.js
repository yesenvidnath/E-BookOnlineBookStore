document.getElementById('loginForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    const formData = new FormData(this);
    const response = await fetch(this.action, {
        method: 'POST',
        body: formData,
    });

    if (response.ok) {
        // Reload the page to reflect changes
        location.reload();
    } else {
        const result = await response.json();
        const loginMessage = document.getElementById('loginMessage');
        loginMessage.innerHTML = `<div class="alert alert-danger">${result.message}</div>`;
    }
});
