let gridInsumos = null;
let insumos = [];

$(document).ready(async function () {
    await cargarCombos();
    configurarDataTable();

    const id = parseInt(localStorage.getItem("ordenCompraId")) || 0;
    $('#idOrden').val(id); // si lo estás usando en el DOM




    if (id > 0) {
        await cargarOrden(id);
        $('#tituloOrden').text('Editar Orden de Compra');
        $('#btnRegistrarModificar').text('Guardar');
    } else {
        const hoy = moment().format('YYYY-MM-DD');
        const fechaEntrega = moment().add(7, 'days').format('YYYY-MM-DD');
        $('#FechaEmision').val(hoy);
        $('#FechaEntrega').val(fechaEntrega);
        $('#tituloOrden').text('Nueva Orden de Compra');
        $('#btnRegistrarModificar').text('Registrar');
    }


    inicializarEventos();
});

async function obtenerInsumosPorProveedor(idProveedor) {
    const res = await fetch(`/Insumos/ListaPorProveedor?IdProveedor=${idProveedor}`);
    const data = await res.json();
    return data.map(x => ({
        Id: x.Id,
        Descripcion: x.Descripcion,
        PrecioLista: x.PrecioLista,
        IdProveedorLista: x.IdProveedorLista
    }));
}


function inicializarEventos() {
    $('#Proveedor, #Local, #UnidadNegocio, #Estado').select2({ width: '100%' });

    $('#Proveedor').on('change', async function () {
        const idProveedor = parseInt($(this).val());
        if (!idProveedor) return;

        insumos = await obtenerInsumosPorProveedor(idProveedor);
    });


    $("#insumoSelect").select2({
        dropdownParent: $("#insumosModal"),
        width: "100%",
        placeholder: "Seleccionar un insumo"
    }).on('change', function () {
        const idSeleccionado = $(this).val();
        const insumo = insumos.find(x => x.Id == idSeleccionado);
        if (insumo) {
            $("#precioInput").val(formatoMoneda.format(insumo.PrecioLista));
            calcularTotalInsumo();
        }
    });

    $('#precioInput, #cantidadInput').on('input', calcularTotalInsumo);
    $('#precioInput').on('blur', function () {
        this.value = formatoMoneda.format(convertirMonedaAFloat(this.value));
    });

    document.querySelectorAll("#UnidadNegocio, #Local, #Proveedor, #Estado, #FechaEmision, #FechaEntrega").forEach(el => {
        el.addEventListener("input", () => validarCampoIndividual(el));
        el.addEventListener("change", () => validarCampoIndividual(el));
        el.addEventListener("blur", () => validarCampoIndividual(el));
    });

    $('#UnidadNegocio, #Local, #Proveedor, #Estado').on('select2:close', function (e) {
        validarCampoIndividual(this);
    });

    $('#UnidadNegocio').on('change', function () {
        const idUnidadNegocio = parseInt($(this).val());
        cargarLocales(idUnidadNegocio);
    });
}


function validarCampoIndividual(el) {
    const valor = el.value.trim();
    const isSelect2 = $(el).hasClass("select2-hidden-accessible");
    const feedback = el.parentElement.querySelector(".invalid-feedback");

    const isInvalid = valor === "" || valor === "Seleccionar";

    if (feedback) {
        feedback.textContent = isInvalid ? "Campo obligatorio" : "";
    }

    if (isInvalid) {
        el.classList.add("is-invalid");
        el.classList.remove("is-valid");
        if (isSelect2) {
            $(el).next('.select2-container').find('.select2-selection').addClass('is-invalid').removeClass('is-valid');
        }
    } else {
        el.classList.remove("is-invalid");
        el.classList.add("is-valid");
        if (isSelect2) {
            $(el).next('.select2-container').find('.select2-selection').removeClass('is-invalid').addClass('is-valid');
        }
    }

    verificarErroresGenerales();
}


function verificarErroresGenerales() {
    const errorMsg = document.getElementById("errorCamposOrden");
    const hayInvalidos = document.querySelectorAll(".is-invalid").length > 0;
    if (errorMsg) errorMsg.classList.toggle("d-none", !hayInvalidos);
}

function validarCampos() {
    const campos = [
        '#UnidadNegocio',
        '#Local',
        '#Proveedor',
        '#Estado',
        '#FechaEmision',
        '#FechaEntrega'
    ];

    let valido = true;
    campos.forEach(id => {
        const el = document.querySelector(id);
        const valor = el?.value.trim();
        const feedback = el?.parentElement.querySelector(".invalid-feedback");

        if (!valor || valor === "Seleccionar") {
            el.classList.add("is-invalid");
            el.classList.remove("is-valid");
            if (feedback) feedback.textContent = "Campo obligatorio";
            valido = false;
        } else {
            el.classList.remove("is-invalid");
            el.classList.add("is-valid");
        }
    });
    verificarErroresGenerales();
    return valido;
}

async function cargarCombos() {
    await cargarCombo('/UnidadesNegocio/Lista', '#UnidadNegocio');
    await cargarCombo('/Locales/Lista', '#Local');
    await cargarCombo('/Proveedores/Lista', '#Proveedor');
    await cargarCombo('/OrdenesComprasEstado/Lista', '#Estado');
}

async function cargarCombo(url, selector) {
    const res = await fetch(url);
    const data = await res.json();

    const $select = $(selector);
    $select.empty();

    $select.append(`<option value="">Seleccionar</option>`);
    data.forEach(x => {
        $select.append(`<option value="${x.Id}">${x.Nombre}</option>`);
    });

    $select.val('').trigger('change');
}


function configurarDataTable() {
    gridInsumos = $('#grd_OrdenCompraProductos').DataTable({
        data: [],
        scrollX: true,
        columns: [
            { data: 'Nombre' },
            { data: 'PrecioLista', render: x => formatoMoneda.format(x) },
            { data: 'CantidadPedida' },
            { data: 'Subtotal', render: x => formatoMoneda.format(x) },
            {
                data: "Id",
                render: function (data, type, row) {
                    return `
                        <button class='btn btn-sm btneditar btnacciones' type='button' onclick='editarProductoOrden(${row.IdInsumo})' title='Editar'>
                            <i class='fa fa-pencil-square-o fa-lg text-white' aria-hidden='true'></i>
                        </button>
                        <button class='btn btn-sm btneditar btnacciones' type='button' onclick='eliminarProductoOrden(${row.IdInsumo})' title='Eliminar'>
                            <i class='fa fa-trash-o fa-lg text-danger' aria-hidden='true'></i>
                        </button>`;
                },
                orderable: false,
                searchable: false
            }
        ]
    });
}

function calcularTotalOrden() {
    let total = 0;
    gridInsumos.rows().every(function () {
        total += parseFloat(this.data().Subtotal);
    });
    $('#CostoTotal').val(formatoMoneda.format(total));
}

async function cargarOrden(id) {
    const res = await fetch(`/OrdenesCompra/EditarInfo?id=${id}`);
    const data = await res.json();

    $('#idOrden').val(data.Id);

    $('#UnidadNegocio').val(data.IdUnidadNegocio).trigger('change');
    await cargarLocales(data.IdUnidadNegocio); // <-- importante
    $('#Local').val(data.IdLocal).trigger('change');

    $('#Proveedor').val(data.IdProveedor).trigger('change');
    $('#Estado').val(data.IdEstado).trigger('change');

    $('#FechaEmision').val(formatearFechaParaInput(data.FechaEmision));
    $('#FechaEntrega').val(formatearFechaParaInput(data.FechaEntrega));



    $('#NotaInterna').val(data.NotaInterna);
    $('#CostoTotal').val(formatoMoneda.format(data.CostoTotal));
    $('#tituloOrden').text('Editar Orden de Compra');

    gridInsumos.clear().rows.add(data.OrdenesComprasInsumos).draw();
    calcularTotalOrden();
}


async function anadirProductoOrden() {
    const yaAgregados = gridInsumos.data().toArray().map(x => x.IdInsumo);

    const idProveedor = $("#Proveedor").val();
    insumos = await obtenerInsumosPorProveedor(idProveedor);

    const disponibles = insumos.filter(x => !yaAgregados.includes(x.Id));

    if (disponibles.length === 0) {
        advertenciaModal("¡Ya agregaste todos los insumos!");
        return;
    }

    const insumoSelect = $("#insumoSelect").empty();
    disponibles.forEach(x => {
        insumoSelect.append(`<option value="${x.Id}" data-idproveedorlista="${x.IdProveedorLista}">${x.Descripcion}</option>`);
    });

    const primero = disponibles[0];
    $("#precioInput").val(formatoMoneda.format(primero.PrecioLista));
    $("#cantidadInput").val(1);
    $("#totalInput").val(formatoMoneda.format(primero.PrecioLista));

    $('#insumosModal').data('edit-id', null).modal("show");
    $('#btnGuardarInsumo').text("Añadir");
}


async function guardarProductoOrden() {
    const id = $('#insumoSelect').val();
    const nombre = $('#insumoSelect option:selected').text();
    const costo = convertirMonedaAFloat($("#precioInput").val());
    const cantidad = parseFloat($("#cantidadInput").val());
    const Subtotal = +(costo * cantidad).toFixed(2);
    const idProveedorLista = parseInt($('#insumoSelect option:selected').data('idproveedorlista')) || 0;

    const modal = $('#insumosModal');
    const editId = modal.data("edit-id");

    if (editId != null) {
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == editId) {
                data.PrecioLista = costo;
                data.CantidadPedida = cantidad;
                data.CantidadRestante = cantidad;
                data.Subtotal = Subtotal;
                data.IdProveedorLista = idProveedorLista;
                this.data(data).draw();
            }
        });
    } else {
        gridInsumos.row.add({
            Id: 0,
            IdInsumo: parseInt(id),
            Nombre: nombre,
            PrecioLista: costo,
            CantidadPedida: cantidad,
            CantidadEntregada: 0,
            CantidadRestante: cantidad,
            PrecioLista: costo,
            Subtotal: Subtotal,
            IdEstado: 1,
            IdProveedorLista: idProveedorLista,
            NotaInterna: ''
        }).draw();
    }

    modal.modal("hide");
    calcularTotalOrden();
}


async function editarProductoOrden(id) {
    const fila = gridInsumos.data().toArray().find(x => x.IdInsumo == id);
    if (!fila) return;

    const idProveedor = $("#Proveedor").val();
    insumos = await obtenerInsumosPorProveedor(idProveedor);


    const insumoSelect = $("#insumoSelect").empty();
    insumos.forEach(x => {
        const disabled = (x.Id != fila.IdInsumo && gridInsumos.data().toArray().some(p => p.IdInsumo == x.Id));
        const selected = x.Id == fila.IdInsumo;
        const option = $(`<option value="${x.Id}" ${selected ? "selected" : ""} ${disabled ? "disabled" : ""} data-idproveedorlista="${x.IdProveedorLista}">${x.Descripcion}</option>`);
        insumoSelect.append(option);
    });

    $("#precioInput").val(formatoMoneda.format(fila.PrecioLista)); // sin formatear para evitar NaN
    $("#cantidadInput").val(fila.CantidadPedida);
    $("#totalInput").val(formatoMoneda.format(fila.Subtotal));

    $('#insumosModal').data('edit-id', fila.IdInsumo).modal("show");
    $('#btnGuardarInsumo').text("Editar");
}

function eliminarProductoOrden(id) {
    let rowToRemove = null;

    gridInsumos.rows().every(function () {
        const data = this.data();
        if (data.IdInsumo == id) {
            rowToRemove = this;
        }
    });

    if (rowToRemove) {
        rowToRemove.remove().draw();
        calcularTotalOrden();
    }
}

$('#precioInput').on('blur', function () {
    this.value = formatoMoneda.format(convertirMonedaAFloat(this.value));
});



async function guardarOrdenCompra() {
    if (!validarCampos()) return;

    const id = parseInt($('#idOrden').val()) || 0;
    const data = {
        Id: id,
        IdUnidadNegocio: parseInt($('#UnidadNegocio').val()),
        IdLocal: parseInt($('#Local').val()),
        IdProveedor: parseInt($('#Proveedor').val()),
        IdEstado: parseInt($('#Estado').val()),
        FechaEmision: $('#FechaEmision').val(),
        FechaEntrega: $('#FechaEntrega').val(),
        NotaInterna: $('#NotaInterna').val(),
        CostoTotal: convertirMonedaAFloat($('#CostoTotal').val()),
        OrdenesComprasInsumos: gridInsumos.data().toArray().map(p => ({
            Id: p.Id,
            IdOrdenCompra: id,
            IdInsumo: p.IdInsumo,
            IdProveedorLista: p.IdProveedorLista, // <--- asegurate que esté esto
            CantidadPedida: p.CantidadPedida,
            CantidadEntregada: p.CantidadEntregada,
            CantidadRestante: p.CantidadRestante,
            PrecioLista: p.PrecioLista,
            Subtotal: p.Subtotal,
            IdEstado: p.IdEstado,
            NotaInterna: p.NotaInterna
        }))


    };

    const url = id === 0 ? '/OrdenesCompra/Insertar' : '/OrdenesCompra/Actualizar';
    const method = id === 0 ? 'POST' : 'PUT';

    try {
        const res = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (!res.ok) {
            const errorBody = await res.text();
            console.error("Error al guardar:", errorBody);
            errorModal("Ocurrió un error al guardar la orden. Detalles: " + res.statusText);
            return;
        }

        const result = await res.json();
        if (result.valor) {
            exitoModal(`Orden guardada correctamente con el numero ${result.id}`);
            window.location.href = "/OrdenesCompra";
            localStorage.removeItem("ordenCompraId");

        } else {
            errorModal("Ocurrió un error al guardar la orden.");
        }

    } catch (err) {
        console.error("Excepción al guardar:", err);
        errorModal("Error inesperado al guardar la orden.");
    }
}

async function obtenerInsumosUnidadNegocio(idUnidad) {
    const res = await fetch(`/Insumos/Lista?IdUnidadNegocio=${idUnidad}`);
    const data = await res.json();
    return data.map(x => ({
        Id: x.Id,
        Descripcion: x.Descripcion,
        PrecioLista: x.PrecioLista,
        IdProveedorLista: x.IdProveedorLista
    }));
}

 
function calcularTotalInsumo() {
    const precio = convertirMonedaAFloat($("#precioInput").val());
    const cantidad = parseFloat($("#cantidadInput").val()) || 0;
    const total = precio * cantidad;
    $("#totalInput").val(formatoMoneda.format(total));
}

function convertirMonedaAFloat(valor) {
    if (typeof valor === "number") return valor;
    return parseFloat(
        valor.replace(/[^\d,.-]/g, '') // elimina todo lo que no sea número, coma, punto o signo
            .replace(/\./g, '')       // elimina separadores de miles
            .replace(',', '.')        // reemplaza coma decimal por punto
    ) || 0;
}


async function cargarLocales(idUnidadNegocio) {
    const response = await fetch(`/Locales/Lista?IdUnidadNegocio=${idUnidadNegocio}`);
    const data = await response.json();

    const $local = $('#Local');
    $local.empty();
    $local.append(`<option value="">Seleccionar</option>`);

    data.forEach(x => {
        $local.append(`<option value="${x.Id}">${x.Nombre}</option>`);
    });

    $local.val('').trigger('change');
}

