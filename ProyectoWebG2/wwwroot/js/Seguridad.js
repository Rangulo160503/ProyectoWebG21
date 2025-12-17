$(function () {
    $("#FormSeguridad").validate({
        rules: {
            Contrasenna: {
                required: true,
                maxlength: 10
            },
            ContrasennaConfirmar: {
                required: true,
                maxlength: 10,
                equalTo: "#Contrasenna"
            }
        },
        messages: {
            Contrasenna: {
                required: "Requerido",
                maxlength: "Máximo 10 caracteres"
            },
            ContrasennaConfirmar: {
                required: "Requerido",
                maxlength: "Máximo 10 caracteres",
                equalTo: "Las confirmación no coincide"
            }
        }
    });
});