document.addEventListener("DOMContentLoaded", function () {
    const emailField = document.getElementById("email");
    const emailHelp = document.getElementById("emailHelp");

    emailField.addEventListener("input", function () {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!emailRegex.test(emailField.value)) {
            emailField.classList.add("is-invalid");
            emailHelp.classList.remove("d-none");
        } else {
            emailField.classList.remove("is-invalid");
            emailHelp.classList.add("d-none");
        }
    });
});
