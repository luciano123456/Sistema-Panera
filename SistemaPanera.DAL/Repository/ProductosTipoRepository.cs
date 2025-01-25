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
    public class ProductosTipoRepository : IProductosTipoRepository<ProductosTipo>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public ProductosTipoRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(ProductosTipo model)
        {
            _dbcontext.ProductosTipos.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            ProductosTipo model = _dbcontext.ProductosTipos.First(c => c.Id == id);
            _dbcontext.ProductosTipos.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(ProductosTipo model)
        {
            _dbcontext.ProductosTipos.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductosTipo> Obtener(int id)
        {
            ProductosTipo model = await _dbcontext.ProductosTipos.FindAsync(id);
            return model;
        }
        public async Task<IQueryable<ProductosTipo>> ObtenerTodos()
        {
            IQueryable<ProductosTipo> query = _dbcontext.ProductosTipos;
            return await Task.FromResult(query);
        }




    }
}
