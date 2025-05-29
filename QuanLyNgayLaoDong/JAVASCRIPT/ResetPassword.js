// Hàm togglePassword để hiển thị/ẩn mật khẩu
function togglePassword() {
    const passwordField = document.getElementById("newPasswordField");
    const eyeIcon = document.getElementById("eyeIcon");

    if (passwordField && eyeIcon) {
        if (passwordField.type === "password") {
            passwordField.type = "text";
            eyeIcon.classList.remove("bi-eye-slash");
            eyeIcon.classList.add("bi-eye");
        } else {
            passwordField.type = "password";
            eyeIcon.classList.remove("bi-eye");
            eyeIcon.classList.add("bi-eye-slash");
        }
    }
}

// Thêm sự kiện click cho nút hiển thị/ẩn mật khẩu khi trang tải
document.addEventListener("DOMContentLoaded", function () {
    const showPasswordBtn = document.querySelector(".show-password-btn");
    if (showPasswordBtn) {
        showPasswordBtn.addEventListener("click", togglePassword);
    }
});