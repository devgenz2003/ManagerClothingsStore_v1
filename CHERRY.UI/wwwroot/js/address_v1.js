// Lấy thẻ select tỉnh/thành phố
var provinceSelect = document.getElementById("provinceSelect");

// Lấy thẻ select quận/huyện
var districtSelect = document.getElementById("districtSelect");

// Lấy thẻ select phường/xã
var wardSelect = document.getElementById("wardSelect");

function loadProvinces() {
    fetch('https://provinces.open-api.vn/api/?depth=1')
        .then(response => response.json())
        .then(data => {
            data.forEach(province => {
                var option = document.createElement("option");
                option.value = province.code;
                option.text = province.name;
                provinceSelect.appendChild(option);
            });
        })
        .catch(error => console.error('Error:', error));
}

// Hàm nạp quận/huyện từ API dựa trên tỉnh/thành phố đã chọn
function loadDistricts() {
    var selectedProvinceCode = provinceSelect.value;
    districtSelect.innerHTML = "<option value=''>Chọn quận/huyện</option>";
    wardSelect.innerHTML = "<option value=''>Chọn phường/xã</option>"; // Reset ward select box

    if (selectedProvinceCode) {
        fetch(`https://provinces.open-api.vn/api/p/${selectedProvinceCode}?depth=2`)
            .then(response => response.json())
            .then(province => {
                province.districts.forEach(district => {
                    var option = document.createElement("option");
                    option.value = district.code;
                    option.text = district.name;
                    districtSelect.appendChild(option);
                });
                // Luôn hiển thị select quận/huyện
            })
            .catch(error => console.error('Error:', error));
    }
}

// Hàm nạp phường/xã từ API dựa trên quận/huyện đã chọn
function loadWards() {
    var selectedDistrictCode = districtSelect.value;
    wardSelect.innerHTML = "<option value=''>Chọn phường/xã</option>";

    if (selectedDistrictCode) {
        fetch(`https://provinces.open-api.vn/api/d/${selectedDistrictCode}?depth=2`)
            .then(response => response.json())
            .then(district => {
                district.wards.forEach(ward => {
                    var option = document.createElement("option");
                    option.value = ward.code;
                    option.text = ward.name;
                    wardSelect.appendChild(option);
                });
                // Luôn hiển thị select phường/xã
                wardSelect.style.display = "block";
            })
            .catch(error => console.error('Error:', error));
    }
}


// Cập nhật địa chỉ giao hàng
function updateShippingAddressDetails() {
    var provinceText = provinceSelect.options[provinceSelect.selectedIndex]?.text || '';
    var districtText = districtSelect.options[districtSelect.selectedIndex]?.text || '';
    var wardText = wardSelect.options[wardSelect.selectedIndex]?.text || '';

    var shippingAddress = [wardText, districtText, provinceText].filter(Boolean).join(', ');
    document.getElementById('shippingAddress').value = shippingAddress;
}

// Sự kiện khi thay đổi lựa chọn
provinceSelect.addEventListener('change', function () {
    loadDistricts();
    updateShippingAddressDetails();
});

districtSelect.addEventListener('change', function () {
    loadWards();
    updateShippingAddressDetails();
});

wardSelect.addEventListener('change', updateShippingAddressDetails);

// Gọi hàm nạp tỉnh/thành phố khi trang tải xong
document.addEventListener('DOMContentLoaded', function () {
    loadProvinces();
});