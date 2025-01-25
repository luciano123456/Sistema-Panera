using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class ProductosTipoService : IProductosTiposervice
    {

        private readonly IProductosTipoRepository<ProductosTipo> _contactRepo;

        public ProductosTipoService(IProductosTipoRepository<ProductosTipo> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(ProductosTipo model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(ProductosTipo model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<ProductosTipo> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }


        public async Task<IQueryable<ProductosTipo>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
