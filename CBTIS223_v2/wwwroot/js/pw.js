$(document).ready(function () {
    // Aquí se implementa la funcionalidad de alternar la visibilidad de la contraseña.

    // Selecciona el elemento con el ID "showPassword" y adjunta un controlador de eventos de clic.
    $("#showPassword").click(function () {
        // Se Obtiene referencias al campo de entrada de contraseña y al ícono de ojo dentro del botón.
        const pwdInput = $("#pwd");
        const icon = $("#showPassword i");

        // Verifica el tipo actual del campo de entrada de contraseña.
        if (pwdInput.attr("type") === "password") {
            // Si la contraseña está actualmente oculta, cambia el tipo a texto y actualiza el ícono de ojo.
            pwdInput.attr("type", "text");
            icon.removeClass("fas fa-eye-slash").addClass("fas fa-eye");
        } else {
            // Si la contraseña está actualmente visible, cambia el tipo de nuevo a contraseña y actualiza el ícono de ojo.
            pwdInput.attr("type", "password");
            icon.removeClass("fas fa-eye").addClass("fas fa-eye-slash");
        }
    });
});