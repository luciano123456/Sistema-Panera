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
    public class ProductoRepository : IProductoRepository<Producto>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public ProductoRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Models.Producto model)
        {
            _dbcontext.Productos.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Producto model = _dbcontext.Productos.First(c => c.Id == id);
            _dbcontext.Productos.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Producto model)
        {
            _dbcontext.Productos.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Producto> Obtener(int id)
        {
            Producto model = await _dbcontext.Productos.FindAsync(id);
            return model;
        }
        

       
        public async Task<IQueryable<Models.Producto>> ObtenerTodos()
        {
            IQueryable<Models.Producto> query = _dbcontext.Productos;
            return await Task.FromResult(query);
        }




    }
}
