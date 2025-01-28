using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface IPrefabricadoRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(Prefabricado model);
        Task<bool> Insertar(Prefabricado model);
        Task<Prefabricado> Obtener(int id);
        Task<IQueryable<Prefabricado>> ObtenerTodos();
        Task<bool> InsertarInsumos(List<PrefabricadosInsumo> insumos);
        Task<List<PrefabricadosInsumo>> ObtenerInsumos(int idPrefabricado);
        Task<bool> ActualizarInsumos(List<PrefabricadosInsumo> insumos);
    }
}
