let gridInsumos = null, gridPrefabricados = null;
$(document).ready(() => {


    listaUnidadesNegocio();
    listaCategorias();
    listaUnidadMedidas();

    if (recetaData && recetaData > 0) {
        cargarDatosReceta();
    } else {
        configurarDataTableInsumos(null);
        configurarDataTablePrefabricados(null);
    }

    



    $('#UnidadesNegocio').on('change', function () {
        // Limpiar el DataTable
        gridInsumos.clear().draw();
        gridPrefabricados.clear().draw();
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
        await configurarDataTablePrefabricados(datosReceta.Prefabricados);

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
    document.getElementById("costoPrefabricados").value = formatoMoneda.format(datos.CostoPrefabricados);
    document.getElementById("costoTotal").value = formatoMoneda.format(datos.CostoTotal);



    document.getElementById("btnNuevoModificar").textContent = "Guardar";

    await calcularDatosReceta();
}




async function configurarDataTablePrefabricados(data) {
    if (!gridPrefabricados) {
        gridPrefabricados = $('#grd_Prefabricados').DataTable({
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
                <button class='btn btn-sm btneditar btnacciones' type='button' onclick='editarPrefabricado(${row.IdPrefabricado})' title='Editar'>
                    <i class='fa fa-pencil-square-o fa-lg text-white' aria-hidden='true'></i>
                </button>
                <button class='btn btn-sm btneditar btnacciones' type='button' onclick='eliminarPrefabricado(${row.IdPrefabricado})' title='Eliminar'>
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
                    gridPrefabricados.columns.adjust();
                }, 10);


            },
        });

    } else {
        gridPrefabricados.clear().rows.add(data).draw();
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
    const PrefabricadoNombre = insumoSelect.options[insumoSelect.selectedIndex]?.text || '';


    let i = 0;


    const modal = $('#insumosModal');
    const isEditing = modal.attr('data-editing') === 'true';
    const editId = modal.attr('edit-id');

    const selectedProduct = insumos.find(p => p.IdInsumo === parseInt(InsumoId));

    // Verificar si el Prefabricado ya existe en la tabla
    let PrefabricadoExistente = false;

    if (isEditing) {
        // Si estamos editando, solo actualizamos la fila correspondiente
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == editId) {
                data.Nombre = PrefabricadoNombre;
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.Cantidad = cantidadInput; // Usar la cantidad del input
                data.SubTotal = totalInput; // Recalcular el total con formato de moneda
                this.data(data).draw();
            }
        });
    } else {
        // Buscar si el Prefabricado ya existe en la tabla
        gridInsumos.rows().every(function () {
            const data = this.data();
            if (data.IdInsumo == InsumoId) {
                // Prefabricado existe, sumamos las cantidades y recalculamos el total
                data.Cantidad = cantidadInput; // Sumar la cantidad proporcionada
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.SubTotal = precioManual * data.Cantidad; // Recalcular el total con formato de moneda
                this.data(data).draw();
                PrefabricadoExistente = true;
            }
        });

        if (!PrefabricadoExistente) {
            // Si no existe, agregar un nuevo Prefabricado
            gridInsumos.row.add({
                IdInsumo: InsumoId,
                Id: 0,
                Nombre: PrefabricadoNombre,
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


async function anadirPrefabricado() {
    const IdUnidadNegocio = $("#UnidadesNegocio").val();
    const PrefabricadosEnTabla = [];

    gridPrefabricados.rows().every(function () {
        const data = this.data();
        PrefabricadosEnTabla.push(Number(data.IdPrefabricado));
    });

    Prefabricados = await cargarPrefabricadosModal(IdUnidadNegocio, PrefabricadosEnTabla);


    const todosYaAgregados = Prefabricados.every(x => PrefabricadosEnTabla.includes(x.Id));

    if (todosYaAgregados) {
        advertenciaModal("¡Ya has agregado todos los Prefabricados de esta unidad de negocio!");
        return false; // Detener si no hay más Prefabricados para añadir
    }

    if (!Prefabricados) return;

    const PrefabricadoSelect = $("#PrefabricadoSelect");
    const precioInput = $("#precioPrefabricadoInput");
    const cantidadInput = $("#cantidadPrefabricadoInput");
    const totalInput = $("#totalPrefabricadoInput");

    PrefabricadoSelect.on("change", async function () {
        const selectedProductId = parseInt(this.value);
        const selectedProduct = Prefabricados.find(p => p.Id === selectedProductId);

        const CostoTotal = selectedProduct.CostoTotal;
        cantidadInput.val(1);
        precioInput.val(formatoMoneda.format(CostoTotal));
        totalInput.val(formatoMoneda.format(CostoTotal));
    });

    PrefabricadoSelect.trigger("change");

    $('#PrefabricadosModal').data('edit-id', null);
    $('#PrefabricadosModal').data('data-editing', false);
    $('#btnGuardarPrefabricado').text('Añadir');
    $("#PrefabricadosModal").modal('show');
}

async function cargarPrefabricadosModal(IdUnidadNegocio, PrefabricadosEnTabla, PrefabricadoSeleccionado = null) {
    Prefabricados = await obtenerPrefabricadosUnidadNegocio(IdUnidadNegocio);
    const PrefabricadoSelect = $("#PrefabricadoSelect");

    PrefabricadoSelect.empty();

    let primerHabilitadoId = null;

    Prefabricados.forEach(Prefabricado => {
        const option = $(`<option value="${Prefabricado.Id}">${Prefabricado.Descripcion}</option>`);

        if (PrefabricadosEnTabla.includes(Prefabricado.Id)) {
            option.prop('disabled', true);
        } else if (primerHabilitadoId === null) {
            primerHabilitadoId = Prefabricado.Id;
        }

        PrefabricadoSelect.append(option);
    });


    if (PrefabricadoSeleccionado !== null) {
        PrefabricadoSelect.val(PrefabricadoSeleccionado).prop("disabled", true);
    } else if (primerHabilitadoId !== null) {
        PrefabricadoSelect.val(primerHabilitadoId).prop("disabled", false);
    }

    return Prefabricados; // Devolver los Prefabricados cargados para su uso posterior
}

async function guardarPrefabricado() {
    const PrefabricadoSelect = document.getElementById('PrefabricadoSelect');
    const precioManual = parseFloat(convertirMonedaAFloat(document.getElementById('precioPrefabricadoInput').value));
    const totalInput = parseFloat(convertirMonedaAFloat(document.getElementById('totalPrefabricadoInput').value));
    const cantidadInput = parseInt(document.getElementById('cantidadPrefabricadoInput').value) || 1; // Obtener cantidad, por defecto 1 si no es válida
    const PrefabricadoId = PrefabricadoSelect.value;
    const PrefabricadoNombre = PrefabricadoSelect.options[PrefabricadoSelect.selectedIndex]?.text || '';


    let i = 0;


    const modal = $('#PrefabricadosModal');
    const isEditing = modal.attr('data-editing') === 'true';
    const editId = modal.attr('edit-id');

    const selectedProduct = Prefabricados.find(p => p.IdPrefabricado === parseInt(PrefabricadoId));

    // Verificar si el Prefabricado ya existe en la tabla
    let PrefabricadoExistente = false;

    if (isEditing) {
        // Si estamos editando, solo actualizamos la fila correspondiente
        gridPrefabricados.rows().every(function () {
            const data = this.data();
            if (data.IdPrefabricado == editId) {
                data.Nombre = PrefabricadoNombre;
                data.CostoTotal = precioManual; // Guardar PrecioVenta
                data.Cantidad = cantidadInput; // Usar la cantidad del input
                data.SubTotal = totalInput; // Recalcular el total con formato de moneda
                this.data(data).draw();
            }
        });
    } else {
        // Buscar si el Prefabricado ya existe en la tabla
        gridPrefabricados.rows().every(function () {
            const data = this.data();
            if (data.IdPrefabricado == PrefabricadoId) {
                // Prefabricado existe, sumamos las cantidades y recalculamos el total
                data.Cantidad = cantidadInput; // Sumar la cantidad proporcionada
                data.CostoUnitario = precioManual; // Guardar PrecioVenta
                data.SubTotal = precioManual * data.Cantidad; // Recalcular el total con formato de moneda
                this.data(data).draw();
                PrefabricadoExistente = true;
            }
        });

        if (!PrefabricadoExistente) {
            // Si no existe, agregar un nuevo Prefabricado
            gridPrefabricados.row.add({
                IdPrefabricado: PrefabricadoId,
                Id: 0,
                Nombre: PrefabricadoNombre,
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

async function editarPrefabricado(id) {
    const IdUnidadNegocio = parseInt($("#UnidadesNegocio").val());
    const PrefabricadosEnTabla = [];
    let PrefabricadoData = null;

    gridPrefabricados.rows().every(function () {
        const data = this.data();
        PrefabricadosEnTabla.push(Number(data.IdPrefabricado));

        if (data.IdPrefabricado == id) {
            PrefabricadoData = data;
        }
    });

    if (!PrefabricadoData) {
        advertenciaModal("No se encontró el Prefabricado a editar.");
        return;
    }

    Prefabricados = await cargarPrefabricadosModal(IdUnidadNegocio, PrefabricadosEnTabla, PrefabricadoData.IdPrefabricado);
    if (!Prefabricados) return;

    $("#cantidadPrefabricadoInput").val(PrefabricadoData.Cantidad);
    $("#precioPrefabricadoInput").val(formatoMoneda.format(PrefabricadoData.CostoUnitario));
    $("#totalPrefabricadoInput").val(formatoMoneda.format(PrefabricadoData.SubTotal));

    $("#PrefabricadoSelect").prop("disabled", true);

    $('#PrefabricadosModal').data('edit-id', PrefabricadoData.Id);
    $('#btnGuardarPrefabricado').text('Editar');
    $("#PrefabricadosModal").modal('show');
}

function eliminarPrefabricado(id) {
    gridPrefabricados.rows().every(function (rowIdx, tableLoop, rowLoop) {
        const data = this.data();
        if (data.IdPrefabricado == id) {
            gridPrefabricados.row(rowIdx).remove().draw();
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
    const url = `/PrefabricadosCategoria/Lista`;
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


async function obtenerPrefabricadosUnidadNegocio(id) {
    const url = `/Prefabricados/Lista?IdUnidadNegocio=${id}`;
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

document.querySelector('#prefabricados-tab').addEventListener('shown.bs.tab', function () {
    if (gridPrefabricados) {
        setTimeout(function () {
            gridPrefabricados.columns.adjust();
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



async function calcularTotalPrefabricado() {
    const precioRaw = document.getElementById('precioPrefabricadoInput').value;
    const cantidad = parseFloat(document.getElementById('cantidadPrefabricadoInput').value) || 0;

    // Extraer solo el número del campo precio
    const precio = formatoNumero(precioRaw);

    const total = precio * cantidad;

    // Mostrar el total formateado en el campo
    document.getElementById('totalPrefabricadoInput').value = formatoMoneda.format(total);
}

document.getElementById('precioPrefabricadoInput').addEventListener('input', function () {
    calcularTotalPrefabricado();
});

document.getElementById('cantidadPrefabricadoInput').addEventListener('input', function () {
    calcularTotalPrefabricado();
});

document.getElementById('precioPrefabricadoInput').addEventListener('blur', function () {


    // Formatear el número al finalizar la edición
    this.value = formatMoneda(convertirMonedaAFloat(this.value));

    // Recalcular el total cada vez que cambia el precio
    calcularTotalPrefabricado();
});




async function calcularDatosReceta() {
    let InsumoTotal = 0;

    if (gridInsumos != null && gridInsumos.rows().count() > 0) {
        gridInsumos.rows().every(function () {
            const producto = this.data();
            InsumoTotal += parseFloat(producto.SubTotal);
        });
    }

    let PrefabricadoTotal = 0;

    if (gridPrefabricados != null && gridPrefabricados.rows().count() > 0) {
        gridPrefabricados.rows().every(function () {
            const producto = this.data();
            PrefabricadoTotal += parseFloat(producto.SubTotal);
        });
    }

    total = PrefabricadoTotal + InsumoTotal;



    document.getElementById("costoTotal").value = formatoMoneda.format(parseFloat(total));
    document.getElementById("costoInsumos").value = formatoMoneda.format(parseFloat(InsumoTotal));
    document.getElementById("costoPrefabricados").value = formatoMoneda.format(parseFloat(PrefabricadoTotal));

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

        function obtenerPrefabricados(grd) {
            let prefabricados = [];
            grd.rows().every(function () {
                const prefabricado = this.data();
                const prefabricadoJson = {
                    "IdReceta": idReceta != "" ? idReceta : 0,
                    "IdPrefabricado": parseInt(prefabricado.IdPrefabricado),
                    "Id": prefabricado.Id != "" ? prefabricado.Id : 0,
                    "Nombre": prefabricado.Nombre,
                    "CostoUnitario": parseFloat(prefabricado.CostoUnitario),
                    "SubTotal": parseFloat(prefabricado.SubTotal),
                    "Cantidad": parseInt(prefabricado.Cantidad),

                };
                prefabricados.push(prefabricadoJson);
            });
            return prefabricados;
        }

        const insumos = obtenerInsumos(gridInsumos);
        const prefabricados = obtenerPrefabricados(gridPrefabricados);

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
            "CostoPrefabricados": parseFloat(convertirMonedaAFloat($("#costoPrefabricados").val())),
            "CostoInsumos": parseFloat(convertirMonedaAFloat($("#costoInsumos").val())),
            "RecetasInsumos": insumos,
            "RecetasPrefabricados": prefabricados
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