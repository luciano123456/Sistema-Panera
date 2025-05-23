using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPanera.Application.Models;
using SistemaPanera.Application.Models.ViewModels;
using SistemaPanera.BLL.Service;
using SistemaPanera.Models;
using System.Diagnostics;

namespace SistemaPanera.Application.Controllers
{
    [Authorize]
    public class InsumosController : Controller
    {
        private readonly IInsumoService _InsumosService;

        public InsumosController(IInsumoService InsumosService)
        {
            _InsumosService = InsumosService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Lista(int IdUnidadNegocio)
        {
            var Insumos = await _InsumosService.ObtenerTodos();

            var lista = Insumos.Select(c => new VMInsumo
            {
                Id = c.Id,
                //CostoUnitario = c.CostoUnitario,
                FechaActualizacion = c.FechaActualizacion,
                IdCategoria = c.IdCategoria,
                IdUnidadMedida = c.IdUnidadMedida,
                Sku = c.Sku,
                Categoria = c.IdCategoriaNavigation.Nombre,
                UnidadMedida = c.IdUnidadMedidaNavigation.Nombre,
                Descripcion = c.Descripcion,
            }).Where(x => x.IdUnidadNegocio == IdUnidadNegocio || IdUnidadNegocio == -1).ToList();

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMInsumo model)
        {
            var Insumos = new Insumo
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                //CostoUnitario = model.CostoUnitario,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
            };

            bool respuesta = await _InsumosService.Insertar(Insumos);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMInsumo model)
        {
            var Insumos = new Insumo
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                //CostoUnitario = model.CostoUnitario,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
            };

            bool respuesta = await _InsumosService.Actualizar(Insumos);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _InsumosService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            var EstadosUsuario = await _InsumosService.Obtener(id);

            if (EstadosUsuario != null)
            {
                return StatusCode(StatusCodes.Status200OK, EstadosUsuario);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
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