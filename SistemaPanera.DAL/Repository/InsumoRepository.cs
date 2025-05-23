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
    public class InsumoRepository : IInsumoRepository<Insumo>
    {

        private readonly SistemaPaneraContext _dbcontext;

        public InsumoRepository(SistemaPaneraContext context)
        {
            _dbcontext = context;
        }
       

        public async Task<bool> Insertar(Models.Insumo model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                _dbcontext.Insumos.Add(model);
                await _dbcontext.SaveChangesAsync();

                // Guardar relaciones de unidades de negocio
                if (model.InsumosUnidadesNegocios != null)
                {
                    foreach (var unidad in model.InsumosUnidadesNegocios)
                    {
                        unidad.IdInsumo = model.Id;
                        _dbcontext.InsumosUnidadesNegocios.Add(unidad);
                    }
                }

                // Guardar relaciones de proveedores
                if (model.InsumosProveedores != null)
                {
                    foreach (var proveedor in model.InsumosProveedores)
                    {
                        proveedor.IdInsumo = model.Id;
                        _dbcontext.InsumosProveedores.Add(proveedor);
                    }
                }

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

        public async Task<bool> Actualizar(Insumo model)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                var insumoExistente = await _dbcontext.Insumos
                    .Include(i => i.InsumosUnidadesNegocios)
                    .Include(i => i.InsumosProveedores)
                    .FirstOrDefaultAsync(i => i.Id == model.Id);

                if (insumoExistente == null) return false;

                _dbcontext.Entry(insumoExistente).CurrentValues.SetValues(model);

                // === UNIDADES DE NEGOCIO ===
                var nuevosUnidades = model.InsumosUnidadesNegocios ?? new List<InsumosUnidadesNegocio>();
                var idsUnidadesNuevos = nuevosUnidades.Select(x => x.IdUnidadNegocio).ToHashSet();

                var unidadesAEliminar = insumoExistente.InsumosUnidadesNegocios
                    .Where(x => !idsUnidadesNuevos.Contains(x.IdUnidadNegocio))
                    .ToList();
                _dbcontext.InsumosUnidadesNegocios.RemoveRange(unidadesAEliminar);

                foreach (var unidad in nuevosUnidades)
                {
                    if (!insumoExistente.InsumosUnidadesNegocios.Any(x => x.IdUnidadNegocio == unidad.IdUnidadNegocio))
                    {
                        unidad.Id = 0;
                        unidad.IdInsumo = model.Id;
                        _dbcontext.InsumosUnidadesNegocios.Add(unidad);
                    }
                }

              

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
            Models.Insumo model = _dbcontext.Insumos.First(c => c.Id == id);
            _dbcontext.Insumos.Remove(model);
            await _dbcontext.SaveChangesAsync();
            return true;
        }


        public async Task<Models.Insumo> Obtener(int id)
        {
            return await _dbcontext.Insumos
                .Include(x => x.InsumosProveedores)
                .Include(x => x.InsumosUnidadesNegocios)
                .ThenInclude(x => x.IdUnidadNegocioNavigation)
                .Include(x => x.InsumosProveedores)
                .ThenInclude(x => x.IdProveedorNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<IQueryable<Models.Insumo>> ObtenerTodos()
        {

            IQueryable<Models.Insumo> query = _dbcontext.Insumos;
            return await Task.FromResult(query);
        }




    }
}
