document.addEventListener("DOMContentLoaded", function () {
    var form = document.getElementById('registerForm');
    form.addEventListener('submit', function (event) {
        event.preventDefault();

        Swal.fire({
            title: 'Xác nhận đăng ký?',
            text: "Vui lòng xác nhận để tiếp tục!",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đăng ký',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'Đang xử lý...',
                    html: 'Vui lòng chờ...',
                    allowOutsideClick: false,
                    onBeforeOpen: () => {
                        Swal.showLoading()
                    },
                });

                var formData = {
                    Username: document.getElementById('username').value,
                    Email: document.getElementById('email').value,
                    SurName: document.getElementById('surname').value,
                    MiddleName: document.getElementById('middlename').value,
                    FirstName: document.getElementById('name').value,
                    PhoneNumber: document.getElementById('phoneNumber').value,
                    Password: document.getElementById('password').value,
                    ConfirmPassword: document.getElementById('confirmPassword').value,
                };
                console.log(formData);
                var queryString = `?role=Client`;
                fetch('https://localhost:7108/api/Register' + queryString, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(formData),
                })
                    .then(response => {
                        if (response.ok) {
                            return response.json();
                        }
                        throw new Error('Something went wrong'); // Xử lý lỗi
                    })
                    .then(data => {
                        Swal.fire({
                            title: 'Đăng ký thành công!',
                            icon: 'success',
                            confirmButtonText: 'OK'
                        }).then(() => {
                            window.location.href = "/Home/Index"; // This should redirect the user
                        });
                    })
                    .catch((error) => {
                        Swal.fire(
                            'Lỗi!',
                            'Đã có lỗi xảy ra: ' + error.message,
                            'error'
                        );
                    });
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    // Lắng nghe sự kiện click trên icon mắt để toggle mật khẩu
    document.querySelectorAll('.toggle-password').forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            // Xác định trường mật khẩu liên quan
            var passwordField = this.previousElementSibling;

            // Toggle giữa hiển thị và ẩn mật khẩu
            if (passwordField.type === 'password') {
                passwordField.type = 'text';
                this.innerHTML = '<i class="fa fa-eye"></i>';
            } else {
                passwordField.type = 'password';
                this.innerHTML = '<i class="fa fa-eye-slash"></i>';
            }
        });
    });
});