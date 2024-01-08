document.addEventListener("DOMContentLoaded", function () {
    function updateInputField(selectElementId, inputElementId) {
        var selectElement = document.getElementById(selectElementId);
        selectElement.addEventListener('change', function () {
            var selectedOptionText = this.options[this.selectedIndex].text;
            var inputElement = document.getElementById(inputElementId);
            inputElement.value = selectedOptionText;
        });
    }

    updateInputField('IDSizes', 'SizesName');
    updateInputField('IDColor', 'ColorName');

    var form = document.getElementById('options_update');
    var idoptions = document.getElementById('idoptions').value;

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Xác nhận gửi dữ liệu?',
            text: "Bạn xác nhận chỉnh sửa thông tin phân loại này chứ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Xác nhận!'
        }).then((result) => {
            if (result.isConfirmed) {
                sendData();
            }
        });
    });

    function sendData() {
        var formData = new FormData();
        formData.append("CostPrice", document.getElementById("CostPrice").value);
        formData.append("RetailPrice", document.getElementById("RetailPrice").value);
        formData.append("DiscountedPrice", document.getElementById("DiscountedPrice").value);
        formData.append("StockQuantity", document.getElementById("StockQuantity").value);
        formData.append("IDColor", document.getElementById("IDColor").value);
        formData.append("ColorName", document.getElementById("ColorName").value);
        formData.append("IDSizes", document.getElementById("IDSizes").value);
        formData.append("SizesName", document.getElementById("SizesName").value);
        formData.append("ModifieBy", document.getElementById("IDUser").value);
        formData.append("IDVariant", document.getElementById("IDVariant").value);
        var statusSelect = document.getElementById('Status');
        var selectedStatus = statusSelect.value;

        formData.append("Status", selectedStatus);
        const fileInput = document.getElementById('ImagePaths');
        for (const file of fileInput.files) {
            formData.append('ImageURL', file);
        }

        var xhr = new XMLHttpRequest();

        xhr.onloadstart = function () {
            Swal.fire({
                title: 'Đang xử lý...',
                html: '<div class="progress"><div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div>',
                showConfirmButton: false,
                allowOutsideClick: false,
            });
        };

        xhr.open('PUT', 'https://localhost:7108/api/Options/Update/' + idoptions, true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                console.log(xhr.status);

                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: 'Dữ liệu đã được cập nhật thành công.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '/optionlist';
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
