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
    public class ProductosController : Controller
    {
        private readonly IProductoService _ProductosService;
        private readonly IProductoInsumoService _ProductosInsumoService;

        public ProductosController(IProductoService ProductosService, IProductoInsumoService productosInsumoService)
        {
            _ProductosService = ProductosService;
            _ProductosInsumoService = productosInsumoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NuevoModif()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista(int IdUnidadNegocio)
        {
            var Productos = await _ProductosService.ObtenerTodos();

            var lista = Productos.Select(c => new VMProducto
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
                CostoUnitario = c.CostoUnitario,
                CostoInsumos = c.CostoInsumos,
                CostoTotal = c.CostoTotal,
            }).Where(x => x.IdUnidadNegocio == IdUnidadNegocio || IdUnidadNegocio == -1).ToList();

            return Ok(lista);
        }
        [HttpGet]
        public async Task<IActionResult> ListaInsumos(int IdProducto)
        {
            var productos = await _ProductosInsumoService.ObtenerInsumosProducto(IdProducto);

            var lista = new List<VMProductoInsumo>();

            foreach (var c in productos)
            {
                lista.Add(new VMProductoInsumo
                {
                    Id = c.Id,
                    Cantidad = c.Cantidad,
                    CostoUnitario = c.CostoUnitario ?? 0,
                    IdProducto = IdProducto,
                    IdTipo = c.IdTipo,
                    SubTotal = c.SubTotal,
                    Tipo = c.IdProductosTipoNavigation.Nombre
                });
            }

            return Ok(lista);
        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMProducto model)
        {
            var Productos = new Producto
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                CostoUnitario = model.CostoUnitario,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoTotal = model.CostoTotal,
                CostoInsumos = model.CostoInsumos
            };

            bool respuesta = await _ProductosService.Insertar(Productos);

            return Ok(new { valor = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMProducto model)
        {
            var Productos = new Producto
            {
                Id = model.Id,
                IdUnidadMedida = model.IdUnidadMedida,
                Sku = model.Sku,
                IdUnidadNegocio = model.IdUnidadNegocio,
                CostoUnitario = model.CostoUnitario,
                FechaActualizacion = DateTime.Now,
                IdCategoria = model.IdCategoria,
                Descripcion = model.Descripcion,
                CostoTotal = model.CostoTotal,
                CostoInsumos = model.CostoInsumos
            };

            bool respuesta = await _ProductosService.Actualizar(Productos);

            return Ok(new { valor = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _ProductosService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
        }

        [HttpGet]
        public async Task<IActionResult> EditarInfo(int id)
        {
            var EstadosUsuario = await _ProductosService.Obtener(id);

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