using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface ISubrecetaService
    {
        Task<bool> Insertar(Subreceta model);
        Task<bool> Actualizar(Subreceta model);
        Task<bool> Eliminar(int id);
        Task<Subreceta> Obtener(int id);
        Task<IQueryable<Subreceta>> ObtenerTodos();

        Task<bool> InsertarInsumos(List<SubrecetasInsumo> insumos);
        Task<List<SubrecetasInsumo>> ObtenerInsumos(int idSubreceta);
        Task<bool> ActualizarInsumos(List<SubrecetasInsumo> insumos);
    }
}
