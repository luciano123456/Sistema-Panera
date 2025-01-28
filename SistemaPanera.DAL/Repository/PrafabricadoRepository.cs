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
            Models.Prefabricado model = await _dbcontext.Prefabricados
                .Include(p => p.PrefabricadosInsumos)
                .FirstOrDefaultAsync(p => p.Id == id);
            return model;
        }

        public async Task<IQueryable<Models.Prefabricado>> ObtenerTodos()
        {
            IQueryable<Models.Prefabricado> query = _dbcontext.Prefabricados;
            return await Task.FromResult(query);
        }

        public async Task<bool> InsertarInsumos(List<PrefabricadosInsumo> insumos)
        {
            foreach (PrefabricadosInsumo p in insumos)
            {
                // Verificar si el insumo ya existe, por ejemplo, por Idinsumo y IdPedido
                var insumoExistente = await _dbcontext.PrefabricadosInsumos
                                                         .FirstOrDefaultAsync(x => x.IdPrefabricado == p.IdPrefabricado && x.IdInsumo == p.IdInsumo);

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
                    _dbcontext.PrefabricadosInsumos.Add(p);
                }
            }


            var insumosIdsModelo = insumos.Select(p => p.IdPrefabricado).Distinct().ToList();
            var insumosAEliminar = await _dbcontext.PrefabricadosInsumos
                                                      .Where(x => insumosIdsModelo.Contains(x.IdPrefabricado)
                                                              && !insumos.Select(p => p.IdInsumo).Contains(x.IdInsumo)
                                                              && x.Id != 0)
                                                      .ToListAsync();


            foreach (var insumo in insumosAEliminar)
            {
                _dbcontext.PrefabricadosInsumos.Remove(insumo);
            }

            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarInsumos(List<PrefabricadosInsumo> insumos)
        {
            foreach (PrefabricadosInsumo p in insumos)
            {
                _dbcontext.PrefabricadosInsumos.Update(p);
            }

            await _dbcontext.SaveChangesAsync();
            return true;

        }


        public async Task<List<PrefabricadosInsumo>> ObtenerInsumos(int idPrefabricado)
        {
            try
            {

                List<PrefabricadosInsumo> productos = _dbcontext.PrefabricadosInsumos
                    .Include(c => c.IdPrefabricadoNavigation)
                    .Include(c => c.IdInsumoNavigation)
                    .Where(c => c.IdPrefabricado == idPrefabricado).ToList();
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
