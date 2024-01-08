document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('edit_submit').addEventListener('click', function (e) {
        e.preventDefault();

        var id = document.getElementById('ID').value;
        var sku = document.getElementById('SKU').value;
        var content = document.getElementById('Content').value;
        var startDate = document.getElementById('StartDate').value;
        var endDate = document.getElementById('EndDate').value;
        var discountAmount = document.getElementById('DiscountAmount').value;
        //var typePercentage = document.getElementById('typePercentage').value;
        var isActive = document.getElementById('IsActive').checked;
        var selectedVariantIds = [];

        var checkboxes = document.querySelectorAll('.productCheckbox');
        checkboxes.forEach(function (checkbox) {
            if (checkbox.checked) {
                selectedVariantIds.push(checkbox.value);
            }
        });

        console.log(selectedVariantIds);

        var requestData = {
            ID: id,
            SKU: sku,
            Content: content,
            StartDate: startDate,
            EndDate: endDate,
            IsActive: isActive,
            //Type: typePercentage,
            DiscountAmount: discountAmount,
            SelectedVariantIds: selectedVariantIds,
            Status: 1
        };

        Swal.fire({
            title: 'Bạn có chắc muốn cập nhật?',
            text: 'Thay đổi sẽ được áp dụng ngay lập tức!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Cập nhật',
            cancelButtonText: 'Hủy bỏ'
        }).then((result) => {
            if (result.isConfirmed) {
                var xhr = new XMLHttpRequest();
                xhr.open('PUT', 'https://localhost:7108/api/Promotion/Edit_promotion/' + id, true);
                xhr.setRequestHeader('Content-Type', 'application/json');
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Cập nhật thành công!',
                                showConfirmButton: false,
                                timer: 1500
                            }).then(function () {
                                window.location.href = '/promotion_list';
                            });
                        } else {
                            console.log(xhr.responseText);

                            Swal.fire({
                                icon: 'error',
                                title: 'Lỗi khi cập nhật!',
                                text: 'Xin vui lòng thử lại sau.',
                                confirmButtonText: 'Đóng'
                            });
                        }
                    }
                };
                xhr.send(JSON.stringify(requestData));
                console.log(requestData)
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var radioPercentage = document.getElementById('typePercentage');
    var radioCash = document.getElementById('typeCash');
    var labelForReducedValue = document.getElementById('labelForReducedValue');
    var reducedValueSuffix = document.getElementById('reducedValueSuffix');
    var inputReducedValue = document.getElementById('inputReducedValue');

    function updateLabelAndSuffix() {
        if (radioPercentage.checked) {
            labelForReducedValue.textContent = 'Giá trị giảm (%)';
            reducedValueSuffix.textContent = '%';
            inputReducedValue.setAttribute('type', 'number');
            inputReducedValue.setAttribute('min', '1');
            inputReducedValue.setAttribute('max', '100');
            inputReducedValue.setAttribute('step', '1');
        } else if (radioCash.checked) {
            labelForReducedValue.textContent = 'Giá trị giảm (tiền mặt)';
            reducedValueSuffix.textContent = 'vnđ';
            inputReducedValue.setAttribute('type', 'number');
            inputReducedValue.removeAttribute('min');
            inputReducedValue.removeAttribute('max');
            inputReducedValue.removeAttribute('step');
        }
    }

    radioPercentage.addEventListener('change', updateLabelAndSuffix);
    radioCash.addEventListener('change', updateLabelAndSuffix);

    if (radioPercentage.checked || radioCash.checked) {
        updateLabelAndSuffix();
    }
});
function attachCheckboxEvents() {
    var checkAll = document.getElementById('checkAll');
    var checkboxes = document.querySelectorAll('.productCheckbox');

    checkAll.addEventListener('change', function () {
        checkboxes.forEach(function (checkbox) {
            checkbox.checked = checkAll.checked;
        });
    });

    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            checkAll.checked = Array.from(checkboxes).every(c => c.checked);
        });
    });
}
document.addEventListener("DOMContentLoaded", function () {
    attachCheckboxEvents();
});
attachCheckboxEvents();

