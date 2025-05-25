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
    public class SubrecetasCategoriaController : Controller
    {
        private readonly ISubRecetasCategoriaService _SubrecetasCategoriaService;

        public SubrecetasCategoriaController(ISubRecetasCategoriaService SubrecetasCategoriaService)
        {
            _SubrecetasCategoriaService = SubrecetasCategoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var SubrecetasCategoria = await _SubrecetasCategoriaService.ObtenerTodos();

            var lista = SubrecetasCategoria.Select(c => new VMSubrecetasCategoria
            {
                Id = c.Id,
                Nombre = c.Nombre,
            }).ToList();

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMSubrecetasCategoria model)
        {
            var SubrecetasCategoria = new SubrecetasCategoria
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _SubrecetasCategoriaService.Insertar(SubrecetasCategoria);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMSubrecetasCategoria model)
        {
            var SubrecetasCategoria = new SubrecetasCategoria
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _SubrecetasCategoriaService.Actualizar(SubrecetasCategoria);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _SubrecetasCategoriaService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            var EstadosUsuario = await _SubrecetasCategoriaService.Obtener(id);

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