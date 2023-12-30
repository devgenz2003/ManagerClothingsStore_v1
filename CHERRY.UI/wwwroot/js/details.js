document.getElementById('add-to-cart-button').addEventListener('click', function (e) {
    e.preventDefault();
    Swal.fire({
        title: 'Bạn có chắc không?',
        text: "Bạn muốn thêm sản phẩm này vào giỏ hàng?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Có, thêm vào!',
        cancelButtonText: 'Không, đợi chút!'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById('add-to-cart-form').submit();
        }
    });
});
