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

        //public async Task<Prefabricado> ObtenerPorNombre(string nombre)
        //{
        //    IQueryable<Prefabricado> queryPrefabricadoSQL = await _contactRepo.ObtenerTodos();

        //    Prefabricado Prefabricado = queryPrefabricadoSQL.Where(c => c.Nombre == nombre).FirstOrDefault();

        //    return Prefabricado;
        //}

        public async Task<IQueryable<Prefabricado>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
