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
    public class RecetasTipoController : Controller
    {
        private readonly IRecetasTiposervice _RecetasTiposervice;

        public RecetasTipoController(IRecetasTiposervice RecetasTiposervice)
        {
            _RecetasTiposervice = RecetasTiposervice;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var RecetasTipo = await _RecetasTiposervice.ObtenerTodos();

            var lista = RecetasTipo.Select(c => new VMRecetasTipo
            {
                Id = c.Id,
                Nombre = c.Nombre,
            }).ToList();

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMRecetasTipo model)
        {
            var RecetasTipo = new RecetasTipo
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _RecetasTiposervice.Insertar(RecetasTipo);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMRecetasTipo model)
        {
            var RecetasTipo = new RecetasTipo
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _RecetasTiposervice.Actualizar(RecetasTipo);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _RecetasTiposervice.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
             var EstadosUsuario = await _RecetasTiposervice.Obtener(id);

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