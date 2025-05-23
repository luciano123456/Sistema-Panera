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
    //public class PrefabricadosCategoriaRepository : IPrefabricadosCategoriaRepository<PrefabricadosCategoria>
    //{

    //    private readonly SistemaPaneraContext _dbcontext;

    //    public PrefabricadosCategoriaRepository(SistemaPaneraContext context)
    //    {
    //        _dbcontext = context;
    //    }
    //    public async Task<bool> Actualizar(PrefabricadosCategoria model)
    //    {
    //        _dbcontext.PrefabricadosCategorias.Update(model);
    //        await _dbcontext.SaveChangesAsync();
    //        return true;
    //    }

    //    public async Task<bool> Eliminar(int id)
    //    {
    //        PrefabricadosCategoria model = _dbcontext.PrefabricadosCategorias.First(c => c.Id == id);
    //        _dbcontext.PrefabricadosCategorias.Remove(model);
    //        await _dbcontext.SaveChangesAsync();
    //        return true;
    //    }

    //    public async Task<bool> Insertar(PrefabricadosCategoria model)
    //    {
    //        _dbcontext.PrefabricadosCategorias.Add(model);
    //        await _dbcontext.SaveChangesAsync();
    //        return true;
    //    }

    //    public async Task<PrefabricadosCategoria> Obtener(int id)
    //    {
    //        PrefabricadosCategoria model = await _dbcontext.PrefabricadosCategorias.FindAsync(id);
    //        return model;
    //    }
    //    public async Task<IQueryable<PrefabricadosCategoria>> ObtenerTodos()
    //    {
    //        IQueryable<PrefabricadosCategoria> query = _dbcontext.PrefabricadosCategorias;
    //        return await Task.FromResult(query);
    //    }




    //}
}
