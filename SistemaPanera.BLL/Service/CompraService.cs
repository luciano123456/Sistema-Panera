using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class CompraService : ICompraService
    {

        private readonly ICompraRepository<Compra> _contactRepo;

        public CompraService(ICompraRepository<Compra> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(Compra model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(Compra model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<Compra> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }
        public async Task<IQueryable<Compra>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }

        public async Task<bool> InsertarInsumos(List<ComprasDetalle> insumos)
        {
            return await _contactRepo.InsertarInsumos(insumos);
        }

        public async Task<bool> ActualizarInsumos(List<ComprasDetalle> productos)
        {
            return await _contactRepo.ActualizarInsumos(productos);
        }

        public async Task<List<ComprasDetalle>> ObtenerInsumos(int idCompra)
        {
            return await _contactRepo.ObtenerInsumos(idCompra);
        }


    }
}
