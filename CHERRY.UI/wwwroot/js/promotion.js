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


function submitForm() {
    var sku = document.getElementById('SKU').value;
    var content = document.getElementById('Content').value;
    var startDate = document.getElementById('StartDate').value;
    var endDate = document.getElementById('EndDate').value;
    var isActive = document.getElementById('IsActive').checked;
    var discountAmount = document.getElementById('DiscountAmount').value;
    var selectedVariantIds = [];

    var checkboxes = document.querySelectorAll('.productCheckbox:checked');
    checkboxes.forEach(function (checkbox) {
        selectedVariantIds.push(checkbox.value);
    });

    var type; 

    var radioPercentage = document.getElementById('typePercentage');
    var radioCash = document.getElementById('typeCash');
    if (radioPercentage.checked) {
        type = 0;
    } else if (radioCash.checked) {
        type = 1;
    }

    var data = {
        CreateBy: 'acb',
        SKU: sku,
        Content: content,
        StartDate: startDate,
        EndDate: endDate,
        IsActive: isActive,
        DiscountAmount: discountAmount,
        Type: type, 
        SelectedVariantIds: selectedVariantIds
    };
    console.log("data:", data);
    fetch('https://localhost:7108/api/Promotion/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (response.ok) {
                Swal.fire({
                    title: 'Thành công',
                    text: 'Chương trình khuyến mãi đã được tạo.',
                    icon: 'success',
                }).then(() => {
                    window.location.href = '/promotion_list';
                });
            } else {
                Swal.fire({
                    title: 'Lỗi',
                    text: 'Có lỗi xảy ra khi tạo chương trình khuyến mãi.',
                    icon: 'error',
                });
            }
        })
        .catch(error => {
            console.error('Lỗi:', error);
        });
}

document.getElementById('createButton').addEventListener('click', function () {
    Swal.fire({
        title: 'Xác nhận tạo chương trình khuyến mãi?',
        text: 'Bạn có chắc muốn tạo chương trình khuyến mãi này?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy',
    }).then((result) => {
        if (result.isConfirmed) {
            submitForm();
        }
    });
});
