let gridInsumos = null, gridSubrecetas = null;
$(document).ready(() => {


    listaUnidadesNegocio();
    listaCategorias();
    listaUnidadMedidas();

    if (recetaData && recetaData > 0) {
        cargarDatosReceta();
    } else {
        configurarDataTableInsumos(null);
        configurarDataTableSubrecetas(null);
    }

    



    $('#UnidadesNegocio').on('change', function () {
        // Limpiar el DataTable
        gridInsumos.clear().draw();
        gridSubrecetas.clear().draw();
        calcularDatosReceta();
    });

    $('#descripcion').on('input', function () {
        validarCampos()
    });

    validarCampos()

})

async function ObtenerDatosReceta(id) {
    const url = `/Recetas/EditarInfo?id=${id}`;
    const response = await fetch(url);
    const data = await response.json();
    return data;
}


async function cargarDatosReceta() {
    if (recetaData && recetaData > 0) {

        const datosReceta = await ObtenerDatosReceta(recetaData);
        await insertarDatosReceta(datosReceta.Receta);
        await configurarDataTableInsumos(datosReceta.Insumos);
        await configurarDataTableSubrecetas(datosReceta.Subrecetas);

        calcularDatosReceta();

        validarCampos();
    }
}


async function insertarDatosReceta(datos) {

    document.getElementById("idReceta").value = datos.Id;


    //Cargamos Datos del Cliente
    document.getElementById("UnidadesNegocio").value = datos.IdUnidadNegocio;
    document.getElementById("descripcion").value = datos.Descripcion;
    document.getElementById("sku").value = datos.Sku;
    document.getElementById("Categorias").value = datos.IdCategoria;
    document.getElementById("UnidadMedidas").value = datos.IdUnidadMedida;
    document.getElementById("costoInsumos").value = formatoMoneda.format(datos.CostoInsumos);
    document.getElementById("costoSubrecetas").value = formatoMoneda.format(datos.CostoSubrecetas);
    document.getElementById("costoTotal").value = formatoMoneda.format(datos.CostoTotal);



    document.getElementById("btnNuevoModificar").textContent = "Guardar";

    await calcularDatosReceta();
}




async function configurarDataTableSubrecetas(data) {
    if (!gridSubrecetas) {
        gridSubrecetas = $('#grd_Subrecetas').DataTable({
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
                <button class='btn btn-sm btneditar btnacciones' type='button' onclick='editarSubreceta(${row.IdSubreceta})' title='Editar'>
                    <i class='fa fa-pencil-square-o fa-lg text-white' aria-hidden='true'></i>
                </button>
                <button class='btn btn-sm btneditar btnacciones' type='button' onclick='eliminarSubreceta(${row.IdSubreceta})' title='Eliminar'>
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
                setTimeout(function () {
                    gridSubrecetas.columns.adjust();
                }, 10);


            },
        });

    } else {
        gridSubrecetas.clear().rows.add(data).draw();
    }
}

async function configurarDataTableInsumos(data) {
    if (!gridInsumos) {
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


            },
        });

    } else {
        gridInsumos.clear().rows.add(data).draw();
    }
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

async function guardarInsumo() {
    const insumoSelect = document.getElementById('insumoSelect');
    const precioManual = parseFloat(convertirMonedaAFloat(document.getElementById('precioInput').value));
    const totalInput = parseFloat(convertirMonedaAFloat(document.getElementById('totalInput').value));
    const cantidadInput = parseInt(document.getElementById('cantidadInput').value) || 1; // Obtener cantidad, por defecto 1 si no es válida
    const InsumoId = insumoSelect.value;
    const SubrecetaNombre = insumoSelect.options[insumoSelect.selectedIndex]?.text || '';


    let i = 0;


    const modal = $('#insumosModal');
    const isEditing = modal.attr('data-editing') === 'true';
    const editId = modal.attr('edit-id');

    const selectedProduct = insumos.find(p => p.IdInsumo === parseInt(InsumoId));

    // Verificar si el Subreceta ya existe en la tabla
    let SubrecetaExistente = false;

    if (isEditing) {
        // Si estamos editando, solo actualizamos la fila correspondiente
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == editId) {
                data.Nombre = SubrecetaNombre;
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.Cantidad = cantidadInput; // Usar la cantidad del input
                data.SubTotal = totalInput; // Recalcular el total con formato de moneda
                this.data(data).draw();
            }
        });
    } else {
        // Buscar si el Subreceta ya existe en la tabla
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == InsumoId) {
                // Subreceta existe, sumamos las cantidades y recalculamos el total
                data.Cantidad = cantidadInput; // Sumar la cantidad proporcionada
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.SubTotal = precioManual * data.Cantidad; // Recalcular el total con formato de moneda
                this.data(data).draw();
                SubrecetaExistente = true;
            }
        });

        if (!SubrecetaExistente) {
            // Si no existe, agregar un nuevo Subreceta
            gridInsumos.row.add({
                IdInsumo: InsumoId,
                Id: 0,
                Nombre: SubrecetaNombre,
                CostoUnitario: precioManual, // Agregar PrecioVenta
                Cantidad: cantidadInput, // Usar la cantidad proporcionada
                SubTotal: totalInput // Recalcular el total con formato de moneda
            }).draw();
        }
    }

    // Limpiar y cerrar el modal
    modal.modal('hide');

    calcularDatosReceta();

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

function eliminarInsumo(id) {
    gridInsumos.rows().every(function (rowIdx, tableLoop, rowLoop) {
        const data = this.data();
        if (data.IdInsumo == id) {
            gridInsumos.row(rowIdx).remove().draw();
        }
    });
    calcularDatosReceta();
}


async function anadirSubreceta() {
    const IdUnidadNegocio = $("#UnidadesNegocio").val();
    const SubrecetasEnTabla = [];

    gridSubrecetas.rows().every(function () {
        const data = this.data();
        SubrecetasEnTabla.push(Number(data.IdSubreceta));
    });

    Subrecetas = await cargarSubrecetasModal(IdUnidadNegocio, SubrecetasEnTabla);


    const todosYaAgregados = Subrecetas.every(x => SubrecetasEnTabla.includes(x.Id));

    if (todosYaAgregados) {
        advertenciaModal("¡Ya has agregado todos los Subrecetas de esta unidad de negocio!");
        return false; // Detener si no hay más Subrecetas para añadir
    }

    if (!Subrecetas) return;

    const SubrecetaSelect = $("#SubrecetaSelect");
    const precioInput = $("#precioSubrecetaInput");
    const cantidadInput = $("#cantidadSubrecetaInput");
    const totalInput = $("#totalSubrecetaInput");

    SubrecetaSelect.on("change", async function () {
        const selectedProductId = parseInt(this.value);
        const selectedProduct = Subrecetas.find(p => p.Id === selectedProductId);

        const CostoTotal = selectedProduct.CostoTotal;
        cantidadInput.val(1);
        precioInput.val(formatoMoneda.format(CostoTotal));
        totalInput.val(formatoMoneda.format(CostoTotal));
    });

    SubrecetaSelect.trigger("change");

    $('#SubrecetasModal').data('edit-id', null);
    $('#SubrecetasModal').data('data-editing', false);
    $('#btnGuardarSubreceta').text('Añadir');
    $("#SubrecetasModal").modal('show');
}

async function cargarSubrecetasModal(IdUnidadNegocio, SubrecetasEnTabla, SubrecetaSeleccionado = null) {
    Subrecetas = await obtenerSubrecetasUnidadNegocio(IdUnidadNegocio);
    const SubrecetaSelect = $("#SubrecetaSelect");

    SubrecetaSelect.empty();

    let primerHabilitadoId = null;

    Subrecetas.forEach(Subreceta => {
        const option = $(`<option value="${Subreceta.Id}">${Subreceta.Descripcion}</option>`);

        if (SubrecetasEnTabla.includes(Subreceta.Id)) {
            option.prop('disabled', true);
        } else if (primerHabilitadoId === null) {
            primerHabilitadoId = Subreceta.Id;
        }

        SubrecetaSelect.append(option);
    });


    if (SubrecetaSeleccionado !== null) {
        SubrecetaSelect.val(SubrecetaSeleccionado).prop("disabled", true);
    } else if (primerHabilitadoId !== null) {
        SubrecetaSelect.val(primerHabilitadoId).prop("disabled", false);
    }

    return Subrecetas; // Devolver los Subrecetas cargados para su uso posterior
}

async function guardarSubreceta() {
    const SubrecetaSelect = document.getElementById('SubrecetaSelect');
    const precioManual = parseFloat(convertirMonedaAFloat(document.getElementById('precioSubrecetaInput').value));
    const totalInput = parseFloat(convertirMonedaAFloat(document.getElementById('totalSubrecetaInput').value));
    const cantidadInput = parseInt(document.getElementById('cantidadSubrecetaInput').value) || 1; // Obtener cantidad, por defecto 1 si no es válida
    const SubrecetaId = SubrecetaSelect.value;
    const SubrecetaNombre = SubrecetaSelect.options[SubrecetaSelect.selectedIndex]?.text || '';


    let i = 0;


    const modal = $('#SubrecetasModal');
    const isEditing = modal.attr('data-editing') === 'true';
    const editId = modal.attr('edit-id');

    const selectedProduct = Subrecetas.find(p => p.IdSubreceta === parseInt(SubrecetaId));

    // Verificar si el Subreceta ya existe en la tabla
    let SubrecetaExistente = false;

    if (isEditing) {
        // Si estamos editando, solo actualizamos la fila correspondiente
        gridSubrecetas.rows().every(function () {
            const data = this.data();
            if (data.IdSubreceta == editId) {
                data.Nombre = SubrecetaNombre;
                data.CostoTotal = precioManual; // Guardar PrecioVenta
                data.Cantidad = cantidadInput; // Usar la cantidad del input
                data.SubTotal = totalInput; // Recalcular el total con formato de moneda
                this.data(data).draw();
            }
        });
    } else {
        // Buscar si el Subreceta ya existe en la tabla
        gridSubrecetas.rows().every(function () {
            const data = this.data();
            if (data.IdSubreceta == SubrecetaId) {
                // Subreceta existe, sumamos las cantidades y recalculamos el total
                data.Cantidad = cantidadInput; // Sumar la cantidad proporcionada
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.SubTotal = precioManual * data.Cantidad; // Recalcular el total con formato de moneda
                this.data(data).draw();
                SubrecetaExistente = true;
            }
        });

        if (!SubrecetaExistente) {
            // Si no existe, agregar un nuevo Subreceta
            gridSubrecetas.row.add({
                IdSubreceta: SubrecetaId,
                Id: 0,
                Nombre: SubrecetaNombre,
                CostoUnitario: precioManual, // Agregar PrecioVenta
                Cantidad: cantidadInput, // Usar la cantidad proporcionada
                SubTotal: totalInput // Recalcular el total con formato de moneda
            }).draw();
        }
    }

    // Limpiar y cerrar el modal
    modal.modal('hide');

    calcularDatosReceta();

}

async function editarSubreceta(id) {
    const IdUnidadNegocio = parseInt($("#UnidadesNegocio").val());
    const SubrecetasEnTabla = [];
    let SubrecetaData = null;

    gridSubrecetas.rows().every(function () {
        const data = this.data();
        SubrecetasEnTabla.push(Number(data.IdSubreceta));

        if (data.IdSubreceta == id) {
            SubrecetaData = data;
        }
    });

    if (!SubrecetaData) {
        advertenciaModal("No se encontró el Subreceta a editar.");
        return;
    }

    Subrecetas = await cargarSubrecetasModal(IdUnidadNegocio, SubrecetasEnTabla, SubrecetaData.IdSubreceta);
    if (!Subrecetas) return;

    $("#cantidadSubrecetaInput").val(SubrecetaData.Cantidad);
    $("#precioSubrecetaInput").val(formatoMoneda.format(SubrecetaData.CostoUnitario));
    $("#totalSubrecetaInput").val(formatoMoneda.format(SubrecetaData.SubTotal));

    $("#SubrecetaSelect").prop("disabled", true);

    $('#SubrecetasModal').data('edit-id', SubrecetaData.Id);
    $('#btnGuardarSubreceta').text('Editar');
    $("#SubrecetasModal").modal('show');
}

function eliminarSubreceta(id) {
    gridSubrecetas.rows().every(function (rowIdx, tableLoop, rowLoop) {
        const data = this.data();
        if (data.IdSubreceta == id) {
            gridSubrecetas.row(rowIdx).remove().draw();
        }
    });
    calcularDatosReceta();
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
    const url = `/SubrecetasCategoria/Lista`;
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


async function obtenerSubrecetasUnidadNegocio(id) {
    const url = `/Subrecetas/Lista?IdUnidadNegocio=${id}`;
    const response = await fetch(url);
    const data = await response.json();


    return data.map(x => ({
        Id: x.Id,
        Descripcion: x.Descripcion,
        CostoTotal: x.CostoTotal
    }));

}

document.querySelector('#insumos-tab').addEventListener('shown.bs.tab', function () {
    if (gridInsumos) {
        setTimeout(function () {
            gridInsumos.columns.adjust();
        }, 10); // Un pequeño retraso para asegurar que las dimensiones estén listas
    }
});

document.querySelector('#Subrecetas-tab').addEventListener('shown.bs.tab', function () {
    if (gridSubrecetas) {
        setTimeout(function () {
            gridSubrecetas.columns.adjust();
        }, 10); // Un pequeño retraso para asegurar que las dimensiones estén listas
    }
});


async function calcularTotalInsumo() {
    const precioRaw = document.getElementById('precioInput').value;
    const cantidad = parseFloat(document.getElementById('cantidadInput').value) || 0;

    // Extraer solo el número del campo precio
    const precio = formatoNumero(precioRaw);

    const total = precio * cantidad;

    // Mostrar el total formateado en el campo
    document.getElementById('totalInput').value = formatoMoneda.format(total);
}

document.getElementById('precioInput').addEventListener('input', function () {
    calcularTotalInsumo();
});

document.getElementById('cantidadInput').addEventListener('input', function () {
    calcularTotalInsumo();
});

document.getElementById('precioInput').addEventListener('blur', function () {


    // Formatear el número al finalizar la edición
    this.value = formatMoneda(convertirMonedaAFloat(this.value));

    // Recalcular el total cada vez que cambia el precio
    calcularTotalInsumo();
});



async function calcularTotalSubreceta() {
    const precioRaw = document.getElementById('precioSubrecetaInput').value;
    const cantidad = parseFloat(document.getElementById('cantidadSubrecetaInput').value) || 0;

    // Extraer solo el número del campo precio
    const precio = formatoNumero(precioRaw);

    const total = precio * cantidad;

    // Mostrar el total formateado en el campo
    document.getElementById('totalSubrecetaInput').value = formatoMoneda.format(total);
}

document.getElementById('precioSubrecetaInput').addEventListener('input', function () {
    calcularTotalSubreceta();
});

document.getElementById('cantidadSubrecetaInput').addEventListener('input', function () {
    calcularTotalSubreceta();
});

document.getElementById('precioSubrecetaInput').addEventListener('blur', function () {


    // Formatear el número al finalizar la edición
    this.value = formatMoneda(convertirMonedaAFloat(this.value));

    // Recalcular el total cada vez que cambia el precio
    calcularTotalSubreceta();
});




async function calcularDatosReceta() {
    let InsumoTotal = 0;

    if (gridInsumos != null && gridInsumos.rows().count() > 0) {
        gridInsumos.rows().every(function () {
            const producto = this.data();
            InsumoTotal += parseFloat(producto.SubTotal);
        });
    }

    let SubrecetaTotal = 0;

    if (gridSubrecetas != null && gridSubrecetas.rows().count() > 0) {
        gridSubrecetas.rows().every(function () {
            const producto = this.data();
            SubrecetaTotal += parseFloat(producto.SubTotal);
        });
    }

    total = SubrecetaTotal + InsumoTotal;



    document.getElementById("costoTotal").value = formatoMoneda.format(parseFloat(total));
    document.getElementById("costoInsumos").value = formatoMoneda.format(parseFloat(InsumoTotal));
    document.getElementById("costoSubrecetas").value = formatoMoneda.format(parseFloat(SubrecetaTotal));

}

function validarCampos() {
    const descripcion = $("#descripcion").val();

    const descripcionValida = descripcion !== "";

    $("#lblDescripcion").css("color", descripcionValida ? "" : "red");
    $("#descripcion").css("border-color", descripcionValida ? "" : "red");


    return descripcionValida;
}



function guardarCambios() {
    const idReceta = $("#idReceta").val();

    if (validarCampos()) {

        function obtenerInsumos(grd) {
            let insumos = [];
            grd.rows().every(function () {
                const insumo = this.data();
                const insumoJson = {
                    "IdReceta": idReceta != "" ? idReceta : 0,
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

        function obtenerSubrecetas(grd) {
            let Subrecetas = [];
            grd.rows().every(function () {
                const Subreceta = this.data();
                const SubrecetaJson = {
                    "IdReceta": idReceta != "" ? idReceta : 0,
                    "IdSubreceta": parseInt(Subreceta.IdSubreceta),
                    "Id": Subreceta.Id != "" ? Subreceta.Id : 0,
                    "Nombre": Subreceta.Nombre,
                    "CostoUnitario": parseFloat(Subreceta.CostoUnitario),
                    "SubTotal": parseFloat(Subreceta.SubTotal),
                    "Cantidad": parseInt(Subreceta.Cantidad),

                };
                Subrecetas.push(SubrecetaJson);
            });
            return Subrecetas;
        }

        const insumos = obtenerInsumos(gridInsumos);
        const Subrecetas = obtenerSubrecetas(gridSubrecetas);

        if (insumos.length == 0) {
            advertenciaModal("Debes agregar al menos un insumo");
            return;
        }

        // Construcción del objeto para el modelo
        const nuevoModelo = {
            "Id": idReceta !== "" ? parseInt(idReceta) : 0,
            "IdUnidadNegocio": parseInt($("#UnidadesNegocio").val()),
            "Descripcion": $("#descripcion").val(),
            "Sku": $("#sku").val(),
            "IdCategoria": parseInt($("#Categorias").val()),
            "IdUnidadMedida": parseInt($("#UnidadMedidas").val()),
            "CostoTotal": parseFloat(convertirMonedaAFloat($("#costoTotal").val())),
            "CostoSubrecetas": parseFloat(convertirMonedaAFloat($("#costoSubrecetas").val())),
            "CostoInsumos": parseFloat(convertirMonedaAFloat($("#costoInsumos").val())),
            "RecetasInsumos": insumos,
            "RecetasSubrecetas": Subrecetas
        };

        // Definir la URL y el método para el envío
        const url = idReceta === "" ? "/Recetas/Insertar" : "/Recetas/Actualizar";
        const method = idReceta === "" ? "POST" : "PUT";

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
                const mensaje = idReceta === "" ? "Receta registrada correctamente" : "Pedido modificada correctamente";
                exitoModal(mensaje);
                window.location.href = "/Recetas/Index";

            })
            .catch(error => {
                console.error('Error:', error);
            });
    } else {
        errorModal("Debes completar los campos requeridos.")
    }
}