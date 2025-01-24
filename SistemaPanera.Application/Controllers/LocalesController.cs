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
    public class LocalesController : Controller
    {
        private readonly ILocalService _LocalService;
        private readonly IProvinciaService _provinciaService;

        public LocalesController(ILocalService LocalService)
        {
            _LocalService = LocalService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista(int IdUnidadNegocio)
        {
            var Locales = await _LocalService.ObtenerTodos();

            var lista = Locales.Select(c => new VMLocal
            {
                Id = c.Id,
                Nombre = c.Nombre,
                IdUnidadNegocio = c.IdUnidadNegocio,
                UnidadNegocio = c.IdUnidadNegocioNavigation.Nombre,
            }).Where(x => x.IdUnidadNegocio == IdUnidadNegocio || IdUnidadNegocio == -1).ToList();

            return Ok(lista);
        }



        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMLocal model)
        {
            var Local = new Local
            {
                Id = model.Id,
                Nombre = model.Nombre,
                IdUnidadNegocio = model.IdUnidadNegocio
            };

            bool respuesta = await _LocalService.Insertar(Local);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMLocal model)
        {
            var Local = new Local
            {
                Id = model.Id,
                Nombre = model.Nombre,
                IdUnidadNegocio = model.IdUnidadNegocio
            };

            bool respuesta = await _LocalService.Actualizar(Local);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _LocalService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            var Local = await _LocalService.Obtener(id);

            if (Local != null)
            {
                return StatusCode(StatusCodes.Status200OK, Local);
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