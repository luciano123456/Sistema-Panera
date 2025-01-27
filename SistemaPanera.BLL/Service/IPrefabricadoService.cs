using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IPrefabricadoService
    {
        Task<bool> Insertar(Prefabricado model);
        Task<bool> Actualizar(Prefabricado model);
        Task<bool> Eliminar(int id);
        Task<Prefabricado> Obtener(int id);
        Task<IQueryable<Prefabricado>> ObtenerTodos();
    }
}
