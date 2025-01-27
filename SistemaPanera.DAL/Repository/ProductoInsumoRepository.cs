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
    public class ProductoInsumoRepository : IProductoInsumoRepository<ProductosInsumo>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public ProductoInsumoRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(ProductosInsumo model)
        {
            _dbcontext.ProductosInsumos.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            ProductosInsumo model = _dbcontext.ProductosInsumos.First(c => c.Id == id);
            _dbcontext.ProductosInsumos.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(ProductosInsumo model)
        {
            _dbcontext.ProductosInsumos.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductosInsumo> Obtener(int id)
        {
            ProductosInsumo model = await _dbcontext.ProductosInsumos.FindAsync(id);
            return model;
        }

        public async Task<IQueryable<ProductosInsumo>> ObtenerTodos()
        {
            IQueryable<ProductosInsumo> query = _dbcontext.ProductosInsumos;
            return await Task.FromResult(query);
        }

        public async Task<IQueryable<ProductosInsumo>> ObtenerInsumosProducto(int id)
        {
            IQueryable<ProductosInsumo> query = _dbcontext.ProductosInsumos.Where(x => x.IdProducto == id);
            return await Task.FromResult(query);
        }




    }
}
