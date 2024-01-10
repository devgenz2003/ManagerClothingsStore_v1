const backEndConfig = {
    "BackEndAPIURL": "https://localhost:7299",
};

$(document).ready(function () {
    var token = getCookie("token"); 

    var html;
    if (token == null) {
        html = `
                                <li><a href="/Home/Index"><i class="icon fa fa-home"></i>Home</a></li>
                                <li><a href="/Category/Index"><i class="icon fa fa-heart"></i>Danh mục</a></li>
                                                                <li><a href="/CartIndex"><i class="icon fa fa-shopping-cart"></i>Giỏ hàng</a></li>
								<li><a href="/Login"><i class="icon fa fa-lock"></i>Login</a></li>
        `;
    } else {
        var tokenData = parseJwt(token);

        var tokenExpiration = tokenData.exp * 1000;
        var currentTimestamp = new Date().getTime();
        if (currentTimestamp >= tokenExpiration) {
            removeCookie("token");
            window.location.href = "/Login";
            return;
        }

        html = `
                                <li><a href="/Home/Index"><i class="icon fa fa-home"></i>Home</a></li>
                                <li><a href="/order_list"><i class="icon fa fa-heart"></i>Đơn hàng</a></li>
                                 <li><a href="/Category/Index"><i class="icon fa fa-heart"></i>Danh mục</a></li>
                                <li><a href="/CartIndex"><i class="icon fa fa-shopping-cart"></i>Giỏ hàng</a></li>
                                <li><a href="/Account/Account"><i class="icon fa fa-user"></i>Tài khoản</a></li>
                                <li><a href="#" id="logout"><i class="icon fa fa-check"></i>Đăng xuất</a></li>
        `;
    }

    $("#nav_top").append(html);

    $(document).on('click', '#logout', function (e) {
        e.preventDefault();
        removeCookie('token'); 
        window.location.href = '/Login';
    });
});

// Hàm lấy giá trị từ cookie
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (const cookie of cookies) {
        const [cookieName, cookieValue] = cookie.trim().split('=');
        if (cookieName === name) {
            return cookieValue;
        }
    }
    return null;
}

function removeCookie(name) {
    document.cookie = `${name}=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;`;
}

function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
}