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
    public class PrefabricadoRepository : IPrefabricadoRepository<Prefabricado>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public PrefabricadoRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Models.Prefabricado model)
        {
            _dbcontext.Prefabricados.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Prefabricado model = _dbcontext.Prefabricados.First(c => c.Id == id);
            _dbcontext.Prefabricados.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Prefabricado model)
        {
            _dbcontext.Prefabricados.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Prefabricado> Obtener(int id)
        {
            Models.Prefabricado model = await _dbcontext.Prefabricados.FindAsync(id);
            return model;
        }

        public async Task<IQueryable<Models.Prefabricado>> ObtenerTodos()
        {
            IQueryable<Models.Prefabricado> query = _dbcontext.Prefabricados;
            return await Task.FromResult(query);
        }




    }
}
