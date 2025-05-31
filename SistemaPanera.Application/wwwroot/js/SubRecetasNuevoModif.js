let gridInsumos = null, gridSubRecetas = null;
$(document).ready(() => {


    listaUnidadesNegocio();
    listaCategorias();
    listaUnidadMedidas();

    if (SubRecetaData && SubRecetaData > 0) {
        cargarDatosSubReceta();
    } else {
        configurarDataTableInsumos(null);
        configurarDataTableSubRecetas(null);
        $("#titulosubReceta").text("Nueva Subreceta");
    }

    



    $('#UnidadesNegocio').on('change', function () {
        // Limpiar el DataTable
        gridInsumos.clear().draw();
        gridSubRecetas.clear().draw();
        calcularDatosReceta();
    });

    $('#descripcion').on('input', function () {
        validarCampos()
    });

    validarCampos()

})

async function ObtenerDatosSubReceta(id) {
    const url = `/SubRecetas/EditarInfo?id=${id}`;
    const response = await fetch(url);
    const data = await response.json();
    return data;
}


async function cargarDatosSubReceta() {
    if (SubRecetaData && SubRecetaData > 0) {

        const datosReceta = await ObtenerDatosSubReceta(SubRecetaData);
        await insertarDatosSubReceta(datosReceta.Subreceta);
        await configurarDataTableInsumos(datosReceta.Insumos);
        await configurarDataTableSubRecetas(datosReceta.SubRecetas);



        calcularDatosReceta();

        validarCampos();
    }
}


async function insertarDatosSubReceta(datos) {

    document.getElementById("idSubReceta").value = datos.Id;

    document.getElementById("UnidadesNegocio").value = datos.IdUnidadNegocio;
    document.getElementById("descripcion").value = datos.Descripcion;
    document.getElementById("sku").value = datos.Sku;
    document.getElementById("Categorias").value = datos.IdCategoria;
    document.getElementById("UnidadMedidas").value = datos.IdUnidadMedida;

    document.getElementById("costoInsumos").value = formatoMoneda.format(datos.CostoInsumos ?? 0);
    document.getElementById("costoSubRecetas").value = formatoMoneda.format(datos.CostoSubRecetas ?? 0);
    document.getElementById("CostoPorcion").value = formatoMoneda.format(datos.CostoPorcion ?? 0);
    document.getElementById("Rendimiento").value = datos.Rendimiento ?? 0;
    document.getElementById("CostoUnitario").value = formatoMoneda.format(datos.CostoUnitario ?? 0);

    document.getElementById("btnNuevoModificar").textContent = "Guardar";

    $("#titulosubReceta").text("Editar Subreceta");


    await calcularDatosReceta(); // si esto depende del rendimiento o costos, mantenelo
}




async function configurarDataTableSubRecetas(data) {
    if (!gridSubRecetas) {
        gridSubRecetas = $('#grd_SubRecetas').DataTable({
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
<button class='btn btn-sm btneditar btnacciones' type='button' onclick='editarSubReceta(${row.IdSubRecetaHija})' title='Editar'>
    <i class='fa fa-pencil-square-o fa-lg text-white' aria-hidden='true'></i>
</button>
<button class='btn btn-sm btneditar btnacciones' type='button' onclick='eliminarSubReceta(${row.IdSubRecetaHija})' title='Eliminar'>
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
                    gridSubRecetas.columns.adjust();
                }, 10);


            },
        });

    } else {
        gridSubRecetas.clear().rows.add(data).draw();
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

        const CostoUnitario = selectedProduct.CostoUnitario;
        cantidadInput.val(1);
        precioInput.val(formatoMoneda.format(CostoUnitario));
        totalInput.val(formatoMoneda.format(CostoUnitario));
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
    const SubRecetaNombre = insumoSelect.options[insumoSelect.selectedIndex]?.text || '';


    let i = 0;


    const modal = $('#insumosModal');
    const isEditing = modal.attr('data-editing') === 'true';
    const editId = modal.attr('edit-id');

    const selectedProduct = insumos.find(p => p.IdInsumo === parseInt(InsumoId));

    // Verificar si el SubReceta ya existe en la tabla
    let SubRecetaExistente = false;

    if (isEditing) {
        // Si estamos editando, solo actualizamos la fila correspondiente
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == editId) {
                data.Nombre = SubRecetaNombre;
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.Cantidad = cantidadInput; // Usar la cantidad del input
                data.SubTotal = totalInput; // Recalcular el total con formato de moneda
                this.data(data).draw();
            }
        });
    } else {
        // Buscar si el SubReceta ya existe en la tabla
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == InsumoId) {
                // SubReceta existe, sumamos las cantidades y recalculamos el total
                data.Cantidad = cantidadInput; // Sumar la cantidad proporcionada
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.SubTotal = precioManual * data.Cantidad; // Recalcular el total con formato de moneda
                this.data(data).draw();
                SubRecetaExistente = true;
            }
        });

        if (!SubRecetaExistente) {
            // Si no existe, agregar un nuevo SubReceta
            gridInsumos.row.add({
                IdInsumo: InsumoId,
                Id: 0,
                Nombre: SubRecetaNombre,
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
        if (data != null && data.IdInsumo == id) {
            gridInsumos.row(rowIdx).remove().draw();
        }
    });
    calcularDatosReceta();
}


async function anadirSubReceta() {
    const IdUnidadNegocio = $("#UnidadesNegocio").val();
    const SubRecetasEnTabla = [];

    gridSubRecetas.rows().every(function () {
        const data = this.data();
        SubRecetasEnTabla.push(Number(data.IdSubReceta));
    });

    SubRecetas = await cargarSubRecetasModal(IdUnidadNegocio, SubRecetasEnTabla);


    const todosYaAgregados = SubRecetas.every(x => SubRecetasEnTabla.includes(x.Id));

    if (todosYaAgregados) {
        advertenciaModal("¡Ya has agregado todas los Subrecetas de esta unidad de negocio!");
        return false; // Detener si no hay más SubRecetas para añadir
    }

    if (!SubRecetas) return;

    const SubRecetaSelect = $("#SubRecetaSelect");
    const precioInput = $("#precioSubRecetaInput");
    const cantidadInput = $("#cantidadSubRecetaInput");
    const totalInput = $("#totalSubRecetaInput");

    SubRecetaSelect.on("change", async function () {
        const selectedProductId = parseInt(this.value);
        const selectedProduct = SubRecetas.find(p => p.Id === selectedProductId);

        const CostoUnitario = selectedProduct.CostoUnitario;
        cantidadInput.val(1);
        precioInput.val(formatoMoneda.format(CostoUnitario));
        totalInput.val(formatoMoneda.format(CostoUnitario));
    });

    SubRecetaSelect.trigger("change");

    $('#SubRecetasModal').data('edit-id', null);
    $('#SubRecetasModal').data('data-editing', false);
    $('#btnGuardarSubReceta').text('Añadir');
    $("#SubRecetasModal").modal('show');
}

async function cargarSubRecetasModal(IdUnidadNegocio, SubRecetasEnTabla, SubRecetaSeleccionado = null) {
    SubRecetas = await obtenerSubRecetasUnidadNegocio(IdUnidadNegocio);
    const SubRecetaSelect = $("#SubRecetaSelect");

    SubRecetaSelect.empty();

    let primerHabilitadoId = null;

    SubRecetas.forEach(SubReceta => {
        const option = $(`<option value="${SubReceta.Id}">${SubReceta.Descripcion}</option>`);

        if (SubRecetasEnTabla.includes(SubReceta.Id)) {
            option.prop('disabled', true);
        } else if (primerHabilitadoId === null) {
            primerHabilitadoId = SubReceta.Id;
        }

        SubRecetaSelect.append(option);
    });


    if (SubRecetaSeleccionado !== null) {
        SubRecetaSelect.val(SubRecetaSeleccionado).prop("disabled", true);
    } else if (primerHabilitadoId !== null) {
        SubRecetaSelect.val(primerHabilitadoId).prop("disabled", false);
    }

    return SubRecetas; // Devolver los SubRecetas cargados para su uso posterior
}

async function guardarSubReceta() {
    const SubRecetaSelect = document.getElementById('SubRecetaSelect');
    const precioManual = parseFloat(convertirMonedaAFloat(document.getElementById('precioSubRecetaInput').value));
    const totalInput = parseFloat(convertirMonedaAFloat(document.getElementById('totalSubRecetaInput').value));
    const cantidadInput = parseInt(document.getElementById('cantidadSubRecetaInput').value) || 1;
    const SubRecetaId = parseInt(SubRecetaSelect.value);
    const SubRecetaNombre = SubRecetaSelect.options[SubRecetaSelect.selectedIndex]?.text || '';

    const modal = $('#SubRecetasModal');
    const isEditing = modal.data('data-editing') === true;
    const editId = parseInt(modal.data('edit-id')) || 0;
    const editKey = modal.data('edit-key') || null;

    let SubRecetaExistente = false;

    if (isEditing) {
        gridSubRecetas.rows().every(function () {
            const data = this.data();

            const matchById = data.Id === editId; // <- usamos el ID del vínculo
            const matchByKey = !data.Id && data.__keyTempId && data.__keyTempId === editKey;

            if (matchById || matchByKey) {
                data.IdSubReceta = SubRecetaId;
                data.IdSubRecetaHija = SubRecetaId;
                data.Nombre = SubRecetaNombre;
                data.CostoUnitario = precioManual;
                data.Cantidad = cantidadInput;
                data.SubTotal = totalInput;
                this.data(data).draw();
                SubRecetaExistente = true;
            }
        });
    } else {
        gridSubRecetas.rows().every(function () {
            const data = this.data();
            if (parseInt(data.IdSubRecetaHija) === SubRecetaId) {
                data.Cantidad += cantidadInput;
                data.CostoUnitario = precioManual;
                data.SubTotal = precioManual * data.Cantidad;
                this.data(data).draw();
                SubRecetaExistente = true;
            }
        });

        if (!SubRecetaExistente) {
            gridSubRecetas.row.add({
                Id: 0,
                IdSubReceta: SubRecetaId,
                IdSubRecetaHija: SubRecetaId,
                Nombre: SubRecetaNombre,
                CostoUnitario: precioManual,
                Cantidad: cantidadInput,
                SubTotal: totalInput,
                __keyTempId: Date.now()
            }).draw();
        }
    }

    modal.modal('hide');
    modal.data('edit-id', '');
    modal.data('edit-key', '');
    modal.data('data-editing', false);

    calcularDatosReceta();
}

async function editarSubReceta(id) {
    const IdUnidadNegocio = parseInt($("#UnidadesNegocio").val());
    const SubRecetasEnTabla = [];
    let SubReceta = null;

    gridSubRecetas.rows().every(function () {
        const data = this.data();
        SubRecetasEnTabla.push(Number(data.IdSubRecetaHija));

        if (parseInt(data.IdSubRecetaHija) === id) {
            SubReceta = data;
        }
    });

    if (!SubReceta) {
        advertenciaModal("No se encontró la subreceta a editar.");
        return;
    }

    SubRecetas = await cargarSubRecetasModal(IdUnidadNegocio, SubRecetasEnTabla, SubReceta.IdSubRecetaHija);
    if (!SubRecetas) return;

    $("#cantidadSubRecetaInput").val(SubReceta.Cantidad);
    $("#precioSubRecetaInput").val(formatoMoneda.format(SubReceta.CostoUnitario));
    $("#totalSubRecetaInput").val(formatoMoneda.format(SubReceta.SubTotal));

    $("#SubRecetaSelect").prop("disabled", true);

    // ✅ Guardar el ID del vínculo (relación), no el padre ni el hijo
    $('#SubRecetasModal').data('edit-id', SubReceta.Id || 0);
    $('#SubRecetasModal').data('edit-key', SubReceta.__keyTempId || null);
    $('#SubRecetasModal').data('data-editing', true);
    $('#btnGuardarSubReceta').text('Editar');
    $("#SubRecetasModal").modal('show');
}

function eliminarSubReceta(id) {
    gridSubRecetas.rows().every(function (rowIdx, tableLoop, rowLoop) {
        const data = this.data();
        if (data != null && data.IdSubRecetaHija == id) {
            gridSubRecetas.row(rowIdx).remove().draw();
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
    const url = `/SubRecetasCategoria/Lista`;
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


async function obtenerSubRecetasUnidadNegocio(id) {
    const url = `/SubRecetas/Lista?IdUnidadNegocio=${id}`;
    const response = await fetch(url);
    const data = await response.json();


    return data.map(x => ({
        Id: x.Id,
        Descripcion: x.Descripcion,
        CostoUnitario: x.CostoUnitario
    }));

}

document.querySelector('#insumos-tab').addEventListener('shown.bs.tab', function () {
    if (gridInsumos) {
        setTimeout(function () {
            gridInsumos.columns.adjust();
        }, 10); // Un pequeño retraso para asegurar que las dimensiones estén listas
    }
});

document.querySelector('#SubReceta-tab').addEventListener('shown.bs.tab', function () {
    if (gridSubRecetas) {
        setTimeout(function () {
            gridSubRecetas.columns.adjust();
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



async function calcularTotalSubReceta() {
    const precioRaw = document.getElementById('precioSubRecetaInput').value;
    const cantidad = parseFloat(document.getElementById('cantidadSubRecetaInput').value) || 0;

    // Extraer solo el número del campo precio
    const precio = formatoNumero(precioRaw);

    const total = precio * cantidad;

    // Mostrar el total formateado en el campo
    document.getElementById('totalSubRecetaInput').value = formatoMoneda.format(total);
}

document.getElementById('precioSubRecetaInput').addEventListener('input', function () {
    calcularTotalSubReceta();
});

document.getElementById('cantidadSubRecetaInput').addEventListener('input', function () {
    calcularTotalSubReceta();
});

document.getElementById('precioSubRecetaInput').addEventListener('blur', function () {


    // Formatear el número al finalizar la edición
    this.value = formatMoneda(convertirMonedaAFloat(this.value));

    // Recalcular el total cada vez que cambia el precio
    calcularTotalSubReceta();
});

document.getElementById('Rendimiento').addEventListener('blur', function () {
    calcularDatosReceta();
});

async function calcularDatosReceta() {
    let InsumoTotal = 0;
    let SubRecetaTotal = 0;

    if (gridInsumos && gridInsumos.rows().count() > 0) {
        gridInsumos.rows().every(function () {
            const producto = this.data();
            InsumoTotal += parseFloat(producto.SubTotal);
        });
    }

    if (gridSubRecetas && gridSubRecetas.rows().count() > 0) {
        gridSubRecetas.rows().every(function () {
            const producto = this.data();
            SubRecetaTotal += parseFloat(producto.SubTotal);
        });
    }

    const CostoPorcion = SubRecetaTotal + InsumoTotal;

    // Obtener rendimiento desde input
    const rendimiento = parseFloat(document.getElementById("Rendimiento").value) || 1;

    // Calcular costo unitario redondeado
    const CostoUnitario = +(CostoPorcion / rendimiento).toFixed(2);

    // Mostrar en campos
    document.getElementById("CostoUnitario").value = formatoMoneda.format(CostoUnitario);
    document.getElementById("CostoPorcion").value = formatoMoneda.format(CostoPorcion);
    document.getElementById("costoInsumos").value = formatoMoneda.format(InsumoTotal);
    document.getElementById("costoSubRecetas").value = formatoMoneda.format(SubRecetaTotal);
}

function validarCampos() {
    const descripcion = $("#descripcion").val();

    const descripcionValida = descripcion !== "";

    $("#lblDescripcion").css("color", descripcionValida ? "" : "red");
    $("#descripcion").css("border-color", descripcionValida ? "" : "red");


    return descripcionValida;
}



function guardarCambios() {
    const idSubreceta = $("#idSubReceta").val();

    if (validarCampos()) {

        function obtenerInsumos(grd) {
            let insumos = [];
            grd.rows().every(function () {
                const insumo = this.data();
                const insumoJson = {
                    "IdSubreceta": idSubreceta !== "" ? parseInt(idSubreceta) : 0,
                    "IdInsumo": parseInt(insumo.IdInsumo),
                    "Id": insumo.Id !== "" ? parseInt(insumo.Id) : 0,
                    "Nombre": insumo.Nombre,
                    "CostoUnitario": parseFloat(insumo.CostoUnitario),
                    "SubTotal": parseFloat(insumo.SubTotal),
                    "Cantidad": parseFloat(insumo.Cantidad),
                };
                insumos.push(insumoJson);
            });
            return insumos;
        }



        function obtenerSubRecetas(grd) {
            let subrecetas = [];
            grd.rows().every(function () {
                const sub = this.data();
                const subJson = {
                    "IdSubRecetaHija": parseInt(sub.IdSubRecetaHija),
                    "Id": sub.Id !== "" ? parseInt(sub.Id) : 0,
                    "Nombre": sub.Nombre,
                    "CostoUnitario": parseFloat(sub.CostoUnitario),
                    "SubTotal": parseFloat(sub.SubTotal),
                    "Cantidad": parseFloat(sub.Cantidad),
                };
                subrecetas.push(subJson);
            });
            return subrecetas;
        }



        const insumos = obtenerInsumos(gridInsumos);
        const subrecetas = obtenerSubRecetas(gridSubRecetas);

        if (insumos.length === 0 && subrecetas.length === 0) {
            advertenciaModal("Debes agregar al menos un insumo o subreceta.");
            return;
        }

        const nuevoModelo = {
            "Id": idSubreceta !== "" ? parseInt(idSubreceta) : 0,
            "IdUnidadNegocio": parseInt($("#UnidadesNegocio").val()),
            "Descripcion": $("#descripcion").val(),
            "Sku": $("#sku").val(),
            "IdCategoria": parseInt($("#Categorias").val()),
            "IdUnidadMedida": parseInt($("#UnidadMedidas").val()),
            "CostoPorcion": parseFloat(convertirMonedaAFloat($("#CostoPorcion").val())),
            "Rendimiento": parseFloat($("#Rendimiento").val()),
            "CostoUnitario": parseFloat(convertirMonedaAFloat($("#CostoUnitario").val())),
            "CostoSubRecetas": parseFloat(convertirMonedaAFloat($("#costoSubRecetas").val())),
            "CostoInsumos": parseFloat(convertirMonedaAFloat($("#costoInsumos").val())),
            "SubrecetasInsumos": insumos,
            "SubrecetasSubrecetaIdSubrecetaPadreNavigations": subrecetas
        };

        const url = idSubreceta === "" ? "/SubRecetas/Insertar" : "/SubRecetas/Actualizar";
        const method = idSubreceta === "" ? "POST" : "PUT";

        console.log(JSON.stringify(nuevoModelo))

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
                if (dataJson.valor) {
                    const mensaje = idSubreceta === "" ? "SubReceta registrada correctamente" : "SubReceta modificada correctamente";
                    exitoModal(mensaje);
                    window.location.href = "/SubRecetas/Index";
                } else {
                    const mensaje = idSubreceta === "" ? "Ha ocurrido un error al crear la subreceta" : "Ha ocurrido un error al modificar la Subreceta";
                    errorModal(mensaje);
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });

    } else {
        errorModal("Debes completar los campos requeridos.");
    }
}
