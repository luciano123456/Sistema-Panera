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
    public class UnidadesMedidaRepository : IUnidadesMedidaRepository<UnidadesMedida>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public UnidadesMedidaRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(UnidadesMedida model)
        {
            _dbcontext.UnidadesMedida.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            UnidadesMedida model = _dbcontext.UnidadesMedida.First(c => c.Id == id);
            _dbcontext.UnidadesMedida.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(UnidadesMedida model)
        {
            _dbcontext.UnidadesMedida.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<UnidadesMedida> Obtener(int id)
        {
            UnidadesMedida model = await _dbcontext.UnidadesMedida.FindAsync(id);
            return model;
        }
        public async Task<IQueryable<UnidadesMedida>> ObtenerTodos()
        {
            IQueryable<UnidadesMedida> query = _dbcontext.UnidadesMedida;
            return await Task.FromResult(query);
        }




    }
}
