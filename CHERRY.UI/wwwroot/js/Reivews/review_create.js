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
                    title: 'Đang gửi...',
                    text: 'Vui lòng đợi...',
                    onBeforeOpen: () => {
                        Swal.showLoading();
                    },
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    allowEnterKey: false
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
                Swal.fire('Thành công!', 'Dữ liệu đã được gửi thành công.', 'success').then((result) => {
                    window.location.href = '/review_list';
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
