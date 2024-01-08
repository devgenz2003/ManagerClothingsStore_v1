document.addEventListener("DOMContentLoaded", function () {
    var radioPercentage = document.getElementById('typePercentage');
    var radioCash = document.getElementById('typeCash');
    var labelForReducedValue = document.getElementById('labelForReducedValue');
    var reducedValueSuffix = document.getElementById('reducedValueSuffix');

    function updateLabelAndSuffix() {
        if (radioPercentage.checked) {
            labelForReducedValue.textContent = 'Giá trị giảm (%)';
            reducedValueSuffix.textContent = '%';
            inputReducedValue.type = 'number';
            inputReducedValue.min = 1;
            inputReducedValue.max = 100;
            inputReducedValue.step = 1;
        } else if (radioCash.checked) {
            labelForReducedValue.textContent = 'Giá trị giảm (tiền mặt)';
            reducedValueSuffix.textContent = 'vnđ';
            inputReducedValue.type = 'number'; // hoặc 'number' tùy thuộc vào cách bạn muốn xử lý giá trị
            delete inputReducedValue.min;
            delete inputReducedValue.max;
            delete inputReducedValue.step;
        }
    }

    // Gắn sự kiện cho radio buttons
    radioPercentage.addEventListener('change', updateLabelAndSuffix);
    radioCash.addEventListener('change', updateLabelAndSuffix);

    // Cập nhật label và suffix khi trang tải xong
    updateLabelAndSuffix();
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
    var code = document.getElementById('Code').value;
    var name = document.getElementById('Name').value;
    var startdate = document.getElementById('StartDate').value;
    var enddate = document.getElementById('EndDate').value;
    var quantity = document.getElementById('Quantity').value;
    var minimum = document.getElementById('MinimumAmount').value;
    var reduce = document.getElementById('ReducedValue').value;
    var isActive = document.getElementById('IsActive').checked;
    var key = document.getElementById('Key').value;
    var selectedUser = [];
    var checkboxes = document.querySelectorAll('.userCheckbox:checked');
    checkboxes.forEach(function (checkbox) {
selectedUser.push(checkbox.value);
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
        Code: code,
        Name: name,
        StartDate: startdate,
        EndDate: enddate,
        Quantity: quantity,
        MinimumAmount: minimum,
        ReducedValue: reduce,
        IsActive: isActive,
        Type: type,
        Key: key,
        SelectedVariantIds: selectedUser
    };
    console.log("data:", data);
    fetch('https://localhost:7108/api/Voucher/create', {
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
                    text: 'Chương trình voucher đã được tạo.',
                    icon: 'success',
                }).then(() => {
                    window.location.href = '/voucher_list';
                });
            } else {
                Swal.fire({
                    title: 'Lỗi',
                    text: 'Có lỗi xảy ra khi tạo voucher khuyến mãi.',
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
        title: 'Xác nhận tạo chương trình voucher?',
        text: 'Bạn có chắc muốn tạo chương trình này?',
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
