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
            try
            {
                var insumos = await _InsumosService.ObtenerTodos();

                var lista = insumos
                    .Where(c => IdUnidadNegocio == -1 || c.InsumosUnidadesNegocios.Any(u => u.IdUnidadNegocio == IdUnidadNegocio))
                    .ToList()
                    .Select(c =>
                    {
                        var proveedorMasBarato = c.InsumosProveedores
                            .Where(p => p.IdListaProveedorNavigation != null && p.IdListaProveedorNavigation.IdProveedorNavigation != null)
                            .OrderBy(p => p.IdListaProveedorNavigation.CostoUnitario)
                            .FirstOrDefault();

                        return new VMInsumo
                        {
                            Id = c.Id,
                            Descripcion = c.Descripcion,
                            Sku = c.Sku,
                            IdCategoria = c.IdCategoria,
                            IdUnidadMedida = c.IdUnidadMedida,
                            FechaActualizacion = c.FechaActualizacion,
                            Categoria = c.IdCategoriaNavigation?.Nombre ?? "",
                            UnidadMedida = c.IdUnidadMedidaNavigation?.Nombre ?? "",
                            UnidadesNegocio = c.InsumosUnidadesNegocios
                                .Select(u => u.IdUnidadNegocioNavigation?.Nombre ?? "")
                                .ToList(),
                            ProveedorDestacado = proveedorMasBarato?.IdListaProveedorNavigation?.IdProveedorNavigation?.Nombre ?? "",
                            CostoUnitario = proveedorMasBarato?.IdListaProveedorNavigation?.CostoUnitario ?? 0,
                            CantidadProveedores = c.InsumosProveedores?.Count ?? 0
                        };
                    })
                    .ToList();

                return Ok(lista);
            } catch ( Exception ex)
            {
                return Ok(null);
            }
        }







        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMInsumo model)
        {
            var insumo = new Insumo
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,

                InsumosUnidadesNegocios = model.InsumosUnidadesNegocios?.Select(u => new InsumosUnidadesNegocio
                {
                    IdUnidadNegocio = u.IdUnidadNegocio
                }).ToList(),

                InsumosProveedores = model.InsumosProveedores?
                    .GroupBy(p => new { p.IdProveedor, p.IdListaProveedor }) // evita duplicados
                    .Select(g => new InsumosProveedor
                    {
                        IdProveedor = g.Key.IdProveedor,
                        IdListaProveedor = g.Key.IdListaProveedor
                        // IdInsumo se asigna en el Insertar
                    }).ToList()
            };

            var respuesta = await _InsumosService.Insertar(insumo);

            return Ok(new
            {
                valor = respuesta,
                mensaje = respuesta ? "Insumo registrado correctamente" : "Error al registrar"
            });
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
                InsumosProveedores = model.InsumosProveedores,
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
                    Id = p.Id,
                    IdProveedor = p.IdProveedor,
                    IdInsumo = p.IdInsumo,
                    IdListaProveedor = p.IdListaProveedor
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