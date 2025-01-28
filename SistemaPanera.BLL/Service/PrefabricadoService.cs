using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class PrefabricadoService : IPrefabricadoService
    {

        private readonly IPrefabricadoRepository<Prefabricado> _contactRepo;

        public PrefabricadoService(IPrefabricadoRepository<Prefabricado> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(Prefabricado model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(Prefabricado model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<Prefabricado> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }
        public async Task<IQueryable<Prefabricado>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }

        public async Task<bool> InsertarInsumos(List<PrefabricadosInsumo> insumos)
        {
            return await _contactRepo.InsertarInsumos(insumos);
        }

        public async Task<bool> ActualizarInsumos(List<PrefabricadosInsumo> productos)
        {
            return await _contactRepo.ActualizarInsumos(productos);
        }

        public async Task<List<PrefabricadosInsumo>> ObtenerInsumos(int idPrefabricado)
        {
            return await _contactRepo.ObtenerInsumos(idPrefabricado);
        }
    }
}
