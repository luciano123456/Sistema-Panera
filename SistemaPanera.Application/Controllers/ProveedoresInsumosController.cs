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
    public class ProveedoresInsumosController : Controller
    {
        private readonly IProveedoresInsumoservice _ProveedoresInsumosService;

        public ProveedoresInsumosController(IProveedoresInsumoservice ProveedoresInsumosService)
        {
            _ProveedoresInsumosService = ProveedoresInsumosService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Lista(int IdProveedor)
        {
            var ProveedoresInsumos = await _ProveedoresInsumosService.ObtenerTodos();

            var lista = ProveedoresInsumos
                .Where(c => IdProveedor == -1 || c.IdProveedor == IdProveedor)
                .Select(c => new VMProveedoresInsumos
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    CostoUnitario = c.CostoUnitario,
                    Codigo = c.Codigo,
                    FechaActualizacion = c.FechaActualizacion,
                    IdProveedor = c.IdProveedor,
                    Proveedor = c.IdProveedorNavigation != null ? c.IdProveedorNavigation.Nombre : ""
            
                })
                .ToList();

            return Ok(lista);
        }



        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMProveedoresInsumos model)
        {
            var ProveedoresInsumos = new ProveedoresInsumos
            {
                Descripcion = model.Descripcion,
                CostoUnitario = model.CostoUnitario,
                Codigo = model.Codigo,
                FechaActualizacion = DateTime.Now,
                IdProveedor = model.IdProveedor,
            };

            bool respuesta = await _ProveedoresInsumosService.Insertar(ProveedoresInsumos);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMProveedoresInsumos model)
        {
            var ProveedoresInsumos = new ProveedoresInsumos
            {
                Id = model.Id,
                Descripcion = model.Descripcion,
                CostoUnitario = model.CostoUnitario,
                Codigo = model.Codigo,
                FechaActualizacion = DateTime.Now,
                IdProveedor = model.IdProveedor,
            };

            bool respuesta = await _ProveedoresInsumosService.Actualizar(ProveedoresInsumos);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _ProveedoresInsumosService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            var ProveedoresInsumos = await _ProveedoresInsumosService.Obtener(id);
            if (ProveedoresInsumos == null) return NotFound();

            var vm = new VMProveedoresInsumos
            {
                Id = ProveedoresInsumos.Id,
                Descripcion = ProveedoresInsumos.Descripcion,
                CostoUnitario = ProveedoresInsumos.CostoUnitario,
                FechaActualizacion = ProveedoresInsumos.FechaActualizacion,
                IdProveedor = ProveedoresInsumos.IdProveedor,
                Codigo = ProveedoresInsumos.Codigo,
            };

            return Ok(vm);
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