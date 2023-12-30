const config = {
    "BackEndAPIURL": "https://localhost:7108",
};

function setCookie(name, value, days) {
    var expires = new Date();
    expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
    document.cookie = name + "=" + value + ";expires=" + expires.toUTCString() + ";path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

document.addEventListener("DOMContentLoaded", function () {
    var form = document.querySelector('.register-form');
    form.addEventListener('submit', handleLogin);
});

function handleLogin(event) {
    event.preventDefault();
    var username = document.getElementById("signInEmail").value;
    var password = document.getElementById("signInPassword").value;

    if (!username || !password) {
        Swal.fire('Thông báo!', 'Vui lòng nhập đầy đủ tài khoản và mật khẩu.', 'warning');
        return;
    }

    Swal.fire({
        title: 'Đang xử lý...',
        html: '<div class="progress"><div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%"></div></div>',
        showConfirmButton: false, 
        allowOutsideClick: false, 
    });

    $.ajax({
        url: config.BackEndAPIURL + '/api/Login',
        type: 'POST',
        data: JSON.stringify({ UserName: username, PassWord: password }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response && response.token) {
                setCookie('token', response.token, 7);
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Đăng nhập thành công.',
                    icon: 'success',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (response.role && response.role === 'Admin') {
                        window.location.href = '/Admin';
                    } else {
                        window.location.href = '/Home/Index';
                    }
                });
            } else {
                Swal.fire('Lỗi!', 'Đăng nhập không thành công. Vui lòng kiểm tra lại thông tin.', 'error');
            }
        },
        error: function () {
            Swal.fire('Lỗi!', 'Đã có lỗi xảy ra khi đăng nhập. Vui lòng thử lại.', 'error');
        }
    });
}

