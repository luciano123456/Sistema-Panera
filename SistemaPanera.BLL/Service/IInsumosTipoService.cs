using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IInsumosTipoService
    {
        Task<bool> Insertar(InsumosTipo model);
        Task<bool> Actualizar(InsumosTipo model);
        Task<bool> Eliminar(int id);
        Task<InsumosTipo> Obtener(int id);
        Task<IQueryable<InsumosTipo>> ObtenerTodos();
    }
}
