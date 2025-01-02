using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IProvinciaService
    {
        Task<IQueryable<Provincia>> ObtenerTodos();
    }
}
