using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface IProductoRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(Producto model);
        Task<bool> Insertar(Producto model);
        Task<Producto> Obtener(int id);
        Task<IQueryable<Producto>> ObtenerTodos();
    }
}
