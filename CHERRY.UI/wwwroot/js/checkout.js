function formatCurrency(amount) {
    return amount.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
}
function parseCurrencyToNumber(currencyString) {
    return parseFloat(currencyString.replace(/[^\d.-]/g, ''));
}
document.addEventListener("DOMContentLoaded", function () {
    var selectedProducts = JSON.parse(sessionStorage.getItem('selectedProducts') || '[]');
    var tableBody = document.querySelector(".table tbody");

    if (selectedProducts.length > 0) {
        selectedProducts.forEach(function (product) {

            var row = document.createElement("tr");
            var deleteButton = document.createElement("button");
            deleteButton.textContent = "Xoá";
            deleteButton.addEventListener("click", function () {
                deleteProduct(product, selectedProducts, tableBody);
            });

            var actionCell = document.createElement("td");
            actionCell.appendChild(deleteButton);
            row.appendChild(actionCell);

            var productNameCell = document.createElement("td");
            productNameCell.textContent = product.ProductName;
            row.appendChild(productNameCell);

            var colorCell = document.createElement("td");
            colorCell.textContent = product.ColorName;
            row.appendChild(colorCell);

            var sizeCell = document.createElement("td");
            sizeCell.textContent = product.SizeName;
            row.appendChild(sizeCell);

            var quantityCell = document.createElement("td");
            quantityCell.textContent = product.Quantity;
            row.appendChild(quantityCell);

            var unitPriceCell = document.createElement("td");
            unitPriceCell.textContent = formatCurrency(product.UnitPrice);
            row.appendChild(unitPriceCell);

            var discountCell = document.createElement("td");
            discountCell.textContent = formatCurrency(product.DiscountedPrice);
            row.appendChild(discountCell);
            console.log(formatCurrency(product.DiscountedPrice));

            var totalPriceCell = document.createElement("td");
            totalPriceCell.textContent = formatCurrency(product.TotalPrice);
            row.appendChild(totalPriceCell);

            tableBody.appendChild(row);
        });
    } else {
        var noProductRow = document.createElement("tr");
        var noProductCell = document.createElement("td");
        noProductCell.setAttribute("colspan", "8");
        noProductCell.textContent = "Không có sản phẩm nào được chọn.";
        noProductRow.appendChild(noProductCell);
        tableBody.appendChild(noProductRow);
    }
});
function deleteProduct(product, selectedProducts, tableBody) {

    Swal.fire({
        title: 'Xác nhận xoá sản phẩm?',
        text: `Bạn có chắc muốn xoá sản phẩm "${product.ProductName}" không?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xoá',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {

            var index = selectedProducts.indexOf(product);
            if (index !== -1) {
                selectedProducts.splice(index, 1);
            }

            sessionStorage.setItem('selectedProducts', JSON.stringify(selectedProducts));

            var rowToDelete = findRowByProduct(product, tableBody);
            if (rowToDelete) {
                rowToDelete.remove();
            }

            if (selectedProducts.length === 0) {
                var noProductRow = document.createElement("tr");
                var noProductCell = document.createElement("td");
                noProductCell.setAttribute("colspan", "7");
                noProductCell.textContent = "Không có sản phẩm nào được chọn.";
                noProductRow.appendChild(noProductCell);
                tableBody.appendChild(noProductRow);
            }
        }
    });
}
function findRowByProduct(product, tableBody) {
    var rows = tableBody.querySelectorAll("tr");
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var productNameCell = row.querySelector("td:nth-child(2)");
        if (productNameCell && productNameCell.textContent === product.ProductName) {
            return row;
        }
    }
    return null;
}
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("confirmOrderButton").addEventListener("click", function () {
        var customerName = document.getElementById("CustomerName").value;
        var customerPhone = document.getElementById("CustomerPhone").value;
        var customerEmail = document.getElementById("CustomerEmail").value;
        var shippingMethod = parseInt(document.getElementById("ShippingMethod").value, 10);
        var shippingAddress = document.getElementById("shippingAddress").value.trim();
        var paymentMethod = parseInt(document.querySelector('input[name="PaymentMethod"]:checked').value, 10);
        var notes = document.getElementById("Notes").value.trim();
        var IDUser = document.getElementById("iduser").value;

        if (customerName === "" || customerPhone === "" || customerEmail === "" || shippingMethod === "" ||
            shippingAddress === "" || paymentMethod === "") {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Vui lòng điền đầy đủ thông tin và chọn phương thức thanh toán.'
            });
            return;
        }

        Swal.fire({
            title: 'Xác nhận đặt hàng?',
            text: 'Bạn có chắc chắn muốn đặt hàng không?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Đặt hàng',
            cancelButtonText: 'Hủy'
        }).then(function (result) {
            if (result.isConfirmed) {
                var selectedProducts = JSON.parse(sessionStorage.getItem('selectedProducts') || '[]');
                var productData = [];

                selectedProducts.forEach(function (product) {
                    productData.push({
                        CreateBy: IDUser,
                        IDOrder: document.getElementById("ID").value,
                        IDOptions: product.IDOptions,
                        Quantity: product.Quantity,
                        Discount: 0,
                        UnitPrice: parseCurrencyToNumber(product.UnitPrice),
                        TotalPrice: parseCurrencyToNumber(product.TotalPrice),
                        Status: 1,
                        HasReviewed: false,
                        HasPurchased: false
                    });
                });

                var selectedVoucher = document.querySelector('input.voucher-radio:checked');
                var voucherCode = selectedVoucher ? selectedVoucher.getAttribute('data-code') : '';

                function parseFormattedCurrencyToNumber(formattedCurrencyString) {
                    return parseFloat(formattedCurrencyString.replace(/\./g, '').replace(/[^\d.-]/g, ''));
                }

                var formattedFinalTotalAmount = document.getElementById('FinalTotalAmount').value;
                var finalTotalAmount = parseFormattedCurrencyToNumber(formattedFinalTotalAmount);

                var formData = {
                    CreateBy: IDUser,
                    VoucherCode: voucherCode,
                    HexCode: document.getElementById("HexCode").value,
                    ID: document.getElementById("ID").value,
                    IDUser: IDUser,
                    CustomerName: customerName,
                    CustomerPhone: customerPhone,
                    CustomerEmail: customerEmail,
                    ShippingMethod: shippingMethod,
                    TrackingCheck: false,
                    ShippingAddress: shippingAddress,
                    ShippingAddressLine2: document.getElementById("ShippingAddressLine2").value,
                    TotalAmount: finalTotalAmount,
                    PaymentMethod: paymentMethod,
                    Notes: notes,
                    Status: 1,
                    OrderVariantCreateVM: productData
                };
                var token = document.cookie.replace(/(?:(?:^|.*;\s*)token\s*=\s*([^;]*).*$)|^.*$/, "$1");

                function sendCreatePaymentUrlRequest() {
                    return new Promise((resolve, reject) => {
                        var xhr = new XMLHttpRequest();
                        xhr.open('POST', '/CreatePaymentUrl', true);
                        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
                        xhr.setRequestHeader("Authorization", "Bearer " + token);

                        xhr.onreadystatechange = function () {
                            if (xhr.readyState === 4) {
                                if (xhr.status === 200) {
                                    try {
                                        var response = JSON.parse(xhr.responseText);

                                        if (!response || !response.paymentUrl) {
                                            reject('Không lấy được URL thanh toán. Vui lòng thử lại sau.');
                                        } else {
                                            var PaymentUrl = response.paymentUrl;
                                            resolve(PaymentUrl);
                                        }
                                    } catch (error) {
                                        reject('Lỗi khi phân tích JSON: ' + error);
                                    }
                                } else {
                                    reject('Đặt hàng không thành công. Vui lòng thử lại sau.');
                                }
                            }
                        };

                        xhr.send(JSON.stringify(formData));
                    });
                }

                function sendCreateOrderRequest() {
                    return new Promise((resolve, reject) => {
                        var xhr = new XMLHttpRequest();
                        xhr.open("POST", "https://localhost:7108/api/Order/create", true);
                        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
                        xhr.setRequestHeader("Authorization", "Bearer " + token);

                        xhr.onreadystatechange = function () {
                            if (xhr.readyState === 4) {
                                if (xhr.status === 200) {
                                    resolve();
                                } else {
                                    reject('Đặt hàng không thành công. Vui lòng thử lại sau.');
                                }
                            }
                        };

                        xhr.send(JSON.stringify(formData));
                    });
                }

                if (paymentMethod === 0) {
                    sendCreatePaymentUrlRequest()
                        .then((PaymentUrl) => {
                            window.location.href = PaymentUrl;
                            return sendCreateOrderRequest();
                        })
                        .catch((error) => {
                            Swal.fire('Lỗi', error, 'error');
                        });
                } else {
                    sendCreateOrderRequest()
                        .then(() => {
                            Swal.fire({
                                title: 'Thành công',
                                text: 'Đơn hàng đã được đặt thành công.',
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonText: 'Xác nhận',
                                timer: 5000,
                                timerProgressBar: true,
                                willClose: () => {
                                    var hexcode = document.getElementById("HexCode").value;
                                    var finalTotalAmount = document.getElementById("FinalTotalAmount").value;
                                    var data = {
                                        recipientEmail: customerEmail,
                                        subject: "Xác nhận đơn hàng thành công",
                                        message: `Mã đơn hàng ${hexcode} tổng giá trị ${finalTotalAmount} đã được đặt thành công`
                                    };
                                    fetch('https://localhost:7108/api/Email/send-email', {
                                        method: 'POST',
                                        headers: {
                                            'Content-Type': 'application/json'
                                        },
                                        body: JSON.stringify(data)
                                    })
                                        .then(response => {
                                            if (response.ok) {
                                                return response.json();
                                            }
                                            throw new Error('Failed to send email.');
                                        })
                                        .then(() => {
                                            window.location.href = '/order_list';
                                        })
                                        .catch((error) => {
                                            Swal.fire('Lỗi', error, 'error');
                                        });
                                }
                            });
                        })
                        .catch((error) => {
                            Swal.fire('Lỗi', error, 'error');
                        });
                }
            }
        });
    });
});
document.getElementById('applyVoucherButton').addEventListener('click', function () {
    var selectedVoucher = document.querySelector('input.voucher-radio:checked');
    if (selectedVoucher) {
        var discountValue = parseFloat(selectedVoucher.getAttribute('data-discount'));
        var discountType = selectedVoucher.getAttribute('data-type');
        document.getElementById('discountText').textContent = formatCurrency(discountValue, discountType);

        $('#voucherModal').modal('hide');
    } else {
        alert('Vui lòng chọn một voucher trước khi áp dụng.');
    }
});
document.getElementById('applyVoucherButton').addEventListener('click', function () {
    var selectedVoucher = document.querySelector('input.voucher-radio:checked');
    if (selectedVoucher) {
        var discountValue = parseFloat(selectedVoucher.getAttribute('data-discount'));
        var discountType = selectedVoucher.getAttribute('data-type');

        document.getElementById('selectedVoucherCode').value = selectedVoucher.id;

        var totalAmount = parseFloat(document.getElementById('TotalAmount').value.replace(/[^\d.-]/g, ''));
        var discountedAmount = 0;

        if (discountType === 'Percent') {
            discountedAmount = totalAmount * (discountValue / 100);
        } else {
            discountedAmount = discountValue;
        }

        var formattedDiscount = discountType === 'Percent' ? '-' + discountValue + '%' : '-' + formatCurrency(discountedAmount);
        document.getElementById('discountText').textContent = formattedDiscount;

        $('#voucherModal').modal('hide');
    } else {
        alert('Vui lòng chọn một voucher trước khi áp dụng.');
    }
});
document.getElementById('applyVoucherButton').addEventListener('click', function () {
    updateTotalPriceAndAmount();
});
function updateTotalPriceAndAmount() {
    var totalAmount = calculateTotalAmount();
    var discountAmount = calculateDiscountAmount();

    // Cập nhật các trường trên giao diện
    document.getElementById('TotalAmount').value = formatCurrency(totalAmount);
    document.getElementById('DiscountedTotalAmount').value = formatCurrency(discountAmount);
    var finalTotalAmount = totalAmount - discountAmount;
    document.getElementById('FinalTotalAmount').value = formatCurrency(finalTotalAmount);
}
function calculateTotalAmount() {
    var selectedProducts = JSON.parse(sessionStorage.getItem('selectedProducts') || '[]');
    var totalAmount = 0;

    selectedProducts.forEach(function (product) {
        totalAmount += parseCurrencyToNumber(product.TotalPrice);
    });

    return totalAmount;
}
function calculateDiscountAmount() {
    var selectedVoucher = document.querySelector('input.voucher-radio:checked');
    var totalAmount = calculateTotalAmount();
    var discountAmount = 0;

    if (selectedVoucher) {
        var discountValue = parseFloat(selectedVoucher.getAttribute('data-discount'));
        var discountType = selectedVoucher.getAttribute('data-type');

        if (discountType === 'Percent') {
            discountAmount = totalAmount * (discountValue / 100);
        } else {
            discountAmount = parseCurrencyToNumber(discountValue);
        }
    }

    return discountAmount;
}
function applyVoucher() {
    updateTotalPriceAndAmount();
    $('#voucherModal').modal('hide');
}
function updateTotalAmount() {
    var selectedProducts = JSON.parse(sessionStorage.getItem('selectedProducts') || '[]');
    var totalAmount = 0;
    selectedProducts.forEach(function (product) {
        totalAmount += product.TotalPrice;
    });
    document.getElementById('TotalAmount').value = formatCurrency(totalAmount);
}
document.getElementById('applyVoucherButton').addEventListener('click', applyVoucher);
document.addEventListener("DOMContentLoaded", function () {
    updateTotalPriceAndAmount();
});