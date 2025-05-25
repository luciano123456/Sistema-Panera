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
    public class SubRecetasController : Controller
    {
        private readonly ISubrecetaService _SubrecetasService;

        public SubRecetasController(ISubrecetaService SubrecetasService)
        {
            _SubrecetasService = SubrecetasService;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Lista(int IdUnidadNegocio)
        {
            try
            {
                var Subrecetas = await _SubrecetasService.ObtenerTodos();

                var lista = Subrecetas.Select(c => new VMSubreceta
                {
                    Id = c.Id,
                    FechaActualizacion = c.FechaActualizacion,
                    IdCategoria = c.IdCategoria,
                    IdUnidadMedida = c.IdUnidadMedida,
                    IdUnidadNegocio = c.IdUnidadNegocio,
                    Sku = c.Sku,
                    Categoria = c.IdCategoriaNavigation.Nombre,
                    UnidadMedida = c.IdUnidadMedidaNavigation.Nombre,
                    UnidadNegocio = c.IdUnidadNegocioNavigation.Nombre,
                    Descripcion = c.Descripcion,
                    CostoUnitario = c.CostoUnitario,
                }).Where(x => x.IdUnidadNegocio == IdUnidadNegocio || IdUnidadNegocio == -1).ToList();

                return Ok(lista);

            } catch (Exception ex)
            {
                return null;
            }
           
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMSubreceta model)
        {
            var Subrecetas = new Subreceta
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoUnitario = model.CostoUnitario,
            };

            bool respuesta = await _SubrecetasService.Insertar(Subrecetas);

            List<SubrecetasInsumo> pedidosInsumo = new List<SubrecetasInsumo>();

            // Agregar los pagos de clientes
            if (model.SubrecetasInsumos != null && model.SubrecetasInsumos.Any())
            {
                foreach (var insumo in model.SubrecetasInsumos)
                {
                    var nuevoInsumo = new SubrecetasInsumo
                    {
                        IdSubreceta = Subrecetas.Id,
                        IdInsumo = insumo.IdInsumo,
                        CostoUnitario = insumo.CostoUnitario,
                        SubTotal = insumo.SubTotal,
                        Cantidad = insumo.Cantidad,
                    };
                    pedidosInsumo.Add(nuevoInsumo);
                }
            }

            bool respInsumos = await _SubrecetasService.InsertarInsumos(pedidosInsumo);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMSubreceta model)
        {
            var Subrecetas = new Subreceta
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoUnitario = model.CostoUnitario,

            };

            bool respuesta = await _SubrecetasService.Actualizar(Subrecetas);

            List<SubrecetasInsumo> SubrecetasInsumo = new List<SubrecetasInsumo>();

            // Agregar los pagos de clientes
            if (model.SubrecetasInsumos != null && model.SubrecetasInsumos.Any())
            {
                foreach (var insumo in model.SubrecetasInsumos)
                {
                    var nuevoInsumo = new SubrecetasInsumo
                    {
                        Cantidad = insumo.Cantidad,
                        CostoUnitario = insumo.CostoUnitario,
                        IdInsumo = insumo.IdInsumo,
                        IdSubreceta = insumo.IdSubreceta,
                        SubTotal = insumo.SubTotal,
                    };
                    SubrecetasInsumo.Add(nuevoInsumo);
                }
            }

            bool respproductos = await _SubrecetasService.InsertarInsumos(SubrecetasInsumo);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _SubrecetasService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (id > 0)
            {

                var model = await _SubrecetasService.Obtener(id);

                var Subreceta = new VMSubreceta
                {
                    Id = model.Id,
                    IdUnidadMedida = model.IdUnidadMedida,
                    Sku = model.Sku,
                    IdUnidadNegocio = model.IdUnidadNegocio,
                    FechaActualizacion = DateTime.Now,
                    IdCategoria = model.IdCategoria,
                    Descripcion = model.Descripcion,
                    CostoUnitario = model.CostoUnitario,
                };

                var SubrecetasInsumos = await _SubrecetasService.ObtenerInsumos(id);


                var insumosJson = SubrecetasInsumos.Select(p => new VMSubrecetasInsumo
                {
                    Id = p.Id,

                    Cantidad = p.Cantidad,
                    CostoUnitario = p.CostoUnitario,
                    IdInsumo = p.IdInsumo,
                    Nombre = p.IdInsumoNavigation.Descripcion.ToString(),
                    IdSubreceta = p.IdSubreceta,
                    SubTotal = p.SubTotal,
                }).ToList();



                result.Add("Subreceta", Subreceta);
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