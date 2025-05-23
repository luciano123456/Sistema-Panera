let gridInsumos;
let isEditing = false;


const columnConfig = [
    { index: 1, filterType: 'text' },
    { index: 2, filterType: 'text' },
    { index: 3, filterType: 'text' },
    { index: 4, filterType: 'text' },
    { index: 5, filterType: 'text' },

];



$(document).ready(() => {

    listaProveedoresFiltro();
    listaInsumos(-1);


    $("#Proveedores").select2({
        dropdownParent: $("#modalEdicion"),
        width: "100%",
        placeholder: "Selecciona una opción",
        allowClear: false
    }).on('change', function () {
        validarCampoIndividual(this);
    }).on('select2:close', function () {
        validarCampoIndividual(this);
    });




    document.querySelectorAll("#formInsumo input, #formInsumo select, #formInsumo textarea #formInsumo").forEach(el => {
        el.setAttribute("autocomplete", "off");
        el.addEventListener("input", () => validarCampoIndividual(el));
        el.addEventListener("change", () => validarCampoIndividual(el));
        el.addEventListener("blur", () => validarCampoIndividual(el));
    });


})




function guardarCambios() {
    if (!validarCampos()) return;

    const idInsumo = $("#txtId").val();

    const nuevoModelo = {
        Id: idInsumo !== "" ? parseInt(idInsumo) : 0,
        Codigo: $("#txtCodigo").val(),
        Descripcion: $("#txtDescripcion").val(),
        CostoUnitario: $("#txtCostoUnitario").val(),
        IdProveedor: parseInt($("#Proveedores").val()),
        
    };

    const url = idInsumo === "" ? "ProveedoresInsumos/Insertar" : "ProveedoresInsumos/Actualizar";
    const method = idInsumo === "" ? "POST" : "PUT";

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
            const mensaje = idInsumo === "" ? "Insumo registrado correctamente" : "Insumo modificado correctamente";
            $('#modalEdicion').modal('hide');
            exitoModal(mensaje);
            aplicarFiltros();
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function nuevoInsumo() {
    limpiarModal();
    listaProveedores()
    $('#modalEdicion').modal('show');
    $("#btnGuardar").text("Registrar");
    $("#modalEdicionLabel").text("Nuevo Insumo");

}

async function mostrarModal(modelo) {

    limpiarModal();

    await listaProveedores();

    const campos = ["Id", "Codigo", "Descripcion", "CostoUnitario"];
    campos.forEach(campo => {
        $(`#txt${campo}`).val(modelo[campo]);
    });

    $("#Proveedores").val(modelo.IdProveedor).trigger("change.select2");


    $('#modalEdicion').modal('show');
    $("#btnGuardar").text("Guardar");
    $("#modalEdicionLabel").text("Editar Insumo");
}




async function aplicarFiltros() {
    listaInsumos(document.getElementById("ProveedorFiltro").value)
}


async function listaInsumos(IdProveedor) {
    const url = `/ProveedoresInsumos/Lista?IdProveedor=${IdProveedor}`;
    const response = await fetch(url);
    const data = await response.json();
    await configurarDataTable(data);
}

const editarInsumo = id => {
    fetch("ProveedoresInsumos/EditarInfo?id=" + id)
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
async function eliminarInsumo(id) {
    let resultado = window.confirm("¿Desea eliminar el Insumo?");

    if (resultado) {
        try {
            const response = await fetch("Insumos/Eliminar?id=" + id, {
                method: "DELETE"
            });

            if (!response.ok) {
                throw new Error("Error al eliminar el Insumo.");
            }

            const dataJson = await response.json();

            if (dataJson.valor) {
                aplicarFiltros();
                exitoModal("Insumo eliminado correctamente")
            }
        } catch (error) {
            console.error("Ha ocurrido un error:", error);
        }
    }
}

async function configurarDataTable(data) {
    if (!gridInsumos) {
        $('#grd_InsumosProveedor thead tr').clone(true).addClass('filters').appendTo('#grd_InsumosProveedor thead');
        gridInsumos = $('#grd_InsumosProveedor').DataTable({
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
                        <button class='btn btn-sm btneditar' type='button' onclick='editarInsumo(${data})' title='Editar'>
                            <i class='fa fa-pencil-square-o fa-lg text-success' aria-hidden='true'></i> Editar
                        </button>
                        <button class='btn btn-sm btneliminar' type='button' onclick='eliminarInsumo(${data})' title='Eliminar'>
                            <i class='fa fa-trash-o fa-lg text-danger' aria-hidden='true'></i> Eliminar
                        </button>
                    </div>
                </div>`;
                    },
                    orderable: false,
                    searchable: false,
                },
                { data: 'Codigo' },
                { data: 'Descripcion' },
                { data: 'CostoUnitario' },
                { data: 'Proveedor' },
                { data: 'FechaActualizacion' },
                
            ],
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: 'Exportar Excel',
                    filename: 'Reporte Insumos',
                    title: '',
                    exportOptions: {
                        columns: [0, 1, 2, 3]
                    },
                    className: 'btn-exportar-excel',
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Exportar PDF',
                    filename: 'Reporte Insumos',
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
                    "targets": [5] // Índices de las columnas de fechas 
                },
                //{
                //    "render": function (data, type, row) {
                //        return formatNumber(data); // Formatear números
                //    },
                //    "targets": [7] // Índices de las columnas de números
                //},

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
                    gridInsumos.columns.adjust();
                }, 10);

                $('body').on('click', '#grd_InsumosProveedor .fa-map-marker', function () {
                    var locationText = $(this).parent().text().trim().replace(' ', ' '); // Obtener el texto visible
                    var url = 'https://www.google.com/maps?q=' + encodeURIComponent(locationText);
                    window.open(url, '_blank');
                });
            },
        });

    } else {
        gridInsumos.clear().rows.add(data).draw();
    }
}


function configurarOpcionesColumnas() {
    const grid = $('#grd_InsumosProveedor').DataTable(); // Accede al objeto DataTable utilizando el id de la tabla
    const columnas = grid.settings().init().columns; // Obtiene la configuración de columnas
    const container = $('#configColumnasMenu'); // El contenedor del dropdown específico para configurar columnas


    const storageKey = `InsumosProveedor_Columnas`; // Clave única para esta pantalla

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

async function listaProveedoresFilter() {
    const url = `/Proveedores/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

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

async function listaUnidadesMedidaFilter() {
    const url = `/UnidadesMedida/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

}

async function listaInsumosCategoriaFilter() {
    const url = `/InsumosCategoria/Lista`;
    const response = await fetch(url);
    const data = await response.json();

    return data.map(x => ({
        Id: x.Id,
        Nombre: x.Nombre
    }));

}




async function listaUnidadesMedida() {
    const data = await listaUnidadesMedidaFilter();

    $('#UnidadesMedida option').remove();

    select = document.getElementById("UnidadesMedida");

    // Opción por defecto (seleccionada y deshabilitada)
    const optionDefault = document.createElement("option");
    optionDefault.value = "";
    optionDefault.text = "Seleccionar";
    optionDefault.disabled = true;
    optionDefault.selected = true;
    select.appendChild(optionDefault);

    for (i = 0; i < data.length; i++) {
        option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);

    }
}

async function listaProveedores() {
    const data = await listaProveedoresFilter();

    $('#Proveedores option').remove();

    const select = document.getElementById("Proveedores");

    // Opción por defecto (seleccionada y deshabilitada)
    const optionDefault = document.createElement("option");
    optionDefault.value = "";
    optionDefault.text = "Seleccionar";
    optionDefault.disabled = true;
    optionDefault.selected = true;
    select.appendChild(optionDefault);

    // Cargar opciones dinámicas
    for (let i = 0; i < data.length; i++) {
        const option = document.createElement("option");
        option.value = data[i].Id;
        option.text = data[i].Nombre;
        select.appendChild(option);
    }
}


async function listaProveedoresFiltro() {
    const data = await listaProveedoresFilter();

    $('#ProveedorFiltro option').remove();

    select = document.getElementById("ProveedorFiltro");

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

function limpiarModal() {
    const formulario = document.querySelector("#formInsumo");
    if (!formulario) return;

    formulario.querySelectorAll("input, select, textarea").forEach(el => {
        if (el.tagName === "SELECT") {
            el.selectedIndex = 0;
        } else {
            el.value = "";
        }
        el.classList.remove("is-invalid", "is-valid");
    });

    // Reset select2 de Proveedores
    const proveedores = document.getElementById("Proveedores");
    if (proveedores) {
        $(proveedores).val("").trigger("change");
        $(proveedores).next(".select2-container").removeClass("is-valid is-invalid");
    }

    // Ocultar mensaje general de error
    const errorMsg = document.getElementById("errorCampos");
    if (errorMsg) errorMsg.classList.add("d-none");
}



function validarCampoIndividual(el) {
    const tag = el.tagName.toLowerCase();
    const id = el.id;
    const valor = el.value ? el.value.trim() : "";
    const feedback = el.nextElementSibling;

    // Validar campos numéricos específicos
    const camposNumericos = ["txtCostoUnitario"];

    const esNumero = camposNumericos.includes(id);

    if (tag === "input" || tag === "select" || tag === "textarea") {
        if (feedback && feedback.classList.contains("invalid-feedback")) {
            feedback.textContent = "Campo obligatorio";
        }

        if (valor === "" || valor === "Seleccionar") {
            el.classList.remove("is-valid");
            el.classList.add("is-invalid");
        } else if (esNumero) {
            const sinFormato = valor.replace(/[^\d,.-]/g, "").replace(/\./g, "").replace(",", ".");
            if (isNaN(parseFloat(sinFormato))) {
                el.classList.remove("is-valid");
                el.classList.add("is-invalid");
                if (feedback) feedback.textContent = "Valor erróneo";
            } else {
                el.classList.remove("is-invalid");
                el.classList.add("is-valid");
            }
        } else {
            el.classList.remove("is-invalid");
            el.classList.add("is-valid");
        }
    }

    // Validación visual para select2
    if ($(el).hasClass("select2-hidden-accessible")) {
        const container = $(el).next(".select2-container");
        container.removeClass("is-valid is-invalid");

        if (!valor || valor === "Seleccionar") {
            container.addClass("is-invalid");
        } else {
            container.addClass("is-valid");
        }

        // Eliminar estilos del select original
        $(el).removeClass("is-valid is-invalid");
    }

    verificarErroresGenerales();
}


function verificarErroresGenerales() {
    const errorMsg = document.getElementById("errorCampos");
    const hayInvalidos = document.querySelectorAll("#formInsumo .is-invalid").length > 0;
    if (!errorMsg) return;

    if (!hayInvalidos) {
        errorMsg.classList.add("d-none");
    }
}

function validarCampos() {
    const campos = [
        "#txtCodigo",
        "#txtDescripcion",
        "#Proveedores",
        "#txtCostoUnitario"
    ];

    const camposNumericos = ["#txtCostoUnitario"];
    let valido = true;

    campos.forEach(selector => {
        const campo = document.querySelector(selector);
        const valor = campo?.value?.trim();
        const feedback = campo?.nextElementSibling;
        const esNumero = camposNumericos.includes(selector);
        const isSelect2 = campo?.classList.contains("select2-hidden-accessible");

        // Reset visual
        if (isSelect2) {
            const container = $(campo).next(".select2-container");
            container.removeClass("is-valid is-invalid");
        }

        if (!campo || !valor || valor === "Seleccionar") {
            if (isSelect2) {
                $(campo).next(".select2-container").addClass("is-invalid");
            } else {
                campo.classList.add("is-invalid");
                campo.classList.remove("is-valid");
            }

            if (feedback && feedback.classList.contains("invalid-feedback")) {
                feedback.textContent = "Campo obligatorio";
            }

            valido = false;
        } else if (esNumero) {
            const sinFormato = valor.replace(/[^\d,.-]/g, "").replace(/\./g, "").replace(",", ".");
            if (isNaN(parseFloat(sinFormato))) {
                if (isSelect2) {
                    $(campo).next(".select2-container").addClass("is-invalid");
                } else {
                    campo.classList.remove("is-valid");
                    campo.classList.add("is-invalid");
                }

                if (feedback && feedback.classList.contains("invalid-feedback")) {
                    feedback.textContent = "Valor erróneo";
                }

                valido = false;
            } else {
                if (isSelect2) {
                    $(campo).next(".select2-container").addClass("is-valid");
                } else {
                    campo.classList.remove("is-invalid");
                    campo.classList.add("is-valid");
                }
            }
        } else {
            if (isSelect2) {
                $(campo).next(".select2-container").addClass("is-valid");
            } else {
                campo.classList.remove("is-invalid");
                campo.classList.add("is-valid");
            }
        }
    });

    document.getElementById("errorCampos").classList.toggle("d-none", valido);
    return valido;
}
