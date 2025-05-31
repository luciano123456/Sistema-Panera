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
                var subrecetas = await _SubrecetasService.ObtenerTodos();

                var lista = subrecetas
                    .Where(x => IdUnidadNegocio == -1 || x.IdUnidadNegocio == IdUnidadNegocio)
                    .Select(c => new VMSubreceta
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
                        CostoSubRecetas = c.CostoSubRecetas,
                        CostoInsumos = c.CostoInsumos,
                        CostoPorcion = c.CostoPorcion,
                        Rendimiento = c.Rendimiento,
                        CostoUnitario = c.CostoUnitario
                    })
                    .ToList();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                // Idealmente, loguear el error con un logger
                return StatusCode(500, new { error = "Error al obtener las subrecetas." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMSubreceta model)
        {
            var subreceta = new Subreceta
            {
                IdUnidadNegocio = model.IdUnidadNegocio,
                Sku = model.Sku,
                Descripcion = model.Descripcion,
                IdUnidadMedida = model.IdUnidadMedida,
                IdCategoria = model.IdCategoria,
                CostoSubRecetas = model.CostoSubRecetas,
                CostoInsumos = model.CostoInsumos,
                CostoPorcion = (decimal)model.CostoPorcion,
                CostoUnitario = model.CostoUnitario,
                Rendimiento = model.Rendimiento,
                FechaActualizacion = DateTime.Now,

                SubrecetasInsumos = model.SubrecetasInsumos?.Select(i => new SubrecetasInsumo
                {
                    IdInsumo = i.IdInsumo,
                    Cantidad = i.Cantidad,
                    CostoUnitario = i.CostoUnitario,
                    SubTotal = i.SubTotal
                }).ToList(),

                SubrecetasSubrecetaIdSubRecetaHijaNavigations = model.SubrecetasSubrecetaIdSubRecetaPadreNavigations?.Select(s => new SubrecetasSubreceta
                {
                    IdSubRecetaHija = s.IdSubRecetaHija,
                    Cantidad = s.Cantidad,
                    CostoUnitario = s.CostoUnitario,
                    SubTotal = s.SubTotal
                }).ToList()
            };

            var resultado = await _SubrecetasService.Insertar(subreceta);
            return Ok(new { valor = resultado });
        }



        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMSubreceta model)
        {
            var subreceta = new Subreceta
            {
                Id = model.Id,
                IdUnidadNegocio = model.IdUnidadNegocio,
                Sku = model.Sku,
                Descripcion = model.Descripcion,
                IdUnidadMedida = model.IdUnidadMedida,
                IdCategoria = model.IdCategoria,
                CostoSubRecetas = model.CostoSubRecetas,
                CostoInsumos = model.CostoInsumos,
                CostoPorcion = (decimal)model.CostoPorcion,
                CostoUnitario = model.CostoUnitario,
                Rendimiento = model.Rendimiento,
                FechaActualizacion = DateTime.Now,

                SubrecetasInsumos = model.SubrecetasInsumos?.Select(i => new SubrecetasInsumo
                {
                    IdInsumo = i.IdInsumo,
                    Cantidad = i.Cantidad,
                    CostoUnitario = i.CostoUnitario,
                    SubTotal = i.SubTotal
                }).ToList(),

                SubrecetasSubrecetaIdSubRecetaPadreNavigations = model.SubrecetasSubrecetaIdSubRecetaPadreNavigations?.Select(s => new SubrecetasSubreceta
                {
                    IdSubRecetaPadre = model.Id,
                    IdSubRecetaHija = s.IdSubRecetaHija,
                    Cantidad = s.Cantidad,
                    CostoUnitario = s.CostoUnitario,
                    SubTotal = s.SubTotal
                }).ToList()


            };

            var resultado = await _SubrecetasService.Actualizar(subreceta);
            return Ok(new { valor = resultado });
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
            if (id <= 0)
                return Ok(new { });

            var model = await _SubrecetasService.Obtener(id);

            var subreceta = new VMSubreceta
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                FechaActualizacion = model.FechaActualizacion,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoUnitario = model.CostoUnitario,
                CostoInsumos = model.CostoInsumos,
                CostoSubRecetas = model.CostoSubRecetas,
                Rendimiento = model.Rendimiento,
            };

            var insumos = model.SubrecetasInsumos.Select(p => new VMSubrecetasInsumo
            {
                Id = p.Id,
                IdSubreceta = p.IdSubreceta,
                IdInsumo = p.IdInsumo,
                Nombre = p.IdInsumoNavigation.Descripcion,
                Cantidad = p.Cantidad,
                CostoUnitario = p.CostoUnitario,
                SubTotal = p.SubTotal
            }).ToList();

            var subrecetas = model.SubrecetasSubrecetaIdSubRecetaPadreNavigations.Select(p => new VMSubrecetasSubreceta
            {
                Id = p.Id,
                IdSubRecetaPadre = p.IdSubRecetaPadre,
                IdSubRecetaHija = p.IdSubRecetaHija,
                Cantidad = p.Cantidad,
                CostoUnitario = p.CostoUnitario,
                SubTotal = p.SubTotal,
                Nombre = p.IdSubRecetaHijaNavigation?.Descripcion,
                IdSubRecetaHijaNavigation = p.IdSubRecetaHijaNavigation,
                IdSubRecetaPadreNavigation = p.IdSubRecetaPadreNavigation
            }).ToList();


            var result = new Dictionary<string, object>
            {
                ["Subreceta"] = subreceta,
                ["Insumos"] = insumos,
                ["SubRecetas"] = subrecetas
            };

            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(System.Text.Json.JsonSerializer.Serialize(result, jsonOptions));
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