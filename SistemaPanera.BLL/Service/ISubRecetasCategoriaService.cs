using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface ISubRecetasCategoriaService
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(SubrecetasCategoria model);
        Task<bool> Insertar(SubrecetasCategoria model);

        Task<SubrecetasCategoria> Obtener(int id);

        Task<IQueryable<SubrecetasCategoria>> ObtenerTodos();
    }

}
