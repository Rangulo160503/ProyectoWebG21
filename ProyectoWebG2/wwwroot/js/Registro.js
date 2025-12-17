// Ejecuta cuando el DOM está listo
$(function () {
    const $form = $("#FormRegistro");
    const $btn = $("#BtnCrearCuenta");
    const $pwd = $("#Contrasena");
    const $pwd2 = $("#ContrasenaConfirmar");

    // Reglas de validación
    $form.validate({
        // Importante: los names deben coincidir con asp-for del formulario
        rules: {
            Cedula: { required: true, minlength: 7 },
            Nombre: { required: true, minlength: 2 },
            Apellidos: { required: true, minlength: 2 },
            Telefono: { required: true, digits: true, minlength: 8, maxlength: 20 },
            CorreoElectronico: { required: true, email: true },
            Contrasena: { required: true, minlength: 6 },
            ContrasenaConfirmar: { required: true, equalTo: "#Contrasena" }
        },
        messages: {
            Cedula: { required: "Requerido", minlength: "Mínimo 7 dígitos" },
            Nombre: { required: "Requerido", minlength: "Mínimo 2 caracteres" },
            Apellidos: { required: "Requerido", minlength: "Mínimo 2 caracteres" },
            Telefono: { required: "Requerido", digits: "Solo números", minlength: "Mínimo 8", maxlength: "Máximo 20" },
            CorreoElectronico: { required: "Requerido", email: "Formato inválido" },
            Contrasena: { required: "Requerido", minlength: "Mínimo 6 caracteres" },
            ContrasenaConfirmar: { required: "Requerido", equalTo: "No coincide" }
        },
        errorClass: "text-danger",
        validClass: "is-valid",
        errorPlacement: function (error, element) {
            // deja que unobtrusive ponga el <span asp-validation-for>
            error.insertAfter(element);
        }
    });

    // Evitar doble submit y mostrar estado
    $form.on("submit", function () {
        if (!$form.valid()) return false;

        $btn.prop("disabled", true).data("txt", $btn.text()).text("Procesando...");
    });

    // Opcional: desbloquear el botón si el usuario corrige (por si hubo error de servidor)
    $form.on("input change", function () {
        if ($btn.is(":disabled") && $form.valid()) {
            $btn.prop("disabled", false).text($btn.data("txt") || "Crear cuenta");
        }
    });

    // (Opcional) Autollenar nombre por cédula si usas algún servicio externo
    // $("#Cedula").on("keyup blur", function () {
    //   const v = this.value.trim();
    //   if (v.length >= 9) {
    //     $.getJSON("https://apis.gometa.org/cedulas/" + encodeURIComponent(v))
    //       .done(function (data) {
    //         if (data && data.nombre && !$("#Nombre").val()) $("#Nombre").val(data.nombre).trigger("change");
    //       });
    //   }
    // });
});
