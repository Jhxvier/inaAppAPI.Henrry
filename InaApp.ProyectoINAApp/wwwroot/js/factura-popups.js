
/*esto es util para los popups de facturas ya que permiten filtrar los resultados en tiempo real */

document.querySelectorAll(".filtro-popup").forEach((filtro) => {
    filtro.addEventListener("input", () => {
        const tabla = document.getElementById(filtro.dataset.tabla);
        const filtros = document.querySelectorAll(
            "[data-tabla=\"" + filtro.dataset.tabla + "\"]");

        tabla.querySelectorAll("tr").forEach((fila) => {
            fila.hidden = [...filtros].some((campo) =>
                !fila.dataset[campo.dataset.campo].includes(
                    campo.value.trim().toLowerCase()));
        });
    });
});
