using Microsoft.EntityFrameworkCore;
using SistemaPanera.Models;
using SistemaPanera.DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public class RecetaRepository : IRecetaRepository<Receta>
    {
        private readonly SistemaPaneraContext _dbcontext;

        public RecetaRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }

        public async Task<bool> Insertar(Receta model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();

            try
            {
                if (model.RecetasInsumos != null)
                {
                    foreach (var i in model.RecetasInsumos)
                        i.Id = 0;
                }

                var subrecetas = model.RecetasSubreceta?.ToList();
                model.RecetasSubreceta = null;

                _dbcontext.Recetas.Add(model);
                await _dbcontext.SaveChangesAsync();

                if (subrecetas != null && subrecetas.Any())
                {
                    foreach (var sub in subrecetas)
                    {
                        sub.Id = 0;
                        sub.IdReceta = model.Id;
                    }

                    _dbcontext.RecetasSubrecetas.AddRange(subrecetas);
                    await _dbcontext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error al insertar receta: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Actualizar(Receta model)
        {
            try
            {
                var existente = await _dbcontext.Recetas
                    .Include(x => x.RecetasInsumos)
                    .Include(x => x.RecetasSubreceta)
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (existente == null)
                    return false;

                _dbcontext.Entry(existente).CurrentValues.SetValues(model);

                _dbcontext.RecetasInsumos.RemoveRange(existente.RecetasInsumos);
                if (model.RecetasInsumos?.Any() == true)
                {
                    foreach (var insumo in model.RecetasInsumos)
                        insumo.IdReceta = model.Id;

                    await _dbcontext.RecetasInsumos.AddRangeAsync(model.RecetasInsumos);
                }

                _dbcontext.RecetasSubrecetas.RemoveRange(existente.RecetasSubreceta);
                if (model.RecetasSubreceta?.Any() == true)
                {
                    foreach (var sub in model.RecetasSubreceta)
                        sub.IdReceta = model.Id;

                    await _dbcontext.RecetasSubrecetas.AddRangeAsync(model.RecetasSubreceta);
                }

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar receta: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var subrecetas = _dbcontext.RecetasSubrecetas
                    .Where(s => s.IdReceta == id).ToList();
                _dbcontext.RecetasSubrecetas.RemoveRange(subrecetas);

                var insumos = _dbcontext.RecetasInsumos
                    .Where(i => i.IdReceta == id).ToList();
                _dbcontext.RecetasInsumos.RemoveRange(insumos);

                var model = _dbcontext.Recetas.First(c => c.Id == id);
                _dbcontext.Recetas.Remove(model);

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar receta: " + ex.Message);
                return false;
            }
        }

        public async Task<Receta> Obtener(int id)
        {
            try
            {
                var model = await _dbcontext.Recetas
                    .Include(p => p.RecetasInsumos)
                        .ThenInclude(p => p.IdInsumoNavigation)
                    .Include(p => p.RecetasSubreceta)
                        .ThenInclude(p => p.IdSubRecetaNavigation)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<IQueryable<Receta>> ObtenerTodos()
        {
            return await Task.FromResult(_dbcontext.Recetas);
        }

        public async Task<List<RecetasInsumo>> ObtenerInsumos(int idReceta)
        {
            try
            {
                return _dbcontext.RecetasInsumos
                    .Include(c => c.IdRecetaNavigation)
                    .Include(c => c.IdInsumoNavigation)
                    .Where(c => c.IdReceta == idReceta)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public Task<bool> InsertarInsumos(List<RecetasInsumo> insumos)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ActualizarInsumos(List<RecetasInsumo> insumos)
        {
            throw new NotImplementedException();
        }
    }
}
