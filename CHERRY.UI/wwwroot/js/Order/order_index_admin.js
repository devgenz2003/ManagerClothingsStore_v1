document.addEventListener("DOMContentLoaded", function () {
    var confirmButtons = document.querySelectorAll(".confirmOrderButton");

    confirmButtons.forEach(function (button) {
        button.addEventListener("click", function (event) {
            event.preventDefault();
            var orderId = this.getAttribute("data-order-id");

            Swal.fire({
                title: 'Xác nhận xử lý đơn hàng?',
                text: 'Bạn có chắc muốn xác nhận đơn hàng này không?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Xác nhận',
                cancelButtonText: 'Hủy'
            }).then(function (result) {
                if (result.isConfirmed) {
                    var xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === XMLHttpRequest.DONE) {
                            if (xhr.status === 200) {
                                Swal.fire({
                                    title: 'Thành công',
                                    text: 'Đã xác nhận đơn hàng thành công.',
                                    icon: 'success',
                                }).then(() => {
                                    location.reload();
                                    const hexcode = document.getElementById("HexCode").textContent;
                                    const customerEmail = document.getElementById("customerEmail").textContent;
                                    console.log(hexcode)
                                    console.log(customerEmail)
                                    const data = {
                                        recipientEmail: customerEmail,
                                        subject: "Xác nhận đơn hàng thành công",
                                        message: `Mã đơn hàng ${hexcode} đã được xác nhận`
                                    };
                                    fetch('https://localhost:7108/api/Email/send-email', {
                                        method: 'POST',
                                        headers: {
                                            'Content-Type': 'application/json'
                                        },
                                        body: JSON.stringify(data)
                                    })
                                        .then(response => {
                                            if (!response.ok) {
                                                throw new Error('Có lỗi khi gửi email.');
                                            }
                                            return response.json();
                                        })
                                        .then(() => {
                                            location.reload();
                                        })
                                });
                            } else {
                                Swal.fire({
                                    title: 'Lỗi',
                                    text: 'Có lỗi xảy ra khi xác nhận đơn hàng.',
                                    icon: 'error'
                                });
                            }
                        }
                    };

                    xhr.open("PUT", `https://localhost:7108/api/Order/MarkAsProcessing/${orderId}`, true);
                    xhr.send();

                }
            });
        });
    });
    var deliveredButton = document.getElementById("Delivered");
    if (deliveredButton) {
        deliveredButton.addEventListener("click", function (event) {
            event.preventDefault();
            var orderId = this.getAttribute("data-order-id");

            Swal.fire({
                title: 'Xác nhận hoàn thành giao hàng?',
                text: 'Bạn có chắc muốn đánh dấu đơn hàng này là đã hoàn thành?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Xác nhận',
                cancelButtonText: 'Hủy'
            }).then(function (result) {
                if (result.isConfirmed) {
                    var xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === XMLHttpRequest.DONE) {
                            if (xhr.status === 200) {
                                Swal.fire({
                                    title: 'Thành công',
                                    text: 'Đã xác nhận hoàn thành đơn hàng.',
                                    icon: 'success',
                                }).then(() => {
                                    location.reload();
                                    const hexcode = document.getElementById("HexCode").textContent;
                                    const customerEmail = document.getElementById("customerEmail").textContent;
                                    console.log(hexcode)
                                    console.log(customerEmail)
                                    const data = {
                                        recipientEmail: customerEmail,
                                        subject: "Xác nhận hoàn thành đơn hàng",
                                        message: `Mã đơn hàng ${hexcode} đã được giao tới người dùng. 
                                        Hãy đánh giá sản phẩm nhé! Thân ái con mẹ m`
                                    };
                                    fetch('https://localhost:7108/api/Email/send-email', {
                                        method: 'POST',
                                        headers: {
                                            'Content-Type': 'application/json'
                                        },
                                        body: JSON.stringify(data)
                                    })
                                        .then(response => {
                                            if (!response.ok) {
                                                throw new Error('Có lỗi khi gửi email.');
                                            }
                                            return response.json();
                                        })
                                        .then(() => {
                                            location.reload();
                                        })
                                });
                            } else {
                                Swal.fire({
                                    title: 'Lỗi',
                                    text: 'Có lỗi xảy ra khi giao đơn hàng.',
                                    icon: 'error'
                                });
                            }
                        }
                    };
                    xhr.open("PUT", `https://localhost:7108/api/Order/MarkAsDelivered/${orderId}`, true);
                    xhr.send();
                }
            });
        });
    }

    var shippingButton = document.getElementById("shipping");
    if (shippingButton) {
        shippingButton.addEventListener("click", function (event) {
            event.preventDefault();
            var orderId = this.getAttribute("data-order-id");

            Swal.fire({
                title: 'Xác nhận giao hàng?',
                text: 'Bạn có chắc muốn xác nhận giao hàng này không?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Xác nhận',
                cancelButtonText: 'Hủy'
            }).then(function (result) {
                if (result.isConfirmed) {
                    var xhr = new XMLHttpRequest();
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === XMLHttpRequest.DONE) {
                            if (xhr.status === 200) {
                                Swal.fire({
                                    title: 'Thành công',
                                    text: 'Đã xác nhận giao đơn hàng thành công.',
                                    icon: 'success',
                                }).then(() => {
                                    location.reload();
                                    const hexcode = document.getElementById("HexCode").textContent;
                                    const customerEmail = document.getElementById("customerEmail").textContent;
                                    console.log(hexcode)
                                    console.log(customerEmail)
                                    const data = {
                                        recipientEmail: customerEmail,
                                        subject: "Xác nhận đơn hàng thành công",
                                        message: `Mã đơn hàng ${hexcode} đã được giao đi`
                                    };
                                    fetch('https://localhost:7108/api/Email/send-email', {
                                        method: 'POST',
                                        headers: {
                                            'Content-Type': 'application/json'
                                        },
                                        body: JSON.stringify(data)
                                    })
                                        .then(response => {
                                            if (!response.ok) {
                                                throw new Error('Có lỗi khi gửi email.');
                                            }
                                            return response.json();
                                        })
                                        .then(() => {
                                            location.reload();
                                        })
                                });
                            } else {
                                Swal.fire({
                                    title: 'Lỗi',
                                    text: 'Có lỗi xảy ra khi giao đơn hàng.',
                                    icon: 'error'
                                });
                            }
                        }
                    };
                    xhr.open("PUT", `https://localhost:7108/api/Order/MarkAsShipped/${orderId}`, true);
                    xhr.send();
                }
            });
        });
    }
});

document.addEventListener('DOMContentLoaded', function () {
    var orderListTable = $('#orderListTable').DataTable({
        "paging": true, // Bật phân trang
        "searching": true, // Bật tính năng tìm kiếm
        "ordering": true
    });
    $('#filterByDateBtn').on('click', function () {
        var startDate = $('#startDate').val();
        var endDate = $('#endDate').val();

        // Kiểm tra giá trị nhập vào
        if (!startDate || !endDate) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Vui lòng nhập đầy đủ Từ ngày và Đến ngày.',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Đóng'
            });
            return;
        }

        // Xóa các hàm tìm kiếm cũ
        $.fn.dataTable.ext.search = [];

        // Thêm hàm tìm kiếm mới vào mảng
        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                var dateInfo = $(orderListTable.cell(dataIndex, 0).node()).find('#date').text();

                // Chuyển đổi chuỗi ngày thành dữ liệu Date để so sánh
                var filterStartDate = new Date(startDate.split('-').reverse().join('-'));
                var filterEndDate = new Date(endDate.split('-').reverse().join('-'));
                var rowDate = new Date(dateInfo.split('-').reverse().join('-'));

                return (rowDate >= filterStartDate && rowDate <= filterEndDate);
            }
        );

        // Áp dụng bộ lọc
        orderListTable.draw();
    });

    $('#filterByTotalBtn').on('click', function () {
        var minTotal = parseFloat($('#minTotal').val());
        var maxTotal = parseFloat($('#maxTotal').val());

        // Kiểm tra giá trị nhập vào
        if (isNaN(minTotal) || isNaN(maxTotal) || minTotal > maxTotal) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Vui lòng nhập giá trị hợp lệ: "Từ" phải nhỏ hơn hoặc bằng "Đến" và không được để trống.',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Đóng'
            });
            return;
        }

        // Xóa các hàm tìm kiếm cũ
        $.fn.dataTable.ext.search = [];

        // Thêm hàm tìm kiếm mới vào mảng
        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                var total = parseFloat(data[3]) || 0; // Chỉnh index cột tổng nếu cần

                if (minTotal <= total && total <= maxTotal) {
                    return true;
                }
                return false;
            }
        );

        // Áp dụng bộ lọc
        orderListTable.draw();
    });
    // Xóa bộ lọc
    $('#clearFilterBtn').on('click', function () {
        $('#minTotal').val('');
        $('#maxTotal').val('');
        $.fn.dataTable.ext.search.pop();
        orderListTable.draw();
    });
    document.getElementById('statusFilter').addEventListener('change', function () {
        var selectedStatus = this.value;
        orderListTable.column(6).search(selectedStatus).draw();
    });
});
