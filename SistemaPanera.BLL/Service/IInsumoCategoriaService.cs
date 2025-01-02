using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IInsumoCategoriaService
    {
        Task<bool> Insertar(InsumosCategoria model);
        Task<bool> Actualizar(InsumosCategoria model);
        Task<bool> Eliminar(int id);
        Task<InsumosCategoria> Obtener(int id);
        Task<IQueryable<InsumosCategoria>> ObtenerTodos();
    }
}
