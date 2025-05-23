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

        //public async Task<bool> InsertarPrefabricados(List<RecetasPrefabricado> Prefabricados)
        //{
        //    try
        //    {
        //        foreach (RecetasPrefabricado p in Prefabricados)
        //        {
        //            // Verificar si el Prefabricado ya existe, por ejemplo, por IdPrefabricado y IdPedido
        //            var PrefabricadoExistente = await _dbcontext.RecetasPrefabricados
        //                                                     .FirstOrDefaultAsync(x => x.IdReceta == p.IdReceta && x.IdPrefabricado == p.IdPrefabricado);

        //            if (PrefabricadoExistente != null)
        //            {
        //                // Si el Prefabricado existe, actualizamos sus propiedades
        //                PrefabricadoExistente.CostoUnitario = p.CostoUnitario;
        //                PrefabricadoExistente.SubTotal = p.SubTotal;
        //                PrefabricadoExistente.Cantidad = p.Cantidad;
        //            }
        //            else
        //            {
        //                // Si el Prefabricado no existe, lo agregamos a la base de datos
        //                _dbcontext.RecetasPrefabricados.Add(p);
        //            }
        //        }


        //        var PrefabricadosIdsModelo = Prefabricados.Select(p => p.IdReceta).Distinct().ToList();
        //        var PrefabricadosAEliminar = await _dbcontext.RecetasPrefabricados
        //                                                  .Where(x => PrefabricadosIdsModelo.Contains(x.IdReceta)
        //                                                          && !Prefabricados.Select(p => p.IdPrefabricado).Contains(x.IdPrefabricado)
        //                                                          && x.Id != 0)
        //                                                  .ToListAsync();


        //        foreach (var Prefabricado in PrefabricadosAEliminar)
        //        {
        //            _dbcontext.RecetasPrefabricados.Remove(Prefabricado);
        //        }

        //        await _dbcontext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //public async Task<bool> ActualizarPrefabricados(List<RecetasPrefabricado> Prefabricados)
        //{
        //    try
        //    {
        //        foreach (RecetasPrefabricado p in Prefabricados)
        //        {
        //            _dbcontext.RecetasPrefabricados.Update(p);
        //        }

        //        await _dbcontext.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        //public async Task<List<RecetasPrefabricado>> ObtenerPrefabricados(int idReceta)
        //{
        //    try
        //    {

        //        List<RecetasPrefabricado> productos = _dbcontext.RecetasPrefabricados
        //            .Include(c => c.IdRecetaNavigation)
        //            .Include(c => c.IdPrefabricadoNavigation)
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
