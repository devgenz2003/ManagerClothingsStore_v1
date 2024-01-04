document.getElementById('paymentButton').addEventListener('click', function () {
    Swal.fire({
        title: 'Bạn có chắc không?',
        text: "Bạn có muốn tiếp tục thanh toán không?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Có, tiếp tục!'
    }).then((result) => {
        if (result.isConfirmed) {
            var selectedProducts = [];
            document.querySelectorAll("input[name='selectedProducts']:checked").forEach(function (checkbox) {
                var row = checkbox.closest('tr');
                var productID = checkbox.value;
                var quantity = row.querySelector('.quant-input input').value;
                var productName = row.querySelector('.cart-product-name-info .cart-product-description a').textContent;
                var colorName = row.querySelector('.product-color span').textContent;
                var sizeName = row.querySelector('.product-color span').textContent;
                var unitPriceElement = row.querySelector('.cart-product-sub-total .cart-sub-total-price');
                var unitPrice = unitPriceElement.textContent.trim();

                var discountedElement = row.querySelector('.cart-product-discounted-total .cart-grand-total-price');
                var discounted = discountedElement.textContent.trim();

                var totalPriceElement = row.querySelector('.cart-product-grand-total .cart-grand-total-price');
                var totalPrice = totalPriceElement.textContent.trim();

                selectedProducts.push({
                    IDOptions: productID,
                    ProductName: productName,
                    ColorName: colorName,
                    SizeName: sizeName,
                    Quantity: quantity,
                    TotalPrice: totalPrice,
                    DiscountedPrice: discounted,
                    UnitPrice: unitPrice
                });
            });

            sessionStorage.setItem('selectedProducts', JSON.stringify(selectedProducts));
            window.location.href = "/checkout";
        }
    });
});

function checkSelectedProducts() {
    return Array.from(document.getElementsByName("selectedProducts")).some(checkbox => checkbox.checked);
}
function updatePaymentButton() {
    var paymentButton = document.getElementById("paymentButton");
    paymentButton.disabled = !checkSelectedProducts();
}
window.onload = updatePaymentButton;
document.querySelectorAll('input[name="selectedProducts"]').forEach(input => input.addEventListener('change', updatePaymentButton));
