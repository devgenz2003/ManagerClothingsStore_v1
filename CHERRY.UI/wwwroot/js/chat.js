document.addEventListener("DOMContentLoaded", function () {
    var chatButton = document.getElementById("chatButton");
    var chatView = document.getElementById("chatView");
    var closeButton = document.querySelector('.chat-close-button');

    closeButton.addEventListener("click", function () {
        chatView.style.display = "none";
    })

    var isDragging = false;
    var dragStartTime = 0;

    chatButton.addEventListener("mousedown", function (e) {
        dragStartTime = Date.now();
        isDragging = false;
        var startX = e.clientX - chatButton.offsetLeft;
        var startY = e.clientY - chatButton.offsetTop;

        function onMouseMove(e) {
            isDragging = true;
            chatButton.style.left = e.clientX - startX + 'px';
            chatButton.style.top = e.clientY - startY + 'px';
        }

        function onMouseUp() {
            document.removeEventListener('mousemove', onMouseMove);
            document.removeEventListener('mouseup', onMouseUp);
            if (Date.now() - dragStartTime < 200) {
                isDragging = false;
            }
        }

        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
    });

    chatButton.addEventListener("click", function (e) {
        e.stopPropagation();
        if (!isDragging) {
            chatView.style.display = chatView.style.display === "block" ? "none" : "block";
        }
    });

    document.addEventListener("click", function (e) {
        if (chatView.style.display === "block" && !chatView.contains(e.target) && e.target !== chatButton) {
            chatView.style.display = "none";
        }
    });

    // Điều chỉnh vị trí của khung chat
    function adjustChatViewPosition() {
        var buttonRect = chatButton.getBoundingClientRect();
        var chatViewStyle = chatView.style;
        chatViewStyle.bottom = (window.innerHeight - buttonRect.bottom) + 'px';
        chatViewStyle.right = (window.innerWidth - buttonRect.right) + 'px';
    }

    // Thực hiện điều chỉnh khi chatView được hiển thị
    chatButton.addEventListener("click", adjustChatViewPosition);
});
