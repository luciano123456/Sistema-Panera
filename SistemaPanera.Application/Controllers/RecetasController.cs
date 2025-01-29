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
    public class RecetasController : Controller
    {
        private readonly IRecetaService _RecetasService;

        public RecetasController(IRecetaService RecetasService)
        {
            _RecetasService = RecetasService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> NuevoModif(int? id)
        {
            if (id != null)
            {
                ViewBag.data = id;
            }
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Lista(int IdUnidadNegocio)
        {
            var Recetas = await _RecetasService.ObtenerTodos();

            var lista = Recetas.Select(c => new VMReceta
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
                CostoPrefabricados = c.CostoPrefabricados,
                CostoInsumos = c.CostoInsumos,
                CostoTotal = c.CostoTotal,
            }).Where(x => x.IdUnidadNegocio == IdUnidadNegocio || IdUnidadNegocio == -1).ToList();

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMReceta model)
        {
            var Recetas = new Receta
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                CostoPrefabricados = model.CostoPrefabricados,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoTotal = model.CostoTotal,
                CostoInsumos = model.CostoInsumos
            };

            bool respuesta = await _RecetasService.Insertar(Recetas);

            List<RecetasInsumo> recetaInsumo = new List<RecetasInsumo>();

            // Agregar los pagos de clientes
            if (model != null && model.RecetasInsumos.Any())
            {
                foreach (var insumo in model.RecetasInsumos)
                {
                    var nuevoInsumo = new RecetasInsumo
                    {
                        IdReceta = Recetas.Id,
                        IdInsumo = insumo.IdInsumo,
                        CostoUnitario = insumo.CostoUnitario,
                        SubTotal = insumo.SubTotal,
                        Cantidad = insumo.Cantidad,
                    };
                    recetaInsumo.Add(nuevoInsumo);
                }
            }

            bool respInsumos = await _RecetasService.InsertarInsumos(recetaInsumo);

            List<RecetasPrefabricado> recetaPrefabricado = new List<RecetasPrefabricado>();

            // Agregar los pagos de clientes
            if (model != null && model.RecetasPrefabricados.Any())
            {
                foreach (var prefabricado in model.RecetasPrefabricados)
                {
                    var nuevoPrefabricado = new RecetasPrefabricado
                    {
                        IdReceta = Recetas.Id,
                        IdPrefabricado = prefabricado.IdPrefabricado,
                        CostoUnitario = prefabricado.CostoUnitario,
                        SubTotal = prefabricado.SubTotal,
                        Cantidad = prefabricado.Cantidad,
                    };
                    recetaPrefabricado.Add(nuevoPrefabricado);
                }
            }

            bool respPrefabricados = await _RecetasService.InsertarPrefabricados(recetaPrefabricado);

            


            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMReceta model)
        {
            var Recetas = new Receta
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                CostoPrefabricados = model.CostoPrefabricados,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoTotal = model.CostoTotal,
                CostoInsumos = model.CostoInsumos
            };


            List<RecetasInsumo> recetaInsumo = new List<RecetasInsumo>();

            // Agregar los pagos de clientes
            if (model != null && model.RecetasInsumos.Any())
            {
                foreach (var insumo in model.RecetasInsumos)
                {
                    var nuevoInsumo = new RecetasInsumo
                    {
                        IdReceta = Recetas.Id,
                        IdInsumo = insumo.IdInsumo,
                        CostoUnitario = insumo.CostoUnitario,
                        SubTotal = insumo.SubTotal,
                        Cantidad = insumo.Cantidad,
                    };
                    recetaInsumo.Add(nuevoInsumo);
                }
            }

            bool respInsumos = await _RecetasService.InsertarInsumos(recetaInsumo);

            List<RecetasPrefabricado> recetaPrefabricado = new List<RecetasPrefabricado>();

            // Agregar los pagos de clientes
            if (model != null && model.RecetasPrefabricados.Any())
            {
                foreach (var prefabricado in model.RecetasPrefabricados)
                {
                    var nuevoPrefabricado = new RecetasPrefabricado
                    {
                        IdReceta = Recetas.Id,
                        IdPrefabricado = prefabricado.IdPrefabricado,
                        CostoUnitario = prefabricado.CostoUnitario,
                        SubTotal = prefabricado.SubTotal,
                        Cantidad = prefabricado.Cantidad,
                    };
                    recetaPrefabricado.Add(nuevoPrefabricado);
                }
            }

            bool respPrefabricados = await _RecetasService.InsertarPrefabricados(recetaPrefabricado);



            bool respuesta = await _RecetasService.Actualizar(Recetas);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _RecetasService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();

            if (id > 0)
            {
                var model = await _RecetasService.Obtener(id);

                var receta = new Receta
                {
                    Id = model.Id,
                    IdUnidadMedida = model.IdUnidadMedida,
                    Sku = model.Sku,
                    IdUnidadNegocio = model.IdUnidadNegocio,
                    CostoPrefabricados = model.CostoPrefabricados,
                    FechaActualizacion = DateTime.Now,
                    IdCategoria = model.IdCategoria,
                    Descripcion = model.Descripcion,
                    CostoTotal = model.CostoTotal,
                    CostoInsumos = model.CostoInsumos
                };


                var Insumos = await _RecetasService.ObtenerInsumos(id);


                var insumosJson = Insumos.Select(p => new VMRecetaInsumo
                {
                    Id = p.Id,

                    Cantidad = p.Cantidad,
                    CostoUnitario = p.CostoUnitario,
                    IdInsumo = p.IdInsumo,
                    Nombre = p.IdInsumoNavigation.Descripcion.ToString(),
                    IdReceta = p.IdReceta,
                    SubTotal = p.SubTotal,
                }).ToList();

                var Prefabricados = await _RecetasService.ObtenerPrefabricados(id);


                var prefabricadosJson = Prefabricados.Select(p => new VMRecetaPrefabricado
                {
                    Id = p.Id,

                    Cantidad = p.Cantidad,
                    CostoUnitario = p.CostoUnitario,
                    IdPrefabricado = p.IdPrefabricado,
                    Nombre = p.IdPrefabricadoNavigation.Descripcion.ToString(),
                    IdReceta = p.IdReceta,
                    SubTotal = p.SubTotal,
                }).ToList();

                result.Add("Receta", receta);
                result.Add("Insumos", insumosJson);
                result.Add("Prefabricados", prefabricadosJson);

                // Serialize with ReferenceHandler.Preserve to handle circular references
                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                return Ok(System.Text.Json.JsonSerializer.Serialize(result, jsonOptions));
            }

            return Ok(new { });

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