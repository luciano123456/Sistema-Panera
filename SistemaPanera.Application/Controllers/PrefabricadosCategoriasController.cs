using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPanera.Application.Models;
using SistemaPanera.Application.Models.ViewModels;
using SistemaPanera.BLL.Service;
using SistemaPanera.Models;
using System.Diagnostics;

namespace SistemaPanera.Application.Controllers
{
    //[Authorize]
    //public class PrefabricadosCategoriaController : Controller
    //{
    //    private readonly IPrefabricadosCategoriaService _PrefabricadosCategoriaService;

    //    public PrefabricadosCategoriaController(IPrefabricadosCategoriaService PrefabricadosCategoriaService)
    //    {
    //        _PrefabricadosCategoriaService = PrefabricadosCategoriaService;
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> Lista()
    //    {
    //        var PrefabricadosCategoria = await _PrefabricadosCategoriaService.ObtenerTodos();

    //        var lista = PrefabricadosCategoria.Select(c => new VMPrefabricadosCategoria
    //        {
    //            Id = c.Id,
    //            Nombre = c.Nombre,
    //        }).ToList();

    //        return Ok(lista);
    //    }


    //    [HttpPost]
    //    public async Task<IActionResult> Insertar([FromBody] VMPrefabricadosCategoria model)
    //    {
    //        var PrefabricadosCategoria = new PrefabricadosCategoria
    //        {
    //            Id = model.Id,
    //            Nombre = model.Nombre,
    //        };

    //        bool respuesta = await _PrefabricadosCategoriaService.Insertar(PrefabricadosCategoria);

    //        return Ok(new { valor = respuesta });
    //    }

    //    [HttpPut]
    //    public async Task<IActionResult> Actualizar([FromBody] VMPrefabricadosCategoria model)
    //    {
    //        var PrefabricadosCategoria = new PrefabricadosCategoria
    //        {
    //            Id = model.Id,
    //            Nombre = model.Nombre,
    //        };

    //        bool respuesta = await _PrefabricadosCategoriaService.Actualizar(PrefabricadosCategoria);

    //        return Ok(new { valor = respuesta });
    //    }

    //    [HttpDelete]
    //    public async Task<IActionResult> Eliminar(int id)
    //    {
    //        bool respuesta = await _PrefabricadosCategoriaService.Eliminar(id);

    //        return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> EditarInfo(int id)
    //    {
    //         var EstadosUsuario = await _PrefabricadosCategoriaService.Obtener(id);

    //        if (EstadosUsuario != null)
    //        {
    //            return StatusCode(StatusCodes.Status200OK, EstadosUsuario);
    //        }
    //        else
    //        {
    //            return StatusCode(StatusCodes.Status404NotFound);
    //        }
    //    }
    //    public IActionResult Privacy()
    //    {
    //        return View();
    //    }

    //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //    public IActionResult Error()
    //    {
    //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //    }
    //}
}