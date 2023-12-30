document.addEventListener('DOMContentLoaded', function () {
    const checkboxes = document.querySelectorAll('.onoffswitch-checkbox');

    checkboxes.forEach((checkbox) => {
        checkbox.addEventListener('change', async function () {
            const userID = this.getAttribute('data-userid'); // Lấy IDUser từ thuộc tính data
            const newRole = this.checked ? 'Admin' : 'Client'; // Xác định vai trò mới dựa trên trạng thái của công tắc
            await changeUserRole(userID, newRole); // Gọi hàm để thay đổi vai trò
        });
    });
});


async function changeUserRole(IDUser, newRole) {
    try {
        const url = `https://localhost:7108/api/User/${IDUser}/changerole`;
        const requestOptions = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ NewRole: newRole }),
        };

        const response = await fetch(url, requestOptions);

        if (response.ok) {
            const element = document.getElementById(IDUser);
            if (newRole === 'Client') {
                element.classList.add('client');
                element.classList.remove('admin');
            } else if (newRole === 'Admin') {
                element.classList.add('admin');
                element.classList.remove('client');
            }
        } else {
        }
    } catch (error) {
    }
}

