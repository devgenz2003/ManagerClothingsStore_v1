document.addEventListener("DOMContentLoaded", function () {
    var form = document.getElementById('createOptionForm');
    form.addEventListener('submit', function (e) {
        e.preventDefault();

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
                sendData(form);
            }
        });
    });

    function sendData(form) {
        var formData = new FormData(form);
        var xhr = new XMLHttpRequest();

        xhr.onloadstart = function () {
            Swal.fire({
                title: 'Đang xử lý...',
                html: '<div class="progress"><div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div>',
                showConfirmButton: false,
                allowOutsideClick: false,
            });
        };

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    Swal.fire({
                        title: 'Thành công!',
                        text: 'Tạo phân loại thành công.',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then(() => {
                        window.location.href = "/optionlist";
                    });
                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'Something went wrong.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            }
        };

        xhr.open('POST', 'https://localhost:7108/api/Options/create');
        xhr.send(formData);
    }

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
