using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface IProductoInsumoRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(ProductosInsumo model);
        Task<bool> Insertar(ProductosInsumo model);
        Task<ProductosInsumo> Obtener(int id);
        Task<IQueryable<ProductosInsumo>> ObtenerTodos();
        Task<IQueryable<ProductosInsumo>> ObtenerInsumosProducto(int id);
    }
}
