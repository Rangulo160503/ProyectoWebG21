document.addEventListener("DOMContentLoaded", () => {
    const horarios = [
        { texto: "Lunes y Miércoles 6pm-8pm", cupos: 3 },
        { texto: "Martes y Jueves 4pm-6pm", cupos: 2 },
        { texto: "Sábados 9am-1pm", cupos: 1 }
    ];

    const listaHorarios = document.getElementById("listaHorarios");
    const btnConfirmar = document.getElementById("btnConfirmar");
    let cursoSeleccionado = "";

    document.querySelectorAll(".btnMatricular").forEach(btn => {
        btn.addEventListener("click", () => {
            cursoSeleccionado = btn.getAttribute("data-nombre");
            listaHorarios.innerHTML = "";
            horarios.forEach((h, i) => {
                listaHorarios.innerHTML += `
                    <div class="form-check">
                        <input class="form-check-input" type="radio" name="horario" id="hor${i}" value="${h.texto}">
                        <label class="form-check-label" for="hor${i}">
                            ${h.texto} (Cupos: ${h.cupos})
                        </label>
                    </div>`;
            });

            const modal = new bootstrap.Modal(document.getElementById("modalMatricular"));
            modal.show();
        });
    });

    btnConfirmar.addEventListener("click", () => {
        const seleccionado = document.querySelector("input[name='horario']:checked");
        if (!seleccionado) return alert("Selecciona un horario.");
        alert(`✅ Matriculado en "${cursoSeleccionado}" (${seleccionado.value})`);
        bootstrap.Modal.getInstance(document.getElementById("modalMatricular")).hide();
    });
});
