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
            var insumos = await _InsumosService.ObtenerTodos();

            var lista = insumos
                .Where(c => IdUnidadNegocio == -1 || c.InsumosUnidadesNegocios.Any(u => u.IdUnidadNegocio == IdUnidadNegocio))
                .Select(c => new VMInsumo
                {
                    Id = c.Id,
                    Descripcion = c.Descripcion,
                    Sku = c.Sku,
                    IdCategoria = c.IdCategoria,
                    IdUnidadMedida = c.IdUnidadMedida,
                    FechaActualizacion = c.FechaActualizacion,
                    Categoria = c.IdCategoriaNavigation.Nombre,
                    UnidadMedida = c.IdUnidadMedidaNavigation.Nombre,

                    // NUEVO: lista de nombres de unidades de negocio
                    UnidadesNegocio = c.InsumosUnidadesNegocios
                        .Select(u => u.IdUnidadNegocioNavigation.Nombre)
                        .ToList(),

                    // NUEVO: lista de nombres de proveedores
                    Proveedores = c.InsumosProveedores
                        .Select(p => p.IdProveedorNavigation.Nombre)
                        .ToList()
                })
                .ToList();

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
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                InsumosProveedores = model.InsumosProveedores,
                InsumosUnidadesNegocios = model.InsumosUnidadesNegocios
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
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                InsumosUnidadesNegocios = model.InsumosUnidadesNegocios
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
            var insumo = await _InsumosService.Obtener(id);
            if (insumo == null) return NotFound();

            var vm = new VMInsumo
            {
                Id = insumo.Id,
                Sku = insumo.Sku,
                Descripcion = insumo.Descripcion,
                IdCategoria = insumo.IdCategoria,
                IdUnidadMedida = insumo.IdUnidadMedida,
                FechaActualizacion = insumo.FechaActualizacion,
                InsumosProveedores = insumo.InsumosProveedores.Select(p => new InsumosProveedor
                {
                    IdProveedor = p.IdProveedor
                }).ToList(),
                InsumosUnidadesNegocios = insumo.InsumosUnidadesNegocios.Select(u => new InsumosUnidadesNegocio
                {
                    IdUnidadNegocio = u.IdUnidadNegocio
                }).ToList()
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