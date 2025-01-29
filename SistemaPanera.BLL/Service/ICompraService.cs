using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface ICompraService
    {
        Task<bool> Insertar(Compra model);
        Task<bool> Actualizar(Compra model);
        Task<bool> Eliminar(int id);
        Task<Compra> Obtener(int id);
        Task<IQueryable<Compra>> ObtenerTodos();

        Task<bool> InsertarInsumos(List<ComprasDetalle> insumos);
        Task<List<ComprasDetalle>> ObtenerInsumos(int idCompra);
        Task<bool> ActualizarInsumos(List<ComprasDetalle> insumos);
    }
}
