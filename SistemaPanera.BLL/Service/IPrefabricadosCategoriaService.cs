using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public interface IPrefabricadosCategoriaService
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(PrefabricadosCategoria model);
        Task<bool> Insertar(PrefabricadosCategoria model);

        Task<PrefabricadosCategoria> Obtener(int id);

        Task<IQueryable<PrefabricadosCategoria>> ObtenerTodos();
    }

}
