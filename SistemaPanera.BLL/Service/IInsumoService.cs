using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IInsumoService
    {
        Task<bool> Insertar(Insumo model);
        Task<bool> Actualizar(Insumo model);
        Task<bool> Eliminar(int id);
        Task<Insumo> Obtener(int id);
        Task<IQueryable<Insumo>> ObtenerTodos();
        Task<IQueryable<Insumo>> ObtenerPorProveedor(int idProveedor);
    }
}
