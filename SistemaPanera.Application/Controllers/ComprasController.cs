using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPanera.Application.Models;
using SistemaPanera.Application.Models.ViewModels;
using SistemaPanera.BLL.Service;
using SistemaPanera.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SistemaPanera.Application.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly ICompraService _ComprasService;

        public ComprasController(ICompraService ComprasService)
        {
            _ComprasService = ComprasService;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Lista(int IdUnidadNegocio, int IdLocal)
        {
            try
            {
                var Compras = await _ComprasService.ObtenerTodos();

                var lista = Compras.Select(c => new VMCompra
                {
                    Id = c.Id,
                    IdUnidadNegocio = c.IdUnidadNegocio,
                    Fecha = c.Fecha,
                    IdLocal = c.IdLocal,
                    IdProveedor = c.IdProveedor,
                    NumeroOrden = c.NumeroOrden,
                    UnidadNegocio = c.IdUnidadNegocioNavigation.Nombre,
                    Local = c.IdLocalNavigation.Nombre,
                    Costo = c.Costo
                }).Where(x => x.IdUnidadNegocio == IdUnidadNegocio || IdUnidadNegocio == -1 && x.IdLocal == IdLocal || IdLocal == -1).ToList();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Ok(null);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMCompra model)
        {
            var Compras = new Compra
            {
                Id = model.Id,
                IdUnidadNegocio = model.IdUnidadNegocio,
                Fecha = model.Fecha,
                IdLocal = model.IdLocal,
                IdProveedor = model.IdProveedor,
                NumeroOrden = model.NumeroOrden,

            };

            bool respuesta = await _ComprasService.Insertar(Compras);

            List<ComprasDetalle> pedidosInsumo = new List<ComprasDetalle>();

            // Agregar los pagos de clientes
            if (model.ComprasDetalles != null && model.ComprasDetalles.Any())
            {
                foreach (var insumo in model.ComprasDetalles)
                {
                    var nuevoInsumo = new ComprasDetalle
                    {
                        IdCompra = Compras.Id,
                        IdInsumo = insumo.IdInsumo,
                        CostoUnitario = insumo.CostoUnitario,
                        SubTotal = insumo.SubTotal,
                        Cantidad = insumo.Cantidad,
                    };
                    pedidosInsumo.Add(nuevoInsumo);
                }
            }

            bool respInsumos = await _ComprasService.InsertarInsumos(pedidosInsumo);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMCompra model)
        {
            var Compras = new Compra
            {
                Id = model.Id,
                IdUnidadNegocio = model.IdUnidadNegocio,
                Fecha = model.Fecha,
                IdLocal = model.IdLocal,
                IdProveedor = model.IdProveedor,
                NumeroOrden = model.NumeroOrden,

            };

            bool respuesta = await _ComprasService.Actualizar(Compras);

            List<ComprasDetalle> ComprasInsumo = new List<ComprasDetalle>();

            // Agregar los pagos de clientes
            if (model.ComprasDetalles != null && model.ComprasDetalles.Any())
            {
                foreach (var insumo in model.ComprasDetalles)
                {
                    var nuevoInsumo = new ComprasDetalle
                    {
                        Cantidad = insumo.Cantidad,
                        CostoUnitario = insumo.CostoUnitario,
                        IdInsumo = insumo.IdInsumo,
                        IdCompra = insumo.IdCompra,
                        SubTotal = insumo.SubTotal,
                    };
                    ComprasInsumo.Add(nuevoInsumo);
                }
            }

            bool respproductos = await _ComprasService.InsertarInsumos(ComprasInsumo);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _ComprasService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (id > 0)
            {

                var model = await _ComprasService.Obtener(id);

                var Compra = new VMCompra
                {
                    Id = model.Id,
                    IdUnidadNegocio = model.IdUnidadNegocio,
                    Fecha = DateTime.Now,
                    IdLocal = model.IdLocal,
                    IdProveedor = model.IdProveedor,
                    NumeroOrden = model.NumeroOrden,

                };

                var ComprasInsumos = await _ComprasService.ObtenerInsumos(id);


                var insumosJson = ComprasInsumos.Select(p => new VMComprasDetalle
                {
                    Id = p.Id,

                    Cantidad = p.Cantidad,
                    CostoUnitario = p.CostoUnitario,
                    IdInsumo = p.IdInsumo,
                    Nombre = p.IdInsumoNavigation.Descripcion.ToString(),
                    IdCompra = p.IdCompra,
                    SubTotal = p.SubTotal,
                }).ToList();



                result.Add("Compra", Compra);
                result.Add("Insumos", insumosJson);

                // Serialize with ReferenceHandler.Preserve to handle circular references
                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                return Ok(System.Text.Json.JsonSerializer.Serialize(result, jsonOptions));
            }

            return Ok(new { });
        }





        public async Task<IActionResult> NuevoModif(int? id)
        {
            if (id != null)
            {
                ViewBag.data = id;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}