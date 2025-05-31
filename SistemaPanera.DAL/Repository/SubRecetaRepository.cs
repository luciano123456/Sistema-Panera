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
    public class SubrecetaRepository : ISubrecetaRepository<Subreceta>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public SubrecetaRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }

        public async Task<bool> Insertar(Subreceta model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();

            try
            {
                // Resetear IDs de insumos
                if (model.SubrecetasInsumos != null)
                    foreach (var i in model.SubrecetasInsumos)
                        i.Id = 0;

                // Guardar temporalmente los hijos (ya tienen IdSubRecetaHija válidos)
                var hijos = model.SubrecetasSubrecetaIdSubRecetaHijaNavigations?.ToList();
                model.SubrecetasSubrecetaIdSubRecetaHijaNavigations = null;

                // Insertar la subreceta principal
                _dbcontext.Subrecetas.Add(model);
                await _dbcontext.SaveChangesAsync(); // Ahora model.Id tiene el nuevo ID

                // Agregar hijos (relación con subreceta padre recién insertada)
                if (hijos != null && hijos.Any())
                {
                    foreach (var h in hijos)
                    {
                        h.Id = 0;
                        h.IdSubRecetaPadre = model.Id; // <- clave: vincular al nuevo padre
                    }

                    _dbcontext.SubrecetasSubrecetas.AddRange(hijos);
                    await _dbcontext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error en Insertar Subreceta: " + ex.Message);
                return false;
            }
        }







        public async Task<bool> Actualizar(Subreceta model)
        {
            try
            {
                var existente = await _dbcontext.Subrecetas
                    .Include(x => x.SubrecetasInsumos)
                    .Include(x => x.SubrecetasSubrecetaIdSubRecetaPadreNavigations)
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (existente == null)
                    return false;

                // Actualizar campos simples
                _dbcontext.Entry(existente).CurrentValues.SetValues(model);

                // Reemplazar insumos
                _dbcontext.SubrecetasInsumos.RemoveRange(existente.SubrecetasInsumos);
                if (model.SubrecetasInsumos != null && model.SubrecetasInsumos.Count > 0)
                {
                    foreach (var insumo in model.SubrecetasInsumos)
                        insumo.IdSubreceta = model.Id;

                    await _dbcontext.SubrecetasInsumos.AddRangeAsync(model.SubrecetasInsumos);
                }

                // Reemplazar subrecetas hijas
                _dbcontext.SubrecetasSubrecetas.RemoveRange(existente.SubrecetasSubrecetaIdSubRecetaPadreNavigations);
                if (model.SubrecetasSubrecetaIdSubRecetaPadreNavigations != null && model.SubrecetasSubrecetaIdSubRecetaPadreNavigations.Count > 0)
                {
                    foreach (var subreceta in model.SubrecetasSubrecetaIdSubRecetaPadreNavigations)
                        subreceta.IdSubRecetaPadre = model.Id;

                    await _dbcontext.SubrecetasSubrecetas.AddRangeAsync(model.SubrecetasSubrecetaIdSubRecetaPadreNavigations);
                }

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log de error si lo necesitas
                return false;
            }
        }


        public async Task<bool> Eliminar(int id)
        {
            try
            {
                // 1. Eliminar registros hijos en Subrecetas_Subrecetas
                var hijos = _dbcontext.SubrecetasSubrecetas
                    .Where(s => s.IdSubRecetaPadre == id)
                    .ToList();

                _dbcontext.SubrecetasSubrecetas.RemoveRange(hijos);

                // 2. Eliminar insumos asociados (si corresponde)
                var insumos = _dbcontext.SubrecetasInsumos
                    .Where(i => i.IdSubreceta == id)
                    .ToList();

                _dbcontext.SubrecetasInsumos.RemoveRange(insumos);

                // 3. Eliminar la subreceta principal
                var model = _dbcontext.Subrecetas.First(c => c.Id == id);
                _dbcontext.Subrecetas.Remove(model);

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar Subreceta: " + ex.Message);
                return false;
            }
        }


        //public async Task<bool> Insertar(Models.Subreceta model)
        //{
        //    _dbcontext.Subrecetas.Add(model);
        //    await _dbcontext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<Models.Subreceta> Obtener(int id)
        {
            try
            {
                var model = await _dbcontext.Subrecetas
                    .Include(p => p.SubrecetasInsumos)
                        .ThenInclude(p => p.IdInsumoNavigation)
                    .Include(p => p.SubrecetasSubrecetaIdSubRecetaPadreNavigations)
                        .ThenInclude(p => p.IdSubRecetaHijaNavigation)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public async Task<IQueryable<Models.Subreceta>> ObtenerTodos()
        {
            IQueryable<Models.Subreceta> query = _dbcontext.Subrecetas;
            return await Task.FromResult(query);
        }

        public async Task<bool> InsertarInsumos(List<SubrecetasInsumo> insumos)
        {
            foreach (SubrecetasInsumo p in insumos)
            {
                // Verificar si el insumo ya existe, por ejemplo, por Idinsumo y IdPedido
                var insumoExistente = await _dbcontext.SubrecetasInsumos
                                                         .FirstOrDefaultAsync(x => x.IdSubreceta == p.IdSubreceta && x.IdInsumo == p.IdInsumo);

                if (insumoExistente != null)
                {
                    // Si el insumo existe, actualizamos sus propiedades
                    insumoExistente.CostoUnitario = p.CostoUnitario;
                    insumoExistente.SubTotal = p.SubTotal;
                    insumoExistente.Cantidad = p.Cantidad;
                }
                else
                {
                    // Si el insumo no existe, lo agregamos a la base de datos
                    _dbcontext.SubrecetasInsumos.Add(p);
                }
            }


            var insumosIdsModelo = insumos.Select(p => p.IdSubreceta).Distinct().ToList();
            var insumosAEliminar = await _dbcontext.SubrecetasInsumos
                                                      .Where(x => insumosIdsModelo.Contains(x.IdSubreceta)
                                                              && !insumos.Select(p => p.IdInsumo).Contains(x.IdInsumo)
                                                              && x.Id != 0)
                                                      .ToListAsync();


            foreach (var insumo in insumosAEliminar)
            {
                _dbcontext.SubrecetasInsumos.Remove(insumo);
            }

            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarInsumos(List<SubrecetasInsumo> insumos)
        {
            foreach (SubrecetasInsumo p in insumos)
            {
                _dbcontext.SubrecetasInsumos.Update(p);
            }

            await _dbcontext.SaveChangesAsync();
            return true;

        }


        public async Task<List<SubrecetasInsumo>> ObtenerInsumos(int idSubreceta)
        {
            try
            {

                List<SubrecetasInsumo> productos = _dbcontext.SubrecetasInsumos
                    .Include(c => c.IdSubrecetaNavigation)
                    .Include(c => c.IdInsumoNavigation)
                    .Where(c => c.IdSubreceta == idSubreceta).ToList();
                return productos;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }



    }
}
