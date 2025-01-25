using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IProductoService
    {
        Task<bool> Insertar(Producto model);
        Task<bool> Actualizar(Producto model);
        Task<bool> Eliminar(int id);
        Task<Producto> Obtener(int id);
        Task<IQueryable<Producto>> ObtenerTodos();
    }
}
