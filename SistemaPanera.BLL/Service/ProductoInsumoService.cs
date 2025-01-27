using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class ProductoInsumoService : IProductoInsumoService
    {

        private readonly IProductoInsumoRepository<ProductosInsumo> _contactRepo;

        public ProductoInsumoService(IProductoInsumoRepository<ProductosInsumo> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(ProductosInsumo model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(ProductosInsumo model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<ProductosInsumo> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }


        public async Task<IQueryable<ProductosInsumo>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }

        public async Task<IQueryable<ProductosInsumo>> ObtenerInsumosProducto(int id)
        {
            return await _contactRepo.ObtenerInsumosProducto(id);
        }



    }
}
