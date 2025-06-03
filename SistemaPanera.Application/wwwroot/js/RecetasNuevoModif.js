let gridInsumos = null, gridRecetas = null;
$(document).ready(() => {


    listaUnidadesNegocio();
    listaCategorias();
    listaUnidadMedidas();

    if (RecetaData && RecetaData > 0) {
        cargarDatosReceta();
    } else {
        configurarDataTableInsumos(null);
        configurarDataTableSubrecetas(null);
        $("#tituloReceta").text("Nueva Receta");
    }

    



    $('#UnidadesNegocio').on('change', function () {
        // Limpiar el DataTable
        gridInsumos.clear().draw();
        gridRecetas.clear().draw();
        calcularDatosReceta();
    });

    $('#descripcion').on('input', function () {
        validarCampos();
    });

    validarCampos();

    $("#RecetaSelect").select2({
        dropdownParent: $("#RecetasModalLabel"), // Asegura que el dropdown se muestre dentro del modal
        width: "100%",
        placeholder: "Selecciona una opción",
        allowClear: false
    });

    $("#insumoSelect").select2({
        dropdownParent: $("#insumosModalLabel"), // Asegura que el dropdown se muestre dentro del modal
        width: "100%",
        placeholder: "Selecciona una opción",
        allowClear: false
    });


})

async function ObtenerDatosReceta(id) {
    const url = `/Recetas/EditarInfo?id=${id}`;
    const response = await fetch(url);
    const data = await response.json();
    return data;
}


async function cargarDatosReceta() {
    if (RecetaData && RecetaData > 0) {

        const datosReceta = await ObtenerDatosReceta(RecetaData);
        await insertarDatosReceta(datosReceta.Receta);
        await configurarDataTableInsumos(datosReceta.Insumos);
        await configurarDataTableSubrecetas(datosReceta.SubRecetas);



        calcularDatosReceta();

        validarCampos();
    }
}


async function insertarDatosReceta(datos) {

    document.getElementById("idReceta").value = datos.Id;

    document.getElementById("UnidadesNegocio").value = datos.IdUnidadNegocio;
    document.getElementById("descripcion").value = datos.Descripcion;
    document.getElementById("sku").value = datos.Sku;
    document.getElementById("Categorias").value = datos.IdCategoria;
    document.getElementById("UnidadMedidas").value = datos.IdUnidadMedida;

    document.getElementById("costoInsumos").value = formatoMoneda.format(datos.CostoInsumos ?? 0);
    document.getElementById("costoRecetas").value = formatoMoneda.format(datos.CostoRecetas ?? 0);
    document.getElementById("CostoPorcion").value = formatoMoneda.format(datos.CostoPorcion ?? 0);
    document.getElementById("Rendimiento").value = datos.Rendimiento ?? 0;
    document.getElementById("CostoUnitario").value = formatoMoneda.format(datos.CostoUnitario ?? 0);

    document.getElementById("btnNuevoModificar").textContent = "Guardar";

    $("#tituloReceta").text("Editar Receta");


    await calcularDatosReceta(); // si esto depende del rendimiento o costos, mantenelo
}




async function configurarDataTableSubrecetas(data) {
    if (!gridRecetas) {
        gridRecetas = $('#grd_Subrecetas').DataTable({
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
<button class='btn btn-sm btneditar btnacciones' type='button' onclick='editarSubreceta(${row.IdSubReceta})' title='Editar'>
    <i class='fa fa-pencil-square-o fa-lg text-white' aria-hidden='true'></i>
</button>
<button class='btn btn-sm btneditar btnacciones' type='button' onclick='eliminarSubreceta(${row.IdSubReceta})' title='Eliminar'>
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
                    gridRecetas.columns.adjust();
                }, 10);


            },
        });

    } else {
        gridRecetas.clear().rows.add(data).draw();
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
        // Si ya está en la tabla y no es el que estamos editando, no lo mostramos
        if (insumosEnTabla.includes(insumo.Id) && insumo.Id !== insumoSeleccionado) return;

        const option = $(`<option value="${insumo.Id}">${insumo.Descripcion}</option>`);

        if (insumo.Id === insumoSeleccionado) {
            option.prop("selected", true).prop("disabled", true);
        } else if (primerHabilitadoId === null) {
            primerHabilitadoId = insumo.Id;
        }

        insumoSelect.append(option);
    });

    // Si no se está editando, seleccionar el primer habilitado
    if (insumoSeleccionado === null && primerHabilitadoId !== null) {
        insumoSelect.val(primerHabilitadoId).prop("disabled", false);
    }

    return insumos; // Devolver los insumos cargados para su uso posterior
}

async function guardarInsumo() {
    const insumoSelect = document.getElementById('insumoSelect');
    const precioManual = parseFloat(convertirMonedaAFloat(document.getElementById('precioInput').value));
    const totalInput = parseFloat(convertirMonedaAFloat(document.getElementById('totalInput').value));
    const cantidadInput = parseFloat(document.getElementById('cantidadInput').value) || 1; // Obtener cantidad, por defecto 1 si no es válida
    const InsumoId = insumoSelect.value;
    const RecetaNombre = insumoSelect.options[insumoSelect.selectedIndex]?.text || '';


    let i = 0;


    const modal = $('#insumosModal');
    const isEditing = modal.attr('data-editing') === 'true';
    const editId = modal.attr('edit-id');

    const selectedProduct = insumos.find(p => p.IdInsumo === parseInt(InsumoId));

    // Verificar si el Receta ya existe en la tabla
    let RecetaExistente = false;

    if (isEditing) {
        // Si estamos editando, solo actualizamos la fila correspondiente
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == editId) {
                data.Nombre = RecetaNombre;
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.Cantidad = cantidadInput; // Usar la cantidad del input
                data.SubTotal = totalInput; // Recalcular el total con formato de moneda
                this.data(data).draw();
            }
        });
    } else {
        // Buscar si el Receta ya existe en la tabla
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == InsumoId) {
                // Receta existe, sumamos las cantidades y recalculamos el total
                data.Cantidad = cantidadInput; // Sumar la cantidad proporcionada
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.SubTotal = precioManual * data.Cantidad; // Recalcular el total con formato de moneda
                this.data(data).draw();
                RecetaExistente = true;
            }
        });

        if (!RecetaExistente) {
            // Si no existe, agregar un nuevo Receta
            gridInsumos.row.add({
                IdInsumo: InsumoId,
                Id: 0,
                Nombre: RecetaNombre,
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


async function anadirSubreceta() {
    const IdUnidadNegocio = $("#UnidadesNegocio").val();
    const SubrecetasEnTabla = [];

    gridRecetas.rows().every(function () {
        const data = this.data();
        SubrecetasEnTabla.push(Number(data.IdSubReceta));
    });

    const Subrecetas = await cargarSubrecetasModal(IdUnidadNegocio, SubrecetasEnTabla);

    if (!Subrecetas) return;

    const todosYaAgregados = Subrecetas.every(x => SubrecetasEnTabla.includes(x.Id));

    if (todosYaAgregados) {
        advertenciaModal("¡Ya has agregado todas las subrecetas de esta unidad de negocio!");
        return false;
    }

    const SubrecetaSelect = $("#RecetaSelect");
    const precioInput = $("#precioSubrecetaInput");
    const cantidadInput = $("#cantidadRecetaInput");
    const totalInput = $("#totalRecetaInput");

    SubrecetaSelect.off("change").on("change", function () {
        const selectedProductId = parseInt(this.value);
        const selectedProduct = Subrecetas.find(p => p.Id === selectedProductId);

        const CostoUnitario = selectedProduct?.CostoUnitario || 0;
        cantidadInput.val(1);
        precioInput.val(formatoMoneda.format(CostoUnitario));
        totalInput.val(formatoMoneda.format(CostoUnitario));
    });

    SubrecetaSelect.trigger("change");

    $('#RecetasModal').data('edit-id', null);
    $('#RecetasModal').data('data-editing', false);
    $('#btnGuardarReceta').text('Añadir');
    $("#RecetasModal").modal('show');
}

async function cargarSubrecetasModal(IdUnidadNegocio, RecetasEnTabla, RecetaSeleccionado = null) {
    Recetas = await obtenerSubrecetasUnidadNegocio(IdUnidadNegocio);
    const RecetaSelect = $("#RecetaSelect");

    RecetaSelect.empty();

    let primerHabilitadoId = null;

    Recetas.forEach(Receta => {
        // Si estoy editando, permito que se muestre solo la Receta seleccionada
        if (RecetasEnTabla.includes(Receta.Id) && Receta.Id !== RecetaSeleccionado) return;

        const option = $(`<option value="${Receta.Id}">${Receta.Descripcion}</option>`);

        if (Receta.Id === RecetaSeleccionado) {
            option.prop("selected", true).prop("disabled", true);
        } else if (primerHabilitadoId === null) {
            primerHabilitadoId = Receta.Id;
        }

        RecetaSelect.append(option);
    });

    // Si no estoy editando, selecciono el primero habilitado
    if (RecetaSeleccionado === null && primerHabilitadoId !== null) {
        RecetaSelect.val(primerHabilitadoId).prop("disabled", false);
    }

    return Recetas;
}

async function guardarSubreceta() {
    const SubRecetaSelect = document.getElementById('RecetaSelect');
    const precioManual = parseFloat(convertirMonedaAFloat(document.getElementById('precioSubrecetaInput').value));
    const totalInput = parseFloat(convertirMonedaAFloat(document.getElementById('totalRecetaInput').value));
    const cantidadInput = parseFloat(document.getElementById('cantidadRecetaInput').value) || 1;
    const IdSubReceta = parseInt(SubRecetaSelect.value);
    const NombreSubReceta = SubRecetaSelect.options[SubRecetaSelect.selectedIndex]?.text || '';

    const modal = $('#RecetasModal');
    const isEditing = modal.data('data-editing') === true;
    const editId = parseInt(modal.data('edit-id')) || 0;
    const editKey = modal.data('edit-key') || null;

    let existeSubReceta = false;

    if (isEditing) {
        gridRecetas.rows().every(function () {
            const data = this.data();

            const matchById = data.Id === editId;
            const matchByKey = !data.Id && data.__keyTempId && data.__keyTempId === editKey;

            if (matchById || matchByKey) {
                data.IdSubReceta = IdSubReceta;
                data.Nombre = NombreSubReceta;
                data.CostoUnitario = precioManual;
                data.Cantidad = cantidadInput;
                data.SubTotal = totalInput;
                this.data(data).draw();
                existeSubReceta = true;
            }
        });
    } else {
        gridRecetas.rows().every(function () {
            const data = this.data();
            if (parseInt(data.IdSubReceta) === IdSubReceta) {
                data.Cantidad += cantidadInput;
                data.CostoUnitario = precioManual;
                data.SubTotal = precioManual * data.Cantidad;
                this.data(data).draw();
                existeSubReceta = true;
            }
        });

        if (!existeSubReceta) {
            gridRecetas.row.add({
                Id: 0,
                IdSubReceta: IdSubReceta,
                Nombre: NombreSubReceta,
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

async function editarSubreceta(id) {
    const IdUnidadNegocio = parseInt($("#UnidadesNegocio").val());
    const SubrecetasEnTabla = [];
    let subreceta = null;

    gridRecetas.rows().every(function () {
        const data = this.data();
        SubrecetasEnTabla.push(Number(data.IdSubReceta));

        if (parseInt(data.IdSubReceta) === id) {
            subreceta = data;
        }
    });

    if (!subreceta) {
        advertenciaModal("No se encontró la subreceta a editar.");
        return;
    }

    const Recetas = await cargarSubrecetasModal(IdUnidadNegocio, SubrecetasEnTabla, subreceta.IdSubReceta);
    if (!Recetas) return;

    $("#cantidadRecetaInput").val(subreceta.Cantidad);
    $("#precioSubrecetaInput").val(formatoMoneda.format(subreceta.CostoUnitario));
    $("#totalRecetaInput").val(formatoMoneda.format(subreceta.SubTotal));

    $("#RecetaSelect").prop("disabled", true);

    $('#RecetasModal').data('edit-id', subreceta.Id || 0);
    $('#RecetasModal').data('edit-key', subreceta.__keyTempId || null);
    $('#RecetasModal').data('data-editing', true);
    $('#btnGuardarReceta').text('Editar');
    $("#RecetasModal").modal('show');
}

function eliminarSubreceta(id) {
    gridRecetas.rows().every(function (rowIdx, tableLoop, rowLoop) {
        const data = this.data();
        if (data != null && data.IdSubReceta == id) {
            gridRecetas.row(rowIdx).remove().draw();
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
    const url = `/RecetasCategoria/Lista`;
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

document.querySelector('#Receta-tab').addEventListener('shown.bs.tab', function () {
    if (gridRecetas) {
        setTimeout(function () {
            gridRecetas.columns.adjust();
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



async function calcularTotalReceta() {
    const precioRaw = document.getElementById('precioSubrecetaInput').value;
    const cantidad = parseFloat(document.getElementById('cantidadRecetaInput').value) || 0;

    // Extraer solo el número del campo precio
    const precio = formatoNumero(precioRaw);

    const total = precio * cantidad;

    // Mostrar el total formateado en el campo
    document.getElementById('totalRecetaInput').value = formatoMoneda.format(total);
}

document.getElementById('precioSubrecetaInput').addEventListener('input', function () {
    calcularTotalReceta();
});

document.getElementById('cantidadRecetaInput').addEventListener('input', function () {
    calcularTotalReceta();
});

document.getElementById('precioSubrecetaInput').addEventListener('blur', function () {


    // Formatear el número al finalizar la edición
    this.value = formatMoneda(convertirMonedaAFloat(this.value));

    // Recalcular el total cada vez que cambia el precio
    calcularTotalReceta();
});

document.getElementById('Rendimiento').addEventListener('blur', function () {
    calcularDatosReceta();
});

async function calcularDatosReceta() {
    let InsumoTotal = 0;
    let RecetaTotal = 0;

    if (gridInsumos && gridInsumos.rows().count() > 0) {
        gridInsumos.rows().every(function () {
            const producto = this.data();
            InsumoTotal += parseFloat(producto.SubTotal);
        });
    }

    if (gridRecetas && gridRecetas.rows().count() > 0) {
        gridRecetas.rows().every(function () {
            const producto = this.data();
            RecetaTotal += parseFloat(producto.SubTotal);
        });
    }

    const CostoPorcion = RecetaTotal + InsumoTotal;

    // Obtener rendimiento desde input
    const rendimiento = parseFloat(document.getElementById("Rendimiento").value) || 1;

    // Calcular costo unitario redondeado
    const CostoUnitario = +(CostoPorcion / rendimiento).toFixed(2);

    // Mostrar en campos
    document.getElementById("CostoUnitario").value = formatoMoneda.format(CostoUnitario);
    document.getElementById("CostoPorcion").value = formatoMoneda.format(CostoPorcion);
    document.getElementById("costoInsumos").value = formatoMoneda.format(InsumoTotal);
    document.getElementById("costoRecetas").value = formatoMoneda.format(RecetaTotal);
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
                insumos.push({
                    "IdReceta": idReceta !== "" ? parseInt(idReceta) : 0,
                    "IdInsumo": parseInt(insumo.IdInsumo),
                    "Id": insumo.Id !== "" ? parseInt(insumo.Id) : 0,
                    "Nombre": insumo.Nombre,
                    "CostoUnitario": parseFloat(insumo.CostoUnitario),
                    "SubTotal": parseFloat(insumo.SubTotal),
                    "Cantidad": parseFloat(insumo.Cantidad)
                });
            });
            return insumos;
        }

        function obtenerSubrecetas(grd) {
            let subrecetas = [];
            grd.rows().every(function () {
                const sub = this.data();
                subrecetas.push({
                    "IdSubReceta": parseInt(sub.IdSubReceta),
                    "Id": sub.Id !== "" ? parseInt(sub.Id) : 0,
                    "Nombre": sub.Nombre,
                    "CostoUnitario": parseFloat(sub.CostoUnitario),
                    "SubTotal": parseFloat(sub.SubTotal),
                    "Cantidad": parseFloat(sub.Cantidad)
                });
            });
            return subrecetas;
        }

        const insumos = obtenerInsumos(gridInsumos);
        const subrecetas = obtenerSubrecetas(gridRecetas);

        if (insumos.length === 0 && subrecetas.length === 0) {
            advertenciaModal("Debes agregar al menos un insumo o subreceta.");
            return;
        }

        const nuevoModelo = {
            "Id": idReceta !== "" ? parseInt(idReceta) : 0,
            "IdUnidadNegocio": parseInt($("#UnidadesNegocio").val()),
            "Descripcion": $("#descripcion").val(),
            "Sku": $("#sku").val(),
            "IdCategoria": parseInt($("#Categorias").val()),
            "IdUnidadMedida": parseInt($("#UnidadMedidas").val()),
            "CostoPorcion": parseFloat(convertirMonedaAFloat($("#CostoPorcion").val())),
            "Rendimiento": parseFloat($("#Rendimiento").val()),
            "CostoUnitario": parseFloat(convertirMonedaAFloat($("#CostoUnitario").val())),
            "CostoSubRecetas": parseFloat(convertirMonedaAFloat($("#costoRecetas").val())),
            "CostoInsumos": parseFloat(convertirMonedaAFloat($("#costoInsumos").val())),
            "RecetasInsumos": insumos,
            "RecetasSubreceta": subrecetas
        };

        const url = idReceta === "" ? "/Recetas/Insertar" : "/Recetas/Actualizar";
        const method = idReceta === "" ? "POST" : "PUT";

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
                    const mensaje = idReceta === "" ? "Receta registrada correctamente" : "Receta modificada correctamente";
                    exitoModal(mensaje);
                    window.location.href = "/Recetas/Index";
                } else {
                    const mensaje = idReceta === "" ? "Ha ocurrido un error al crear la Receta" : "Ha ocurrido un error al modificar la Receta";
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
