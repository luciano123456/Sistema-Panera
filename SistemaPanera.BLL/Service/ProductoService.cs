using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class ProductoService : IProductoService
    {

        private readonly IProductoRepository<Producto> _contactRepo;

        public ProductoService(IProductoRepository<Producto> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(Producto model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(Producto model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<Producto> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }

        public async Task<IQueryable<Producto>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }

    }
}
