﻿let gridProveedores;
let isEditing = false;


const columnConfig = [
    { index: 1, filterType: 'text' },
    { index: 2, filterType: 'text' },
    { index: 3, filterType: 'text' },
    { index: 4, filterType: 'text' },
    { index: 5, filterType: 'text' },
    { index: 6, filterType: 'text' },
];

const Modelo_base = {
    Id: 0,
    Nombre: "",
    Apodo: "",
    Ubicacion: "",
    Telefono: "",
    Cbu: "",
    Cuit: "",
}

$(document).ready(() => {

    listaProveedores();

    document.querySelectorAll("#formProveedor input, #formProveedor select, #formProveedor textarea").forEach(el => {
        el.setAttribute("autocomplete", "off");
        el.addEventListener("input", () => validarCampoIndividual(el));
        el.addEventListener("change", () => validarCampoIndividual(el));
        el.addEventListener("blur", () => validarCampoIndividual(el));
    });

})



function guardarCambios() {
    if (validarCampos()) {
        const idProveedor = $("#txtId").val();
        const nuevoModelo = {
            "Id": idProveedor !== "" ? idProveedor : 0,
            "Nombre": $("#txtNombre").val(),
            "Apodo": $("#txtApodo").val(),
            "Ubicacion": $("#txtUbicacion").val(),
            "Telefono": $("#txtTelefono").val(),
            "Cbu": $("#txtCbu").val(),
            "Cuit": $("#txtCuit").val(),
        };

        const url = idProveedor === "" ? "Proveedores/Insertar" : "Proveedores/Actualizar";
        const method = idProveedor === "" ? "POST" : "PUT";

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
                const mensaje = idProveedor === "" ? "Proveedor registrado correctamente" : "Proveedor modificado correctamente";
                $('#modalEdicion').modal('hide');
                exitoModal(mensaje);
                listaProveedores();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    } else {
        return false;
    }
}


function nuevoProveedor() {
    limpiarModal();
    $('#modalEdicion').modal('show');
    $("#btnGuardar").text("Registrar");
    $("#modalEdicionLabel").text("Nuevo Proveedor");
}

async function mostrarModal(modelo) {
    limpiarModal();
    const campos = ["Id", "Nombre", "Apodo", "Ubicacion", "Telefono", "Cbu", "Cuit"];
    campos.forEach(campo => {
        $(`#txt${campo}`).val(modelo[campo]);
    });


    $('#modalEdicion').modal('show');
    $("#btnGuardar").text("Guardar");
    $("#modalEdicionLabel").text("Editar Proveedor");

    $('#lblNombre, #txtNombre').css('color', '').css('border-color', '');
}





async function listaProveedores() {
    const url = `/Proveedores/Lista`;
    const response = await fetch(url);
    const data = await response.json();
    await configurarDataTable(data);
}

const editarProveedor = id => {
    fetch("Proveedores/EditarInfo?id=" + id)
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
async function eliminarProveedor(id) {
    let resultado = window.confirm("¿Desea eliminar el Proveedor?");

    if (resultado) {
        try {
            const response = await fetch("Proveedores/Eliminar?id=" + id, {
                method: "DELETE"
            });

            if (!response.ok) {
                throw new Error("Error al eliminar el Proveedor.");
            }

            const dataJson = await response.json();

            if (dataJson.valor) {
                listaProveedores();
                exitoModal("Proveedor eliminado correctamente")
            }
        } catch (error) {
            console.error("Ha ocurrido un error:", error);
        }
    }
}

async function configurarDataTable(data) {
    if (!gridProveedores) {
        $('#grd_Proveedores thead tr').clone(true).addClass('filters').appendTo('#grd_Proveedores thead');
        gridProveedores = $('#grd_Proveedores').DataTable({
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
                        <button class='btn btn-sm btneditar' type='button' onclick='editarProveedor(${data})' title='Editar'>
                            <i class='fa fa-pencil-square-o fa-lg text-success' aria-hidden='true'></i> Editar
                        </button>
                        <button class='btn btn-sm btneliminar' type='button' onclick='eliminarProveedor(${data})' title='Eliminar'>
                            <i class='fa fa-trash-o fa-lg text-danger' aria-hidden='true'></i> Eliminar
                        </button>
                    </div>
                </div>`;
                    },
                    orderable: false,
                    searchable: false,
                },
                { data: 'Nombre' },
                { data: 'Apodo' },
                { data: 'Cuit' },
                { data: 'Cbu' },
                {
                    data: function (row) {
                        return row.Ubicacion && row.Ubicacion.trim() !== "" ? '<div class="location-cell"><i title="Ir a Google Maps" class="fa fa-map-marker fa-2x text-warning"></i> ' + row.Ubicacion + '</div>' : row.Ubicacion;
                    }
                },

                { data: 'Telefono' },
            ],
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: 'Exportar Excel',
                    filename: 'Reporte Proveedores',
                    title: '',
                    exportOptions: {
                        columns: [0, 1, 2, 3]
                    },
                    className: 'btn-exportar-excel',
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Exportar PDF',
                    filename: 'Reporte Proveedores',
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
                    gridProveedores.columns.adjust();
                }, 10);

                $('body').on('mouseenter', '#grd_Proveedores .fa-map-marker', function () {
                    $(this).css('cursor', 'pointer');
                });



                $('body').on('click', '#grd_Proveedores .fa-map-marker', function () {
                    var locationText = $(this).parent().text().trim().replace(' ', ' '); // Obtener el texto visible
                    var url = 'https://www.google.com/maps?q=' + encodeURIComponent(locationText);
                    window.open(url, '_blank');
                });
            },
        });

    } else {
        gridProveedores.clear().rows.add(data).draw();
    }
}


function configurarOpcionesColumnas() {
    const grid = $('#grd_Proveedores').DataTable(); // Accede al objeto DataTable utilizando el id de la tabla
    const columnas = grid.settings().init().columns; // Obtiene la configuración de columnas
    const container = $('#configColumnasMenu'); // El contenedor del dropdown específico para configurar columnas


    const storageKey = `Proveedores_Columnas`; // Clave única para esta pantalla

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

function limpiarModal() {
    const formulario = document.querySelector("#formProveedor");
    if (!formulario) return;

    formulario.querySelectorAll("input, select, textarea").forEach(el => {
        if (el.tagName === "SELECT") {
            el.selectedIndex = 0;
        } else {
            el.value = "";
        }
        el.classList.remove("is-invalid", "is-valid");
    });

    // Ocultar mensaje general de error
    const errorMsg = document.getElementById("errorCampos");
    if (errorMsg) errorMsg.classList.add("d-none");
}


function validarCampoIndividual(el) {
    const tag = el.tagName.toLowerCase();
    const id = el.id;
    const valor = el.value ? el.value.trim() : ""; // Para inputs/selects

    const feedback = el.nextElementSibling;

    if(id != "txtNombre") return //Solo valida el nombre


    // Validación para inputs/selects normales
    if (tag === "input" || tag === "select" || tag === "textarea") {
        if (feedback && feedback.classList.contains("invalid-feedback")) {
            feedback.textContent = "Campo obligatorio";
        }

        if (valor === "" || valor === "Seleccionar") {
            el.classList.remove("is-valid");
            el.classList.add("is-invalid");
        } else {
            el.classList.remove("is-invalid");
            el.classList.add("is-valid");
        }
    }

    verificarErroresGenerales();
}

function verificarErroresGenerales() {
    const errorMsg = document.getElementById("errorCampos");
    const hayInvalidos = document.querySelectorAll("#formProveedor .is-invalid").length > 0;
    if (!errorMsg) return;

    if (!hayInvalidos) {
        errorMsg.classList.add("d-none");
    }
}

function validarCampos() {
    const campos = [
        "#txtNombre",
    ];

    let valido = true;

    campos.forEach(selector => {
        const campo = document.querySelector(selector);
        const valor = campo?.value.trim();
        const feedback = campo?.nextElementSibling;

        if (!campo || !valor || valor === "Seleccionar") {
            campo.classList.add("is-invalid");
            campo.classList.remove("is-valid");
            if (feedback) feedback.textContent = "Campo obligatorio";
            valido = false;
        } else {
            campo.classList.remove("is-invalid");
            campo.classList.add("is-valid");
        }
    });

    document.getElementById("errorCampos").classList.toggle("d-none", valido);
    return valido;
}
