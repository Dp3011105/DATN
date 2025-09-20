function validateEmail(input) {
    const errorElement = document.getElementById('emailError');
    const value = input.value.trim();
    // Sử dụng RegExp constructor để tránh xung đột với @
    const emailPattern = new RegExp('^[a-zA-Z0-9._%+-]+' + '\\@' + '[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$');

    if (!emailPattern.test(value)) {
        errorElement.style.display = 'block';
        input.setCustomValidity('Email không hợp lệ');
    } else {
        errorElement.style.display = 'none';
        input.setCustomValidity('');
    }
    // Gọi hàm updateSubmitButtonState từ file JavaScript chính (nếu có)
    if (typeof updateSubmitButtonState === 'function') {
        updateSubmitButtonState();
    }
}