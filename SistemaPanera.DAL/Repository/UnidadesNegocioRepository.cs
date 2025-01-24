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
    public class UnidadesNegocioRepository : IUnidadesNegocioRepository<UnidadesNegocio>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public UnidadesNegocioRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(UnidadesNegocio model)
        {
            _dbcontext.UnidadesNegocios.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            UnidadesNegocio model = _dbcontext.UnidadesNegocios.First(c => c.Id == id);
            _dbcontext.UnidadesNegocios.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(UnidadesNegocio model)
        {
            _dbcontext.UnidadesNegocios.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<UnidadesNegocio> Obtener(int id)
        {
            UnidadesNegocio model = await _dbcontext.UnidadesNegocios.FindAsync(id);
            return model;
        }
        public async Task<IQueryable<UnidadesNegocio>> ObtenerTodos()
        {
            IQueryable<UnidadesNegocio> query = _dbcontext.UnidadesNegocios;
            return await Task.FromResult(query);
        }




    }
}
