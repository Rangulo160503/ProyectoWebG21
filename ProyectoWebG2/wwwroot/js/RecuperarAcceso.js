$(function () {
    $("#FormRecuperarAcceso").validate({
        rules: {
            CorreoElectronico: {
                required: true
            }
        },
        messages: {
            CorreoElectronico: {
                required: "Requerido"
            }
        }
    });
});
