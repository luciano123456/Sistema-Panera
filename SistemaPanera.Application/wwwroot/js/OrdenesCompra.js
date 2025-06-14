let gridOrdenes;

const columnConfig = [
    { index: 1, filterType: 'text' },
    { index: 2, filterType: 'select', fetchDataFunc: listaUnidadesNegocioFilter },
    { index: 3, filterType: 'text',  },
    { index: 4, filterType: 'text' },
    { index: 5, filterType: 'select', fetchDataFunc: listaProveedoresFilter },
    { index: 6, filterType: 'text' },
    { index: 7, filterType: 'text' },
    { index: 8, filterType: 'select', fetchDataFunc: listaEstadosOrdenCompraFilter },
    { index: 9, filterType: 'text' },
   
];

$(document).ready(() => {
    listaUnidadesNegocioFiltro();
    listaOrdenesCompra(-1);
});

async function aplicarFiltros() {
    listaOrdenesCompra(document.getElementById("UnidadNegocioFiltro").value);
}

async function listaOrdenesCompra(idUnidadNegocio) {
    const url = `/OrdenesCompra/Lista?IdUnidadNegocio=${idUnidadNegocio}`;
    const response = await fetch(url);
    const data = await response.json();
    configurarDataTable(data);
}


async function configurarDataTable(data) {
    if (!gridOrdenes) {
        $('#grd_OrdenesCompra thead tr').clone(true).addClass('filters').appendTo('#grd_OrdenesCompra thead');

        gridOrdenes = $('#grd_OrdenesCompra').DataTable({
            data: data,
            language: {
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
                                <button class='btn btn-sm btneditar' onclick='editarOrden(${data})'>
                                    <i class='fa fa-pencil-square-o text-success'></i> Editar
                                </button>
                                <button class='btn btn-sm btneliminar' onclick='eliminarOrden(${data})'>
                                    <i class='fa fa-trash-o text-danger'></i> Eliminar
                                </button>
                            </div>
                        </div>`;
                    },
                    orderable: false,
                    searchable: false,
                },
                { data: 'Id', title: 'Nro' },
                { data: 'UnidadNegocio', title: 'Unidad de Negocio' },
                { data: 'Local', title: 'Local' },
                {
                    data: 'FechaEmision',
                    title: 'Fecha Emisión',
                    render: data => formatearFechaParaVista(data)
                },
                { data: 'Proveedor', title: 'Proveedor' },
                {
                    data: 'FechaEntrega',
                    title: 'Fecha Entrega',
                    render: data => formatearFechaParaVista(data)
                },
                {
                    data: 'CostoTotal',
                    title: 'Costo Total',
                    render: data => `$ ${formatNumber(data)}`
                },

                { data: 'Estado', title: 'Estado' },
                { data: 'NotaInterna', title: 'Nota Interna' }
            ],

            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: 'Exportar Excel',
                    filename: 'Reporte Ordenes de Compra',
                    title: '',
                    exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8]
                    },
                    className: 'btn-exportar-excel',
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Exportar PDF',
                    filename: 'Reporte Ordenes de Compra',
                    title: '',
                    exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8]
                    },
                    className: 'btn-exportar-pdf',
                },
                {
                    extend: 'print',
                    text: 'Imprimir',
                    title: '',
                    exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8]
                    },
                    className: 'btn-exportar-print'
                },
                'pageLength'
            ],

            orderCellsTop: true,
            fixedHeader: true,

          

            initComplete: async function () {
                const api = this.api();

                columnConfig.forEach(async (config) => {
                    const cell = $('.filters th').eq(config.index);

                    if (config.filterType === 'select') {
                        const select = $('<select><option value="">Seleccionar</option></select>')
                            .appendTo(cell.empty())
                            .on('change', function () {
                                const selectedText = $(this).find('option:selected').text();
                                api.column(config.index)
                                    .search($(this).val() ? '^' + selectedText + '$' : '', true, false)
                                    .draw();
                            });

                        const items = await config.fetchDataFunc();
                        items.forEach(item => select.append(`<option value="${item.Id}">${item.Nombre}</option>`));
                    } else if (config.filterType === 'text') {
                        $('<input type="text" placeholder="Buscar..." />')
                            .appendTo(cell.empty())
                            .on('keyup change', function () {
                                api.column(config.index).search(this.value).draw();
                            });
                    }
                });

                $('.filters th').eq(0).html(''); // vaciar columna acciones

                // Doble clic para editar
                $('#grd_OrdenesCompra tbody').on('dblclick', 'tr', function () {
                    const id = gridOrdenes.row(this).data().Id;
                    editarOrden(id);
                });

                // Selección visual de fila
                let filaSeleccionada = null;
                $('#grd_OrdenesCompra tbody').on('click', 'tr', function () {
                    if (filaSeleccionada) {
                        $(filaSeleccionada).removeClass('seleccionada');
                        $('td', filaSeleccionada).removeClass('seleccionada');
                    }

                    filaSeleccionada = $(this);
                    $(filaSeleccionada).addClass('seleccionada');
                    $('td', filaSeleccionada).addClass('seleccionada');
                });

                setTimeout(function () {
                    gridOrdenes.columns.adjust();
                }, 50);

                // Opcional: ícono de mapa
                $('body').on('click', '#grd_OrdenesCompra .fa-map-marker', function () {
                    const locationText = $(this).parent().text().trim().replace(' ', ' ');
                    const url = 'https://www.google.com/maps?q=' + encodeURIComponent(locationText);
                    window.open(url, '_blank');
                });
            },
        });

    } else {
        gridOrdenes.clear().rows.add(data).draw();
    }
}

function editarOrden(id) {
    localStorage.setItem("ordenCompraId", id); // Guardar ID
    window.location.href = "/OrdenesCompra/NuevoModif";
}

function nuevoOrden() {
    localStorage.removeItem("ordenCompraId"); // Limpiar ID
    window.location.href = "/OrdenesCompra/NuevoModif";
}


async function eliminarOrden(id) {
    const confirmar = await confirmarModal("¿Estás seguro que deseas eliminar esta orden de compra?");
    if (!confirmar) return;

    try {
        const res = await fetch(`/OrdenesCompra/Eliminar?id=${id}`, {
            method: 'DELETE'
        });

        if (!res.ok) {
            const errorBody = await res.text();
            console.error("Error al eliminar:", errorBody);
            errorModal("Ocurrió un error al eliminar la orden.");
            return;
        }

        const result = await res.json();
        if (result.valor) {
            aplicarFiltros();
            exitoModal("Orden eliminada correctamente");
        } else {
            errorModal("No se pudo eliminar la orden.");
        }
    } catch (err) {
        console.error("Excepción al eliminar:", err);
        errorModal("Error inesperado al eliminar la orden.");
    }
}


async function listaUnidadesNegocioFilter() {
    const response = await fetch('/UnidadesNegocio/Lista');
    const data = await response.json();
    return data.map(x => ({ Id: x.Id, Nombre: x.Nombre }));
}

async function listaLocalesFilter() {
    const response = await fetch('/Locales/Lista');
    const data = await response.json();
    return data.map(x => ({ Id: x.Id, Nombre: x.Nombre }));
}

async function listaProveedoresFilter() {
    const response = await fetch('/Proveedores/Lista');
    const data = await response.json();
    return data.map(x => ({ Id: x.Id, Nombre: x.Nombre }));
}

async function listaEstadosOrdenCompraFilter() {
    const response = await fetch('/OrdenesComprasEstado/Lista');
    const data = await response.json();
    return data.map(x => ({ Id: x.Id, Nombre: x.Nombre }));
}

function formatNumber(value) {
    return new Intl.NumberFormat('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(value);
}

function listaUnidadesNegocioFiltro() {
    fetch('/UnidadesNegocio/Lista').then(r => r.json()).then(data => {
        const select = document.getElementById("UnidadNegocioFiltro");
        select.innerHTML = "<option value='-1'>Todos</option>";
        data.forEach(item => {
            const opt = document.createElement("option");
            opt.value = item.Id;
            opt.textContent = item.Nombre;
            select.appendChild(opt);
        });
    });
}

$(document).on('click', function (e) {
    // Verificar si el clic está fuera de cualquier dropdown
    if (!$(e.target).closest('.acciones-menu').length) {
        $('.acciones-dropdown').hide(); // Cerrar todos los dropdowns
    }
});