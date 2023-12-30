document.addEventListener('DOMContentLoaded', function () {
    var form = document.querySelector('form');
    form.addEventListener('submit', function (event) {
        event.preventDefault();

        var formData = new FormData(form);

        Swal.fire({
            title: 'Bạn có chắc chắn?',
            text: "Xác nhận gửi dữ liệu!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có, gửi đi!'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Đang xử lý...',
                    html: '<div class="progress"><div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div>',
                    showConfirmButton: false,
                    allowOutsideClick: false,
                });

                sendFormData(formData);
            }
        });
    });

    function sendFormData(formData) {
        document.getElementById('loadingSpinner').style.display = 'block';

        var xhr = new XMLHttpRequest();
        xhr.open('POST', 'https://localhost:7108/api/Variants/create');

        xhr.onload = function () {
            document.getElementById('loadingSpinner').style.display = 'none';

            if (xhr.status === 200) {
                Swal.fire('Thành công!', 'Dữ liệu đã được gửi thành công.', 'success').then((result) => {
                    if (result.isConfirmed || result.isDismissed) {
                        window.location.href = '/variantlist';
                    }
                });
            } else {
                console.error("Error:", xhr.statusText);
                Swal.fire('Lỗi!', 'Có lỗi xảy ra khi gửi dữ liệu.', 'error');
            }
        };

        xhr.send(formData);
    }
});

document.addEventListener('DOMContentLoaded', function () {
    function updateInputField(selectElementId, inputElementId) {
        var selectElement = document.getElementById(selectElementId);
        selectElement.addEventListener('change', function () {
            var selectedOptionText = this.options[this.selectedIndex].text;
            var inputElement = document.getElementById(inputElementId);
            inputElement.value = selectedOptionText;
        });
    }

    updateInputField('IDCategory', 'CategoryName');
    updateInputField('IDBrand', 'BrandName');
    updateInputField('IDMaterial', 'MaterialName');
});