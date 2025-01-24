using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface ILocalService
    {
        Task<bool> Insertar(Local model);
        Task<bool> Actualizar(Local model);
        Task<bool> Eliminar(int id);
        Task<Local> Obtener(int id);
        Task<IQueryable<Local>> ObtenerTodos();
    }
}
