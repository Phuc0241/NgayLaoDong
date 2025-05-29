function togglePassword() {
    const passwordField = document.getElementById("passwordField");
    const eyeIcon = document.getElementById("eyeIcon");
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

// --- Hàm mới để cuộn đến lỗi đầu tiên ---
function scrollToFirstValidationError() {
    // Tìm phần tử hiển thị lỗi tổng quát
    const generalErrorMessage = document.getElementById('generalErrorMessage');

    // Tìm tất cả các phần tử có class 'validation-message' (lỗi cụ thể cho từng trường)
    const fieldErrorMessages = document.querySelectorAll('.validation-message');

    // Mảng để lưu trữ tất cả các lỗi đang hiển thị
    const allErrors = [];

    // Thêm lỗi tổng quát nếu nó đang hiển thị và có nội dung
    if (generalErrorMessage && !generalErrorMessage.classList.contains('d-none')) {
        if (generalErrorMessage.textContent.trim() !== "") {
            allErrors.push(generalErrorMessage);
        }
    }

    // Thêm các lỗi cụ thể của từng trường nếu chúng đang hiển thị và có nội dung
    fieldErrorMessages.forEach(errorElement => {
        if (errorElement.textContent.trim() !== "") {
            allErrors.push(errorElement);
        }
    });

    // Nếu có ít nhất một lỗi được tìm thấy
    if (allErrors.length > 0) {
        // Cuộn đến lỗi đầu tiên
        allErrors[0].scrollIntoView({ behavior: "smooth", block: "center" });

        // Tùy chọn: Thêm hiệu ứng nhẹ để người dùng chú ý đến lỗi đầu tiên
        allErrors[0].style.transition = 'background-color 0.5s ease-in-out';
        allErrors[0].style.backgroundColor = '#fff3cd'; // Màu vàng nhạt
        setTimeout(() => {
            allErrors[0].style.backgroundColor = ''; // Trở về màu cũ
        }, 3000); // Sau 3 giây
    }
}
// --- Hết hàm mới ---


// Bắt sự kiện khi DOM đã tải xong
document.addEventListener('DOMContentLoaded', function () {
    // Gọi hàm để cuộn đến lỗi ngay khi trang được tải (sau khi submit form bị lỗi)
    scrollToFirstValidationError();
});