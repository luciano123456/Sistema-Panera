using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IProductosTiposervice
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(ProductosTipo model);
        Task<bool> Insertar(ProductosTipo model);

        Task<ProductosTipo> Obtener(int id);

        Task<IQueryable<ProductosTipo>> ObtenerTodos();
    }

}
