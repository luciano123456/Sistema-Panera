using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IRecetaService
    {
        Task<bool> Insertar(Receta model);
        Task<bool> Actualizar(Receta model);
        Task<bool> Eliminar(int id);
        Task<Receta> Obtener(int id);
        Task<IQueryable<Receta>> ObtenerTodos();

        Task<bool> InsertarInsumos(List<RecetasInsumo> insumos);
        Task<List<RecetasInsumo>> ObtenerInsumos(int idReceta);
        Task<bool> ActualizarInsumos(List<RecetasInsumo> insumos);

        //Task<bool> InsertarPrefabricados(List<RecetasPrefabricado> insumos);
        //Task<List<RecetasPrefabricado>> ObtenerPrefabricados(int idReceta);
        //Task<bool> ActualizarPrefabricados(List<RecetasPrefabricado> insumos);
    }
}
