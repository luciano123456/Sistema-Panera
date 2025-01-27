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
            var EstadosUsuario = await _PrefabricadosService.Obtener(id);

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