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


document.addEventListener('DOMContentLoaded', function () {
    var editForm = document.querySelector('form');
    var idvariant = document.getElementById('idvariant');
    editForm.addEventListener('submit', function (event) {
        event.preventDefault();

        var formData = new FormData();

        formData.append("IDCategory", document.getElementById("IDCategory").value);
        formData.append("CategoryName", document.getElementById("CategoryName").value);
        formData.append("IDBrand", document.getElementById("IDBrand").value);
        formData.append("BrandName", document.getElementById("BrandName").value);
        formData.append("IDMaterial", document.getElementById("IDMaterial").value);
        formData.append("MaterialName", document.getElementById("MaterialName").value);
        formData.append("VariantName", document.getElementById("VariantName").value);
        formData.append("Description", document.getElementById("Description").value);
        formData.append("Style", document.getElementById("Style").value);
        formData.append("Origin", document.getElementById("Origin").value);
        formData.append("SKU_v2", document.getElementById("SKU_v2").value);
        var fileInput = document.getElementById('ImagePaths');
        var files = fileInput.files;

        // Kiểm tra nếu người dùng đã chọn file
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                formData.append('ImagePaths', file);
            }
        }

        var inputs = editForm.querySelectorAll('input, select'); 
        inputs.forEach(function (input) {
            var initialValue = input.getAttribute('data-initial-value');
            var currentValue = input.value;

            // Compare current value with initial value
            if (currentValue !== initialValue) {
                formData.append(input.name, currentValue);
            }
        });
        Swal.fire({
            title: 'Xác nhận',
            text: 'Bạn có chắc muốn lưu thay đổi?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Lưu',
            cancelButtonText: 'Hủy'
        }).then(function (result) {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Đang xử lý...',
                    allowOutsideClick: false,
                    onBeforeOpen: function () {
                        Swal.showLoading();
                    }
                });

                var xhr = new XMLHttpRequest();
                xhr.open('PUT', 'https://localhost:7108/api/Variants/Update/' + idvariant.value, true);
                //xhr.setRequestHeader('Content-Type', 'multipart/form-data');

                xhr.onload = function () {
                    if (xhr.status === 200) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Thành công!',
                            text: 'Dữ liệu đã được cập nhật thành công.'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = 'variantlist';
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Lỗi!',
                            text: 'Đã có lỗi xảy ra khi cập nhật dữ liệu.'
                        });
                        console.log(xhr.responseText);
                    }
                };

                xhr.onerror = function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi!',
                        text: 'Không thể gửi yêu cầu đến server.'
                    });
                };

                xhr.send(formData);
            }
        });
    });
});