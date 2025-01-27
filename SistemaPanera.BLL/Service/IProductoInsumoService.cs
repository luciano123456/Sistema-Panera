using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IProductoInsumoService
    {
        Task<bool> Insertar(ProductosInsumo model);
        Task<bool> Actualizar(ProductosInsumo model);
        Task<bool> Eliminar(int id);
        Task<ProductosInsumo> Obtener(int id);
        Task<IQueryable<ProductosInsumo>> ObtenerTodos();
        Task<IQueryable<ProductosInsumo>> ObtenerInsumosProducto(int id);
    }
}
