﻿let gridInsumos = null;

let insumos = [];


const columnConfig = [
    { index: 1, filterType: 'text' },
    { index: 2, filterType: 'text' },
    { index: 3, filterType: 'text' },
    { index: 4, filterType: 'text' },
];


$(document).ready(() => {

    listaUnidadesNegocio();
    listaCategorias();
    listaUnidadMedidas();
    

    if (CompraData && CompraData > 0) {
        cargarDatosCompra()
    } else {
        configurarDataTable(null);
    }

    $('#UnidadesNegocio').on('change', function () {
        // Limpiar el DataTable
        gridInsumos.clear().draw();
        calcularDatosCompra();
    });

    
    calcularDatosCompra();
    $('#descripcion').on('input', function () {
        validarCampos()
    });

   
    validarCampos()
})



async function cargarDatosCompra() {
    if (CompraData && CompraData > 0) {

        const datosCompra = await ObtenerDatosCompra(CompraData);
        await configurarDataTable(datosCompra.Insumos);
        await insertarDatosCompra(datosCompra.Compra);

        validarCampos();
    }
}


async function ObtenerDatosCompra(id) {
    const url = `/Compras/EditarInfo?id=${id}`;
    const response = await fetch(url);
    const data = await response.json();
    return data;
}

async function insertarDatosCompra(datos) {

    document.getElementById("IdCompra").value = datos.Id;
   

    //Cargamos Datos del Cliente
    document.getElementById("UnidadesNegocio").value = datos.IdUnidadNegocio;
    document.getElementById("descripcion").value = datos.Descripcion;
    document.getElementById("sku").value = datos.Sku;
    document.getElementById("Categorias").value = datos.IdCategoria;
    document.getElementById("UnidadMedidas").value = datos.IdUnidadMedida;
    document.getElementById("costoTotal").value = formatoMoneda.format(datos.CostoTotal);

    
  
    document.getElementById("btnNuevoModificar").textContent = "Guardar";

    await calcularDatosCompra ();
}


function validarCampos() {
    const descripcion = $("#descripcion").val();

    const descripcionValida = descripcion !== "";

    $("#lblDescripcion").css("color", descripcionValida ? "" : "red");
    $("#descripcion").css("border-color", descripcionValida ? "" : "red");


    return descripcionValida;
}


async function configurarDataTable(data) {
    if (!gridInsumos) {
        $('#grd_Insumos thead tr').clone(true).addClass('filters').appendTo('#grd_Compras thead');
        gridInsumos = $('#grd_Insumos').DataTable({
            data: data != null ? data.$values : data,
            language: {
                sLengthMenu: "Mostrar MENU registros",
                lengthMenu: "Anzeigen von _MENU_ Einträgen",
                url: "//cdn.datatables.net/plug-ins/2.0.7/i18n/es-MX.json"
            },
            scrollX: "100px",
            scrollCollapse: true,
            columns: [
                { data: 'Nombre' },
                { data: 'CostoUnitario' },
                { data: 'Cantidad' },
                { data: 'SubTotal' },
                {
                    data: "Id",
                    render: function (data, type, row) {
                        return `
                <button class='btn btn-sm btneditar btnacciones' type='button' onclick='editarInsumo(${row.IdInsumo})' title='Editar'>
                    <i class='fa fa-pencil-square-o fa-lg text-white' aria-hidden='true'></i>
                </button>
                <button class='btn btn-sm btneditar btnacciones' type='button' onclick='eliminarInsumo(${row.IdInsumo})' title='Eliminar'>
                    <i class='fa fa-trash-o fa-lg text-danger' aria-hidden='true'></i>
                </button>`;
                    },
                    orderable: true,
                    searchable: true,
                }
            ],
            orderCellsTop: true,
            fixedHeader: true,
            "columnDefs": [

                {
                    "render": function (data, type, row) {
                        return formatNumber(data); // Formatear números
                    },
                    "targets": [1, 3] // Índices de las columnas de números
                },

            ],

            initComplete: async function () {
                var api = this.api();

                // Iterar sobre las columnas y aplicar la configuración de filtros
                columnConfig.forEach(async (config) => {
                    var cell = $('.filters th').eq(config.index);

                    if (config.filterType === 'select') {
                        var select = $('<select id="filter' + config.index + '"><option value="">Seleccionar</option></select>')
                            .appendTo(cell.empty())
                            .on('change', async function () {
                                var val = $(this).val();
                                var selectedText = $(this).find('option:selected').text(); // Obtener el texto del nombre visible
                                await api.column(config.index).search(val ? '^' + selectedText + '$' : '', true, false).draw(); // Buscar el texto del nombre
                            });

                        var data = await config.fetchDataFunc(); // Llamada a la función para obtener los datos
                        data.forEach(function (item) {
                            select.append('<option value="' + item.Id + '">' + item.Nombre + '</option>')
                        });

                    } else if (config.filterType === 'text') {
                        var input = $('<input type="text" placeholder="Buscar..." />')
                            .appendTo(cell.empty())
                            .off('keyup change') // Desactivar manejadores anteriores
                            .on('keyup change', function (e) {
                                e.stopPropagation();
                                var regexr = '({search})';
                                var cursorPosition = this.selectionStart;
                                api.column(config.index)
                                    .search(this.value != '' ? regexr.replace('{search}', '(((' + this.value + ')))') : '', this.value != '', this.value == '')
                                    .draw();
                                $(this).focus()[0].setSelectionRange(cursorPosition, cursorPosition);
                            });
                    }
                });

                $('.filters th').eq(0).html(''); // Limpiar la última columna si es necesario

                setTimeout(function () {
                    gridInsumos.columns.adjust();
                }, 10);

                $('body').on('mouseenter', '#grd_Compras .fa-map-marker', function () {
                    $(this).css('cursor', 'pointer');
                });


            },
        });

    } else {
        gridInsumos.clear().rows.add(data).draw();
    }
}

async function listaUnidadesNegocioFilter() {
    const url = `/UnidadesNegocio/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

}


async function listaUnidadesNegocio() {
    const data = await listaUnidadesNegocioFilter();

    $('#UnidadesNegocio option').remove();

    select = document.getElementById("UnidadesNegocio");

    for (i = 0; i < data.length; i++) {
        option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);

    }
}

async function listaCategoriasFilter() {
    const url = `/ComprasCategoria/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

}


async function listaCategorias() {
    const data = await listaCategoriasFilter();

    $('#Categorias option').remove();

    select = document.getElementById("Categorias");

    for (i = 0; i < data.length; i++) {
        option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);

    }
}

async function listaUnidadMedidasFilter() {
    const url = `/UnidadesMedida/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

}


async function listaUnidadMedidas() {
    const data = await listaUnidadMedidasFilter();

    $('#UnidadMedidas option').remove();

    select = document.getElementById("UnidadMedidas");

    for (i = 0; i < data.length; i++) {
        option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);

    }
}

async function obtenerInsumosUnidadNegocio(id) {
    const url = `/Insumos/Lista?IdUnidadNegocio=${id}`;
    const response = await fetch(url);
    const data = await response.json();


    return data.map(x => ({
        Id: x.Id,
        Descripcion: x.Descripcion,
        CostoUnitario: x.CostoUnitario
    }));

}


async function anadirInsumo() {
    const IdUnidadNegocio = $("#UnidadesNegocio").val();
    const insumosEnTabla = [];

    gridInsumos.rows().every(function () {
        const data = this.data();
        insumosEnTabla.push(Number(data.IdInsumo));
    });

    insumos = await cargarInsumosModal(IdUnidadNegocio, insumosEnTabla);


    const todosYaAgregados = insumos.every(x => insumosEnTabla.includes(x.Id));

    if (todosYaAgregados) {
        advertenciaModal("¡Ya has agregado todos los insumos de esta unidad de negocio!");
        return false; // Detener si no hay más insumos para añadir
    }

    if (!insumos) return;

    const insumoSelect = $("#insumoSelect");
    const precioInput = $("#precioInput");
    const cantidadInput = $("#cantidadInput");
    const totalInput = $("#totalInput");

    insumoSelect.on("change", async function () {
        const selectedProductId = parseInt(this.value);
        const selectedProduct = insumos.find(p => p.Id === selectedProductId);

        const costoUnitario = selectedProduct.CostoUnitario;
        cantidadInput.val(1);
        precioInput.val(formatoMoneda.format(costoUnitario));
        totalInput.val(formatoMoneda.format(costoUnitario));
    });

    insumoSelect.trigger("change");

    $('#insumosModal').data('edit-id', null);
    $('#insumosModal').data('data-editing', false);
    $('#btnGuardarInsumo').text('Añadir');
    $("#insumosModal").modal('show');
}


async function calcularTotal() {
    const precioRaw = document.getElementById('precioInput').value;
    const cantidad = parseFloat(document.getElementById('cantidadInput').value) || 0;

    // Extraer solo el número del campo precio
    const precio = formatoNumero(precioRaw);

    const total = precio * cantidad;

    // Mostrar el total formateado en el campo
    document.getElementById('totalInput').value = formatoMoneda.format(total);
}

document.getElementById('precioInput').addEventListener('input', function () {
    calcularTotal();
});

document.getElementById('cantidadInput').addEventListener('input', function () {
    calcularTotal();
});

document.getElementById('precioInput').addEventListener('blur', function () {


    // Formatear el número al finalizar la edición
    this.value = formatMoneda(convertirMonedaAFloat(this.value));

    // Recalcular el total cada vez que cambia el precio
    calcularTotal();
});



async function guardarInsumo() {
    const insumoSelect = document.getElementById('insumoSelect');
    const precioManual = parseFloat(convertirMonedaAFloat(document.getElementById('precioInput').value));
    const totalInput = parseFloat(convertirMonedaAFloat(document.getElementById('totalInput').value));
    const cantidadInput = parseInt(document.getElementById('cantidadInput').value) || 1; // Obtener cantidad, por defecto 1 si no es válida
    const InsumoId = insumoSelect.value;
    const CompraNombre = insumoSelect.options[insumoSelect.selectedIndex]?.text || '';


    let i = 0;


    const modal = $('#insumosModal');
    const isEditing = modal.attr('data-editing') === 'true';
    const editId = modal.attr('edit-id');

    const selectedProduct = insumos.find(p => p.IdInsumo === parseInt(InsumoId));

    // Verificar si el Compra ya existe en la tabla
    let CompraExistente = false;

    if (isEditing) {
        // Si estamos editando, solo actualizamos la fila correspondiente
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == editId) {
                data.Nombre = CompraNombre;
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.Cantidad = cantidadInput; // Usar la cantidad del input
                data.SubTotal = totalInput; // Recalcular el total con formato de moneda
                this.data(data).draw();
            }
        });
    } else {
        // Buscar si el Compra ya existe en la tabla
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == InsumoId) {
                // Compra existe, sumamos las cantidades y recalculamos el total
                data.Cantidad = cantidadInput; // Sumar la cantidad proporcionada
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.SubTotal = precioManual * data.Cantidad; // Recalcular el total con formato de moneda
                this.data(data).draw();
                CompraExistente = true;
            }
        });

        if (!CompraExistente) {
            // Si no existe, agregar un nuevo Compra
            gridInsumos.row.add({
                IdInsumo: InsumoId,
                Id: 0,
                Nombre: CompraNombre,
                CostoUnitario: precioManual, // Agregar PrecioVenta
                Cantidad: cantidadInput, // Usar la cantidad proporcionada
                SubTotal: totalInput // Recalcular el total con formato de moneda
            }).draw();
        }
    }

    // Limpiar y cerrar el modal
    modal.modal('hide');

    calcularDatosCompra();

}

async function editarInsumo(id) {
    const IdUnidadNegocio = parseInt($("#UnidadesNegocio").val());
    const insumosEnTabla = [];
    let insumoData = null;

    gridInsumos.rows().every(function () {
        const data = this.data();
        insumosEnTabla.push(Number(data.IdInsumo));

        if (data.IdInsumo == id) {
            insumoData = data;
        }
    });

    if (!insumoData) {
        advertenciaModal("No se encontró el insumo a editar.");
        return;
    }

    insumos = await cargarInsumosModal(IdUnidadNegocio, insumosEnTabla, insumoData.IdInsumo);
    if (!insumos) return;

    $("#cantidadInput").val(insumoData.Cantidad);
    $("#precioInput").val(formatoMoneda.format(insumoData.CostoUnitario));
    $("#totalInput").val(formatoMoneda.format(insumoData.SubTotal));

    $("#insumoSelect").prop("disabled", true);

    $('#insumosModal').data('edit-id', insumoData.Id);
    $('#btnGuardarInsumo').text('Editar');
    $("#insumosModal").modal('show');
}
async function cargarInsumosModal(IdUnidadNegocio, insumosEnTabla, insumoSeleccionado = null) {
    insumos = await obtenerInsumosUnidadNegocio(IdUnidadNegocio);
    const insumoSelect = $("#insumoSelect");

    insumoSelect.empty();

    let primerHabilitadoId = null;

    insumos.forEach(insumo => {
        const option = $(`<option value="${insumo.Id}">${insumo.Descripcion}</option>`);

        if (insumosEnTabla.includes(insumo.Id)) {
            option.prop('disabled', true);
        } else if (primerHabilitadoId === null) {
            primerHabilitadoId = insumo.Id;
        }

        insumoSelect.append(option);
    });


    if (insumoSeleccionado !== null) {
        insumoSelect.val(insumoSeleccionado).prop("disabled", true);
    } else if (primerHabilitadoId !== null) {
        insumoSelect.val(primerHabilitadoId).prop("disabled", false);
    }

    return insumos; // Devolver los insumos cargados para su uso posterior
}

function eliminarInsumo(id) {
    gridInsumos.rows().every(function (rowIdx, tableLoop, rowLoop) {
        const data = this.data();
        if (data.IdInsumo == id) {
            gridInsumos.row(rowIdx).remove().draw();
        }
    });
    calcularDatosCompra();
}

$(document).on('click', function (e) {
    // Verificar si el clic está fuera de cualquier dropdown
    if (!$(e.target).closest('.acciones-menu').length) {
        $('.acciones-dropdown').hide(); // Cerrar todos los dropdowns
    }
});

async function calcularDatosCompra() {
    let InsumoTotal = 0;

    if (gridInsumos != null && gridInsumos.rows().count() > 0) {
        gridInsumos.rows().every(function () {
            const producto = this.data();
            InsumoTotal += parseFloat(producto.SubTotal);
        });
    }

    total = InsumoTotal;



    document.getElementById("costoTotal").value = formatoMoneda.format(parseFloat(InsumoTotal));

}



function guardarCambios() {
    const idCompra = $("#IdCompra").val();

    if (validarCampos()) {

        function obtenerInsumos(grd) {
            let insumos = [];
            grd.rows().every(function () {
                const insumo = this.data();
                const insumoJson = {
                    "IdCompra": idCompra != "" ? idCompra : 0,
                    "IdInsumo": parseInt(insumo.IdInsumo),
                    "Id": insumo.Id != "" ? insumo.Id : 0,
                    "Nombre": insumo.Nombre,
                    "CostoUnitario": parseFloat(insumo.CostoUnitario),
                    "SubTotal": parseFloat(insumo.SubTotal),
                    "Cantidad": parseInt(insumo.Cantidad),

                };
                insumos.push(insumoJson);
            });
            return insumos;
        }

        const insumos = obtenerInsumos(gridInsumos);

        if (insumos.length == 0) {
            advertenciaModal("Debes agregar al menos un insumo");
            return;
        }

        // Construcción del objeto para el modelo
        const nuevoModelo = {
            "Id": idCompra !== "" ? parseInt(idCompra) : 0,
            "IdUnidadNegocio": parseInt($("#UnidadesNegocio").val()),
            "Descripcion": $("#descripcion").val(),
            "Sku": $("#sku").val(),
            "IdCategoria": parseInt($("#Categorias").val()),
            "IdUnidadMedida": parseInt($("#UnidadMedidas").val()),
            "CostoTotal": parseFloat(convertirMonedaAFloat($("#costoTotal").val())),
            "ComprasInsumos": insumos
        };

        // Definir la URL y el método para el envío
        const url = idCompra === "" ? "/Compras/Insertar" : "/Compras/Actualizar";
        const method = idCompra === "" ? "POST" : "PUT";

        console.log(JSON.stringify(nuevoModelo))

        // Enviar los datos al servidor
        fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify(nuevoModelo)
        })
            .then(response => {

                if (!response.ok) throw new Error(response.statusText);
                return response.json();
            })
            .then(dataJson => {
                console.log("Respuesta del servidor:", dataJson);
                const mensaje = idCompra === "" ? "Compra registrado correctamente" : "Pedido modificado correctamente";
                exitoModal(mensaje);
                window.location.href = "/Compras/Index";

            })
            .catch(error => {
                console.error('Error:', error);
            });
    } else {
        errorModal("Debes completar los campos requeridos.")
    }
}