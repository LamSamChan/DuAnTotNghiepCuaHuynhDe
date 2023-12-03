$(document).ready(function () {
    var isFirstRun = true; // Biến để kiểm tra lần đầu chạy

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
        // Save active menu item to sessionStorage
        sessionStorage.setItem("activeMenuItem", $(this).index().toString());

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

    var activeMenuItemIndex = sessionStorage.getItem("activeMenuItem");
    if (activeMenuItemIndex !== null && isFirstRun) {
        // Remove the "active" class from all menu items
        $(".menu > ul > li").removeClass("active");
        // Remove the "active" class from all sub-menu items
        $(".menu .sub-menu li").removeClass("active");

        // Add the "active" class only to the specified menu item
        $(".menu > ul > li").eq(parseInt(activeMenuItemIndex)).addClass("active");
        $(".menu > ul > li").eq(parseInt(activeMenuItemIndex)).find("ul").slideDown();

        isFirstRun = false; // Đánh dấu đã chạy lần đầu
    }
});
