using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface IProductosTipoRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(ProductosTipo model);
        Task<bool> Insertar(ProductosTipo model);
        Task<ProductosTipo> Obtener(int id);
        Task<IQueryable<ProductosTipo>> ObtenerTodos();
    }
}
