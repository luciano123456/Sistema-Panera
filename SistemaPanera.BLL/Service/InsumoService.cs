using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class InsumoService : IInsumoService
    {

        private readonly IInsumoRepository<Insumo> _contactRepo;
        private readonly Provincia _provinciaRepo;

        public InsumoService(IInsumoRepository<Insumo> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(Insumo model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(Insumo model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<Insumo> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }



        //public async Task<Insumo> ObtenerPorNombre(string nombre)
        //{
        //    IQueryable<Insumo> queryInsumoSQL = await _contactRepo.ObtenerTodos();

        //    Insumo Insumo = queryInsumoSQL.Where(c => c.Nombre == nombre).FirstOrDefault();

        //    return Insumo;
        //}

        public async Task<IQueryable<Insumo>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
