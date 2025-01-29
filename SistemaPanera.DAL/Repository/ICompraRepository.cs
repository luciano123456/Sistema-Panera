using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface ICompraRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(Compra model);
        Task<bool> Insertar(Compra model);
        Task<Compra> Obtener(int id);
        Task<IQueryable<Compra>> ObtenerTodos();
        Task<bool> InsertarInsumos(List<ComprasDetalle> insumos);
        Task<List<ComprasDetalle>> ObtenerInsumos(int idCompra);
        Task<bool> ActualizarInsumos(List<ComprasDetalle> insumos);
    }
}
