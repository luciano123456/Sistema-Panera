using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class PrefabricadosCategoriaService : IPrefabricadosCategoriaService
    {

        private readonly IPrefabricadosCategoriaRepository<PrefabricadosCategoria> _contactRepo;

        public PrefabricadosCategoriaService(IPrefabricadosCategoriaRepository<PrefabricadosCategoria> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(PrefabricadosCategoria model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(PrefabricadosCategoria model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<PrefabricadosCategoria> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }


        public async Task<IQueryable<PrefabricadosCategoria>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
