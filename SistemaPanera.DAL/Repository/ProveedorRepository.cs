using Microsoft.EntityFrameworkCore;
using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using SistemaPanera.DAL.DataContext;

namespace SistemaPanera.DAL.Repository
{
    public class ProveedorRepository : IGenericRepository<Models.Proveedor>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public ProveedorRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Models.Proveedor model)
        {
            _dbcontext.Proveedores.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Proveedor model = _dbcontext.Proveedores.First(c => c.Id == id);
            _dbcontext.Proveedores.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Proveedor model)
        {
            _dbcontext.Proveedores.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Proveedor> Obtener(int id)
        {
            Models.Proveedor model = await _dbcontext.Proveedores.FindAsync(id);
            return model;
        }

        public async Task<IQueryable<Models.Proveedor>> ObtenerTodos()
        {
            IQueryable<Models.Proveedor> query = _dbcontext.Proveedores;
            return await Task.FromResult(query);
        }




    }
}
