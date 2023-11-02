// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(".menu > ul > li").click(function (e) {
    // remove active from already active
    $(this).siblings().removeClass("active");
    // add active to clicked
    $(this).toggleClass("active");
    // if has sub menu open it
    $(this).find("ul").slideToggle();
    // close other sub menu if any open
    $(this).siblings().find("ul").slideUp();
    // remove active class of sub menu items
    $(this).siblings().find("ul").find("li").removeClass("active");
    e.stopPropagation();
});

$(".menu-btn").click(function () {
    $(".sidebar").toggleClass("active");
});
$(".menu .sub-menu li").click(function (e) {
    // Xử lý sự kiện khi menu con cấp 3 được chọn
    // Ví dụ: Cập nhật lớp "active" cho menu con cấp 3
    $(this).siblings().removeClass("active");
    $(this).toggleClass("active");
    e.stopPropagation();
});


// Tạo vùng checkbox
const checkbox = document.getElementById('disable-checkbox');
const content = document.querySelector('.content');
const inputs = content.querySelectorAll('input');

checkbox.addEventListener('change', function () {
    inputs.forEach(input => {
        input.disabled = checkbox.checked;
    });

    if (checkbox.checked) {
        content.classList.add('disabled');
    } else {
        content.classList.remove('disabled');
    }
});





