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
    public class LocalRepository : IGenericRepository<Local>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public LocalRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Models.Local model)
        {
            _dbcontext.Locales.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Local model = _dbcontext.Locales.First(c => c.Id == id);
            _dbcontext.Locales.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Local model)
        {
            _dbcontext.Locales.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Local> Obtener(int id)
        {
            Models.Local model = await _dbcontext.Locales.FindAsync(id);
            return model;
        }

        public async Task<IQueryable<Models.Local>> ObtenerTodos()
        {
            IQueryable<Models.Local> query = _dbcontext.Locales;
            return await Task.FromResult(query);
        }




    }
}
