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
    public class UnidadesNegocioController : Controller
    {
        private readonly IUnidadesNegocioService _UnidadesNegocioService;

        public UnidadesNegocioController(IUnidadesNegocioService UnidadesNegocioService)
        {
            _UnidadesNegocioService = UnidadesNegocioService;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var UnidadesNegocio = await _UnidadesNegocioService.ObtenerTodos();

            var lista = UnidadesNegocio.Select(c => new VMUnidadesNegocio
            {
                Id = c.Id,
                Nombre = c.Nombre,
            }).ToList();

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMUnidadesNegocio model)
        {
            var UnidadesNegocio = new UnidadesNegocio
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _UnidadesNegocioService.Insertar(UnidadesNegocio);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMUnidadesNegocio model)
        {
            var UnidadesNegocio = new UnidadesNegocio
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _UnidadesNegocioService.Actualizar(UnidadesNegocio);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _UnidadesNegocioService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
             var EstadosUsuario = await _UnidadesNegocioService.Obtener(id);

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