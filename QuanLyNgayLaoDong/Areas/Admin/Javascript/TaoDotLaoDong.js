document.addEventListener('DOMContentLoaded', () => {
    const ROWS_PER_PAGE = 5;
    const MAX_DESC_LENGTH = 20;
    let currentPage = 1;
    let allRows = [];

    const table = document.getElementById('dotLaoDongTable');
    const tbody = table?.querySelector('tbody');
    const rows = tbody ? Array.from(tbody.getElementsByTagName('tr')) : [];
    const paginationContainer = document.getElementById('pagination');
    const filterDotLaoDong = document.getElementById('filterDotLaoDong');
    const filterKhuVuc = document.getElementById('filterKhuVuc');
    const filterBuoi = document.getElementById('filterBuoi');
    const filterLoaiLaoDong = document.getElementById('filterLoaiLaoDong');
    const filterThoiGian = document.getElementById('filterThoiGian');
    const searchInput = document.getElementById('searchInput');

    // Lưu trữ tất cả hàng để lọc và tìm kiếm
    allRows = rows.map(row => ({
        element: row,
        dotLaoDong: row.querySelector('[data-dot]')?.getAttribute('data-dot') || '',
        ngayLaoDong: row.querySelector('[data-ngay]')?.getAttribute('data-ngay') || '',
        khuVuc: row.querySelector('[data-khu-vuc]')?.getAttribute('data-khu-vuc') || '',
        buoi: row.querySelector('[data-buoi]')?.getAttribute('data-buoi') || '',
        loaiLaoDong: row.querySelector('[data-loai]')?.getAttribute('data-loai') || '',
        giaTri: row.querySelector('[data-gia-tri]')?.getAttribute('data-gia-tri') || '',
        moTa: row.querySelector('[data-mo-ta]')?.getAttribute('data-mo-ta') || '',
        thoiGian: row.querySelector('[data-thoi-gian]')?.getAttribute('data-thoi-gian') || ''
    }));

    const truncateText = (text) => {
        return text && text.length > MAX_DESC_LENGTH ? text.substring(0, MAX_DESC_LENGTH - 3) + '...' : text || '';
    };

    const showAlert = (message, type = 'success') => {
        const alertBox = document.createElement('div');
        alertBox.className = `alert alert-${type} position-fixed top-0 start-50 translate-middle-x mt-3 shadow`;
        alertBox.style.zIndex = 9999;
        alertBox.style.minWidth = '300px';
        alertBox.innerHTML = `<strong>${type === 'success' ? '✔️ Thành công!' : '❌ Lỗi!'}</strong> ${message}`;
        document.body.appendChild(alertBox);
        setTimeout(() => alertBox.remove(), 3000);
    };

    const applyFiltersAndSearch = () => {
        const dotFilter = filterDotLaoDong?.value || '';
        const khuVucFilter = filterKhuVuc?.value || '';
        const buoiFilter = filterBuoi?.value || '';
        const loaiFilter = filterLoaiLaoDong?.value || '';
        const thoiGianFilter = filterThoiGian?.value || '';
        const searchText = searchInput?.value.toLowerCase() || '';

        const filteredRows = allRows.filter(row => {
            const matchesDot = dotFilter ? row.dotLaoDong === dotFilter : true;
            const matchesKhuVuc = khuVucFilter ? row.khuVuc === khuVucFilter : true;
            const matchesBuoi = buoiFilter ? row.buoi === buoiFilter : true;
            const matchesLoai = loaiFilter ? row.loaiLaoDong === loaiFilter : true;
            const matchesThoiGian = thoiGianFilter ? row.thoiGian === thoiGianFilter : true;

            const matchesSearch = searchText ? (
                row.dotLaoDong.toLowerCase().includes(searchText) ||
                row.ngayLaoDong.toLowerCase().includes(searchText) ||
                row.khuVuc.toLowerCase().includes(searchText) ||
                row.buoi.toLowerCase().includes(searchText) ||
                row.loaiLaoDong.toLowerCase().includes(searchText) ||
                row.giaTri.toString().toLowerCase().includes(searchText) ||
                (row.moTa || '').toLowerCase().includes(searchText) ||
                row.thoiGian.toLowerCase().includes(searchText)
            ) : true;

            return matchesDot && matchesKhuVuc && matchesBuoi && matchesLoai && matchesThoiGian && matchesSearch;
        });

        return filteredRows.map(row => row.element);
    };

    const displayRows = (page) => {
        const filteredRows = applyFiltersAndSearch();
        const start = (page - 1) * ROWS_PER_PAGE;
        const end = start + ROWS_PER_PAGE;

        rows.forEach(row => row.style.display = 'none');
        filteredRows.slice(start, end).forEach(row => {
            const descCell = row.cells[7];
            if (descCell) {
                if (!descCell.dataset.original) {
                    descCell.dataset.original = descCell.textContent;
                }
                descCell.textContent = truncateText(descCell.dataset.original);
            }
            row.style.display = '';
        });

        updatePagination(filteredRows);
    };

    const updatePagination = (filteredRows) => {
        if (!paginationContainer) return;
        const totalPages = Math.ceil(filteredRows.length / ROWS_PER_PAGE);
        paginationContainer.innerHTML = '';

        const createPageItem = (label, disabled, onClick) => {
            const li = document.createElement('li');
            li.className = `page-item ${disabled ? 'disabled' : ''}`;
            li.innerHTML = `<a class="page-link" href="#">${label}</a>`;
            li.addEventListener('click', (e) => {
                e.preventDefault();
                if (!disabled) onClick();
            });
            return li;
        };

        paginationContainer.appendChild(createPageItem('«', currentPage === 1, () => {
            currentPage--;
            displayRows(currentPage);
        }));

        for (let i = 1; i <= totalPages; i++) {
            const li = createPageItem(i, false, () => {
                currentPage = i;
                displayRows(currentPage);
            });
            if (i === currentPage) li.classList.add('active');
            paginationContainer.appendChild(li);
        }

        paginationContainer.appendChild(createPageItem('»', currentPage === totalPages, () => {
            currentPage++;
            displayRows(currentPage);
        }));
    };

    if (rows.length > 0 && table && tbody && paginationContainer) {
        displayRows(currentPage);
    } else if (paginationContainer) {
        paginationContainer.innerHTML = '';
    }

    const populateDays = (monthSelect, daySelect, selectedDate) => {
        const selectedMonthText = monthSelect.value;
        daySelect.innerHTML = '<option value="">-- Chọn ngày lao động --</option>';

        if (selectedMonthText) {
            const currentYear = new Date().getFullYear();
            const selectedMonth = parseInt(selectedMonthText.replace('Tháng ', ''), 10) - 1;
            const daysInMonth = new Date(currentYear, selectedMonth + 1, 0).getDate();

            for (let day = 1; day <= daysInMonth; day++) {
                const formattedDate = `${currentYear}-${String(selectedMonth + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
                const option = document.createElement('option');
                option.value = formattedDate;
                option.textContent = `${String(day).padStart(2, '0')}/${String(selectedMonth + 1).padStart(2, '0')}/${currentYear}`;
                daySelect.appendChild(option);
            }

            if (selectedDate) {
                daySelect.value = selectedDate;
                if (!daySelect.value) {
                    console.warn(`Ngày ${selectedDate} không tồn tại trong danh sách ngày của tháng ${selectedMonthText}.`);
                    showAlert(`Ngày lao động ${selectedDate} không hợp lệ cho tháng ${selectedMonthText}. Vui lòng chọn lại.`, 'danger');
                }
            }
        }
    };

    const modal = document.getElementById('themDotLaoDongModal');
    if (modal) {
        modal.addEventListener('shown.bs.modal', () => {
            const dotLaoDongSelect = document.getElementById('DotLaoDong');
            const ngayLaoDongSelect = document.getElementById('NgayLaoDong');
            const currentYear = new Date().getFullYear();

            if (!dotLaoDongSelect || !ngayLaoDongSelect) return;

            dotLaoDongSelect.addEventListener('change', () => {
                populateDays(dotLaoDongSelect, ngayLaoDongSelect);
            });

            ngayLaoDongSelect.addEventListener('change', () => {
                const selectedDate = new Date(ngayLaoDongSelect.value);
                if (ngayLaoDongSelect.value && selectedDate.getFullYear() < currentYear) {
                    ngayLaoDongSelect.setCustomValidity(`Năm không được nhỏ hơn ${currentYear}.`);
                    ngayLaoDongSelect.classList.add('is-invalid');
                    showAlert('Ngày không hợp lệ.', 'danger');
                } else {
                    ngayLaoDongSelect.setCustomValidity('');
                    ngayLaoDongSelect.classList.remove('is-invalid');
                    showAlert('Ngày lao động hợp lệ!', 'success');
                }
            });

            const form = modal.querySelector('.needs-validation');
            if (form) {
                form.addEventListener('submit', (event) => {
                    const ngayLaoDongInput = form.querySelector('#NgayLaoDong');
                    const selectedDate = new Date(ngayLaoDongInput.value);

                    if (ngayLaoDongInput.value && selectedDate.getFullYear() < currentYear) {
                        ngayLaoDongInput.setCustomValidity(`Năm không được nhỏ hơn ${currentYear}.`);
                        ngayLaoDongInput.classList.add('is-invalid');
                        showAlert('Ngày không hợp lệ.', 'danger');
                    } else {
                        ngayLaoDongInput.setCustomValidity('');
                        ngayLaoDongInput.classList.remove('is-invalid');
                    }

                    if (!form.checkValidity()) {
                        event.preventDefault();
                        event.stopPropagation();
                    } else {
                        showAlert('Gửi biểu mẫu thành công!', 'success');
                    }
                    form.classList.add('was-validated');
                }, false);
            }
        });
    }

    window.fillEditForm = function (id) {
        fetch(`/Admin/AdminHome/ChinhSuaDotLaoDong?id=${id}`, {
            method: 'GET',
            headers: { 'Accept': 'application/json' }
        })
            .then(response => {
                if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
                return response.json();
            })
            .then(data => {
                console.log('Dữ liệu trả về từ server:', data);

                const editID = document.getElementById('editID');
                const editDotLaoDongSelect = document.getElementById('editDotLaoDong');
                const editNgayLaoDongSelect = document.getElementById('editNgayLaoDong');
                const editKhuVuc = document.getElementById('editKhuVuc');
                const editBuoi = document.getElementById('editBuoi');
                const editLoaiLaoDong = document.getElementById('editLoaiLaoDong');
                const editGiaTri = document.getElementById('editGiaTri');
                const editMoTa = document.getElementById('editMoTa');
                const editThoiGian = document.getElementById('editThoiGian');

                if (!editID || !editDotLaoDongSelect || !editNgayLaoDongSelect || !editKhuVuc || !editBuoi || !editLoaiLaoDong || !editGiaTri || !editMoTa || !editThoiGian) {
                    throw new Error('Một hoặc nhiều phần tử trong form không tồn tại.');
                }

                editID.value = data.ID || '';
                editDotLaoDongSelect.value = data.DotLaoDong || '';
                populateDays(editDotLaoDongSelect, editNgayLaoDongSelect, data.NgayLaoDong || '');
                editKhuVuc.value = data.KhuVuc || '';
                editBuoi.value = data.Buoi || '';
                editLoaiLaoDong.value = data.LoaiLaoDong || '';
                editGiaTri.value = data.GiaTri !== null ? data.GiaTri.toString() : '';
                editMoTa.value = data.MoTa || '';
                editThoiGian.value = data.ThoiGian || '';

                showAlert('Đã tải dữ liệu để sửa.', 'success');
            })
            .catch(error => {
                console.error('Lỗi khi tải dữ liệu:', error);
                showAlert(`Lỗi khi tải dữ liệu: ${error.message}`, 'danger');
            });
    };

    const editModal = document.getElementById('suaDotLaoDongModal');
    if (editModal) {
        editModal.addEventListener('shown.bs.modal', () => {
            const editDotLaoDongSelect = document.getElementById('editDotLaoDong');
            const editNgayLaoDongSelect = document.getElementById('editNgayLaoDong');
            const currentYear = new Date().getFullYear();

            if (!editDotLaoDongSelect || !editNgayLaoDongSelect) return;

            editDotLaoDongSelect.addEventListener('change', () => {
                populateDays(editDotLaoDongSelect, editNgayLaoDongSelect);
            });

            editNgayLaoDongSelect.addEventListener('change', () => {
                const selectedDate = new Date(editNgayLaoDongSelect.value);
                if (editNgayLaoDongSelect.value && selectedDate.getFullYear() < currentYear) {
                    editNgayLaoDongSelect.setCustomValidity(`Năm không được nhỏ hơn ${currentYear}.`);
                    editNgayLaoDongSelect.classList.add('is-invalid');
                    showAlert('Ngày không hợp lệ.', 'danger');
                } else {
                    editNgayLaoDongSelect.setCustomValidity('');
                    editNgayLaoDongSelect.classList.remove('is-invalid');
                    showAlert('Ngày lao động hợp lệ!', 'success');
                }
            });

            const editForm = editModal.querySelector('.needs-validation');
            if (editForm) {
                editForm.addEventListener('submit', (event) => {
                    const ngayLaoDongInput = editForm.querySelector('#editNgayLaoDong');
                    const selectedDate = new Date(ngayLaoDongInput.value);

                    if (ngayLaoDongInput.value && selectedDate.getFullYear() < currentYear) {
                        ngayLaoDongInput.setCustomValidity(`Năm không được nhỏ hơn ${currentYear}.`);
                        ngayLaoDongInput.classList.add('is-invalid');
                        showAlert('Ngày không hợp lệ.', 'danger');
                    } else {
                        ngayLaoDongInput.setCustomValidity('');
                        ngayLaoDongInput.classList.remove('is-invalid');
                    }

                    if (!editForm.checkValidity()) {
                        event.preventDefault();
                        event.stopPropagation();
                    } else {
                        const formData = new FormData(editForm);
                        console.log('Dữ liệu gửi đi:', Object.fromEntries(formData));
                        showAlert('Gửi biểu mẫu thành công!', 'success');
                    }
                    editForm.classList.add('was-validated');
                }, false);
            }
        });
    }

    // Xử lý nút "Xem Chi Tiết"
    window.viewDetails = function (id) {
        fetch(`/Admin/AdminHome/ChiTietDotLaoDong?id=${id}`, {
            method: 'GET',
            headers: { 'Accept': 'application/json' }
        })
            .then(response => {
                if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
                return response.json();
            })
            .then(data => {
                document.getElementById('detailDotLaoDong').textContent = data.DotLaoDong || '';
                document.getElementById('detailNgayLaoDong').textContent = data.NgayLaoDong || '';
                document.getElementById('detailKhuVuc').textContent = data.KhuVuc || '';
                document.getElementById('detailBuoi').textContent = data.Buoi || '';
                document.getElementById('detailLoaiLaoDong').textContent = data.LoaiLaoDong || '';
                document.getElementById('detailGiaTri').textContent = data.GiaTri !== null ? data.GiaTri : '';
                document.getElementById('detailMoTa').textContent = data.MoTa || 'Không có mô tả';
                document.getElementById('detailThoiGian').textContent = data.ThoiGian || '';

                const chiTietModal = new bootstrap.Modal(document.getElementById('chiTietDotLaoDongModal'));
                chiTietModal.show();
            })
            .catch(error => {
                console.error('Lỗi khi tải chi tiết:', error);
                showAlert(`Lỗi khi tải chi tiết: ${error.message}`, 'danger');
            });
    };

    // Xử lý nút "Xóa"
    window.deleteDotLaoDong = function (id) {
        if (confirm('Bạn có chắc chắn muốn xóa đợt lao động này?')) {
            fetch(`/Admin/AdminHome/XoaDotLaoDong?id=${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        const row = document.querySelector(`tr[data-id="${id}"]`);
                        if (row) {
                            row.remove();
                            allRows = allRows.filter(r => r.element.getAttribute('data-id') !== id.toString());
                            currentPage = 1;
                            displayRows(currentPage);
                        }
                        showAlert(data.message, 'success');
                    } else {
                        showAlert(data.message, 'danger');
                    }
                })
                .catch(error => {
                    console.error('Lỗi khi xóa:', error);
                    showAlert(`Lỗi khi xóa: ${error.message}`, 'danger');
                });
        }
    };

    // Xử lý lọc và tìm kiếm
    if (filterDotLaoDong) filterDotLaoDong.addEventListener('change', () => { currentPage = 1; displayRows(currentPage); });
    if (filterKhuVuc) filterKhuVuc.addEventListener('change', () => { currentPage = 1; displayRows(currentPage); });
    if (filterBuoi) filterBuoi.addEventListener('change', () => { currentPage = 1; displayRows(currentPage); });
    if (filterLoaiLaoDong) filterLoaiLaoDong.addEventListener('change', () => { currentPage = 1; displayRows(currentPage); });
    if (filterThoiGian) filterThoiGian.addEventListener('change', () => { currentPage = 1; displayRows(currentPage); });
    if (searchInput) searchInput.addEventListener('input', () => { currentPage = 1; displayRows(currentPage); });
});