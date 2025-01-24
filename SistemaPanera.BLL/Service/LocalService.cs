using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class LocalService : ILocalService
    {

        private readonly IGenericRepository<Local> _contactRepo;
        private readonly Provincia _provinciaRepo;

        public LocalService(IGenericRepository<Local> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(Local model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(Local model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<Local> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }

        public async Task<IQueryable<Local>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
