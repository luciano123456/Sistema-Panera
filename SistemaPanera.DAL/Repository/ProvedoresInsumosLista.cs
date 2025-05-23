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
    public class ProveedoresInsumosRepository : IProveedoresInsumosRepository<ProveedoresInsumos>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public ProveedoresInsumosRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
       

        public async Task<bool> Insertar(Models.ProveedoresInsumos model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                _dbcontext.ProveedoresInsumos.Add(model);
                await _dbcontext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Actualizar(ProveedoresInsumos model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                _dbcontext.ProveedoresInsumos.Update(model);

                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        public async Task<bool> Eliminar(int id)
        {
            Models.ProveedoresInsumos model = _dbcontext.ProveedoresInsumos.First(c => c.Id == id);
            _dbcontext.ProveedoresInsumos.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }


        public async Task<Models.ProveedoresInsumos> Obtener(int id)
        {
            return await _dbcontext.ProveedoresInsumos
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<IQueryable<Models.ProveedoresInsumos>> ObtenerTodos()
        {

            IQueryable<Models.ProveedoresInsumos> query = _dbcontext.ProveedoresInsumos;
            return await Task.FromResult(query);
        }




    }
}
