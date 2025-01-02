using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IColorService
    {
        Task<bool> Insertar(Color model);
        Task<bool> Actualizar(Color model);
        Task<bool> Eliminar(int id);
        Task<Color> Obtener(int id);
        Task<IQueryable<Color>> ObtenerTodos();
    }
}
