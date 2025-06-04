using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class ProveedoresInsumoservice : IProveedoresInsumoservice
    {

        private readonly IProveedoresInsumosRepository<ProveedoresInsumos> _contactRepo;
        private readonly Provincia _provinciaRepo;

        public ProveedoresInsumoservice(IProveedoresInsumosRepository<ProveedoresInsumos> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(ProveedoresInsumos model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> ImportarDesdeLista(int idProveedor, List<ProveedoresInsumos> lista)
        {
            return await _contactRepo.ImportarDesdeLista(idProveedor, lista);
        }

        public async Task<bool> Insertar(ProveedoresInsumos model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<ProveedoresInsumos> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }

        public async Task<IQueryable<ProveedoresInsumos>> ObtenerPorProveedor(int idProveedor)
        {
            return await _contactRepo.ObtenerPorProveedor(idProveedor);
        }



        //public async Task<ProveedoresInsumos> ObtenerPorNombre(string nombre)
        //{
        //    IQueryable<ProveedoresInsumos> queryProveedoresInsumosQL = await _contactRepo.ObtenerTodos();

        //    ProveedoresInsumos ProveedoresInsumos = queryProveedoresInsumosQL.Where(c => c.Nombre == nombre).FirstOrDefault();

        //    return ProveedoresInsumos;
        //}

        public async Task<IQueryable<ProveedoresInsumos>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
