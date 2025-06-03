let gridSubRecetas;
let isEditing = false;


const columnConfig = [
    { index: 1, filterType: 'text' }, // Descripción
    { index: 2, filterType: 'text' }, // SKU
    { index: 3, filterType: 'select', fetchDataFunc: listaUnidadesNegocioFilter }, // Unidad Negocio
    { index: 4, filterType: 'select', fetchDataFunc: listaUnidadesMedidaFilter }, // Unidad Medida
    { index: 5, filterType: 'select', fetchDataFunc: listaSubRecetasCategoriaFilter }, // Categoría
    { index: 6, filterType: 'text' }, // Costo SubRecetas
    { index: 7, filterType: 'text' }, // Costo Insumos
    { index: 8, filterType: 'text' }, // Rendimiento
    { index: 9, filterType: 'text' }, // Costo Unitario
    { index: 10, filterType: 'text' }, // Costo Porción
];


$(document).ready(() => {

    listaUnidadesNegocioFiltro();
    listaSubRecetas(-1);

    $('#txtDescripcion, #txtCostoUnitario, #txtSku').on('input', function () {
        validarCampos()
    });


})



function guardarCambios() {
    if (validarCampos()) {
        const idSubReceta = $("#txtId").val();
        const nuevoModelo = {
            "Id": idSubReceta !== "" ? idSubReceta : 0,
            "Descripcion": $("#txtDescripcion").val(),
            "IdUnidadMedida": $("#UnidadesMedida").val(),
            "IdUnidadNegocio": $("#UnidadesNegocio").val(),
            "IdCategoria": $("#Categorias").val(),
            "Sku": $("#txtSku").val(),
            "CostoUnitario": $("#txtCostoUnitario").val(),
        };

        const url = idSubReceta === "" ? "SubRecetas/Insertar" : "SubRecetas/Actualizar";
        const method = idSubReceta === "" ? "POST" : "PUT";

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
                const mensaje = idSubReceta === "" ? "SubReceta registrado correctamente" : "SubReceta modificado correctamente";
                $('#modalEdicion').modal('hide');
                exitoModal(mensaje);
                aplicarFiltros();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    } else {
        errorModal('Debes completar los campos requeridos');
    }
}


function validarCampos() {
    const descripcion = $("#txtDescripcion").val();
    const sku = $("#txtSku").val();
    const costoUnitario = $("#txtCostoUnitario").val();
    const campoValidoDescripcion = descripcion !== "";
    const campoValidoSku = sku !== "";
    const campoValidoCostoUnitario = costoUnitario !== "";

    $("#lblDescripcion").css("color", campoValidoDescripcion ? "" : "red");
    $("#txtDescripcion").css("border-color", campoValidoDescripcion ? "" : "red");

    $("#lblSku").css("color", campoValidoSku ? "" : "red");
    $("#txtSku").css("border-color", campoValidoSku ? "" : "red");

    $("#lblCostoUnitario").css("color", campoValidoCostoUnitario ? "" : "red");
    $("#txtCostoUnitario").css("border-color", campoValidoCostoUnitario ? "" : "red");

    return campoValidoDescripcion && campoValidoSku && campoValidoCostoUnitario;
}

async function nuevoSubReceta() {
    window.location.href = "/SubRecetas/NuevoModif";
}

async function mostrarModal(modelo) {
    const campos = ["Id", "Sku", "CostoUnitario", "Descripcion"];
    campos.forEach(campo => {
        $(`#txt${campo}`).val(modelo[campo]);
    });

    listaUnidadesNegocio();
    listaUnidadesMedida();
    listaSubRecetasCategoria();

    $('#modalEdicion').modal('show');
    $("#btnGuardar").text("Guardar");
    $("#modalEdicionLabel").text("Editar SubReceta");

    $('#lblDescripcion, #txtDescripcion').css('color', '').css('border-color', '');
    $('#lblSku, #txtSku').css('color', '').css('border-color', '');
    $('#lblCostoUnitario, #txtCostoUnitario').css('color', '').css('border-color', '');
}




function limpiarModal() {
    const campos = ["Id", "Sku", "CostoUnitario", "Descripcion"];
    campos.forEach(campo => {
        $(`#txt${campo}`).val("");
    });

    $('#lblDescripcion, #txtDescripcion').css('color', '').css('border-color', '');
    $('#lblSku, #txtSku').css('color', '').css('border-color', '');
    $('#lblCostoUnitario, #txtCostoUnitario').css('color', '').css('border-color', '');
}


async function aplicarFiltros() {
    listaSubRecetas(document.getElementById("UnidadNegocioFiltro").value)
}


async function listaSubRecetas(UnidadNegocio) {
    const url = `/SubRecetas/Lista?IdUnidadNegocio=${UnidadNegocio}`;
    const response = await fetch(url);
    const data = await response.json();
    await configurarDataTable(data);
}


function editarSubReceta(id) {
    // Redirige a la vista 'PedidoNuevoModif' con el parámetro id
    window.location.href = '/SubRecetas/NuevoModif/' + id;
}


async function eliminarSubReceta(id) {
    let resultado = window.confirm("¿Desea eliminar la Subreceta?");

    if (resultado) {
        try {
            const response = await fetch("/SubRecetas/Eliminar?id=" + id, {
                method: "DELETE"
            });

            if (!response.ok) {
                throw new Error("Error al eliminar la SubReceta.");
            }
            const dataJson = await response.json();

            if (dataJson.valor) {
                aplicarFiltros();
                exitoModal(dataJson.mensaje);
            } else {
                advertenciaModal(dataJson.mensaje);
            }

        } catch (error) {
            console.error("Ha ocurrido un error:", error);
        }
    }
}

async function configurarDataTable(data) {
    if (!gridSubRecetas) {
        $('#grd_SubRecetas thead tr').clone(true).addClass('filters').appendTo('#grd_SubRecetas thead');
        gridSubRecetas = $('#grd_SubRecetas').DataTable({
            data: data,
            language: {
                sLengthMenu: "Mostrar MENU registros",
                lengthMenu: "Anzeigen von _MENU_ Einträgen",
                url: "//cdn.datatables.net/plug-ins/2.0.7/i18n/es-MX.json"
            },
            scrollX: "100px",
            scrollCollapse: true,
            columns: [
                {
                    data: "Id",
                    title: '',
                    width: "1%",
                    render: function (data) {
                        return `
                <div class="acciones-menu" data-id="${data}">
                    <button class='btn btn-sm btnacciones' type='button' onclick='toggleAcciones(${data})'>
                        <i class='fa fa-ellipsis-v fa-lg text-white'></i>
                    </button>
                    <div class="acciones-dropdown" style="display: none;">
                        <button class='btn btn-sm btneditar' onclick='editarSubReceta(${data})'><i class='fa fa-pencil-square-o text-success'></i> Editar</button>
                        <button class='btn btn-sm btneliminar' onclick='eliminarSubReceta(${data})'><i class='fa fa-trash-o text-danger'></i> Eliminar</button>
                    </div>
                </div>`;
                    },
                    orderable: false,
                    searchable: false,
                },
                { data: 'Descripcion', title: 'Descripción' },
               
                { data: 'Sku', title: 'SKU' },
                { data: 'UnidadNegocio', title: 'Unidad Negocio' },
                { data: 'UnidadMedida', title: 'Unidad Medida' },
                { data: 'Categoria', title: 'Categoría' },
                { data: 'CostoSubRecetas', title: 'Costo SubRecetas' },
                { data: 'CostoInsumos', title: 'Costo Insumos' },
                { data: 'Rendimiento', title: 'Rendimiento' },
                { data: 'CostoUnitario', title: 'Costo Unitario' },
                { data: 'CostoPorcion', title: 'Costo Porción' },
            ],

            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: 'Exportar Excel',
                    filename: 'Reporte SubRecetas',
                    title: '',
                    exportOptions: {
                        columns: [0, 1, 2, 3]
                    },
                    className: 'btn-exportar-excel',
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Exportar PDF',
                    filename: 'Reporte SubRecetas',
                    title: '',
                    exportOptions: {
                        columns: [0, 1, 2, 3]
                    },
                    className: 'btn-exportar-pdf',
                },
                {
                    extend: 'print',
                    text: 'Imprimir',
                    title: '',
                    exportOptions: {
                        columns: [0, 1, 2, 3]
                    },
                    className: 'btn-exportar-print'
                },
                'pageLength'
            ],
            orderCellsTop: true,
            fixedHeader: true,

            "columnDefs": [
               
                {
                    "render": function (data) {
                        return formatNumber(data);
                    },
                    "targets": [6, 7, 9, 10] // CostoSubRecetas, CostoInsumos, Rendimiento, CostoUnitario, CostoPorcion
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

                configurarOpcionesColumnas();

                setTimeout(function () {
                    gridSubRecetas.columns.adjust();
                }, 10);

                // Cambiar el cursor a 'pointer' cuando pase sobre cualquier fila o columna
                $('#grd_SubRecetas tbody').on('mouseenter', 'tr', function () {
                    $(this).css('cursor', 'pointer');
                });

                // Doble clic para ejecutar la función editarPedido(id)
                $('#grd_SubRecetas tbody').on('dblclick', 'tr', function () {
                    var id = gridSubRecetas.row(this).data().Id; // Obtener el ID de la fila seleccionada
                    editarSubReceta(id); // Llamar a la función de editar
                });

                let filaSeleccionada = null; // Variable para almacenar la fila seleccionada
                $('#grd_SubRecetas tbody').on('click', 'tr', function () {
                    // Remover la clase de la fila anteriormente seleccionada
                    if (filaSeleccionada) {
                        $(filaSeleccionada).removeClass('seleccionada');
                        $('td', filaSeleccionada).removeClass('seleccionada');

                    }

                    // Obtener la fila actual
                    filaSeleccionada = $(this);

                    // Agregar la clase a la fila actual
                    $(filaSeleccionada).addClass('seleccionada');
                    $('td', filaSeleccionada).addClass('seleccionada');

                });



                $('body').on('click', '#grd_SubRecetas .fa-map-marker', function () {
                    var locationText = $(this).parent().text().trim().replace(' ', ' '); // Obtener el texto visible
                    var url = 'https://www.google.com/maps?q=' + encodeURIComponent(locationText);
                    window.open(url, '_blank');
                });
            },
        });

    } else {
        gridSubRecetas.clear().rows.add(data).draw();
    }
}


function configurarOpcionesColumnas() {
    const grid = $('#grd_SubRecetas').DataTable(); // Accede al objeto DataTable utilizando el id de la tabla
    const columnas = grid.settings().init().columns; // Obtiene la configuración de columnas
    const container = $('#configColumnasMenu'); // El contenedor del dropdown específico para configurar columnas


    const storageKey = `SubRecetas_Columnas`; // Clave única para esta pantalla

    const savedConfig = JSON.parse(localStorage.getItem(storageKey)) || {}; // Recupera configuración guardada o inicializa vacía

    container.empty(); // Limpia el contenedor

    columnas.forEach((col, index) => {
        if (col.data && col.data !== "Id") { // Solo agregar columnas que no sean "Id"
            // Recupera el valor guardado en localStorage, si existe. Si no, inicializa en 'false' para no estar marcado.
            const isChecked = savedConfig && savedConfig[`col_${index}`] !== undefined ? savedConfig[`col_${index}`] : true;

            // Asegúrate de que la columna esté visible si el valor es 'true'
            grid.column(index).visible(isChecked);

            const columnName = col.data;

            // Ahora agregamos el checkbox, asegurándonos de que se marque solo si 'isChecked' es 'true'
            container.append(`
                <li>
                    <label class="dropdown-item">
                        <input type="checkbox" class="toggle-column" data-column="${index}" ${isChecked ? 'checked' : ''}>
                        ${columnName}
                    </label>
                </li>
            `);
        }
    });

    // Asocia el evento para ocultar/mostrar columnas
    $('.toggle-column').on('change', function () {
        const columnIdx = parseInt($(this).data('column'), 10);
        const isChecked = $(this).is(':checked');
        savedConfig[`col_${columnIdx}`] = isChecked;
        localStorage.setItem(storageKey, JSON.stringify(savedConfig));
        grid.column(columnIdx).visible(isChecked);
    });
}

$(document).on('click', function (e) {
    // Verificar si el clic está fuera de cualquier dropdown
    if (!$(e.target).closest('.acciones-menu').length) {
        $('.acciones-dropdown').hide(); // Cerrar todos los dropdowns
    }
});


async function listaUnidadesNegocioFilter() {
    const url = `/UnidadesNegocio/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

}


async function listaUnidadesMedidaFilter() {
    const url = `/UnidadesMedida/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

}

async function listaSubRecetasCategoriaFilter() {
    const url = `/SubRecetasCategoria/Lista`;
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

async function listaUnidadesMedida() {
    const data = await listaUnidadesMedidaFilter();

    $('#UnidadesMedida option').remove();

    select = document.getElementById("UnidadesMedida");

    for (i = 0; i < data.length; i++) {
        option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);

    }
}

async function listaSubRecetasCategoria() {
    const data = await listaSubRecetasCategoriaFilter();

    $('#Categorias option').remove();

    select = document.getElementById("Categorias");

    for (i = 0; i < data.length; i++) {
        option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);

    }
}





async function listaUnidadesNegocioFiltro() {
    const data = await listaUnidadesNegocioFilter();

    $('#UnidadNegocioFiltro option').remove();

    select = document.getElementById("UnidadNegocioFiltro");

    option = document.createElement("option");
    option.value = -1;
    option.text = "-";
    select.appendChild(option);

    for (i = 0; i < data.length; i++) {
        option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);

    }
}