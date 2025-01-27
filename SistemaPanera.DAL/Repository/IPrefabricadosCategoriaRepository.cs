using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface IPrefabricadosCategoriaRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(PrefabricadosCategoria model);
        Task<bool> Insertar(PrefabricadosCategoria model);
        Task<PrefabricadosCategoria> Obtener(int id);
        Task<IQueryable<PrefabricadosCategoria>> ObtenerTodos();
    }
}
