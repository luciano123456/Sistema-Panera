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
    public class PrefabricadosController : Controller
    {
        private readonly IPrefabricadoService _PrefabricadosService;

        public PrefabricadosController(IPrefabricadoService PrefabricadosService)
        {
            _PrefabricadosService = PrefabricadosService;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Lista(int IdUnidadNegocio)
        {
            var Prefabricados = await _PrefabricadosService.ObtenerTodos();

            var lista = Prefabricados.Select(c => new VMPrefabricado
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
                CostoTotal = c.CostoTotal,
            }).Where(x => x.IdUnidadNegocio == IdUnidadNegocio || IdUnidadNegocio == -1).ToList();

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMPrefabricado model)
        {
            var Prefabricados = new Prefabricado
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoTotal = model.CostoTotal,
            };

            bool respuesta = await _PrefabricadosService.Insertar(Prefabricados);

            List<PrefabricadosInsumo> pedidosInsumo = new List<PrefabricadosInsumo>();

            // Agregar los pagos de clientes
            if (model.PrefabricadosInsumos != null && model.PrefabricadosInsumos.Any())
            {
                foreach (var insumo in model.PrefabricadosInsumos)
                {
                    var nuevoInsumo = new PrefabricadosInsumo
                    {
                        IdPrefabricado = Prefabricados.Id,
                        IdInsumo = insumo.IdInsumo,
                        CostoUnitario = insumo.CostoUnitario,
                        SubTotal = insumo.SubTotal,
                        Cantidad = insumo.Cantidad,
                    };
                    pedidosInsumo.Add(nuevoInsumo);
                }
            }

            bool respInsumos = await _PrefabricadosService.InsertarInsumos(pedidosInsumo);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMPrefabricado model)
        {
            var Prefabricados = new Prefabricado
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoTotal = model.CostoTotal,

            };

            bool respuesta = await _PrefabricadosService.Actualizar(Prefabricados);

            List<PrefabricadosInsumo> prefabricadosInsumo = new List<PrefabricadosInsumo>();

            // Agregar los pagos de clientes
            if (model.PrefabricadosInsumos != null && model.PrefabricadosInsumos.Any())
            {
                foreach (var insumo in model.PrefabricadosInsumos)
                {
                    var nuevoInsumo = new PrefabricadosInsumo
                    {
                        Cantidad = insumo.Cantidad,
                        CostoUnitario = insumo.CostoUnitario,
                        IdInsumo = insumo.IdInsumo,
                        IdPrefabricado = insumo.IdPrefabricado,
                        SubTotal = insumo.SubTotal,
                    };
                    prefabricadosInsumo.Add(nuevoInsumo);
                }
            }

            bool respproductos = await _PrefabricadosService.InsertarInsumos(prefabricadosInsumo);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _PrefabricadosService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (id > 0)
            {

                var model = await _PrefabricadosService.Obtener(id);

                var Prefabricado = new VMPrefabricado
                {
                    Id = model.Id,
                    IdUnidadMedida = model.IdUnidadMedida,
                    Sku = model.Sku,
                    IdUnidadNegocio = model.IdUnidadNegocio,
                    FechaActualizacion = DateTime.Now,
                    IdCategoria = model.IdCategoria,
                    Descripcion = model.Descripcion,
                    CostoTotal = model.CostoTotal,
                };

                var prefabricadosInsumos = await _PrefabricadosService.ObtenerInsumos(id);


                var insumosJson = prefabricadosInsumos.Select(p => new VMPrefabricadosInsumo
                {
                    Id = p.Id,
                    
                    Cantidad = p.Cantidad,
                    CostoUnitario = p.CostoUnitario,
                    IdInsumo = p.IdInsumo,
                    Nombre = p.IdInsumoNavigation.Descripcion.ToString(),
                    IdPrefabricado = p.IdPrefabricado,
                    SubTotal = p.SubTotal,
                }).ToList();



                result.Add("prefabricado", Prefabricado);
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
            if(id != null) { 
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