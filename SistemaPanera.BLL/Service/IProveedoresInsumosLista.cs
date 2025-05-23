using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IProveedoresInsumoservice
    {
        Task<bool> Insertar(ProveedoresInsumos model);
        Task<bool> Actualizar(ProveedoresInsumos model);
        Task<bool> Eliminar(int id);
        Task<ProveedoresInsumos> Obtener(int id);
        Task<IQueryable<ProveedoresInsumos>> ObtenerTodos();
    }
}
