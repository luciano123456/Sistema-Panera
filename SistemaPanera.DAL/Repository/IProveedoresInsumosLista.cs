using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface IProveedoresInsumosRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(ProveedoresInsumos model);
        Task<bool> Insertar(ProveedoresInsumos model);
        Task<ProveedoresInsumos> Obtener(int id);
        Task<IQueryable<ProveedoresInsumos>> ObtenerTodos();
        Task<IQueryable<ProveedoresInsumos>> ObtenerPorProveedor(int idProveedor);
        Task<bool> ImportarDesdeLista(int idProveedor, List<ProveedoresInsumos> lista);
    }
}
