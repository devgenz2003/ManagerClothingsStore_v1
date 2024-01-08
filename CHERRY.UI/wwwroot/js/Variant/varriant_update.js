document.addEventListener('DOMContentLoaded', function () {
    var editForm = document.getElementById('edit_form');
    var idvariant = document.getElementById('idvariant');
    var editSubmit = document.getElementById('edit_submit');

    editForm.addEventListener('submit', function (event) {
        event.preventDefault();

        var formData = new FormData(editForm);

        // Thêm các trường dữ liệu vào FormData
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
                    html: '<div class="progress"><div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div>',
                    showConfirmButton: false,
                    allowOutsideClick: false,
                });

                var xhr = new XMLHttpRequest();
                xhr.open('PUT', 'https://localhost:7108/api/Variants/Update/' + idvariant.value, true);

                xhr.onload = function () {
                    if (xhr.status === 200) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Thành công!',
                            text: 'Dữ liệu đã được cập nhật thành công.'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = '/variantlist';
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

    if (editSubmit) {
        editSubmit.addEventListener('click', function (event) {
            editForm.dispatchEvent(new Event('submit'));
        });
    }
});
