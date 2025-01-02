using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IUnidadDeMedidaService
    {
        Task<bool> Insertar(UnidadesDeMedida model);
        Task<bool> Actualizar(UnidadesDeMedida model);
        Task<bool> Eliminar(int id);
        Task<UnidadesDeMedida> Obtener(int id);
        Task<IQueryable<UnidadesDeMedida>> ObtenerTodos();
    }
}
