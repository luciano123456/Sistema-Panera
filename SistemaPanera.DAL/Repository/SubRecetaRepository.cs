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
        public async Task<bool> Actualizar(Models.Subreceta model)
        {
            _dbcontext.Subrecetas.Update(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Models.Subreceta model = _dbcontext.Subrecetas.First(c => c.Id == id);
            _dbcontext.Subrecetas.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Models.Subreceta model)
        {
            _dbcontext.Subrecetas.Add(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Subreceta> Obtener(int id)
        {
            Models.Subreceta model = await _dbcontext.Subrecetas
                .Include(p => p.SubrecetasInsumos)
                .FirstOrDefaultAsync(p => p.Id == id);
            return model;
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
