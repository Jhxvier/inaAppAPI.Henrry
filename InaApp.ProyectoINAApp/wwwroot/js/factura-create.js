(() => {
    "use strict";

    const escalaMonetaria = 10000n;
    const porcentajeImpuesto = 1300n;
    const detallesIniciales = document.getElementById("factura-detalles-iniciales");
    const cuerpoDetalles = document.getElementById("detalles");
    const descuentoInput = document.getElementById("Descuento");
    let detalles = detallesIniciales ? JSON.parse(detallesIniciales.textContent) : [];

    function decimalAEntero(valor) {
        if (typeof valor === "bigint") {
            return valor;
        }

        const texto = String(valor ?? "0").trim().replace(",", ".");
        const esNegativo = texto.startsWith("-");
        const sinSigno = texto.replace(/^[+-]/, "");
        const [entero = "0", fraccion = ""] = sinSigno.split(".");
        const fraccionNormalizada = `${fraccion}0000`.slice(0, 4);
        const resultado = BigInt(entero || "0") * escalaMonetaria + BigInt(fraccionNormalizada);
        return esNegativo ? -resultado : resultado;
    }

    function formatoMoneda(valor) {
        const esNegativo = valor < 0n;
        const absoluto = esNegativo ? -valor : valor;
        const centavosRedondeados = (absoluto + 50n) / 100n;
        const entero = centavosRedondeados / 100n;
        const fraccion = (centavosRedondeados % 100n).toString().padStart(2, "0");
        return `${esNegativo ? "-" : ""}${entero}.${fraccion}`;
    }

    function escaparHtml(valor) {
        const elemento = document.createElement("div");
        elemento.textContent = valor;
        return elemento.innerHTML;
    }

    function calcularLinea(detalle) {
        detalle.precioUnitario = decimalAEntero(detalle.precioUnitario);
        detalle.subtotal = BigInt(detalle.cantidad) * detalle.precioUnitario;
        detalle.impuesto = detalle.subtotal * porcentajeImpuesto / escalaMonetaria;
        detalle.totalLinea = detalle.subtotal + detalle.impuesto;
    }

    function pintarDetalles() {
        cuerpoDetalles.innerHTML = detalles.map((detalle, indice) => `
            <tr>
                <td>
                    ${escaparHtml(detalle.producto)}
                    <input type="hidden" name="Detalles[${indice}].ProductoId" value="${detalle.productoId}" />
                    <input type="hidden" name="Detalles[${indice}].Producto" value="${escaparHtml(detalle.producto)}" />
                    <input type="hidden" name="Detalles[${indice}].Cantidad" value="${detalle.cantidad}" />
                    <input type="hidden" name="Detalles[${indice}].PrecioUnitario" value="${formatoDecimal(detalle.precioUnitario)}" />
                </td>
                <td>${detalle.cantidad}</td>
                <td>₡${formatoMoneda(detalle.precioUnitario)}</td>
                <td>₡${formatoMoneda(detalle.subtotal)}</td>
                <td>₡${formatoMoneda(detalle.impuesto)}</td>
                <td>₡${formatoMoneda(detalle.totalLinea)}</td>
                <td class="text-center">
                    <button type="button" class="btn btn-danger btn-sm" data-indice="${indice}">Eliminar</button>
                </td>
            </tr>`).join("");

        const subtotal = detalles.reduce((total, detalle) => total + detalle.subtotal, 0n);
        const impuesto = detalles.reduce((total, detalle) => total + detalle.impuesto, 0n);
        const descuento = decimalAEntero(descuentoInput.value);

        document.getElementById("subtotal").textContent = formatoMoneda(subtotal);
        document.getElementById("impuesto").textContent = formatoMoneda(impuesto);
        document.getElementById("total").textContent = formatoMoneda(subtotal + impuesto - descuento);
    }

    function formatoDecimal(valor) {
        const esNegativo = valor < 0n;
        const absoluto = esNegativo ? -valor : valor;
        const entero = absoluto / escalaMonetaria;
        const fraccion = (absoluto % escalaMonetaria).toString().padStart(4, "0");
        return `${esNegativo ? "-" : ""}${entero}.${fraccion}`;
    }

    document.getElementById("agregar").addEventListener("click", () => {
        const productoSeleccionado = document.getElementById("producto").selectedOptions[0];
        const cantidad = Number(document.getElementById("cantidad").value);

        if (!productoSeleccionado.value || cantidad < 1) {
            alert("Seleccione un producto e indique una cantidad válida.");
            return;
        }

        const stock = Number(productoSeleccionado.dataset.stock);
        const productoYaAgregado = detalles.some(detalle => detalle.productoId === Number(productoSeleccionado.value));

        if (cantidad > stock || productoYaAgregado) {
            alert("No hay stock suficiente o el producto ya fue agregado.");
            return;
        }

        const detalle = {
            productoId: Number(productoSeleccionado.value),
            producto: productoSeleccionado.dataset.nombre,
            cantidad,
            precioUnitario: decimalAEntero(productoSeleccionado.dataset.precio)
        };

        calcularLinea(detalle);
        detalles.push(detalle);
        document.getElementById("cantidad").value = "";
        document.getElementById("producto").value = "";
        pintarDetalles();
    });

    cuerpoDetalles.addEventListener("click", event => {
        const botonEliminar = event.target.closest("button[data-indice]");
        if (!botonEliminar) {
            return;
        }

        detalles.splice(Number(botonEliminar.dataset.indice), 1);
        pintarDetalles();
    });

    descuentoInput.addEventListener("input", pintarDetalles);
    detalles.forEach(calcularLinea);
    pintarDetalles();
})();
