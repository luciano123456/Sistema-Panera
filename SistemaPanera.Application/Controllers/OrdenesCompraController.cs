using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPanera.Application.Models.ViewModels;
using SistemaPanera.Models;

[Authorize]
public class OrdenesCompraController : Controller
{
    private readonly IOrdenesCompraService _service;

    public OrdenesCompraController(IOrdenesCompraService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Lista()
    {
        var ordenes = await _service.ObtenerTodosAsync();
        var lista = ordenes.Select(o => new
        {
            o.Id,
            o.FechaEmision,
            o.FechaEntrega,
            Proveedor = o.IdProveedorNavigation?.Nombre,
            Local = o.IdLocalNavigation?.Nombre,
            UnidadNegocio = o.IdUnidadNegocioNavigation?.Nombre,
            o.CostoTotal,
            Estado = o.IdEstadoNavigation?.Nombre,
            NotaInterna = o.NotaInterna
        }).ToList();

        return Ok(lista);
    }


    [HttpGet]
    public async Task<IActionResult> EditarInfo(int id)
    {
        try
        {
            var orden = await _service.ObtenerPorIdAsync(id);
            if (orden == null)
                return NotFound();

            var vm = new VMOrdenesCompra
            {
                Id = orden.Id,
                IdUnidadNegocio = orden.IdUnidadNegocio,
                IdLocal = orden.IdLocal,
                FechaEmision = orden.FechaEmision,
                FechaEntrega = orden.FechaEntrega,
                IdProveedor = orden.IdProveedor,
                IdEstado = orden.IdEstado,
                NotaInterna = orden.NotaInterna,
                CostoTotal = orden.CostoTotal,
                OrdenesComprasInsumos = orden.OrdenesComprasInsumos?.Select(i => new VMOrdenesComprasInsumo
                {
                    Id = i.Id,
                    IdInsumo = i.IdInsumo,
                    CantidadPedida = i.CantidadPedida,
                    CantidadEntregada = i.CantidadEntregada,
                    CantidadRestante = i.CantidadRestante,
                    PrecioLista = i.PrecioLista,
                    Subtotal = i.Subtotal,
                    IdEstado = i.IdEstado,
                    NotaInterna = i.NotaInterna,
                    IdProveedorLista = i.IdProveedorLista,
                    Nombre = i.IdInsumoNavigation.Descripcion,
                    Proveedor = i.IdProveedorListaNavigation != null
                    ? i.IdProveedorListaNavigation.IdProveedorNavigation.Nombre
                    : ""

                }).ToList() ?? new List<VMOrdenesComprasInsumo>()
            };

            return Ok(vm);
        } catch (Exception ex)
        {
            return StatusCode(500, new { valor = false, error = "Error al obtener la orden", detalle = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Insertar([FromBody] VMOrdenesCompra model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entidad = new OrdenesCompra
            {
                IdUnidadNegocio = model.IdUnidadNegocio,
                IdLocal = model.IdLocal,
                FechaEmision = model.FechaEmision,
                FechaEntrega = model.FechaEntrega,
                IdProveedor = model.IdProveedor,
                IdEstado = model.IdEstado,
                NotaInterna = model.NotaInterna,
                CostoTotal = model.CostoTotal,
                OrdenesComprasInsumos = model.OrdenesComprasInsumos.Select(i => new OrdenesComprasInsumo
                {
                    Id = i.Id,
                    IdInsumo = i.IdInsumo,
                    CantidadPedida = i.CantidadPedida,
                    CantidadEntregada = 0,
                    CantidadRestante = i.CantidadPedida,
                    PrecioLista = i.PrecioLista,
                    Subtotal = i.Subtotal,
                    IdEstado = 1,
                    IdProveedorLista = i.IdProveedorLista
                }).ToList()
            };

            await _service.CrearAsync(entidad);
            return Ok(new { valor = true, id = entidad.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { valor = false, error = "Error al insertar la orden", detalle = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] VMOrdenesCompra model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Error en '{entry.Key}': {error.ErrorMessage}");
                    }
                }

            }


            var entidad = new OrdenesCompra
            {
                Id = model.Id,
                IdUnidadNegocio = model.IdUnidadNegocio,
                IdLocal = model.IdLocal,
                FechaEmision = model.FechaEmision,
                FechaEntrega = model.FechaEntrega,
                IdProveedor = model.IdProveedor,
                IdEstado = model.IdEstado,
                NotaInterna = model.NotaInterna,
                CostoTotal = model.CostoTotal,
                OrdenesComprasInsumos = model.OrdenesComprasInsumos.Select(i => new OrdenesComprasInsumo
                {
                    Id = i.Id,
                    IdInsumo = i.IdInsumo,
                    CantidadPedida = i.CantidadPedida,
                    CantidadEntregada = 0,
                    CantidadRestante = i.CantidadPedida,
                    PrecioLista = i.PrecioLista,
                    Subtotal = i.Subtotal,
                    IdEstado = 1,
                    IdProveedorLista = i.IdProveedorLista
                }).ToList()
            };

            await _service.EditarAsync(entidad);
            return Ok(new { valor = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { valor = false, error = "Error al actualizar la orden", detalle = ex.Message });
        }
    }



    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _service.EliminarAsync(id);
        return Ok(new { valor = true });
    }

    public IActionResult NuevoModif()
    {
        return View();
    }


}
