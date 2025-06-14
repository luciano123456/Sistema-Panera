using SistemaPanera.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class OrdenesCompraService : IOrdenesCompraService
{
    private readonly IOrdenesCompraRepository _repo;

    public OrdenesCompraService(IOrdenesCompraRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<OrdenesCompra>> ObtenerTodosAsync() => await _repo.ObtenerTodosAsync();

    public async Task<OrdenesCompra?> ObtenerPorIdAsync(int id) => await _repo.ObtenerPorIdAsync(id);

    public async Task CrearAsync(OrdenesCompra entidad) => await _repo.CrearAsync(entidad);

    public async Task EditarAsync(OrdenesCompra entidad) => await _repo.EditarAsync(entidad);

    public async Task EliminarAsync(int id) => await _repo.EliminarAsync(id);
}