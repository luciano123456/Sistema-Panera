
using SistemaPanera.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IOrdenesCompraService
{
    Task<IEnumerable<OrdenesCompra>> ObtenerTodosAsync();
    Task<OrdenesCompra?> ObtenerPorIdAsync(int id);
    Task CrearAsync(OrdenesCompra entidad);
    Task EditarAsync(OrdenesCompra entidad);
    Task EliminarAsync(int id);
}