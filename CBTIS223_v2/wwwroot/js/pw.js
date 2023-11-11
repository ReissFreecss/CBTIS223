$(document).ready(function () {
    // Aqu� se implementa la funcionalidad de alternar la visibilidad de la contrase�a.

    // Selecciona el elemento con el ID "showPassword" y adjunta un controlador de eventos de clic.
    $("#showPassword").click(function () {
        // Se Obtiene referencias al campo de entrada de contrase�a y al �cono de ojo dentro del bot�n.
        const pwdInput = $("#pwd");
        const icon = $("#showPassword i");

        // Verifica el tipo actual del campo de entrada de contrase�a.
        if (pwdInput.attr("type") === "password") {
            // Si la contrase�a est� actualmente oculta, cambia el tipo a texto y actualiza el �cono de ojo.
            pwdInput.attr("type", "text");
            icon.removeClass("fas fa-eye-slash").addClass("fas fa-eye");
        } else {
            // Si la contrase�a est� actualmente visible, cambia el tipo de nuevo a contrase�a y actualiza el �cono de ojo.
            pwdInput.attr("type", "password");
            icon.removeClass("fas fa-eye").addClass("fas fa-eye-slash");
        }
    });
});