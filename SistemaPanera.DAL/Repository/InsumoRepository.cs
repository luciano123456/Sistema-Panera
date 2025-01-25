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
    public class InsumoRepository : IInsumoRepository<Insumo>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public InsumoRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Models.Insumo model)
        {
            _dbcontext.Insumos.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Insumo model = _dbcontext.Insumos.First(c => c.Id == id);
            _dbcontext.Insumos.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Insumo model)
        {
            _dbcontext.Insumos.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Insumo> Obtener(int id)
        {
            Models.Insumo model = await _dbcontext.Insumos.FindAsync(id);
            return model;
        }

        public async Task<IQueryable<Models.Insumo>> ObtenerTodos()
        {
            IQueryable<Models.Insumo> query = _dbcontext.Insumos;
            return await Task.FromResult(query);
        }




    }
}
