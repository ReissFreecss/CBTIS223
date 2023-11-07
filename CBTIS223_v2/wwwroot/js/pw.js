$(document).ready(function () {
        // Toggle password visibility
        $("#showPassword").click(function () {
            const pwdInput = $("#pwd");
            const icon = $("#showPassword i");

            if (pwdInput.attr("type") === "password") {
                pwdInput.attr("type", "text");
                icon.removeClass("fas fa-eye-slash").addClass("fas fa-eye");
            } else {
                pwdInput.attr("type", "password");
                icon.removeClass("fas fa-eye").addClass("fas fa-eye-slash");
            }
        });
    });