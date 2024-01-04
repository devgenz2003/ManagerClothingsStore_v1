document.addEventListener("DOMContentLoaded", function () {
    var buttons = document.querySelectorAll('.btn-page');
    buttons.forEach(function (button) {
        button.addEventListener('click', function () {
            var status = this.getAttribute('data-status');
            var allRows = document.querySelectorAll('tbody tr');

            buttons.forEach(function (btn) {
                btn.classList.remove('active');
            });

            this.classList.add('active');

            if (status === 'all') {
                allRows.forEach(function (row) {
                    row.style.display = 'table-row';
                });
            } else {
                allRows.forEach(function (row) {
                    var dataStatus = row.getAttribute('data-status');
                    if (dataStatus === status) {
                        row.style.display = 'table-row';
                    } else {
                        row.style.display = 'none';
                    }
                });
            }
        });
    });

    var allRows = document.querySelectorAll('tbody tr');
    allRows.forEach(function (row) {
        row.style.display = 'table-row';
    });
});
document.addEventListener("DOMContentLoaded", function () {
    var pagination = document.getElementById('pagination');
    pagination.addEventListener('click', function (e) {
        e.preventDefault();
        var pageUrl = e.target.getAttribute('href');
        loadPage(pageUrl);
    });

    function loadPage(url) {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', url, true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                document.body.innerHTML = xhr.responseText;
            }
        };
        xhr.send();
    }
});
function markAsCancelled(IDOrder) {
    Swal.fire({
        title: 'Xác nhận huỷ đơn',
        text: 'Bạn có chắc muốn huỷ đơn hàng này?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Huỷ đơn',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            executeCancellation(IDOrder);
        }
    });
}

function executeCancellation(IDOrder) {
    var xhr = new XMLHttpRequest();
    xhr.open('PUT', `https://localhost:7108/api/Order/MarkAsCancelled/${IDOrder}`);
    xhr.setRequestHeader('Content-Type', 'application/json');

    xhr.onload = function () {
        if (xhr.status === 200) {
            Swal.fire({
                title: 'Thành công!',
                text: 'Huỷ đơn thành công.',
                icon: 'success',
                confirmButtonText: 'OK'
            });
            console.log('Đã đánh dấu hủy đơn hàng thành công!');
        } else {
            console.error('Lỗi khi đánh dấu hủy đơn hàng:', xhr.statusText);
        }
    };

    xhr.onerror = function () {
        console.error('Yêu cầu gặp sự cố');
    };

    xhr.send(JSON.stringify({}));
}
