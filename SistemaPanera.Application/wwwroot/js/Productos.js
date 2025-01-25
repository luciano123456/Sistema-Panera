﻿let gridProductos;
let isEditing = false;


const columnConfig = [
    { index: 1, filterType: 'text' },
    { index: 2, filterType: 'text' },
    { index: 3, filterType: 'text' },
    { index: 4, filterType: 'select', fetchDataFunc: listaUnidadesNegocioFilter },
    { index: 5, filterType: 'select', fetchDataFunc: listaUnidadesMedidaFilter },
    { index: 6, filterType: 'select', fetchDataFunc: listaProductosCategoriaFilter },
    { index: 7, filterType: 'text' },
    { index: 8, filterType: 'text' },
    { index: 9, filterType: 'text' },
];

$(document).ready(() => {

    listaUnidadesNegocioFiltro();
    listaProductos(-1);

    $('#txtDescripcion, #txtCostoUnitario, #txtSku').on('input', function () {
        validarCampos()
    });


})



function guardarCambios() {
    if (validarCampos()) {
        const idProducto = $("#txtId").val();
        const nuevoModelo = {
            "Id": idProducto !== "" ? idProducto : 0,
            "Descripcion": $("#txtDescripcion").val(),
            "IdUnidadMedida": $("#UnidadesMedida").val(),
            "IdUnidadNegocio": $("#UnidadesNegocio").val(),
            "IdCategoria": $("#Categorias").val(),
            "Sku": $("#txtSku").val(),
            "CostoUnitario": $("#txtCostoUnitario").val(),
        };

        const url = idProducto === "" ? "Productos/Insertar" : "Productos/Actualizar";
        const method = idProducto === "" ? "POST" : "PUT";

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
                const mensaje = idProducto === "" ? "Producto registrado correctamente" : "Producto modificado correctamente";
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
function nuevoProducto() {
    limpiarModal();
    listaUnidadesNegocio();
    listaUnidadesMedida();
    listaProductosCategoria();
    $('#modalEdicion').modal('show');
    $("#btnGuardar").text("Registrar");
    $("#modalEdicionLabel").text("Nuevo Producto");
    $('#lblNombre').css('color', 'red');
    $('#lblDescripcion, #txtDescripcion').css('color', 'red').css('border-color', 'red');
    $('#lblSku, #txtSku').css('color', 'red').css('border-color', 'red');
    $('#lblCostoUnitario, #txtCostoUnitario').css('color', 'red').css('border-color', 'red');
}

async function mostrarModal(modelo) {
    const campos = ["Id", "Sku", "CostoUnitario", "Descripcion"];
    campos.forEach(campo => {
        $(`#txt${campo}`).val(modelo[campo]);
    });

    listaUnidadesNegocio();
    listaUnidadesMedida();
    listaProductosCategoria();

    $('#modalEdicion').modal('show');
    $("#btnGuardar").text("Guardar");
    $("#modalEdicionLabel").text("Editar Producto");

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
    listaProductos(document.getElementById("UnidadNegocioFiltro").value)
}


async function listaProductos(UnidadNegocio) {
    const url = `/Productos/Lista?IdUnidadNegocio=${UnidadNegocio}`;
    const response = await fetch(url);
    const data = await response.json();
    await configurarDataTable(data);
}

const editarProducto = id => {
    fetch("Productos/EditarInfo?id=" + id)
        .then(response => {
            if (!response.ok) throw new Error("Ha ocurrido un error.");
            return response.json();
        })
        .then(dataJson => {
            if (dataJson !== null) {
                mostrarModal(dataJson);
            } else {
                throw new Error("Ha ocurrido un error.");
            }
        })
        .catch(error => {
            errorModal("Ha ocurrido un error.");
        });
}
async function eliminarProducto(id) {
    let resultado = window.confirm("¿Desea eliminar el Producto?");

    if (resultado) {
        try {
            const response = await fetch("Productos/Eliminar?id=" + id, {
                method: "DELETE"
            });

            if (!response.ok) {
                throw new Error("Error al eliminar el Producto.");
            }

            const dataJson = await response.json();

            if (dataJson.valor) {
                aplicarFiltros();
                exitoModal("Producto eliminado correctamente")
            }
        } catch (error) {
            console.error("Ha ocurrido un error:", error);
        }
    }
}

async function configurarDataTable(data) {
    if (!gridProductos) {
        $('#grd_Productos thead tr').clone(true).addClass('filters').appendTo('#grd_Productos thead');
        gridProductos = $('#grd_Productos').DataTable({
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
                    width: "1%", // Ancho fijo para la columna
                    render: function (data) {
                        return `
                <div class="acciones-menu" data-id="${data}">
                    <button class='btn btn-sm btnacciones' type='button' onclick='toggleAcciones(${data})' title='Acciones'>
                        <i class='fa fa-ellipsis-v fa-lg text-white' aria-hidden='true'></i>
                    </button>
                    <div class="acciones-dropdown" style="display: none;">
                        <button class='btn btn-sm btneditar' type='button' onclick='editarProducto(${data})' title='Editar'>
                            <i class='fa fa-pencil-square-o fa-lg text-success' aria-hidden='true'></i> Editar
                        </button>
                        <button class='btn btn-sm btneliminar' type='button' onclick='eliminarProducto(${data})' title='Eliminar'>
                            <i class='fa fa-trash-o fa-lg text-danger' aria-hidden='true'></i> Eliminar
                        </button>
                    </div>
                </div>`;
                    },
                    orderable: false,
                    searchable: false,
                },
                { data: 'Descripcion' },
                { data: 'FechaActualizacion' },
                { data: 'Sku' },
                { data: 'UnidadNegocio' },
                { data: 'UnidadMedida' },
                { data: 'Categoria' },
                { data: 'CostoUnitario' },
                { data: 'CostoInsumos' },
                { data: 'CostoTotal' },
            ],
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: 'Exportar Excel',
                    filename: 'Reporte Productos',
                    title: '',
                    exportOptions: {
                        columns: [0, 1, 2, 3]
                    },
                    className: 'btn-exportar-excel',
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Exportar PDF',
                    filename: 'Reporte Productos',
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
                    "render": function (data, type, row) {
                        if (data) {
                            const date = new Date(data); // Convierte la cadena en un objeto Date
                            //return date.toLocaleDateString('es-ES'); // Formato: 'DD/MM/YYYY'
                            return moment(date, 'YYYY-MM-DD hh:mm').format('DD/MM/YYYY hh:mm'); // Formato: 'DD/MM/YYYY'
                        }
                    },
                    "targets": [2] // Índices de las columnas de fechas
                },
                {
                    "render": function (data, type, row) {
                        return formatNumber(data); // Formatear números
                    },
                    "targets": [7,8,9] // Índices de las columnas de números
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
                    gridProductos.columns.adjust();
                }, 10);

                $('body').on('mouseenter', '#grd_Productos .fa-map-marker', function () {
                    $(this).css('cursor', 'pointer');
                });



                $('body').on('click', '#grd_Productos .fa-map-marker', function () {
                    var locationText = $(this).parent().text().trim().replace(' ', ' '); // Obtener el texto visible
                    var url = 'https://www.google.com/maps?q=' + encodeURIComponent(locationText);
                    window.open(url, '_blank');
                });
            },
        });

    } else {
        gridProductos.clear().rows.add(data).draw();
    }
}


function configurarOpcionesColumnas() {
    const grid = $('#grd_Productos').DataTable(); // Accede al objeto DataTable utilizando el id de la tabla
    const columnas = grid.settings().init().columns; // Obtiene la configuración de columnas
    const container = $('#configColumnasMenu'); // El contenedor del dropdown específico para configurar columnas


    const storageKey = `Productos_Columnas`; // Clave única para esta pantalla

    const savedConfig = JSON.parse(localStorage.getItem(storageKey)) || {}; // Recupera configuración guardada o inicializa vacía

    container.empty(); // Limpia el contenedor

    columnas.forEach((col, index) => {
        if (col.data && col.data !== "Id") { // Solo agregar columnas que no sean "Id"
            // Recupera el valor guardado en localStorage, si existe. Si no, inicializa en 'false' para no estar marcado.
            const isChecked = savedConfig && savedConfig[`col_${index}`] !== undefined ? savedConfig[`col_${index}`] : true;

            // Asegúrate de que la columna esté visible si el valor es 'true'
            grid.column(index).visible(isChecked);

            const columnName = index != 5 ? col.data : "Direccion";

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

async function listaProductosCategoriaFilter() {
    const url = `/ProductosCategoria/Lista`;
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

async function listaProductosCategoria() {
    const data = await listaProductosCategoriaFilter();

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