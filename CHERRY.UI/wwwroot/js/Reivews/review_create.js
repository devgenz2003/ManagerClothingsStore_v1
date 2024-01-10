document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.star').forEach(star => {
        star.addEventListener('click', function () {
            var rating = this.getAttribute('data-rating');
            document.getElementById('rating-value').value = rating;

            var stars = document.querySelectorAll('.star');
            stars.forEach(st => {
                st.classList.remove('selected');
                if (st.getAttribute('data-rating') <= rating) {
                    st.classList.add('selected');
                }
            });

            var ratingLabel = document.getElementById('rating-label');
            switch (rating) {
                case '1': ratingLabel.textContent = 'Tệ'; break;
                case '2': ratingLabel.textContent = 'Tạm được'; break;
                case '3': ratingLabel.textContent = 'Bình thường'; break;
                case '4': ratingLabel.textContent = 'Tốt'; break;
                case '5': ratingLabel.textContent = 'Tuyệt vời'; break;
                default: ratingLabel.textContent = '';
            }
        });
    });

    var form = document.querySelector('#review-form');
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
        xhr.open('POST', 'https://localhost:7108/api/Review/create');

        xhr.onload = function () {

            document.getElementById('loadingSpinner').style.display = 'none';

            if (xhr.status === 200) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Dữ liệu đã được gửi thành công.',
                    icon: 'success',
                    timer: 5000, // Thời gian chờ trước khi tự động chuyển hướng (tính bằng mili giây, 5000 mili giây = 5 giây)
                    showConfirmButton: false // Ẩn nút OK
                }).then(() => {
                    window.location.href = '/Home/Index'; // Chuyển hướng sau khi thông báo biến mất
                });
            } else {
                console.error("Error:", xhr.statusText);
                Swal.fire('Lỗi!', 'Có lỗi xảy ra khi gửi dữ liệu. Mã lỗi: ' + xhr.status + '. ' + xhr.responseText, 'error');
            }
        };
        xhr.onerror = function () {
            console.error("Request failed");
            Swal.fire('Lỗi!', 'Không thể gửi đánh giá. Vui lòng thử lại sau.', 'error');
        };

        xhr.send(formData);
    }
});
