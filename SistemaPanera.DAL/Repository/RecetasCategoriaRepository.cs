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
    public class RecetasCategoriaRepository : IRecetasCategoriaRepository<RecetasCategoria>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public RecetasCategoriaRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(RecetasCategoria model)
        {
            _dbcontext.RecetasCategorias.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            RecetasCategoria model = _dbcontext.RecetasCategorias.First(c => c.Id == id);
            _dbcontext.RecetasCategorias.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(RecetasCategoria model)
        {
            _dbcontext.RecetasCategorias.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<RecetasCategoria> Obtener(int id)
        {
            RecetasCategoria model = await _dbcontext.RecetasCategorias.FindAsync(id);
            return model;
        }
        public async Task<IQueryable<RecetasCategoria>> ObtenerTodos()
        {
            IQueryable<RecetasCategoria> query = _dbcontext.RecetasCategorias;
            return await Task.FromResult(query);
        }




    }
}
