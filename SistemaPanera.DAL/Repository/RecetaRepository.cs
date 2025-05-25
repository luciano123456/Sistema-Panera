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
    public class RecetaRepository : IRecetaRepository<Receta>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public RecetaRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
        public async Task<bool> Actualizar(Models.Receta model)
        {
            _dbcontext.Recetas.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Receta model = _dbcontext.Recetas.First(c => c.Id == id);
            _dbcontext.Recetas.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Receta model)
        {
            _dbcontext.Recetas.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Receta> Obtener(int id)
        {
            Models.Receta model = await _dbcontext.Recetas.FindAsync(id);
            return model;
        }

        public async Task<IQueryable<Models.Receta>> ObtenerTodos()
        {
            IQueryable<Models.Receta> query = _dbcontext.Recetas;
            return await Task.FromResult(query);
        }


        public async Task<bool> InsertarInsumos(List<RecetasInsumo> insumos)
        {
            try
            {
                foreach (RecetasInsumo p in insumos)
                {
                    // Verificar si el insumo ya existe, por ejemplo, por Idinsumo y IdPedido
                    var insumoExistente = await _dbcontext.RecetasInsumos
                                                             .FirstOrDefaultAsync(x => x.IdReceta == p.IdReceta && x.IdInsumo == p.IdInsumo);

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
                        _dbcontext.RecetasInsumos.Add(p);
                    }
                }


                var insumosIdsModelo = insumos.Select(p => p.IdReceta).Distinct().ToList();
                var insumosAEliminar = await _dbcontext.RecetasInsumos
                                                          .Where(x => insumosIdsModelo.Contains(x.IdReceta)
                                                                  && !insumos.Select(p => p.IdInsumo).Contains(x.IdInsumo)
                                                                  && x.Id != 0)
                                                          .ToListAsync();


                foreach (var insumo in insumosAEliminar)
                {
                    _dbcontext.RecetasInsumos.Remove(insumo);
                }

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> ActualizarInsumos(List<RecetasInsumo> insumos)
        {
            try
            {
                foreach (RecetasInsumo p in insumos)
                {
                    _dbcontext.RecetasInsumos.Update(p);
                }

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<RecetasInsumo>> ObtenerInsumos(int idReceta)
        {
            try
            {

                List<RecetasInsumo> productos = _dbcontext.RecetasInsumos
                    .Include(c => c.IdRecetaNavigation)
                    .Include(c => c.IdInsumoNavigation)
                    .Where(c => c.IdReceta == idReceta).ToList();
                return productos;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        //public async Task<bool> InsertarSubrecetas(List<RecetasSubreceta> Subrecetas)
        //{
        //    try
        //    {
        //        foreach (RecetasSubreceta p in Subrecetas)
        //        {
        //            // Verificar si el Subreceta ya existe, por ejemplo, por IdSubreceta y IdPedido
        //            var SubrecetaExistente = await _dbcontext.RecetasSubrecetas
        //                                                     .FirstOrDefaultAsync(x => x.IdReceta == p.IdReceta && x.IdSubreceta == p.IdSubreceta);

        //            if (SubrecetaExistente != null)
        //            {
        //                // Si el Subreceta existe, actualizamos sus propiedades
        //                SubrecetaExistente.CostoUnitario = p.CostoUnitario;
        //                SubrecetaExistente.SubTotal = p.SubTotal;
        //                SubrecetaExistente.Cantidad = p.Cantidad;
        //            }
        //            else
        //            {
        //                // Si el Subreceta no existe, lo agregamos a la base de datos
        //                _dbcontext.RecetasSubrecetas.Add(p);
        //            }
        //        }


        //        var SubrecetasIdsModelo = Subrecetas.Select(p => p.IdReceta).Distinct().ToList();
        //        var SubrecetasAEliminar = await _dbcontext.RecetasSubrecetas
        //                                                  .Where(x => SubrecetasIdsModelo.Contains(x.IdReceta)
        //                                                          && !Subrecetas.Select(p => p.IdSubreceta).Contains(x.IdSubreceta)
        //                                                          && x.Id != 0)
        //                                                  .ToListAsync();


        //        foreach (var Subreceta in SubrecetasAEliminar)
        //        {
        //            _dbcontext.RecetasSubrecetas.Remove(Subreceta);
        //        }

        //        await _dbcontext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //public async Task<bool> ActualizarSubrecetas(List<RecetasSubreceta> Subrecetas)
        //{
        //    try
        //    {
        //        foreach (RecetasSubreceta p in Subrecetas)
        //        {
        //            _dbcontext.RecetasSubrecetas.Update(p);
        //        }

        //        await _dbcontext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        //public async Task<List<RecetasSubreceta>> ObtenerSubrecetas(int idReceta)
        //{
        //    try
        //    {

        //        List<RecetasSubreceta> productos = _dbcontext.RecetasSubrecetas
        //            .Include(c => c.IdRecetaNavigation)
        //            .Include(c => c.IdSubrecetaNavigation)
        //            .Where(c => c.IdReceta == idReceta).ToList();
        //        return productos;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return null;
        //    }
        //}

    }
}
