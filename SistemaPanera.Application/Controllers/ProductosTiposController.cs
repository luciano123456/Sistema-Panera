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
    public class ProductosTipoController : Controller
    {
        private readonly IProductosTiposervice _ProductosTiposervice;

        public ProductosTipoController(IProductosTiposervice ProductosTiposervice)
        {
            _ProductosTiposervice = ProductosTiposervice;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var ProductosTipo = await _ProductosTiposervice.ObtenerTodos();

            var lista = ProductosTipo.Select(c => new VMProductosTipo
            {
                Id = c.Id,
                Nombre = c.Nombre,
            }).ToList();

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMProductosTipo model)
        {
            var ProductosTipo = new ProductosTipo
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _ProductosTiposervice.Insertar(ProductosTipo);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMProductosTipo model)
        {
            var ProductosTipo = new ProductosTipo
            {
                Id = model.Id,
                Nombre = model.Nombre,
            };

            bool respuesta = await _ProductosTiposervice.Actualizar(ProductosTipo);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _ProductosTiposervice.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
             var EstadosUsuario = await _ProductosTiposervice.Obtener(id);

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