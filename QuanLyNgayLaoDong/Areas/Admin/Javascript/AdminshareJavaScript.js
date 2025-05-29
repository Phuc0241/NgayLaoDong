// Hàm mở modal
function openModal() {
    document.getElementById('confirmationModal').style.display = 'flex';
}

// Hàm đóng modal
function closeModal() {
    document.getElementById('confirmationModal').style.display = 'none';
}


// Đóng modal khi nhấn bên ngoài
window.onclick = function (event) {
    const modal = document.getElementById('confirmationModal');
    if (event.target == modal) {
        modal.style.display = 'none';
    }
}
