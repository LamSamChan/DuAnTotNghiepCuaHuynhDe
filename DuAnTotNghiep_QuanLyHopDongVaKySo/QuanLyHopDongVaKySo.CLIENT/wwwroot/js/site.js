$(document).ready(function () {
    // Handle menu button click
    $(".menu-btn").click(function () {
        $(".sidebar").toggleClass("active");
    });

    // Handle menu item click
    $(".menu > ul > li").click(function (e) {
        // Remove active from already active
        $(".menu > ul > li.active").removeClass("active");
        // Add active to clicked
        $(this).addClass("active");

        // Toggle the sub-menu
        $(this).find("ul").slideToggle();
        // Close other sub menus if any are open
        $(this).siblings().find("ul").slideUp();
        // Remove active class from sub-menu items
        $(this).siblings().find("ul").find("li").removeClass("active");
        // Save active menu item to localStorage
        localStorage.setItem("activeMenuItem", $(this).index().toString());

        e.stopPropagation();
    });

    // Handle sub-menu item click
    $(".menu .sub-menu li").click(function (e) {
        // Xử lý sự kiện khi menu con cấp 3 được chọn
        // Ví dụ: Cập nhật lớp "active" cho menu con cấp 3
        $(this).siblings().removeClass("active");
        $(this).toggleClass("active");
        e.stopPropagation();
    });


    var activeMenuItemIndex = localStorage.getItem("activeMenuItem");
    if (activeMenuItemIndex !== null) {
        $(".menu > ul > li").eq(parseInt(activeMenuItemIndex)).addClass("active");
        $(".menu > ul > li").eq(parseInt(activeMenuItemIndex)).find("ul").slideDown();
    }
});
