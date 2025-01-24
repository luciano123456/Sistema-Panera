using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class UnidadesNegocioService : IUnidadesNegocioService
    {

        private readonly IUnidadesNegocioRepository<UnidadesNegocio> _contactRepo;

        public UnidadesNegocioService(IUnidadesNegocioRepository<UnidadesNegocio> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(UnidadesNegocio model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(UnidadesNegocio model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<UnidadesNegocio> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }


        public async Task<IQueryable<UnidadesNegocio>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
