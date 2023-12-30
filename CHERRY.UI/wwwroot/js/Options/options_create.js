document.addEventListener("DOMContentLoaded", function () {
    var form = document.getElementById('createOptionForm');
    document.getElementById('createOptionForm').addEventListener('submit', function (e) {
        e.preventDefault();

        // Hiển thị thông báo xác nhận trước khi gửi
        Swal.fire({
            title: 'Xác nhận gửi dữ liệu?',
            text: "Bạn xác nhận tạo phân loại này chứ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Xác nhận!'
        }).then((result) => {
            if (result.isConfirmed) {
                sendFormData(form);
            }
        });
    });
});

function sendFormData(form) {
    var formData = new FormData(form);
    fetch('https://localhost:7108/api/Options/create', {
        method: 'POST',
        body: formData
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(function (data) {
            // Hiển thị thông báo thành công
            Swal.fire({
                title: 'Thành công!',
                text: 'Tạo phân loại thành công.',
                icon: 'success',
                confirmButtonText: 'OK'
            }).then((result) => {
                window.location.href = "/optionlist"; 
            });
        })
        .catch(function (error) {
            // Hiển thị thông báo lỗi
            Swal.fire({
                title: 'Error!',
                text: 'Something went wrong.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

document.addEventListener('DOMContentLoaded', function () {
    function updateInputField(selectElementId, inputElementId) {
        var selectElement = document.getElementById(selectElementId);
        selectElement.addEventListener('change', function () {
            var selectedOptionText = this.options[this.selectedIndex].text;
            var inputElement = document.getElementById(inputElementId);
            inputElement.value = selectedOptionText;
        });
    }

    updateInputField('IDColor', 'ColorName');
    updateInputField('IDSizes', 'SizesName');
});