document.addEventListener("DOMContentLoaded", function () {
    // Tự động ẩn thông báo sau 3 giây
    setTimeout(function () {
        document.querySelectorAll(".alert").forEach(function (alert) {
            alert.style.transition = "opacity 0.5s ease";
            alert.style.opacity = "0";
            setTimeout(() => alert.remove(), 500);
        });
    }, 3000); // Thông báo biến mất sau 3 giây

    const rowsPerPage = 5;
    const table = document.getElementById("accountTable");
    const tbody = table.querySelector("tbody");
    const allRows = Array.from(tbody.querySelectorAll("tr"));
    const pagination = document.getElementById("pagination");

    const searchInput = document.getElementById("searchInput");
    const roleFilter = document.getElementById("roleFilter");

    let currentPage = 1;

    function normalize(str) {
        return str
            .normalize("NFD")
            .replace(/[\u0300-\u036f]/g, "")
            .toLowerCase()
            .trim();
    }

    function getFilteredRows() {
        const keyword = normalize(searchInput.value);
        const selectedRole = normalize(roleFilter.value);

        return allRows.filter(row => {
            const username = normalize(row.children[1].textContent);
            const role = normalize(row.children[4].textContent);

            const matchesKeyword = username.includes(keyword);
            const matchesRole = !selectedRole || role.includes(selectedRole);

            return matchesKeyword && matchesRole;
        });
    }

    function updateSTT(filteredRows) {
        filteredRows.forEach((row, index) => {
            row.children[0].textContent = index + 1;
        });
    }

    function showPage(page, filteredRows) {
        currentPage = page;
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        allRows.forEach(row => row.style.display = "none");
        const visibleRows = filteredRows.slice(start, end);
        visibleRows.forEach(row => row.style.display = "");

        updateSTT(filteredRows);
        renderPagination(filteredRows.length);
    }

    function renderPagination(totalItems) {
        const totalPages = Math.ceil(totalItems / rowsPerPage);
        pagination.innerHTML = "";

        // ←
        const prev = document.createElement("button");
        prev.textContent = "←";
        prev.className = "arrow";
        prev.disabled = currentPage === 1;
        prev.onclick = () => {
            if (currentPage > 1) {
                currentPage--;
                showPage(currentPage, getFilteredRows());
            }
        };
        pagination.appendChild(prev);

        // Page numbers
        for (let i = 1; i <= totalPages; i++) {
            const btn = document.createElement("button");
            btn.textContent = i;
            if (i === currentPage) btn.classList.add("active");
            btn.onclick = () => showPage(i, getFilteredRows());
            pagination.appendChild(btn);
        }

        // →
        const next = document.createElement("button");
        next.textContent = "→";
        next.className = "arrow";
        next.disabled = currentPage === totalPages;
        next.onclick = () => {
            if (currentPage < totalPages) {
                currentPage++;
                showPage(currentPage, getFilteredRows());
            }
        };
        pagination.appendChild(next);
    }

    // Toggle section visibility
    window.toggleSection = function (sectionId) {
        const sections = [
            'add-form-container',
            'edit-form-container',
            'delete-form-container',
            'detail-container',
            'reset-form-container'
        ];
        sections.forEach(id => {
            const section = document.getElementById(id);
            if (section) {
                section.style.display = id === sectionId && section.style.display === 'none' ? 'block' : 'none';
            }
        });

        const backdrop = document.getElementById("backdrop");
        if (backdrop) {
            const isVisible = sections.some(id => {
                const sec = document.getElementById(id);
                return sec && sec.style.display === "block";
            });
            backdrop.style.display = isVisible ? "block" : "none";
        }
    };

    // Toggle edit form and pre-fill data
    window.toggleEditForm = function (id, username, email, role) {
        document.getElementById('edit-taikhoan-id').value = id;
        document.getElementById('edit-username').value = username;
        document.getElementById('edit-email').value = email;
        const roleSelect = document.getElementById('edit-role-id');
        const roleMap = {
            'admin': '1',
            'quanly': '2',
            'sinhvien': '3',
            'loptruong': '4'
        };
        roleSelect.value = roleMap[normalize(role)] || '';
        toggleSection('edit-form-container');
    };

    // Toggle delete form and set ID
    window.toggleDeleteForm = function (id) {
        document.getElementById('delete-taikhoan-id').value = id;
        toggleSection('delete-form-container');
    };

    // Toggle reset password form
    window.toggleResetForm = function (id) {
        const form = document.getElementById("reset-form-container");
        const backdrop = document.getElementById("backdrop");
        const input = document.getElementById("reset-taikhoan-id");

        if (form && backdrop && input) {
            input.value = id;
            form.style.display = "block";
            backdrop.style.display = "block";
        }
    };

    // Export to Excel
    window.exportToExcel = async function () {
        try {
            // Lấy tiêu đề cột từ thead
            const headers = Array.from(document.querySelectorAll("#accountTable thead th"))
                .map(th => th.textContent.trim())
                .filter((_, index) => index !== 5); // Loại bỏ cột "Tác Vụ"

            // Lấy dữ liệu từ tất cả các hàng (bao gồm cả hàng ẩn)
            const data = allRows.map(row => {
                return Array.from(row.children)
                    .filter((_, index) => index !== 5) // Loại bỏ cột "Tác Vụ"
                    .map(cell => cell.textContent.trim());
            });

            // Tạo dữ liệu cho sheet (gộp tiêu đề và dữ liệu)
            const sheetData = [headers, ...data];

            // Tạo workbook và worksheet
            const ws = XLSX.utils.aoa_to_sheet(sheetData);
            const wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "DanhSachTaiKhoan");

            // Xuất file Excel
            const wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
            const blob = new Blob([wbout], { type: 'application/octet-stream' });

            // Kiểm tra xem trình duyệt có hỗ trợ showSaveFilePicker không
            if (window.showSaveFilePicker) {
                const handle = await window.showSaveFilePicker({
                    suggestedName: 'DanhSachTaiKhoan.xlsx',
                    types: [
                        {
                            description: 'Excel Files',
                            accept: { 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet': ['.xlsx'] }
                        }
                    ]
                });
                const writable = await handle.createWritable();
                await writable.write(blob);
                await writable.close();
            } else {
                // Phương án dự phòng: sử dụng FileSaver.js
                saveAs(blob, 'DanhSachTaiKhoan.xlsx');
            }

            // Thêm thông báo "Đã tải file Excel thành công"
            const notificationContainer = document.createElement('div');
            notificationContainer.className = 'alert alert-success';
            notificationContainer.style.cssText = 'text-align: center; margin-bottom: 10px;';
            notificationContainer.textContent = 'Đã tải file Excel thành công!';
            document.querySelector('h2').insertAdjacentElement('afterend', notificationContainer);

            // Đảm bảo thông báo mới cũng tự động ẩn sau 3 giây
            setTimeout(() => {
                notificationContainer.style.transition = 'opacity 0.5s ease';
                notificationContainer.style.opacity = '0';
                setTimeout(() => notificationContainer.remove(), 500);
            }, 3000);
        } catch (error) {
            console.error('Lỗi khi xuất file Excel:', error);
            // Thêm thông báo lỗi
            const errorContainer = document.createElement('div');
            errorContainer.className = 'alert alert-danger';
            errorContainer.style.cssText = 'text-align: center; margin-bottom: 10px;';
            errorContainer.textContent = 'Đã ngắt xuất file';
            document.querySelector('h2').insertAdjacentElement('afterend', errorContainer);

            // Đảm bảo thông báo lỗi cũng tự động ẩn sau 3 giây
            setTimeout(() => {
                errorContainer.style.transition = 'opacity 0.5s ease';
                errorContainer.style.opacity = '0';
                setTimeout(() => errorContainer.remove(), 500);
            }, 3000);
        }
    };

    // Trigger filter
    searchInput.addEventListener("input", () => showPage(1, getFilteredRows()));
    roleFilter.addEventListener("change", () => showPage(1, getFilteredRows()));

    // Init
    showPage(1, getFilteredRows());

    // Show details if present
    const detailContainer = document.getElementById('detail-container');
    if (detailContainer) {
        detailContainer.style.display = 'block';
        const backdrop = document.getElementById('backdrop');
        if (backdrop) backdrop.style.display = 'block';
    }
});